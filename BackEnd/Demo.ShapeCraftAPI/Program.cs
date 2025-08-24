using AspNetCoreRateLimit;
using Demo.ShapeCraftAPI.Services;
using Demo.ShapeCraftHackathon.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(
    builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

builder.Services.AddScoped<TokenService>();
builder.Services.AddShapeCraftDI(builder.Configuration);


builder.Services.AddCors(options =>
{
    options.AddPolicy("shapefrontend", policy =>
    {
        policy.WithOrigins("https://shapecrafthackathon.vercel.app")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],

            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],

            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!)
            )
        };
    });
 

var app = builder.Build();

app.UseIpRateLimiting();

app.Use(async (context, next) =>
{
    await next.Invoke();

    if (context.Response.StatusCode == 429)
    {
        context.Response.ContentType = "application/json";
        var response = new
        {
            message = "Rate limit exceeded. Please try again later.",
            retryAfter = context.Response.Headers["Retry-After"]
        };
        await context.Response.WriteAsJsonAsync(response);
    }
});

app.UseRouting();

app.UseCors("shapefrontend");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

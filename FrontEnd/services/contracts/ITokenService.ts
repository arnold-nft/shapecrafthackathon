export interface ITokenService {
  getToken(userAddress: string): Promise<string | undefined>;
}

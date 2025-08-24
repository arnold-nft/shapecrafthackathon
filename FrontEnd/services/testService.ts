export class TestService {
    async generateToken({ userAddress }: {userAddress: string}) {
        try {
            const response = await fetch('http://localhost:5248/token/generate', {
              method: 'POST',
              headers: {
                'Content-Type': 'application/json',
              },
              body: JSON.stringify(userAddress),
            });

            if (!response.ok) {
                throw new Error('Failed to fetch the token');
            }

            const data = await response.json();
            const token = data.token;
            
            return token;
        } catch (error) {
            console.error('Error fetching token:', error);
        }
    }

    async callAPI() {
        const token = await this.generateToken({ userAddress: "useraddy" });

        if (!token) {
            console.error("No token available.");
            return;
      }
      
      const question = 'What are the fees when I am buying an azuki?';

        const response = await fetch(
          `http://localhost:5248/chat/generate?question=${encodeURIComponent(question)}`,
          {
            //const response = await fetch('http://localhost:5248/test/GetData', {
            method: 'GET',
            headers: {
              Authorization: `Bearer ${token}`,
            },
          }
        );

        if (!response.ok) {
            console.error(`Failed to fetch data: ${response.statusText}`);
            return;
        }

        const data = await response.json();
        console.log("Data received:", data);
    }
}

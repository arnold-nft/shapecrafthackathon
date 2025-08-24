import { ITokenService } from './contracts/ITokenService';

export class TokenService implements ITokenService {
  async getToken(userAddress: string): Promise<string | undefined> {
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
      return data.token;
    } catch (error) {
      console.error('Error fetching token:', error);
      return undefined;
    }
  }
}

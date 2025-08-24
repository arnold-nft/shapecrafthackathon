import { IChatService } from './contracts/IChatService';

export class ChatService implements IChatService {
  async getChat(jwtToken: string, question: string): Promise<string | undefined> {
    if (!jwtToken) {
      console.error('No token available.');
      return;
    }

    const response = await fetch(
      `http://localhost:5248/chat/generate?question=${encodeURIComponent(question)}`,
      {
        method: 'GET',
        headers: {
          Authorization: `Bearer ${jwtToken}`,
        },
      }
    );

    if (!response.ok) {
      console.error(`Failed to fetch data: ${response.statusText}`);
      return;
    }

    const data = await response.json();
    return data.answer;
  }
}

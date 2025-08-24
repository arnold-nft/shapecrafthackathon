export interface IChatService {
  getChat(jwtToken: string, question: string): Promise<string | undefined>;
}

'use client';

import { useState, useRef, useEffect } from 'react';
import { useJwtToken } from '../hooks/use-jwt-token';
import { ChatService } from '@/services/ChatService';

export default function Home() {
  const { jwtToken, isUserLoggedInAndIsThereAValidJwtToken, isLoading } = useJwtToken();
  const chatService = new ChatService();

  const [messages, setMessages] = useState<{ text: string; sender: 'user' | 'bot' }[]>([]);
  const [input, setInput] = useState('');
  const [loading, setLoading] = useState(false);
  const messagesEndRef = useRef<HTMLDivElement | null>(null);

  useEffect(() => {
    messagesEndRef.current?.scrollIntoView({ behavior: 'smooth' });
  }, [messages, loading]);

  if (isLoading) return <p className="mt-10 text-center text-gray-500">Loading...</p>;
  if (!isUserLoggedInAndIsThereAValidJwtToken) {
    return (
      <p className="mt-10 text-center text-gray-500">Please connect your wallet to continue.</p>
    );
  }

  async function generateAnswer() {
    if (!input.trim()) return;
    if (!jwtToken) return;

    const userMessage = { text: input, sender: 'user' as const };
    setMessages((prev) => [...prev, userMessage]);
    setInput('');
    setLoading(true);

    try {
      const response = await chatService.getChat(jwtToken, input);
      const botMessage = { text: response ?? 'No response', sender: 'bot' as const };
      setMessages((prev) => [...prev, botMessage]);
    } catch (error) {
      setMessages((prev) => [
        ...prev,
        { text: `⚠️ Error fetching response: ${error}`, sender: 'bot' },
      ]);
    } finally {
      setLoading(false);
    }
  }

  return (
    <div className="flex min-h-0 flex-1 flex-col">
      <div className="flex-1 overflow-y-auto px-40">
        {messages.map((msg, idx) => (
          <div
            key={idx}
            className={`flex ${msg.sender === 'user' ? 'justify-end' : 'justify-start'}`}
          >
            <div
              className={`max-w-xl rounded-2xl px-4 py-2 whitespace-pre-wrap shadow-sm ${
                msg.sender === 'user'
                  ? 'rounded-br-none bg-blue-500 text-white'
                  : 'rounded-bl-none border border-gray-200 bg-white text-gray-900'
              } `}
            >
              {msg.text}
            </div>
          </div>
        ))}

        {loading && (
          <div className="flex justify-start">
            <div className="rounded-2xl rounded-bl-none border border-gray-200 bg-white px-4 py-2 shadow-sm">
              <span className="flex space-x-1">
                <span className="h-2 w-2 animate-bounce rounded-full bg-gray-400"></span>
                <span className="h-2 w-2 animate-bounce rounded-full bg-gray-400 delay-150"></span>
                <span className="h-2 w-2 animate-bounce rounded-full bg-gray-400 delay-300"></span>
              </span>
            </div>
          </div>
        )}

        <div ref={messagesEndRef} />
      </div>

      <div className="border-t border-gray-300 bg-white p-4">
        <div className="mx-auto flex max-w-3xl items-center">
          <input
            value={input}
            onChange={(e) => setInput(e.target.value)}
            onKeyDown={(e) => e.key === 'Enter' && generateAnswer()}
            placeholder="Send a message..."
            className="flex-1 resize-none rounded-xl border border-gray-300 px-4 py-2 focus:ring-2 focus:ring-blue-500 focus:outline-none"
          />
          <button
            onClick={generateAnswer}
            disabled={loading}
            className="ml-2 rounded-xl bg-blue-500 px-4 py-2 text-white hover:bg-blue-600 disabled:opacity-50"
          >
            Send
          </button>
        </div>
      </div>
    </div>
  );
}

'use client';

import { TestService } from '@/services/testService';
import { useJwtToken } from '../hooks/use-jwt-token';

export default function Home() {
  const testService = new TestService();

  console.log('TESTER');

  const { jwtToken, isUserLoggedInAndIsThereAValidJwtToken, isLoading } = useJwtToken();

  if (isLoading) return <p>Loading...</p>;

  if (!isUserLoggedInAndIsThereAValidJwtToken) {
    return <p>Please connect your wallet to continue.</p>;
  }

  if (!isLoading && jwtToken) {
    testService.callAPI(jwtToken);
  }

  return (
    <>
      
      <h1>test</h1>
      <div>Your JWT: {jwtToken}</div>
    </>
  );
}

import { useState, useEffect } from 'react';
import { useAccount } from 'wagmi';
import {jwtDecode} from 'jwt-decode';
import { TokenService } from '../services/TokenService';

interface JwtPayload {
  exp: number;
  [key: string]: unknown;
}

interface UseJwtTokenReturn {
  jwtToken: string | null;
  isUserLoggedInAndIsThereAValidJwtToken: boolean;
  isLoading: boolean;
}

const LOCAL_STORAGE_KEY = 'jwt_token';

const isTokenValid = (token: string): boolean => {
  try {
    const decoded = jwtDecode<JwtPayload>(token);
    const currentTime = Date.now() / 1000;
    return decoded.exp > currentTime;
  } catch (e) {
      console.log(e);
    return false;
  }
};

export const useJwtToken = (): UseJwtTokenReturn => {
  const { address, isConnected } = useAccount();
  const [jwtToken, setJwtToken] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(true);

  useEffect(() => {
    const fetchToken = async () => {
      setIsLoading(true);

      if (!isConnected || !address) {
        setJwtToken(null);
        setIsLoading(false);
        return;
      }

      const token = localStorage.getItem(LOCAL_STORAGE_KEY);

      if (token && isTokenValid(token)) {
        setJwtToken(token);
        setIsLoading(false);
        return;
      }

      const tokenService = new TokenService();
      const newToken = await tokenService.getToken(address);

      if (newToken && isTokenValid(newToken)) {
        localStorage.setItem(LOCAL_STORAGE_KEY, newToken);
        setJwtToken(newToken);
      } else {
        localStorage.removeItem(LOCAL_STORAGE_KEY);
        setJwtToken(null);
      }

      setIsLoading(false);
    };

    fetchToken();
  }, [address, isConnected]);

  return {
    jwtToken,
    isUserLoggedInAndIsThereAValidJwtToken: !!jwtToken && isConnected,
    isLoading,
  };
};

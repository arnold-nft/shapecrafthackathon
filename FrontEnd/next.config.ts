import type { NextConfig } from 'next';

const nextConfig: NextConfig = {
  experimental: {
    nextScriptWorkers: false,
  },
  env: {
    NEXT_DISABLE_DEVTOOLS: 'true',
  },
};

export default nextConfig;

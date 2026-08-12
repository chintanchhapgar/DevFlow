import axios from "axios";

import { authStorage } from "@/features/auth/auth-storage";

export const projectApiClient = axios.create({
  baseURL: import.meta.env.VITE_PROJECT_API_BASE_URL,
  headers: {
    "Content-Type": "application/json",
  },
});

projectApiClient.interceptors.request.use((config) => {
  const accessToken = authStorage.getAccessToken();

  if (accessToken) {
    config.headers.Authorization = `Bearer ${accessToken}`;
  }

  return config;
});
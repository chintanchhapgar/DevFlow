import axios, {
  type AxiosError,
  type InternalAxiosRequestConfig,
} from "axios";

import { authStorage } from "@/features/auth/auth-storage";
import { refreshAccessToken } from "@/features/auth/api/refresh-api";

const apiClient = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL,
  headers: {
    "Content-Type": "application/json",
  },
});

let refreshPromise: Promise<string> | null = null;

apiClient.interceptors.request.use(
  (config) => {
    const accessToken =
      authStorage.getAccessToken();

    if (accessToken) {
      config.headers.Authorization =
        `Bearer ${accessToken}`;
    }

    return config;
  },
);

apiClient.interceptors.response.use(
  (response) => response,

  async (error: AxiosError) => {
    const originalRequest =
      error.config as
        | (InternalAxiosRequestConfig & {
            _retry?: boolean;
          })
        | undefined;

    if (
      error.response?.status !== 401 ||
      !originalRequest ||
      originalRequest._retry
    ) {
      return Promise.reject(error);
    }

    const refreshToken =
      authStorage.getRefreshToken();

    if (!refreshToken) {
      authStorage.clear();

      window.location.href = "/login";

      return Promise.reject(error);
    }

    originalRequest._retry = true;

    try {
      if (!refreshPromise) {
        refreshPromise =
          refreshAccessToken(refreshToken)
            .then((result) => {
              authStorage.setTokens(
                result.accessToken,
                result.refreshToken,
              );

              return result.accessToken;
            })
            .finally(() => {
              refreshPromise = null;
            });
      }

      const newAccessToken =
        await refreshPromise;

      originalRequest.headers.Authorization =
        `Bearer ${newAccessToken}`;

      return apiClient(originalRequest);
    } catch (refreshError) {
      authStorage.clear();

      window.location.href = "/login";

      return Promise.reject(refreshError);
    }
  },
);

export { apiClient };
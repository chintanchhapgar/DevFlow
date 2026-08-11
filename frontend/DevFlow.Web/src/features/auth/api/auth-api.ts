import { apiClient } from "@/lib/api/api-client";

export interface LoginRequest {
  email: string;
  password: string;
}

export interface AuthenticationResponse {
  accessToken: string;
  refreshToken: string;
  refreshTokenExpiresOnUtc: string;
}

export async function login(
  request: LoginRequest,
): Promise<AuthenticationResponse> {
  const response = await apiClient.post(
    "/api/auth/login",
    request,
  );

  return response.data.data;
}
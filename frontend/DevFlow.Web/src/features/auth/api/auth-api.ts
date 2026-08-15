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

export interface RegisterRequest {
  email: string;
  password: string;
  firstName: string;
  lastName: string;
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

export async function register(request: RegisterRequest): Promise<void> {
  await apiClient.post("/api/auth/register", request);
}

export async function requestPasswordReset(email: string): Promise<void> {
  await apiClient.post("/api/auth/forgot-password", { email });
}

export async function resetPassword(
  token: string,
  newPassword: string,
): Promise<void> {
  await apiClient.post("/api/auth/reset-password", { token, newPassword });
}

export async function verifyEmail(token: string): Promise<void> {
  await apiClient.get("/api/auth/verify-email", { params: { token } });
}

import { apiClient } from "@/lib/api/api-client";

export interface LoginRequest {
  email: string;
  password: string;
}

export interface AuthenticationResponse {
  requiresTwoFactor: boolean;
  userId: string | null;
  accessToken: string | null;
  refreshToken: string | null;
  refreshTokenExpiresOnUtc: string | null;
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

export interface MfaSetupResponse {
  manualEntryKey: string;
  qrCodeUri: string;
  qrCodeImage: string;
}

export async function completeMfaLogin(
  userId: string,
  code: string,
): Promise<AuthenticationResponse> {
  const response = await apiClient.post("/api/auth/mfa/login", {
    userId,
    code,
    isRecoveryCode: false,
  });
  return response.data.data;
}

export async function setupMfa(): Promise<MfaSetupResponse> {
  const response = await apiClient.post("/api/auth/mfa/setup");
  return response.data.data;
}

export async function verifyMfaSetup(code: string): Promise<string[]> {
  const response = await apiClient.post("/api/auth/mfa/verify", { code });
  return response.data.data.recoveryCodes;
}

export async function disableMfa(
  code: string,
  isRecoveryCode: boolean,
): Promise<void> {
  await apiClient.post("/api/auth/mfa/disable", {
    code,
    isRecoveryCode,
  });
}

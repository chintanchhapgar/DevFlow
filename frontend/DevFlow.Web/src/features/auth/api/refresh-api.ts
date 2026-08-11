import { authApiClient } from "@/lib/api/auth-api-client";

export interface RefreshTokenResponse {
  accessToken: string;
  refreshToken: string;
  refreshTokenExpiresOnUtc: string;
}

export async function refreshAccessToken(
  refreshToken: string,
): Promise<RefreshTokenResponse> {
  const response = await authApiClient.post(
    "/api/auth/refresh",
    {
      refreshToken,
    },
  );

  return response.data.data;
}
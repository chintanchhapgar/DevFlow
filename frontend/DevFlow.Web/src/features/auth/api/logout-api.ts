import { apiClient } from "@/lib/api/api-client";

export interface LogoutRequest {
  refreshToken: string;
}

export async function logout(
  refreshToken: string,
): Promise<void> {
  await apiClient.post(
    "/api/auth/logout",
    {
      refreshToken,
    },
  );
}
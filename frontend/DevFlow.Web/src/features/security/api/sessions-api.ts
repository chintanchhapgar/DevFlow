import { apiClient } from "@/lib/api/api-client";
import type { Session } from "../types/session";

interface ApiResponse<T> {
  success: boolean;
  message: string;
  data: T | null;
  error: unknown;
  traceId: string;
  timestamp: string;
}

export async function getSessions(): Promise<Session[]> {
  const response =
    await apiClient.get<ApiResponse<Session[]>>(
      "/api/auth/sessions"
    );

  return response.data.data ?? [];
}

export async function revokeSession(
  sessionId: string
): Promise<void> {
  await apiClient.delete(
    `/api/auth/sessions/${sessionId}`
  );
}

export async function revokeOtherSessions(): Promise<void> {
  await apiClient.post(
    "/api/auth/sessions/revoke-others"
  );
}
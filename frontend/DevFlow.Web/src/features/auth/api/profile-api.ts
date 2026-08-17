import { apiClient } from "@/lib/api/api-client";

export interface UserProfile {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  fullName: string;
  role: string;
  isTwoFactorEnabled: boolean;
}

export async function getProfile(): Promise<UserProfile> {
  const response = await apiClient.get("/api/auth/profile");

  return response.data.data;
}

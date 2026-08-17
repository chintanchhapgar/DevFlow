import { apiClient } from "@/lib/api/api-client";

export interface UserListItem {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  fullName: string;
  role: string;
}

interface GetUsersResponse {
  users: UserListItem[];
  page: number;
  pageSize: number;
  totalCount: number;
}

export async function getUsers(): Promise<GetUsersResponse> {
  const response = await apiClient.get("/api/users", {
    params: { page: 1, pageSize: 100 },
  });

  return response.data.data;
}

export async function updateUserRole(
  userId: string,
  role: number,
): Promise<void> {
  await apiClient.put(`/api/users/${userId}/role`, { role });
}

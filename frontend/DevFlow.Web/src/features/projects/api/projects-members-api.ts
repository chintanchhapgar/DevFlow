import { projectApiClient } from "@/lib/api/project-api-client";

export interface ProjectMember {
  userId: string;
  role: string | null;
  joinedOnUtc: string;
}

export async function getProjectMembers(
  projectId: string,
): Promise<ProjectMember[]> {
  const response = await projectApiClient.get<
    ProjectMember[]
  >(`/api/projects/${projectId}/members`);

  return response.data;
}
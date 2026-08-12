import { projectApiClient } from "@/lib/api/project-api-client";

export interface GetProjectsParams {
  page?: number;
  pageSize?: number;
  search?: string;
  sortBy?: string;
  sortDirection?: "asc" | "desc";
}

export interface ProjectListItem {
  projectId: string;
  key: string;
  name: string;
  status: string;
  visibility: string;
  ownerId: string;
  memberCount: number;
}

export interface PagedProjects {
  items: ProjectListItem[];
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
}

export async function getProjects(
  params: GetProjectsParams = {},
): Promise<PagedProjects> {
  const {
    page = 1,
    pageSize = 20,
    search,
    sortBy,
    sortDirection,
  } = params;

  const response = await projectApiClient.get("/api/projects", {
    params: {
      page,
      pageSize,
      ...(search?.trim()
        ? { search: search.trim() }
        : {}),
      ...(sortBy ? { sortBy } : {}),
      ...(sortDirection ? { sortDirection } : {}),
    },
  });

  return response.data.data;
}
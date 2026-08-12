export interface Project {
  projectId: string;
  key: string;
  name: string;
  status: string;
  visibility: string;
  ownerId: string;
  memberCount: number;
}

export interface ProjectsResponse {
  items: Project[];
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
}
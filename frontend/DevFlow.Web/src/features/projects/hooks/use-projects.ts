import { useQuery } from "@tanstack/react-query";

import {
  getProjects,
  type GetProjectsParams,
} from "../api/projects-api";

export function useProjects(
  params: GetProjectsParams = {},
) {
  const {
    page = 1,
    pageSize = 20,
    search,
    sortBy,
    sortDirection,
  } = params;

  return useQuery({
    queryKey: [
      "projects",
      {
        page,
        pageSize,
        search: search ?? "",
        sortBy: sortBy ?? "",
        sortDirection: sortDirection ?? "",
      },
    ],

    queryFn: () =>
      getProjects({
        page,
        pageSize,
        search,
        sortBy,
        sortDirection,
      }),

    placeholderData: (previousData) =>
      previousData,

    staleTime: 30_000,
  });
}
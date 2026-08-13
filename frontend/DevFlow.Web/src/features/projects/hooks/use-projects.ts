import { useQuery } from "@tanstack/react-query";

import {
  getProjects,
  type GetProjectsParams,
} from "../api/projects-api";

import { projectKeys } from "./use-project";

export function useProjects(
  params: GetProjectsParams = {},
) {
  return useQuery({
    queryKey: projectKeys.list(params),

    queryFn: () => getProjects(params),

    staleTime: 30_000,

    refetchOnWindowFocus: false,
  });
}
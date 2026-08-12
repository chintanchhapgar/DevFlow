import {
  useMutation,
  useQuery,
  useQueryClient,
} from "@tanstack/react-query";

import {
  getSessions,
  revokeOtherSessions,
  revokeSession,
} from "../api/sessions-api";

export const sessionsQueryKey = [
  "auth",
  "sessions",
] as const;

export function useSessions() {
  return useQuery({
    queryKey: sessionsQueryKey,
    queryFn: getSessions,
  });
}

export function useRevokeSession() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: revokeSession,

    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: sessionsQueryKey,
      });
    },
  });
}

export function useRevokeOtherSessions() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: revokeOtherSessions,

    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: sessionsQueryKey,
      });
    },
  });
}
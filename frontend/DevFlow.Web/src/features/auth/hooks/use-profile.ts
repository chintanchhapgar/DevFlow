import { useQuery } from "@tanstack/react-query";

import {
  getProfile,
  type UserProfile,
} from "@/features/auth/api/profile-api";

export function useProfile() {
  return useQuery<UserProfile>({
    queryKey: ["auth", "profile"],
    queryFn: getProfile,
    retry: false,
  });
}
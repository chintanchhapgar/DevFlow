import { useState } from "react";
import { ShieldCheck, Users } from "lucide-react";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";

import { Button } from "@/components/ui/button";
import { getUsers, updateUserRole } from "../api/users-api";

const roleOptions = [
  { label: "Member", value: 1 },
  { label: "Project Manager", value: 2 },
  { label: "Administrator", value: 3 },
  { label: "System Administrator", value: 4 },
] as const;

function roleValue(role: string): number {
  return roleOptions.find(
    (option) => option.label.replaceAll(" ", "") === role,
  )?.value ?? 1;
}

export function UsersPage() {
  const queryClient = useQueryClient();
  const [error, setError] = useState<string | null>(null);
  const usersQuery = useQuery({ queryKey: ["users"], queryFn: getUsers });
  const updateRole = useMutation({
    mutationFn: ({ userId, role }: { userId: string; role: number }) =>
      updateUserRole(userId, role),
    onSuccess: async () => {
      setError(null);
      await queryClient.invalidateQueries({ queryKey: ["users"] });
    },
    onError: () => setError("Unable to update the user role."),
  });

  return (
    <div className="mx-auto w-full max-w-5xl space-y-6">
      <div className="flex items-center gap-3">
        <div className="flex h-10 w-10 items-center justify-center rounded-xl bg-slate-50 text-slate-600 ring-1 ring-slate-200">
          <ShieldCheck className="h-5 w-5" />
        </div>
        <div>
          <h1 className="text-2xl font-semibold tracking-tight text-slate-900">User management</h1>
          <p className="mt-1 text-sm text-slate-500">Assign system roles for workspace users.</p>
        </div>
      </div>

      <section className="overflow-hidden rounded-2xl border border-slate-200 bg-white shadow-sm">
        {error && <p className="m-5 rounded-lg bg-red-50 p-3 text-sm text-red-700">{error}</p>}
        {usersQuery.isLoading && <p className="p-5 text-sm text-slate-500">Loading users…</p>}
        {usersQuery.isError && <div className="p-5"><p className="text-sm text-red-700">Unable to load users.</p><Button className="mt-3" size="sm" variant="outline" onClick={() => usersQuery.refetch()}>Try again</Button></div>}
        {usersQuery.data && <div className="divide-y divide-slate-100">
          {usersQuery.data.users.map((user) => (
            <div key={user.id} className="flex flex-col gap-3 px-5 py-4 sm:flex-row sm:items-center sm:justify-between">
              <div className="flex items-center gap-3"><Users className="h-5 w-5 text-slate-400" /><div><p className="text-sm font-medium text-slate-900">{user.fullName}</p><p className="text-xs text-slate-500">{user.email}</p></div></div>
              <select aria-label={`Role for ${user.fullName}`} value={roleValue(user.role)} disabled={updateRole.isPending} onChange={(event) => updateRole.mutate({ userId: user.id, role: Number(event.target.value) })} className="h-9 rounded-lg border border-slate-200 bg-white px-3 text-sm text-slate-700">
                {roleOptions.map((option) => <option key={option.value} value={option.value}>{option.label}</option>)}
              </select>
            </div>
          ))}
        </div>}
      </section>
    </div>
  );
}

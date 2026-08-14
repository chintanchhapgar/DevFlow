import {
  useMutation,
  useQuery,
  useQueryClient,
} from "@tanstack/react-query";

import {
  assignWorkItem,
  changeWorkItemPriority,
  changeWorkItemStatus,
  createWorkItem,
  deleteAttachment,
  deleteWorkItem,
  getAttachments,
  getWorkItems,
  updateWorkItem,
  uploadAttachment,
  moveWorkItemToSprint,
  type CreateWorkItemRequest,
  type GetWorkItemsParams,
  type UpdateWorkItemRequest,
  type WorkItemPriority,
  type WorkItemStatus,
} from "../api/project-resources-api";

import { projectKeys } from "./use-project";

export const projectResourceKeys = {
  all: ["project-resources"] as const,

  workItems: (projectId: string) =>
    [
      ...projectResourceKeys.all,
      "work-items",
      projectId,
    ] as const,

  workItemList: (
    projectId: string,
    params: GetWorkItemsParams,
  ) =>
    [
      ...projectResourceKeys.workItems(projectId),
      params,
    ] as const,

  attachments: (workItemId: string) =>
    [
      ...projectResourceKeys.all,
      "attachments",
      workItemId,
    ] as const,
};

export function useProjectWorkItems(
  projectId: string | undefined,
  params: GetWorkItemsParams = {},
) {
  return useQuery({
    queryKey: projectId
      ? projectResourceKeys.workItemList(
          projectId,
          params,
        )
      : [...projectResourceKeys.all, "work-items"],

    queryFn: () => {
      if (!projectId) {
        throw new Error("Project ID is required.");
      }

      return getWorkItems(projectId, params);
    },

    enabled: Boolean(projectId),
    staleTime: 30_000,
    refetchOnWindowFocus: false,
  });
}

export function useWorkItemAttachments(
  workItemId: string | undefined,
) {
  return useQuery({
    queryKey: workItemId
      ? projectResourceKeys.attachments(workItemId)
      : [...projectResourceKeys.all, "attachments"],

    queryFn: () => {
      if (!workItemId) {
        throw new Error("Work item ID is required.");
      }

      return getAttachments(workItemId);
    },

    enabled: Boolean(workItemId),
    staleTime: 30_000,
    refetchOnWindowFocus: false,
  });
}

function invalidateProjectWorkItems(
  queryClient: ReturnType<typeof useQueryClient>,
  projectId: string,
) {
  return Promise.all([
    queryClient.invalidateQueries({
      queryKey: projectResourceKeys.workItems(projectId),
    }),

    queryClient.invalidateQueries({
      queryKey: projectKeys.detail(projectId),
    }),
  ]);
}

export function useCreateWorkItem() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({
      projectId,
      request,
    }: {
      projectId: string;
      request: CreateWorkItemRequest;
    }) => createWorkItem(projectId, request),

    onSuccess: (_, variables) =>
      invalidateProjectWorkItems(
        queryClient,
        variables.projectId,
      ),
  });
}

export function useUpdateWorkItem() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({
      projectId,
      workItemId,
      request,
    }: {
      projectId: string;
      workItemId: string;
      request: UpdateWorkItemRequest;
    }) => updateWorkItem(workItemId, request),

    onSuccess: (_, variables) =>
      invalidateProjectWorkItems(
        queryClient,
        variables.projectId,
      ),
  });
}

export function useDeleteWorkItem() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({
      projectId,
      workItemId,
    }: {
      projectId: string;
      workItemId: string;
    }) => deleteWorkItem(workItemId),

    onSuccess: (_, variables) =>
      Promise.all([
        invalidateProjectWorkItems(
          queryClient,
          variables.projectId,
        ),

        queryClient.removeQueries({
          queryKey: projectResourceKeys.attachments(
            variables.workItemId,
          ),
        }),
      ]),
  });
}

export function useChangeWorkItemStatus() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({
      projectId,
      workItemId,
      status,
    }: {
      projectId: string;
      workItemId: string;
      status: WorkItemStatus;
    }) => changeWorkItemStatus(workItemId, status),

    onSuccess: (_, variables) =>
      invalidateProjectWorkItems(
        queryClient,
        variables.projectId,
      ),
  });
}

export function useChangeWorkItemPriority() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({
      projectId,
      workItemId,
      priority,
    }: {
      projectId: string;
      workItemId: string;
      priority: WorkItemPriority;
    }) => changeWorkItemPriority(workItemId, priority),

    onSuccess: (_, variables) =>
      invalidateProjectWorkItems(
        queryClient,
        variables.projectId,
      ),
  });
}

export function useAssignWorkItem() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({
      projectId,
      workItemId,
      assigneeId,
    }: {
      projectId: string;
      workItemId: string;
      assigneeId: string;
    }) => assignWorkItem(workItemId, assigneeId),

    onSuccess: (_, variables) =>
      invalidateProjectWorkItems(
        queryClient,
        variables.projectId,
      ),
  });
}

export function useUploadAttachment() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({
      workItemId,
      file,
    }: {
      workItemId: string;
      file: File;
    }) => uploadAttachment(workItemId, file),

    onSuccess: (_, variables) =>
      queryClient.invalidateQueries({
        queryKey: projectResourceKeys.attachments(
          variables.workItemId,
        ),
      }),
  });
}

export function useDeleteAttachment() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({
      workItemId,
      attachmentId,
    }: {
      workItemId: string;
      attachmentId: string;
    }) => deleteAttachment(attachmentId),

    onSuccess: (_, variables) =>
      queryClient.invalidateQueries({
        queryKey: projectResourceKeys.attachments(
          variables.workItemId,
        ),
      }),
  });
}
export function useMoveWorkItemToSprint() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({
      projectId,
      workItemId,
      sprintId,
    }: {
      projectId: string;
      workItemId: string;
      sprintId: string;
    }) => moveWorkItemToSprint(workItemId, sprintId),

    onSuccess: (_, variables) =>
      invalidateProjectWorkItems(
        queryClient,
        variables.projectId,
      ),
  });
}
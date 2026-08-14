import { projectApiClient } from "@/lib/api/project-api-client";


export const WorkItemStatus = {
  Todo: 1,
  InProgress: 2,
  InReview: 3,
  Testing: 4,
  Done: 5,
  Cancelled: 6,
} as const;

export type WorkItemStatus =
  (typeof WorkItemStatus)[keyof typeof WorkItemStatus];

export const WorkItemType = {
  Task: 1,
  Bug: 2,
  Story: 3,
  Epic: 4,
  Subtask: 5,
} as const;

export type WorkItemType =
  (typeof WorkItemType)[keyof typeof WorkItemType];

export const WorkItemPriority = {
  Lowest: 1,
  Low: 2,
  Medium: 3,
  High: 4,
  Highest: 5,
} as const;

export type WorkItemPriority =
  (typeof WorkItemPriority)[keyof typeof WorkItemPriority];

export interface WorkItem {
  id: string;
  projectId?: string;
  key: string;
  title: string;
  description?: string | null;
  type: string | number;
  status: string | number;
  priority: string | number;
  assigneeId?: string | null;
  reporterId?: string;
  epicId?: string | null;
  parentId?: string | null;
  sprintId?: string | null;
  estimateHours?: number | null;
  dueDate?: string | null;
  createdOnUtc?: string;
  updatedOnUtc?: string | null;
}

export interface PagedWorkItems {
  items: WorkItem[];
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
}

export interface GetWorkItemsParams {
  page?: number;
  pageSize?: number;
  search?: string;
  status?: WorkItemStatus;
  type?: WorkItemType;
  priority?: WorkItemPriority;
  assigneeId?: string;
}

export interface CreateWorkItemRequest {
  title: string;
  description?: string | null;
  type: WorkItemType;
  priority: WorkItemPriority;
  assigneeId?: string | null;
  dueDate?: string | null;
  estimateHours?: number | null;
}

export interface UpdateWorkItemRequest {
  title: string;
  description?: string | null;
  dueDate?: string | null;
  estimateHours?: number | null;
}

export async function getWorkItems(
  projectId: string,
  params: GetWorkItemsParams = {},
): Promise<PagedWorkItems> {
  const response = await projectApiClient.get(
    `/api/projects/${projectId}/work-items`,
    {
      params: {
        page: params.page ?? 1,
        pageSize: params.pageSize ?? 50,
        search: params.search?.trim() || undefined,
        status: params.status,
        type: params.type,
        priority: params.priority,
        assigneeId: params.assigneeId,
      },
    },
  );

  return response.data.data;
}

export async function getWorkItem(
  workItemId: string,
): Promise<WorkItem> {
  const response = await projectApiClient.get(
    `/api/work-items/${workItemId}`,
  );

  return response.data.data;
}

export async function createWorkItem(
  projectId: string,
  request: CreateWorkItemRequest,
): Promise<{ workItemId: string; key: string; title: string }> {
  const response = await projectApiClient.post(
    `/api/projects/${projectId}/work-items`,
    request,
  );

  return response.data.data;
}

export async function updateWorkItem(
  workItemId: string,
  request: UpdateWorkItemRequest,
): Promise<{ workItemId: string; key: string; title: string }> {
  const response = await projectApiClient.put(
    `/api/work-items/${workItemId}`,
    request,
  );

  return response.data.data;
}

export async function changeWorkItemStatus(
  workItemId: string,
  status: WorkItemStatus,
): Promise<void> {
  await projectApiClient.put(
    `/api/work-items/${workItemId}/status`,
    { status },
  );
}

export async function changeWorkItemPriority(
  workItemId: string,
  priority: WorkItemPriority,
): Promise<void> {
  await projectApiClient.put(
    `/api/work-items/${workItemId}/priority`,
    { priority },
  );
}

export async function assignWorkItem(
  workItemId: string,
  assigneeId: string,
): Promise<void> {
  await projectApiClient.put(
    `/api/work-items/${workItemId}/assign`,
    { assigneeId },
  );
}

export async function deleteWorkItem(
  workItemId: string,
): Promise<void> {
  await projectApiClient.delete(
    `/api/work-items/${workItemId}`,
  );
}

export interface Attachment {
  attachmentId: string;
  originalFileName: string;
  contentType: string;
  extension: string;
  sizeInBytes: number;
  createdOnUtc: string;
  uploadedBy: string;
}

export async function getAttachments(
  workItemId: string,
): Promise<Attachment[]> {
  const response = await projectApiClient.get(
    `/api/work-items/${workItemId}/attachments`,
  );

  return response.data.data;
}

export async function uploadAttachment(
  workItemId: string,
  file: File,
): Promise<Attachment> {
  const body = new FormData();
  body.append("file", file);

  const response = await projectApiClient.post(
    `/api/work-items/${workItemId}/attachments`,
    body,
    {
      headers: {
        "Content-Type": "multipart/form-data",
      },
    },
  );

  return response.data.data;
}

export async function deleteAttachment(
  attachmentId: string,
): Promise<void> {
  await projectApiClient.delete(
    `/api/attachments/${attachmentId}`,
  );
}

export async function downloadAttachment(
  attachmentId: string,
  fileName: string,
): Promise<void> {
  const response = await projectApiClient.get(
    `/api/attachments/${attachmentId}`,
    { responseType: "blob" },
  );

  const url = URL.createObjectURL(response.data);
  const link = document.createElement("a");

  link.href = url;
  link.download = fileName;
  document.body.appendChild(link);
  link.click();
  link.remove();

  URL.revokeObjectURL(url);
}

export async function moveWorkItemToSprint(
  workItemId: string,
  sprintId: string,
): Promise<void> {
  await projectApiClient.put(
    `/api/work-items/${workItemId}/sprint`,
    { sprintId },
  );
}
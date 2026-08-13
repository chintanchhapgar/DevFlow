import {
  useRef,
  useState,
} from "react";
import {
  Download,
  FileIcon,
  LoaderCircle,
  Paperclip,
  Trash2,
  Upload,
} from "lucide-react";

import { Button } from "@/components/ui/button";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";

import { downloadAttachment } from "../api/project-resources-api";
import {
  useDeleteAttachment,
  useUploadAttachment,
  useWorkItemAttachments,
} from "../hooks/use-project-resources";

type AttachmentsPanelProps = {
  workItemId: string | undefined;
  workItemTitle?: string;
};

function formatBytes(bytes: number) {
  if (bytes < 1024) {
    return `${bytes} B`;
  }

  if (bytes < 1024 * 1024) {
    return `${(bytes / 1024).toFixed(1)} KB`;
  }

  return `${(bytes / (1024 * 1024)).toFixed(1)} MB`;
}

function formatDate(value: string) {
  return new Intl.DateTimeFormat(undefined, {
    dateStyle: "medium",
  }).format(new Date(value));
}

export function AttachmentsPanel({
  workItemId,
  workItemTitle,
}: AttachmentsPanelProps) {
  const fileInputRef = useRef<HTMLInputElement>(null);

  const attachmentsQuery = useWorkItemAttachments(
    workItemId,
  );
  const uploadAttachment = useUploadAttachment();
  const deleteAttachment = useDeleteAttachment();

  const [error, setError] = useState<string | null>(null);
  const [attachmentToDelete, setAttachmentToDelete] =
    useState<{
      attachmentId: string;
      originalFileName: string;
    } | null>(null);

  const isBusy =
    uploadAttachment.isPending ||
    deleteAttachment.isPending;

  function chooseFile() {
    fileInputRef.current?.click();
  }

  async function handleFileChange(
    event: React.ChangeEvent<HTMLInputElement>,
  ) {
    const file = event.target.files?.[0];

    event.target.value = "";

    if (!file || !workItemId) {
      return;
    }

    setError(null);

    try {
      await uploadAttachment.mutateAsync({
        workItemId,
        file,
      });
    } catch {
      setError(
        "Unable to upload this attachment. Please try again.",
      );
    }
  }

  async function handleDownload(
    attachmentId: string,
    fileName: string,
  ) {
    setError(null);

    try {
      await downloadAttachment(attachmentId, fileName);
    } catch {
      setError("Unable to download this attachment.");
    }
  }

  async function handleDelete() {
    if (!workItemId || !attachmentToDelete) {
      return;
    }

    setError(null);

    try {
      await deleteAttachment.mutateAsync({
        workItemId,
        attachmentId: attachmentToDelete.attachmentId,
      });

      setAttachmentToDelete(null);
    } catch {
      setError("Unable to delete this attachment.");
    }
  }

  if (!workItemId) {
    return (
      <section className="rounded-2xl border border-slate-200 bg-white p-10 text-center">
        <Paperclip className="mx-auto h-8 w-8 text-slate-400" />

        <h2 className="mt-3 text-base font-semibold text-slate-900">
          Select a work item
        </h2>

        <p className="mt-1 text-sm text-slate-500">
          Choose a work item in the Work tab to view and manage
          its attachments.
        </p>
      </section>
    );
  }

  if (attachmentsQuery.isLoading) {
    return (
      <section className="rounded-2xl border border-slate-200 bg-white p-5">
        <div className="space-y-3">
          {[0, 1, 2].map((index) => (
            <div
              key={index}
              className="h-14 animate-pulse rounded-lg bg-slate-100"
            />
          ))}
        </div>
      </section>
    );
  }

  if (attachmentsQuery.isError) {
    return (
      <section className="rounded-2xl border border-red-200 bg-red-50 p-5">
        <p className="font-medium text-red-800">
          Unable to load attachments.
        </p>

        <p className="mt-1 text-sm text-red-700">
          Please try again.
        </p>

        <Button
          type="button"
          variant="outline"
          size="sm"
          className="mt-4"
          onClick={() => attachmentsQuery.refetch()}
        >
          Try again
        </Button>
      </section>
    );
  }

  const attachments = attachmentsQuery.data ?? [];

  return (
    <>
      <section className="overflow-hidden rounded-2xl border border-slate-200 bg-white">
        <div className="flex flex-col gap-3 border-b border-slate-100 px-5 py-4 sm:flex-row sm:items-center sm:justify-between">
          <div>
            <h2 className="text-base font-semibold text-slate-900">
              Attachments
            </h2>

            <p className="mt-1 text-sm text-slate-500">
              {workItemTitle
                ? `Files attached to ${workItemTitle}.`
                : "Files attached to this work item."}
            </p>
          </div>

          <Button
            type="button"
            size="sm"
            disabled={isBusy}
            onClick={chooseFile}
          >
            {uploadAttachment.isPending ? (
              <LoaderCircle className="h-4 w-4 animate-spin" />
            ) : (
              <Upload className="h-4 w-4" />
            )}
            Upload file
          </Button>

          <input
            ref={fileInputRef}
            type="file"
            className="hidden"
            onChange={handleFileChange}
          />
        </div>

        {error && (
          <div className="border-b border-red-100 bg-red-50 px-5 py-3 text-sm text-red-700">
            {error}
          </div>
        )}

        {attachments.length === 0 ? (
          <div className="px-5 py-12 text-center">
            <Paperclip className="mx-auto h-8 w-8 text-slate-400" />

            <h3 className="mt-3 text-sm font-semibold text-slate-900">
              No attachments yet
            </h3>

            <p className="mt-1 text-sm text-slate-500">
              Upload a file to keep useful context with this work
              item.
            </p>

            <Button
              type="button"
              variant="outline"
              size="sm"
              className="mt-4"
              disabled={isBusy}
              onClick={chooseFile}
            >
              <Upload className="h-4 w-4" />
              Upload file
            </Button>
          </div>
        ) : (
          <div className="divide-y divide-slate-100">
            {attachments.map((attachment) => (
              <div
                key={attachment.attachmentId}
                className="flex items-center gap-3 px-5 py-3"
              >
                <div className="flex h-9 w-9 shrink-0 items-center justify-center rounded-lg bg-slate-100 text-slate-500">
                  <FileIcon className="h-4 w-4" />
                </div>

                <div className="min-w-0 flex-1">
                  <p className="truncate text-sm font-medium text-slate-800">
                    {attachment.originalFileName}
                  </p>

                  <p className="mt-0.5 text-xs text-slate-500">
                    {formatBytes(attachment.sizeInBytes)}
                    {" · "}
                    {formatDate(attachment.createdOnUtc)}
                  </p>
                </div>

                <Button
                  type="button"
                  variant="ghost"
                  size="icon"
                  disabled={isBusy}
                  aria-label={`Download ${attachment.originalFileName}`}
                  onClick={() =>
                    handleDownload(
                      attachment.attachmentId,
                      attachment.originalFileName,
                    )
                  }
                >
                  <Download className="h-4 w-4" />
                </Button>

                <Button
                  type="button"
                  variant="ghost"
                  size="icon"
                  disabled={isBusy}
                  aria-label={`Delete ${attachment.originalFileName}`}
                  onClick={() =>
                    setAttachmentToDelete(attachment)
                  }
                >
                  <Trash2 className="h-4 w-4 text-red-600" />
                </Button>
              </div>
            ))}
          </div>
        )}
      </section>

      <Dialog
        open={Boolean(attachmentToDelete)}
        onOpenChange={(open) => {
          if (!open) {
            setAttachmentToDelete(null);
          }
        }}
      >
        <DialogContent>
          <DialogHeader>
            <DialogTitle>Delete attachment?</DialogTitle>

            <DialogDescription>
              {attachmentToDelete
                ? `${attachmentToDelete.originalFileName} will be permanently removed.`
                : ""}
            </DialogDescription>
          </DialogHeader>

          <DialogFooter>
            <Button
              type="button"
              variant="outline"
              disabled={isBusy}
              onClick={() => setAttachmentToDelete(null)}
            >
              Cancel
            </Button>

            <Button
              type="button"
              variant="destructive"
              disabled={isBusy}
              onClick={handleDelete}
            >
              {deleteAttachment.isPending && (
                <LoaderCircle className="h-4 w-4 animate-spin" />
              )}
              Delete
            </Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>
    </>
  );
}
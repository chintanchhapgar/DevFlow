export interface Session {
  sessionId: string;
  deviceName: string | null;
  browser: string | null;
  operatingSystem: string | null;
  ipAddress: string | null;
  userAgent: string | null;
  createdAtUtc: string;
  lastUsedAtUtc: string | null;
  isCurrent: boolean;
}
import { useEffect, useState } from "react";
import {
  Bell,
  Check,
  Clock3,
  LayoutPanelTop,
  Mail,
  MonitorCog,
} from "lucide-react";

import { Button } from "@/components/ui/button";

type Settings = {
  emailNotifications: boolean;
  assignmentNotifications: boolean;
  dueDateReminders: boolean;
  compactLayout: boolean;
  startOfWeek: "monday" | "sunday";
  timezone: string;
};

const storageKey = "devflow-settings";

const defaultSettings: Settings = {
  emailNotifications: true,
  assignmentNotifications: true,
  dueDateReminders: true,
  compactLayout: false,
  startOfWeek: "monday",
  timezone: Intl.DateTimeFormat().resolvedOptions().timeZone || "UTC",
};

function readSettings(): Settings {
  try {
    const saved = localStorage.getItem(storageKey);

    if (!saved) {
      return defaultSettings;
    }

    return {
      ...defaultSettings,
      ...JSON.parse(saved),
    };
  } catch {
    return defaultSettings;
  }
}

export function SettingsPage() {
  const [settings, setSettings] = useState<Settings>(readSettings);
  const [isSaved, setIsSaved] = useState(false);

  useEffect(() => {
    document.documentElement.dataset.layout = settings.compactLayout
      ? "compact"
      : "comfortable";
  }, [settings.compactLayout]);

  function updateSetting<Key extends keyof Settings>(
    key: Key,
    value: Settings[Key],
  ) {
    setIsSaved(false);
    setSettings((current) => ({
      ...current,
      [key]: value,
    }));
  }

  function saveSettings() {
    localStorage.setItem(storageKey, JSON.stringify(settings));
    setIsSaved(true);
  }

  return (
    <div className="mx-auto w-full max-w-4xl space-y-6">
      <div>
        <p className="text-sm font-medium text-[var(--devflow-primary)]">
          Account
        </p>

        <h1 className="mt-1 text-2xl font-semibold tracking-tight text-slate-900">
          Settings
        </h1>

        <p className="mt-1.5 text-sm text-slate-500">
          Customize how DevFlow works for you.
        </p>
      </div>

      <section className="overflow-hidden rounded-2xl border border-slate-200 bg-white shadow-sm">
        <SectionHeading
          icon={Bell}
          title="Notifications"
          description="Choose which workspace updates you want to receive."
        />

        <div className="divide-y divide-slate-100">
          <SettingToggle
            title="Email notifications"
            description="Receive important workspace updates by email."
            checked={settings.emailNotifications}
            onCheckedChange={(checked) =>
              updateSetting("emailNotifications", checked)
            }
          />

          <SettingToggle
            title="Assignment updates"
            description="Get notified when work is assigned or reassigned to you."
            checked={settings.assignmentNotifications}
            onCheckedChange={(checked) =>
              updateSetting("assignmentNotifications", checked)
            }
          />

          <SettingToggle
            title="Due date reminders"
            description="Receive a reminder before work assigned to you is due."
            checked={settings.dueDateReminders}
            onCheckedChange={(checked) =>
              updateSetting("dueDateReminders", checked)
            }
          />
        </div>
      </section>

      <section className="overflow-hidden rounded-2xl border border-slate-200 bg-white shadow-sm">
        <SectionHeading
          icon={LayoutPanelTop}
          title="Workspace"
          description="Set your preferred workspace layout."
        />

        <div className="divide-y divide-slate-100">
          <SettingToggle
            title="Compact layout"
            description="Use tighter spacing in workspace lists and panels."
            checked={settings.compactLayout}
            onCheckedChange={(checked) =>
              updateSetting("compactLayout", checked)
            }
          />

          <div className="flex flex-col gap-3 px-5 py-4 sm:flex-row sm:items-center sm:justify-between">
            <div>
              <p className="text-sm font-medium text-slate-800">
                Start of week
              </p>

              <p className="mt-1 text-xs text-slate-500">
                Used when dates and schedules are displayed.
              </p>
            </div>

            <select
              value={settings.startOfWeek}
              onChange={(event) =>
                updateSetting(
                  "startOfWeek",
                  event.target.value as Settings["startOfWeek"],
                )
              }
              className="h-10 rounded-lg border border-slate-200 bg-white px-3 text-sm text-slate-700 outline-none focus:border-slate-400 focus:ring-2 focus:ring-slate-200"
            >
              <option value="monday">Monday</option>
              <option value="sunday">Sunday</option>
            </select>
          </div>
        </div>
      </section>

      <section className="overflow-hidden rounded-2xl border border-slate-200 bg-white shadow-sm">
        <SectionHeading
          icon={Clock3}
          title="Regional preferences"
          description="Control how dates and times are shown."
        />

        <div className="px-5 py-4">
          <label className="block text-sm font-medium text-slate-800">
            Time zone

            <select
              value={settings.timezone}
              onChange={(event) =>
                updateSetting("timezone", event.target.value)
              }
              className="mt-2 h-10 w-full max-w-md rounded-lg border border-slate-200 bg-white px-3 text-sm text-slate-700 outline-none focus:border-slate-400 focus:ring-2 focus:ring-slate-200"
            >
              <option value="Asia/Calcutta">India Standard Time</option>
              <option value="UTC">Coordinated Universal Time</option>
              <option value="Europe/London">London</option>
              <option value="America/New_York">New York</option>
              <option value="America/Los_Angeles">Los Angeles</option>
              <option value="Asia/Singapore">Singapore</option>
              <option value="Australia/Sydney">Sydney</option>
            </select>
          </label>
        </div>
      </section>

      <div className="flex items-center justify-end gap-3">
        {isSaved && (
          <span className="inline-flex items-center gap-1.5 text-sm font-medium text-emerald-600">
            <Check className="h-4 w-4" />
            Settings saved
          </span>
        )}

        <Button type="button" onClick={saveSettings}>
          Save changes
        </Button>
      </div>
    </div>
  );
}

function SectionHeading({
  icon: Icon,
  title,
  description,
}: {
  icon: React.ComponentType<{ className?: string }>;
  title: string;
  description: string;
}) {
  return (
    <div className="flex items-start gap-3 border-b border-slate-100 px-5 py-4">
      <div className="flex h-9 w-9 shrink-0 items-center justify-center rounded-xl bg-slate-50 text-slate-500">
        <Icon className="h-4 w-4" />
      </div>

      <div>
        <h2 className="text-sm font-semibold text-slate-900">{title}</h2>
        <p className="mt-1 text-xs text-slate-500">{description}</p>
      </div>
    </div>
  );
}

function SettingToggle({
  title,
  description,
  checked,
  onCheckedChange,
}: {
  title: string;
  description: string;
  checked: boolean;
  onCheckedChange: (checked: boolean) => void;
}) {
  return (
    <div className="flex items-center justify-between gap-5 px-5 py-4">
      <div>
        <p className="text-sm font-medium text-slate-800">{title}</p>
        <p className="mt-1 text-xs leading-5 text-slate-500">
          {description}
        </p>
      </div>

      <button
        type="button"
        role="switch"
        aria-checked={checked}
        aria-label={title}
        onClick={() => onCheckedChange(!checked)}
        className={`relative h-6 w-11 shrink-0 rounded-full transition-colors ${
          checked ? "bg-[var(--devflow-primary)]" : "bg-slate-200"
        }`}
      >
        <span
        className={`absolute left-0.5 top-0.5 h-5 w-5 rounded-full bg-white shadow-sm transition-transform ${
            checked ? "translate-x-5" : "translate-x-0"
        }`}
        />
      </button>
    </div>
  );
}
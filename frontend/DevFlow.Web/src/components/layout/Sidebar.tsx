import {
  Activity,
  FolderKanban,
  LayoutDashboard,
  Settings,
  Shield,
  User,
  Workflow,
} from "lucide-react";
import { NavLink } from "react-router-dom";

const navigation = [
  {
    label: "Dashboard",
    path: "/",
    icon: LayoutDashboard,
  },
  {
    label: "Projects",
    path: "/projects",
    icon: FolderKanban,
  },
  {
    label: "Work",
    path: "/work",
    icon: Workflow,
  },
  {
    label: "Activity",
    path: "/activity",
    icon: Activity,
  },
];

const accountNavigation = [
  {
    label: "Profile",
    path: "/profile",
    icon: User,
  },
  {
    label: "Security",
    path: "/security",
    icon: Shield,
  },
  {
    label: "Settings",
    path: "/settings",
    icon: Settings,
  },
];

function NavigationItem({
  label,
  path,
  icon: Icon,
}: {
  label: string;
  path: string;
  icon: React.ComponentType<{
    className?: string;
  }>;
}) {
  return (
    <NavLink
      to={path}
      end={path === "/"}
      className={({ isActive }) =>
        [
          "group relative flex items-center gap-3 rounded-lg px-3 py-2.5 text-sm transition-all duration-150",
          isActive
            ? "bg-blue-50 text-blue-700"
            : "text-slate-600 hover:bg-slate-50 hover:text-slate-900",
        ].join(" ")
      }
    >
      {({ isActive }) => (
        <>
          {isActive && (
            <span className="absolute left-0 top-1/2 h-5 w-0.5 -translate-y-1/2 rounded-full bg-blue-600" />
          )}

          <Icon
            className={[
              "h-4 w-4 shrink-0",
              isActive
                ? "text-blue-600"
                : "text-slate-400 group-hover:text-slate-600",
            ].join(" ")}
          />

          <span className="font-medium">
            {label}
          </span>
        </>
      )}
    </NavLink>
  );
}

export function Sidebar() {
  return (
    <aside className="hidden w-64 shrink-0 border-r border-slate-200 bg-white md:flex md:flex-col">

      {/* Brand */}
      <div className="flex h-16 items-center border-b border-slate-200 px-5">
        <div className="flex items-center gap-2.5">
          <div className="flex h-7 w-7 items-center justify-center rounded-lg bg-blue-600 text-sm font-bold text-white shadow-sm">
            ◆
          </div>

          <span className="text-sm font-bold tracking-tight text-slate-900">
            DEVFLOW
          </span>
        </div>
      </div>

      {/* Navigation */}
      <nav className="flex-1 px-3 py-5">

        <div className="mb-2 px-3 text-[10px] font-semibold uppercase tracking-[0.16em] text-slate-400">
          Workspace
        </div>

        <div className="space-y-1">
          {navigation.map((item) => (
            <NavigationItem
              key={item.path}
              {...item}
            />
          ))}
        </div>

        <div className="my-6 h-px bg-slate-200" />

        <div className="mb-2 px-3 text-[10px] font-semibold uppercase tracking-[0.16em] text-slate-400">
          Account
        </div>

        <div className="space-y-1">
          {accountNavigation.map((item) => (
            <NavigationItem
              key={item.path}
              {...item}
            />
          ))}
        </div>
      </nav>

      {/* Workspace footer */}
      <div className="border-t border-slate-200 p-3">
        <div className="flex items-center gap-3 rounded-lg px-3 py-2">
          <div className="flex h-8 w-8 items-center justify-center rounded-full bg-blue-50 text-xs font-semibold text-blue-600">
            D
          </div>

          <div className="min-w-0">
            <p className="truncate text-xs font-medium text-slate-700">
              DevFlow Workspace
            </p>

            <p className="truncate text-[11px] text-slate-400">
              Personal workspace
            </p>
          </div>
        </div>
      </div>
    </aside>
  );
}
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
          "group flex items-center gap-3 rounded-lg px-3 py-2.5 text-sm font-medium transition-colors",
          isActive
            ? "bg-[#eef3f8] text-[#456b9a]"
            : "text-slate-500 hover:bg-slate-50 hover:text-slate-800",
        ].join(" ")
      }
    >
      <Icon className="h-[18px] w-[18px] shrink-0" />

      <span>{label}</span>
    </NavLink>
  );
}

export function Sidebar() {
  return (
    <aside
      className="
        hidden
        w-64
        shrink-0
        border-r
        border-slate-200
        bg-white
        md:flex
        md:flex-col
      "
    >
      {/* Brand */}
      <div
        className="
          flex
          h-16
          shrink-0
          items-center
          border-b
          border-slate-200
          px-5
        "
      >
        <NavLink
          to="/"
          className="flex items-center gap-3"
        >
          {/* Logo */}
          <div
            className="
              flex
              h-9
              w-9
              items-center
              justify-center
              rounded-lg
              bg-[#eef3f8]
              text-sm
              font-bold
              text-[#456b9a]
              ring-1
              ring-[#dbe4ed]
            "
          >
            ◆
          </div>

          {/* Brand Name */}
          <div>
            <div className="text-sm font-bold tracking-tight text-slate-900">
              DEVFLOW
            </div>

            <div className="mt-0.5 text-[11px] font-medium text-slate-400">
              Project Management
            </div>
          </div>
        </NavLink>
      </div>

      {/* Navigation */}
      <nav className="flex-1 overflow-y-auto px-3 py-5">
        {/* Main */}
        <div>
          <p className="mb-2 px-3 text-[10px] font-semibold uppercase tracking-wider text-slate-400">
            Workspace
          </p>

          <div className="space-y-1">
            {navigation.map((item) => (
              <NavigationItem
                key={item.path}
                {...item}
              />
            ))}
          </div>
        </div>

        {/* Divider */}
        <div className="my-6 border-t border-slate-100" />

        {/* Account */}
        <div>
          <p className="mb-2 px-3 text-[10px] font-semibold uppercase tracking-wider text-slate-400">
            Account
          </p>

          <div className="space-y-1">
            {accountNavigation.map((item) => (
              <NavigationItem
                key={item.path}
                {...item}
              />
            ))}
          </div>
        </div>
      </nav>

      {/* Bottom Status */}
      <div className="border-t border-slate-100 p-4">
        <div className="flex items-center gap-3 rounded-lg bg-slate-50 px-3 py-2.5">
          <span className="flex h-2 w-2 shrink-0 rounded-full bg-emerald-500" />

          <div className="min-w-0">
            <p className="text-xs font-medium text-slate-700">
              System Online
            </p>

            <p className="truncate text-[10px] text-slate-400">
              All services operational
            </p>
          </div>
        </div>
      </div>
    </aside>
  );
}
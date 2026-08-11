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
          "flex items-center gap-3 rounded-lg px-3 py-2 text-sm transition",
          isActive
            ? "bg-primary text-primary-foreground"
            : "text-slate-400 hover:bg-white/5 hover:text-white",
        ].join(" ")
      }
    >
      <Icon className="h-4 w-4" />
      {label}
    </NavLink>
  );
}

export function Sidebar() {
  return (
    <aside className="hidden w-64 shrink-0 border-r border-white/10 bg-slate-950 md:flex md:flex-col">
      <div className="flex h-16 items-center border-b border-white/10 px-6">
        <div>
          <div className="text-lg font-bold">
            DevFlow
          </div>

          <div className="text-xs text-slate-500">
            Project Management
          </div>
        </div>
      </div>

      <nav className="flex-1 space-y-1 p-4">
        {navigation.map((item) => (
          <NavigationItem
            key={item.path}
            {...item}
          />
        ))}

        <div className="my-5 border-t border-white/10" />

        {accountNavigation.map((item) => (
          <NavigationItem
            key={item.path}
            {...item}
          />
        ))}
      </nav>
    </aside>
  );
}
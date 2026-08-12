import {
  Bell,
  Search,
} from "lucide-react";

import { UserMenu } from "./UserMenu";

export function Header() {
  return (
    <header
      className="
        sticky
        top-0
        z-40
        h-16
        border-b
        border-slate-200
        bg-white/95
        backdrop-blur
      "
    >
      <div className="flex h-full items-center justify-between gap-4 px-4 sm:px-6">
        {/* Left */}
        <div className="flex min-w-0 flex-1 items-center gap-4">
          {/* Mobile / Page Context */}
          <div className="hidden sm:block">
            <p className="text-sm font-medium text-slate-700">
              Workspace
            </p>
          </div>

          {/* Search */}
          <div className="relative hidden w-full max-w-md md:block">
            <Search
              className="
                pointer-events-none
                absolute
                left-3
                top-1/2
                h-4
                w-4
                -translate-y-1/2
                text-slate-400
              "
            />

            <input
              type="search"
              placeholder="Search DevFlow..."
              className="
                h-9
                w-full
                rounded-lg
                border
                border-slate-200
                bg-slate-50
                pl-9
                pr-4
                text-sm
                text-slate-800
                outline-none
                placeholder:text-slate-400
                transition
                focus:border-[#9db2c8]
                focus:bg-white
                focus:ring-2
                focus:ring-[#456b9a]/10
              "
            />
          </div>
        </div>

        {/* Right */}
        <div className="flex shrink-0 items-center gap-2">
          {/* Notifications */}
          <button
            type="button"
            aria-label="Notifications"
            className="
              relative
              flex
              h-9
              w-9
              items-center
              justify-center
              rounded-lg
              text-slate-500
              transition-colors
              hover:bg-slate-50
              hover:text-slate-700
              focus:outline-none
              focus:ring-2
              focus:ring-[#456b9a]/10
            "
          >
            <Bell className="h-[18px] w-[18px]" />

            {/* Notification indicator */}
            <span
              className="
                absolute
                right-2
                top-2
                h-1.5
                w-1.5
                rounded-full
                bg-[#456b9a]
              "
            />
          </button>

          {/* Divider */}
          <div className="mx-1 hidden h-6 w-px bg-slate-200 sm:block" />

          {/* User */}
          <UserMenu />
        </div>
      </div>
    </header>
  );
}
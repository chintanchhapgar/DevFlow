import { Bell, Search } from "lucide-react";

import { UserMenu } from "@/components/layout/UserMenu";

export function Header() {
  return (
    <header className="sticky top-0 z-40 h-16 border-b border-slate-200 bg-white/95 backdrop-blur-xl">
      <div className="flex h-full items-center justify-between px-6">

        {/* Left */}
        <div className="flex items-center gap-4">
          <div className="hidden h-5 w-px bg-slate-200 md:block" />

          <div>
            <p className="text-sm font-medium text-slate-800">
              Workspace
            </p>

            <p className="text-[11px] text-slate-400">
              DevFlow
            </p>
          </div>
        </div>

        {/* Right */}
        <div className="flex items-center gap-2">

          {/* Search */}
          <button
            type="button"
            className="
              hidden
              h-9
              items-center
              gap-2
              rounded-lg
              border
              border-slate-200
              bg-slate-50
              px-3
              text-xs
              text-slate-500
              transition
              hover:border-slate-300
              hover:bg-white
              sm:flex
            "
          >
            <Search className="h-3.5 w-3.5" />

            <span>Search</span>

            <kbd className="ml-4 rounded border border-slate-200 bg-white px-1.5 py-0.5 text-[10px] text-slate-400">
              ⌘K
            </kbd>
          </button>

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
              transition
              hover:bg-slate-50
              hover:text-slate-900
            "
          >
            <Bell className="h-4 w-4" />

            <span className="absolute right-2 top-2 h-1.5 w-1.5 rounded-full bg-blue-600" />
          </button>

          <UserMenu />
        </div>
      </div>
    </header>
  );
}
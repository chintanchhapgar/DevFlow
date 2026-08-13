import { useEffect, useRef, useState } from "react";
import {
  LogOut,
  Shield,
  User,
} from "lucide-react";
import { useNavigate } from "react-router-dom";

import { useProfile } from "@/features/auth/hooks/use-profile";
import { logout } from "@/features/auth/api/logout-api";
import { authStorage } from "@/features/auth/auth-storage";

export function UserMenu() {
  const navigate = useNavigate();

  const { data: profile } = useProfile();

  const [isOpen, setIsOpen] = useState(false);
  const [isLoggingOut, setIsLoggingOut] = useState(false);

  const menuRef = useRef<HTMLDivElement>(null);

  /*
   * Close menu when clicking outside
   * or pressing Escape.
   */
  useEffect(() => {
    if (!isOpen) {
      return;
    }

    function handleClickOutside(event: MouseEvent) {
      if (
        menuRef.current &&
        !menuRef.current.contains(
          event.target as Node,
        )
      ) {
        setIsOpen(false);
      }
    }

    function handleKeyDown(event: KeyboardEvent) {
      if (event.key === "Escape") {
        setIsOpen(false);
      }
    }

    document.addEventListener(
      "mousedown",
      handleClickOutside,
    );

    document.addEventListener(
      "keydown",
      handleKeyDown,
    );

    return () => {
      document.removeEventListener(
        "mousedown",
        handleClickOutside,
      );

      document.removeEventListener(
        "keydown",
        handleKeyDown,
      );
    };
  }, [isOpen]);

  /*
   * Logout current session.
   */
  async function handleLogout() {
    if (isLoggingOut) {
      return;
    }

    setIsLoggingOut(true);

    const refreshToken =
      authStorage.getRefreshToken();

    try {
      if (refreshToken) {
        await logout(refreshToken);
      }
    } catch {
      /*
       * Local authentication must still be cleared
       * if the logout API request fails.
       */
    } finally {
      authStorage.clear();

      setIsOpen(false);

      navigate("/login", {
        replace: true,
      });

      setIsLoggingOut(false);
    }
  }

  function handleProfile() {
    setIsOpen(false);
    navigate("/profile");
  }

  function handleSecurity() {
    setIsOpen(false);
    navigate("/security");
  }

  const initials =
    profile?.firstName?.charAt(0).toUpperCase() ??
    "U";

  return (
    <div
      ref={menuRef}
      className="relative"
    >
      {/* Avatar Button */}
      <button
        type="button"
        onClick={() =>
          setIsOpen((value) => !value)
        }
        aria-label="Open user menu"
        aria-expanded={isOpen}
        aria-haspopup="menu"
        className="
          flex
          h-9
          w-9
          items-center
          justify-center
          rounded-full
          bg-[#eef3f8]
          text-sm
          font-semibold
          text-[#456b9a]
          ring-1
          ring-[#dbe4ed]
          transition
          hover:bg-[#e5edf5]
          focus:outline-none
          focus:ring-2
          focus:ring-[#456b9a]/20
        "
      >
        {initials}
      </button>

      {/* Dropdown */}
      {isOpen && (
        <div
          role="menu"
          className="
            absolute
            right-0
            top-12
            z-50
            w-72
            overflow-hidden
            rounded-xl
            border
            border-slate-200
            bg-white
            shadow-xl
            shadow-slate-900/10
          "
        >
          {/* User Information */}
          <div
            className="
              border-b
              border-slate-100
              bg-slate-50/70
              px-4
              py-4
            "
          >
            <div className="flex items-center gap-3">
              {/* Avatar */}
              <div
                className="
                  flex
                  h-11
                  w-11
                  shrink-0
                  items-center
                  justify-center
                  rounded-full
                  bg-[#eef3f8]
                  text-sm
                  font-semibold
                  text-[#456b9a]
                  ring-1
                  ring-[#dbe4ed]
                "
              >
                {initials}
              </div>

              {/* User Details */}
              <div className="min-w-0">
                <p className="truncate text-sm font-semibold text-slate-900">
                  {profile?.fullName ?? "User"}
                </p>

                <p className="mt-0.5 truncate text-xs text-slate-500">
                  {profile?.email ?? ""}
                </p>

                {profile?.role && (
                  <span
                    className="
                      mt-2
                      inline-flex
                      rounded-full
                      bg-[#eef3f8]
                      px-2
                      py-0.5
                      text-[10px]
                      font-semibold
                      text-[#456b9a]
                    "
                  >
                    {profile.role}
                  </span>
                )}
              </div>
            </div>
          </div>

          {/* Menu Items */}
          <div className="p-2">
            {/* Profile */}
            <button
              type="button"
              role="menuitem"
              onClick={handleProfile}
              className="
                flex
                w-full
                items-center
                gap-3
                rounded-lg
                px-3
                py-2.5
                text-sm
                font-medium
                text-slate-600
                transition-colors
                hover:bg-slate-50
                hover:text-slate-900
                focus:outline-none
                focus:ring-2
                focus:ring-[#456b9a]/10
              "
            >
              <span
                className="
                  flex
                  h-8
                  w-8
                  items-center
                  justify-center
                  rounded-lg
                  bg-slate-50
                  text-slate-500
                "
              >
                <User className="h-4 w-4" />
              </span>

              <span className="flex-1 text-left">
                Profile
              </span>
            </button>

            {/* Security */}
            <button
              type="button"
              role="menuitem"
              onClick={handleSecurity}
              className="
                flex
                w-full
                items-center
                gap-3
                rounded-lg
                px-3
                py-2.5
                text-sm
                font-medium
                text-slate-600
                transition-colors
                hover:bg-slate-50
                hover:text-slate-900
                focus:outline-none
                focus:ring-2
                focus:ring-[#456b9a]/10
              "
            >
              <span
                className="
                  flex
                  h-8
                  w-8
                  items-center
                  justify-center
                  rounded-lg
                  bg-slate-50
                  text-slate-500
                "
              >
                <Shield className="h-4 w-4" />
              </span>

              <span className="flex-1 text-left">
                Security
              </span>
            </button>

            {/* Divider */}
            <div className="my-2 border-t border-slate-100" />

            {/* Logout */}
            <button
              type="button"
              role="menuitem"
              onClick={handleLogout}
              disabled={isLoggingOut}
              className="
                flex
                w-full
                items-center
                gap-3
                rounded-lg
                px-3
                py-2.5
                text-sm
                font-medium
                text-slate-600
                transition-colors
                hover:bg-red-50
                hover:text-red-600
                focus:outline-none
                focus:ring-2
                focus:ring-red-500/10
                disabled:pointer-events-none
                disabled:opacity-50
              "
            >
              <span
                className="
                  flex
                  h-8
                  w-8
                  items-center
                  justify-center
                  rounded-lg
                  bg-slate-50
                  text-slate-500
                  transition-colors
                  group-hover:bg-red-50
                "
              >
                <LogOut className="h-4 w-4" />
              </span>

              <span className="flex-1 text-left">
                {isLoggingOut
                  ? "Logging out..."
                  : "Logout"}
              </span>
            </button>
          </div>
        </div>
      )}
    </div>
  );
}
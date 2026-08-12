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
        !menuRef.current.contains(event.target as Node)
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
      handleClickOutside
    );

    document.addEventListener(
      "keydown",
      handleKeyDown
    );

    return () => {
      document.removeEventListener(
        "mousedown",
        handleClickOutside
      );

      document.removeEventListener(
        "keydown",
        handleKeyDown
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
       * Even if the API request fails,
       * clear local authentication state.
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
    profile?.firstName
      ?.charAt(0)
      .toUpperCase() ?? "U";

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
          bg-blue-600
          font-semibold
          text-white
          shadow-sm
          transition
          hover:bg-blue-700
          focus:outline-none
          focus:ring-2
          focus:ring-blue-500/30
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
            w-64
            overflow-hidden
            rounded-xl
            border
            border-slate-200
            bg-white
            shadow-lg
            shadow-slate-900/10
          "
        >
          {/* User Information */}
          <div className="border-b border-slate-200 px-4 py-4">
            <div className="flex items-center gap-3">

              {/* Avatar */}
              <div
                className="
                  flex
                  h-10
                  w-10
                  shrink-0
                  items-center
                  justify-center
                  rounded-full
                  bg-blue-50
                  font-semibold
                  text-blue-600
                "
              >
                {initials}
              </div>

              {/* User Details */}
              <div className="min-w-0">
                <p className="truncate text-sm font-semibold text-slate-900">
                  {profile?.fullName ?? "User"}
                </p>

                <p className="truncate text-xs text-slate-500">
                  {profile?.email ?? ""}
                </p>
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
                py-2
                text-sm
                text-slate-600
                transition
                hover:bg-slate-50
                hover:text-slate-900
              "
            >
              <User className="h-4 w-4 text-slate-400" />

              <span>Profile</span>
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
                py-2
                text-sm
                text-slate-600
                transition
                hover:bg-slate-50
                hover:text-slate-900
              "
            >
              <Shield className="h-4 w-4 text-slate-400" />

              <span>Security</span>
            </button>

            {/* Divider */}
            <div className="my-2 border-t border-slate-200" />

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
                py-2
                text-sm
                text-red-600
                transition
                hover:bg-red-50
                hover:text-red-700
                disabled:cursor-not-allowed
                disabled:opacity-50
              "
            >
              <LogOut className="h-4 w-4" />

              <span>
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
import { UserMenu } from "@/components/layout/UserMenu";

export function Header() {
  return (
    <header className="flex h-16 items-center justify-between border-b border-white/10 px-6">
      <div className="text-sm font-medium text-slate-300">
        DevFlow
      </div>

      <UserMenu />
    </header>
  );
}
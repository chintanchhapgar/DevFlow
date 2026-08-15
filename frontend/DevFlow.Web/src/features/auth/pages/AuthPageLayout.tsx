import type { ReactNode } from "react";
import { CheckCircle2 } from "lucide-react";

interface AuthPageLayoutProps {
  children: ReactNode;
}

export function AuthPageLayout({ children }: AuthPageLayoutProps) {
  return (
    <div className="min-h-screen bg-slate-50 text-slate-900">
      <div className="grid min-h-screen lg:grid-cols-[1.05fr_0.95fr]">
        <aside className="relative hidden overflow-hidden bg-[#eef3f8] lg:flex">
          <div className="absolute -left-24 -top-24 h-72 w-72 rounded-full bg-[#456b9a]/10" />
          <div className="absolute -bottom-32 -right-20 h-96 w-96 rounded-full bg-[#456b9a]/10" />
          <div className="relative flex w-full flex-col justify-between p-12 xl:p-16">
            <Brand />
            <div className="max-w-lg">
              <div className="mb-5 inline-flex items-center gap-2 rounded-full border border-slate-200 bg-white px-3 py-1.5 text-xs font-medium text-slate-600 shadow-sm">
                <span className="h-1.5 w-1.5 rounded-full bg-emerald-500" /> Built for modern teams
              </div>
              <h1 className="text-4xl font-bold leading-tight tracking-tight text-slate-900 xl:text-5xl">
                Plan work.<br />Build together.<br /><span className="text-[#456b9a]">Ship better.</span>
              </h1>
              <p className="mt-6 max-w-md text-base leading-7 text-slate-500">DevFlow brings projects, work, activity, and team collaboration together in one focused workspace.</p>
              <div className="mt-8 space-y-3">
                {["Organize projects and work", "Track team activity", "Keep your workspace secure"].map((item) => (
                  <div key={item} className="flex items-center gap-3 text-sm text-slate-600"><CheckCircle2 className="h-4 w-4 text-[#456b9a]" />{item}</div>
                ))}
              </div>
            </div>
            <p className="text-xs text-slate-400">© {new Date().getFullYear()} DevFlow</p>
          </div>
        </aside>
        <main className="flex min-h-screen items-center justify-center bg-white px-5 py-10 sm:px-8">
          <div className="w-full max-w-md"><div className="mb-10 lg:hidden"><Brand centered /></div>{children}</div>
        </main>
      </div>
    </div>
  );
}

function Brand({ centered = false }: { centered?: boolean }) {
  return <div className={`flex items-center gap-3 ${centered ? "justify-center" : ""}`}><div className="flex h-10 w-10 items-center justify-center rounded-xl bg-[#456b9a] text-lg font-bold text-white shadow-sm">◆</div><div><div className="text-lg font-bold tracking-tight text-slate-800">DEVFLOW</div><div className="text-xs text-slate-500">Project Management</div></div></div>;
}

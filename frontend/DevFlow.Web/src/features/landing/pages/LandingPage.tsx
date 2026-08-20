import { Link } from "react-router-dom";
import {
  ArrowRight,
  BarChart3,
  Check,
  Clock3,
  FolderKanban,
  Layers3,
  Menu,
  Play,
  Sparkles,
  UsersRound,
  X,
} from "lucide-react";
import { useState } from "react";

const navigation = [
  { label: "Product", href: "#product" },
  { label: "How it works", href: "#how-it-works" },
  { label: "Built for teams", href: "#teams" },
];

export function LandingPage() {
  const [menuOpen, setMenuOpen] = useState(false);

  return (
    <main className="min-h-screen overflow-hidden bg-[#fbfcfe] text-slate-900">
      <div className="relative isolate overflow-hidden bg-[#0b1220] text-white">
        <div className="pointer-events-none absolute inset-0 -z-10 overflow-hidden">
          <div className="absolute left-1/2 top-[-440px] h-[800px] w-[800px] -translate-x-1/2 rounded-full bg-[#456b9a]/30 blur-3xl" />
          <div className="absolute right-[-140px] top-52 h-96 w-96 rounded-full bg-[#456b9a]/15 blur-3xl" />
          <div className="absolute bottom-0 left-[8%] h-72 w-72 rounded-full bg-[#5c7ba3]/10 blur-3xl" />
        </div>

        <header className="mx-auto flex w-full max-w-7xl items-center justify-between px-5 py-5 sm:px-8 lg:px-10">
          <Link to="/" className="flex items-center gap-2.5" aria-label="DevFlow home">
            <span className="flex h-9 w-9 items-center justify-center rounded-xl bg-[#456b9a] text-sm text-white shadow-lg shadow-[#456b9a]/30">◆</span>
            <span className="text-lg font-bold tracking-tight text-white">DevFlow</span>
          </Link>

          <nav className="hidden items-center gap-7 md:flex" aria-label="Primary navigation">
            {navigation.map((item) => <a key={item.href} href={item.href} className="text-sm font-medium text-slate-400 transition hover:text-white">{item.label}</a>)}
          </nav>

          <div className="hidden items-center gap-3 md:flex">
            <Link to="/login" className="px-3 py-2 text-sm font-semibold text-slate-300 transition hover:text-white">Sign in</Link>
            <Link to="/register" className="rounded-xl bg-[#456b9a] px-4 py-2.5 text-sm font-semibold text-white shadow-lg shadow-[#456b9a]/20 transition hover:-translate-y-0.5 hover:bg-[#3f618c]">Start for free</Link>
          </div>
          <button type="button" aria-label="Toggle navigation" onClick={() => setMenuOpen(!menuOpen)} className="rounded-lg p-2 text-slate-200 md:hidden">{menuOpen ? <X className="h-5 w-5" /> : <Menu className="h-5 w-5" />}</button>
        </header>

        {menuOpen && <div className="mx-5 rounded-2xl border border-white/10 bg-[#111b2e] p-4 shadow-xl md:hidden"><nav className="grid gap-2">{navigation.map((item) => <a onClick={() => setMenuOpen(false)} key={item.href} href={item.href} className="rounded-lg px-3 py-2 text-sm font-medium text-slate-300 hover:bg-white/5">{item.label}</a>)}<Link onClick={() => setMenuOpen(false)} to="/login" className="rounded-lg px-3 py-2 text-sm font-semibold text-slate-300">Sign in</Link><Link onClick={() => setMenuOpen(false)} to="/register" className="rounded-lg bg-[#456b9a] px-3 py-2.5 text-center text-sm font-semibold text-white">Start for free</Link></nav></div>}

        <section className="mx-auto max-w-7xl px-5 pb-16 pt-16 sm:px-8 sm:pt-24 lg:px-10 lg:pb-24 lg:pt-28">
          <div className="mx-auto max-w-4xl text-center">
            <div className="inline-flex items-center gap-2 rounded-full border border-[#7890aa]/35 bg-[#456b9a]/15 px-4 py-2 text-xs font-semibold text-[#dbe4ed]"><span className="h-1.5 w-1.5 animate-pulse rounded-full bg-[#7890aa]" /><Sparkles className="h-3.5 w-3.5" /> Built for modern teams</div>
            <h1 className="mt-7 text-5xl font-semibold tracking-[-0.06em] text-white sm:text-6xl lg:text-7xl">Work that flows.<br /><span className="bg-gradient-to-r from-[#dbe4ed] via-[#7890aa] to-[#5c7ba3] bg-clip-text text-transparent">Teams that ship.</span></h1>
            <p className="mx-auto mt-7 max-w-2xl text-base leading-7 text-slate-400 sm:text-lg">DevFlow brings every project, decision, and deadline into one beautifully focused workspace—so your team can spend less time coordinating and more time creating.</p>
            <div className="mt-9 flex flex-col items-center justify-center gap-3 sm:flex-row"><Link to="/register" className="inline-flex h-12 w-full items-center justify-center gap-2 rounded-2xl bg-[#456b9a] px-6 text-sm font-semibold text-white shadow-2xl shadow-[#456b9a]/30 transition hover:-translate-y-0.5 hover:bg-[#3f618c] sm:w-auto">Start building for free <ArrowRight className="h-4 w-4" /></Link><a href="#product" className="inline-flex h-12 w-full items-center justify-center gap-2 rounded-2xl border border-white/15 bg-white/5 px-6 text-sm font-semibold text-slate-200 transition hover:border-white/25 hover:bg-white/10 sm:w-auto"><Play className="h-4 w-4 fill-current text-[#7890aa]" /> Explore DevFlow</a></div>
            <p className="mt-4 text-xs text-slate-500">No credit card required · Set up in minutes</p>
          </div>
          <div className="relative mx-auto mt-14 max-w-5xl sm:mt-20"><div className="absolute -inset-5 rounded-[2rem] bg-[#456b9a]/20 blur-3xl" /><div className="relative rounded-2xl border border-white/10 bg-[#111b2e]/85 p-2 shadow-2xl shadow-black/40 backdrop-blur"><DashboardPreview /></div></div>
          <div className="mx-auto mt-14 grid max-w-3xl grid-cols-2 border-t border-white/10 pt-8 text-center sm:grid-cols-4"><HeroStat value="One place" label="For your work" /><HeroStat value="Live" label="Team visibility" /><HeroStat value="Secure" label="By design" /><HeroStat value="Focused" label="Every day" /></div>
        </section>
      </div>

      <section id="product" className="bg-[#0f1728] py-16 text-white sm:py-24">
        <div className="mx-auto max-w-7xl px-5 sm:px-8 lg:px-10">
          <div className="overflow-hidden rounded-[2rem] border border-white/10 bg-[#111b2e] shadow-2xl shadow-black/20">
            <div className="border-b border-white/10 bg-[#16233a] px-6 py-10 sm:px-10 lg:flex lg:items-end lg:justify-between">
              <div className="max-w-2xl"><p className="text-sm font-bold uppercase tracking-[0.18em] text-[#7890aa]">One shared source of truth</p><h2 className="mt-3 text-3xl font-semibold tracking-tight text-white sm:text-4xl">Everything your team needs to make meaningful progress.</h2></div>
              <p className="mt-4 max-w-xs text-sm leading-6 text-slate-400 lg:mt-0">A focused workspace for the work, context, and people behind every delivery.</p>
            </div>
            <div className="grid gap-4 p-5 sm:grid-cols-3 sm:p-7"><Feature icon={<FolderKanban />} title="Projects with momentum" text="Turn ambitious plans into clear next steps, from kickoff to launch." /><Feature icon={<Layers3 />} title="A calmer workday" text="See priorities, unblock teammates, and keep work moving without the noise." /><Feature icon={<BarChart3 />} title="Progress you can trust" text="Make confident calls with live project insight, reports, and activity." /></div>
          </div>
        </div>
      </section>

      <section id="how-it-works" className="bg-[#0b1220] py-16 text-white sm:py-24"><div className="mx-auto max-w-7xl px-5 sm:px-8 lg:px-10"><div className="rounded-[2rem] border border-white/10 bg-gradient-to-br from-[#456b9a]/45 via-[#16233a] to-[#111b2e] p-6 shadow-2xl shadow-black/20 sm:p-10 lg:p-12"><div className="grid gap-10 lg:grid-cols-[0.8fr_1.2fr] lg:items-end"><div><p className="text-sm font-bold uppercase tracking-[0.18em] text-[#7890aa]">Built around real work</p><h2 className="mt-3 text-3xl font-semibold tracking-tight sm:text-4xl">From idea to impact, without the handoffs.</h2><p className="mt-5 max-w-md leading-7 text-slate-400">A single rhythm for planning, doing, and learning—made to keep everyone aligned as the work evolves.</p><Link to="/projects" className="mt-7 inline-flex items-center gap-2 rounded-lg border border-[#7890aa]/35 bg-[#456b9a]/25 px-4 py-2.5 text-sm font-semibold text-white transition hover:bg-[#456b9a]/45">View your projects <ArrowRight className="h-4 w-4" /></Link></div><div className="grid gap-3 sm:grid-cols-3"><Step number="01" title="Shape the work" text="Create projects and map the work that matters." /><Step number="02" title="Move together" text="Keep priorities visible and collaboration effortless." /><Step number="03" title="Learn & improve" text="Turn activity and reports into better decisions." /></div></div></div></div></section>

      <section id="teams" className="bg-[#0f1728] py-16 text-white sm:py-24"><div className="mx-auto max-w-7xl px-5 sm:px-8 lg:px-10"><div className="rounded-[2rem] border border-white/10 bg-[#111b2e] p-4 shadow-2xl shadow-black/20 sm:p-6"><div className="relative overflow-hidden rounded-[1.5rem] bg-gradient-to-br from-[#456b9a] via-[#3f618c] to-[#16233a] px-6 py-12 text-center sm:px-12 sm:py-16"><div className="absolute -right-16 -top-20 h-56 w-56 rounded-full bg-[#7890aa]/15 blur-3xl" /><div className="relative"><div className="mx-auto flex h-12 w-12 items-center justify-center rounded-2xl bg-white/15"><UsersRound className="h-6 w-6" /></div><h2 className="mx-auto mt-5 max-w-2xl text-3xl font-semibold tracking-tight sm:text-4xl">Make your next great project feel simpler.</h2><p className="mx-auto mt-4 max-w-xl leading-7 text-[#dbe4ed]">Bring your team’s focus, workflow, and progress together in DevFlow.</p><div className="mt-8 flex flex-col justify-center gap-3 sm:flex-row"><Link to="/register" className="inline-flex h-12 items-center justify-center rounded-xl bg-white px-5 text-sm font-semibold text-[#456b9a] transition hover:bg-[#eef3f8]">Create your workspace</Link><Link to="/login" className="inline-flex h-12 items-center justify-center rounded-xl border border-white/25 px-5 text-sm font-semibold text-white transition hover:bg-white/10">Sign in to DevFlow</Link></div></div></div></div></div></section>

      <footer className="border-t border-white/10 bg-[#0b1220] text-white"><div className="mx-auto flex max-w-7xl flex-col gap-4 px-5 py-8 sm:flex-row sm:items-center sm:justify-between sm:px-8 lg:px-10"><div className="flex items-center gap-2 text-sm font-semibold"><span className="flex h-6 w-6 items-center justify-center rounded-md bg-[#456b9a] text-[9px] text-white">◆</span> DevFlow</div><div className="flex flex-wrap gap-x-5 gap-y-2 text-xs text-slate-500"><Link to="/projects" className="hover:text-white">Projects</Link><Link to="/work" className="hover:text-white">My work</Link><Link to="/time" className="hover:text-white">Time</Link><Link to="/reports" className="hover:text-white">Reports</Link><Link to="/activity" className="hover:text-white">Activity</Link></div><p className="text-xs text-slate-600">© {new Date().getFullYear()} DevFlow</p></div></footer>
    </main>
  );
}

function Feature({ icon, title, text }: { icon: React.ReactNode; title: string; text: string }) {
  return <article className="rounded-2xl border border-white/10 bg-white/[0.03] p-6 transition duration-300 hover:-translate-y-1 hover:border-[#7890aa]/40 hover:bg-[#456b9a]/10"><div className="flex h-10 w-10 items-center justify-center rounded-xl bg-[#456b9a]/20 text-[#7890aa]">{icon}</div><h3 className="mt-5 text-lg font-semibold text-white">{title}</h3><p className="mt-2 leading-6 text-slate-400">{text}</p></article>;
}

function Step({ number, title, text }: { number: string; title: string; text: string }) {
  return <article className="rounded-2xl border border-white/10 bg-[#0b1220]/40 p-5"><p className="text-xs font-bold tracking-[0.16em] text-[#7890aa]">{number}</p><h3 className="mt-8 text-lg font-semibold">{title}</h3><p className="mt-2 text-sm leading-6 text-slate-400">{text}</p></article>;
}

function HeroStat({ value, label }: { value: string; label: string }) {
  return <div className="border-white/10 px-3 py-2 sm:border-r sm:last:border-r-0"><p className="text-base font-semibold text-[#dbe4ed]">{value}</p><p className="mt-1 text-[10px] font-medium uppercase tracking-[0.13em] text-slate-500">{label}</p></div>;
}

function DashboardPreview() {
  return <div className="relative mx-auto mt-5 max-w-5xl rounded-2xl border border-slate-200/90 bg-white p-2 shadow-2xl shadow-[#456b9a]/10 sm:mt-8"><div className="overflow-hidden rounded-xl border border-slate-100 bg-slate-50"><div className="flex h-11 items-center gap-2 border-b border-slate-200 bg-white px-4"><span className="h-2.5 w-2.5 rounded-full bg-rose-300" /><span className="h-2.5 w-2.5 rounded-full bg-amber-300" /><span className="h-2.5 w-2.5 rounded-full bg-emerald-300" /><div className="ml-3 h-5 w-36 rounded-md bg-slate-100" /></div><div className="grid min-h-[280px] grid-cols-[48px_1fr] sm:grid-cols-[172px_1fr]"><aside className="border-r border-slate-200 bg-white p-3 sm:p-4"><div className="hidden space-y-3 sm:block"><div className="h-5 w-20 rounded bg-[#456b9a]" />{["Overview", "Projects", "My work", "Reports"].map((item, index) => <div key={item} className={`flex items-center gap-2 rounded-md px-2 py-1.5 text-xs ${index === 0 ? "bg-[#eef3f8] font-semibold text-[#456b9a]" : "text-slate-500"}`}><span className="h-2 w-2 rounded-full bg-current opacity-60" />{item}</div>)}</div></aside><div className="p-4 sm:p-6"><div className="flex items-center justify-between"><div><div className="h-3 w-20 rounded bg-slate-200" /><div className="mt-2 h-6 w-36 rounded bg-slate-900" /></div><div className="h-8 w-20 rounded-lg bg-[#456b9a]" /></div><div className="mt-5 grid grid-cols-3 gap-3">{[["24", "Open work"], ["8", "On track"], ["94%", "Team pace"]].map(([value, label]) => <div key={label} className="rounded-xl border border-slate-200 bg-white p-3"><p className="text-base font-bold sm:text-xl">{value}</p><p className="mt-1 text-[10px] text-slate-500 sm:text-xs">{label}</p></div>)}</div><div className="mt-4 grid gap-3 md:grid-cols-[1.3fr_0.7fr]"><div className="rounded-xl border border-slate-200 bg-white p-3"><div className="flex items-center justify-between"><span className="text-xs font-semibold">Current sprint</span><span className="text-[10px] text-slate-500">7 days left</span></div><div className="mt-4 space-y-2">{["Design system polish", "Customer onboarding", "Mobile experience"].map((task, index) => <div key={task} className="flex items-center gap-2 text-[10px] text-slate-600 sm:text-xs"><span className={`h-3 w-3 rounded border ${index === 0 ? "border-emerald-500 bg-emerald-500" : "border-slate-300"}`}>{index === 0 && <Check className="h-3 w-3 text-white" />}</span>{task}</div>)}</div></div><div className="hidden rounded-xl bg-[#456b9a] p-3 text-white md:block"><Clock3 className="h-4 w-4 text-[#dbe4ed]" /><p className="mt-5 text-lg font-bold">18h 24m</p><p className="text-[10px] text-[#dbe4ed]">Focused this week</p></div></div></div></div></div></div>;
}

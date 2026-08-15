import type { ComponentProps } from "react";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";

export function AuthFormField({ id, label, hint, ...props }: ComponentProps<typeof Input> & { id: string; label: string; hint?: string }) {
  return <div className="space-y-2"><Label htmlFor={id}>{label}</Label><Input id={id} required className="h-11 border-[#cbd5e1] bg-white text-slate-900 placeholder:text-slate-400 focus-visible:border-[#7890aa] focus-visible:ring-[#456b9a]/20" {...props} />{hint && <p className="text-xs text-slate-500">{hint}</p>}</div>;
}

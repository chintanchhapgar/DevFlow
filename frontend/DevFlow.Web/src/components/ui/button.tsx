import * as React from "react";
import { Slot } from "@radix-ui/react-slot";

import { cn } from "@/lib/utils";

type ButtonVariant =
  | "default"
  | "secondary"
  | "outline"
  | "ghost"
  | "destructive"
  | "link";

type ButtonSize =
  | "default"
  | "sm"
  | "lg"
  | "icon";

export interface ButtonProps
  extends React.ButtonHTMLAttributes<HTMLButtonElement> {
  asChild?: boolean;
  variant?: ButtonVariant;
  size?: ButtonSize;
}

const buttonBase =
  "inline-flex items-center justify-center gap-2 whitespace-nowrap rounded-lg text-sm font-medium transition-colors outline-none disabled:pointer-events-none disabled:opacity-50 focus-visible:ring-2 focus-visible:ring-[#9db2c8] focus-visible:ring-offset-2 focus-visible:ring-offset-white";

const variants: Record<ButtonVariant, string> = {
  default:
    "bg-[#456b9a] text-white shadow-sm hover:bg-[#3f618c] active:bg-[#385879]",

  secondary:
    "bg-slate-100 text-slate-700 hover:bg-slate-200",

  outline:
    "border border-slate-200 bg-white text-slate-700 shadow-sm hover:bg-slate-50 hover:text-slate-900",

  ghost:
    "text-slate-600 hover:bg-slate-100 hover:text-slate-900",

  destructive:
    "bg-red-600 text-white shadow-sm hover:bg-red-700 active:bg-red-800",

  link:
    "text-[#456b9a] underline-offset-4 hover:underline",
};

const sizes: Record<ButtonSize, string> = {
  default: "h-10 px-4 py-2.5",

  sm: "h-9 rounded-md px-3",

  lg: "h-11 rounded-lg px-6",

  icon: "h-10 w-10",
};

export function Button({
  className,
  variant = "default",
  size = "default",
  asChild = false,
  ...props
}: ButtonProps) {
  const Comp = asChild ? Slot : "button";

  return (
    <Comp
      data-slot="button"
      className={cn(
        buttonBase,
        variants[variant],
        sizes[size],
        className,
      )}
      {...props}
    />
  );
}
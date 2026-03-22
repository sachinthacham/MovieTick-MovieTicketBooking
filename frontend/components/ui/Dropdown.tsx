"use client"

import * as React from "react"
import { cn } from "@/lib/utils"

export function DropdownMenu({ children }: { children: React.ReactNode }) {
  const [isOpen, setIsOpen] = React.useState(false)
  const ref = React.useRef<HTMLDivElement>(null)

  React.useEffect(() => {
    function handleClickOutside(event: MouseEvent) {
      if (ref.current && !ref.current.contains(event.target as Node)) {
        setIsOpen(false)
      }
    }

    document.addEventListener("mousedown", handleClickOutside)
    return () => document.removeEventListener("mousedown", handleClickOutside)
  }, [ref])

  // Simple context to pass state down
  return (
    <div ref={ref} className="relative inline-block text-left" onClick={() => setIsOpen(!isOpen)}>
      {React.Children.map(children, (child) => {
        if (React.isValidElement<{ isOpen: boolean }>(child)) {
          return React.cloneElement(child, { isOpen })
        }
        return child
      })}
    </div>
  )
}

export function DropdownMenuTrigger({ children, asChild = false }: { children: React.ReactNode, asChild?: boolean }) {
  if (asChild && React.isValidElement(children)) {
    return children
  }
  return <button className="inline-flex w-full justify-center gap-x-1.5 rounded-md bg-(--background) px-3 py-2 text-sm font-semibold text-(--foreground) shadow-sm ring-1 ring-inset ring-(--border) hover:bg-(--muted)">{children}</button>
}

export function DropdownMenuContent({ children, isOpen, align = "right" }: { children?: React.ReactNode, isOpen?: boolean, align?: "left" | "right" }) {
  if (!isOpen) return null

  return (
    <div className={cn(
      "absolute z-50 mt-2 w-56 origin-top-right rounded-md bg-(--background) shadow-lg ring-1 ring-black ring-opacity-5 focus:outline-none animate-in fade-in zoom-in-95 duration-100",
      align === "right" ? "right-0" : "left-0"
    )}>
      <div className="py-1">{children}</div>
    </div>
  )
}

export function DropdownMenuItem({ children, onClick, className }: { children: React.ReactNode, onClick?: () => void, className?: string }) {
  return (
    <button
      onClick={onClick}
      className={cn("block w-full px-4 py-2 text-left text-sm text-(--foreground) hover:bg-(--muted) hover:text-(--foreground)", className)}
    >
      {children}
    </button>
  )
}

export function DropdownMenuSeparator() {
  return <div className="h-px bg-(--border)" />
}

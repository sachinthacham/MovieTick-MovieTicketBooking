"use client"

import * as React from "react"
import { cn } from "@/lib/utils"
// Simplistic Modal implementation.
// For a deep production app, typically Radix UI Dialog is used for accessibility.

interface ModalProps {
  isOpen: boolean
  onClose: () => void
  children: React.ReactNode
  className?: string
  title?: string
}

export function Modal({ isOpen, onClose, children, className, title }: ModalProps) {
  React.useEffect(() => {
    if (isOpen) {
      document.body.style.overflow = "hidden"
    } else {
      document.body.style.overflow = "unset"
    }
    return () => {
      document.body.style.overflow = "unset"
    }
  }, [isOpen])

  if (!isOpen) return null

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/50 backdrop-blur-sm animate-in fade-in duration-200">
      <div 
        className="fixed inset-0"
        onClick={onClose}
        aria-hidden="true"
      />
      <div 
        className={cn(
          "relative z-50 w-full max-w-2xl scale-100 rounded-xl border border-(--border) bg-(--background) p-6 shadow-lg animate-in zoom-in-95 duration-200", 
          className
        )}
      >
        {title && (
          <div className="flex items-center justify-between mb-4 pb-4 border-b border-(--border)">
            <h2 className="text-lg font-bold">{title}</h2>
            <button
              onClick={onClose}
              className="h-8 w-8 rounded-full flex items-center justify-center text-(--muted-foreground) hover:bg-(--muted) transition-colors"
              aria-label="Close"
            >
              ×
            </button>
          </div>
        )}
        {children}
      </div>
    </div>
  )
}

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
}

export function Modal({ isOpen, onClose, children, className }: ModalProps) {
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
          "relative z-50 w-full max-w-lg scale-100 rounded-xl border border-(--border) bg-(--background) p-6 shadow-lg animate-in zoom-in-95 duration-200", 
          className
        )}
      >
        {children}
      </div>
    </div>
  )
}

'use client'

import * as React from 'react'
import { Upload, X, ImageIcon } from 'lucide-react'
import { cn } from '@/lib/utils'
import { Button } from './Button'

interface ImageUploadProps {
  value?: string | null
  onChange: (file: File) => void
  onRemove?: () => void
  className?: string
  label?: string
  accept?: string
}

export function ImageUpload({
  value,
  onChange,
  onRemove,
  className,
  label = 'Upload Image',
  accept = 'image/*',
}: ImageUploadProps) {
  const inputRef = React.useRef<HTMLInputElement>(null)
  const [preview, setPreview] = React.useState<string | null>(value ?? null)

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0]
    if (!file) return
    const url = URL.createObjectURL(file)
    setPreview(url)
    onChange(file)
  }

  const handleRemove = () => {
    setPreview(null)
    if (inputRef.current) inputRef.current.value = ''
    onRemove?.()
  }

  return (
    <div className={cn('space-y-2', className)}>
      <input
        ref={inputRef}
        type="file"
        accept={accept}
        className="sr-only"
        onChange={handleFileChange}
      />
      {preview ? (
        <div className="relative w-full aspect-video rounded-xl overflow-hidden border border-(--border) bg-(--muted)">
          {/* eslint-disable-next-line @next/next/no-img-element */}
          <img src={preview} alt="Preview" className="w-full h-full object-cover" />
          <Button
            type="button"
            size="icon"
            variant="secondary"
            className="absolute top-2 right-2 h-7 w-7 rounded-full shadow"
            onClick={handleRemove}
          >
            <X className="h-4 w-4" />
          </Button>
        </div>
      ) : (
        <button
          type="button"
          onClick={() => inputRef.current?.click()}
          className="w-full border-2 border-dashed border-(--border) rounded-xl py-10 flex flex-col items-center gap-3 text-(--muted-foreground) hover:border-(--primary) hover:text-(--primary) transition-colors"
        >
          <ImageIcon className="h-10 w-10 opacity-40" />
          <div className="text-sm">
            <span className="font-semibold">{label}</span> or drag & drop
          </div>
          <div className="text-xs opacity-60">PNG, JPG, WEBP up to 10MB</div>
        </button>
      )}
      {!preview && (
        <Button type="button" variant="outline" size="sm" onClick={() => inputRef.current?.click()}>
          <Upload className="h-4 w-4 mr-2" />
          {label}
        </Button>
      )}
    </div>
  )
}

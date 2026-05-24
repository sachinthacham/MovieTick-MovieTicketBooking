'use client'

import * as React from 'react'
import { ChevronLeft, ChevronRight, Search, Loader2 } from 'lucide-react'
import { Button } from './Button'
import { Input } from './Input'
import { cn } from '@/lib/utils'

export interface Column<T> {
  key: keyof T | string
  header: string
  render?: (row: T) => React.ReactNode
  className?: string
}

interface DataTableProps<T> {
  columns: Column<T>[]
  data: T[]
  isLoading?: boolean
  searchable?: boolean
  searchPlaceholder?: string
  totalPages?: number
  currentPage?: number
  onPageChange?: (page: number) => void
  onSearch?: (query: string) => void
  actions?: (row: T) => React.ReactNode
  emptyMessage?: string
}

export function DataTable<T extends { id?: string | number }>({
  columns,
  data,
  isLoading,
  searchable,
  searchPlaceholder = 'Search...',
  totalPages = 1,
  currentPage = 1,
  onPageChange,
  onSearch,
  actions,
  emptyMessage = 'No records found.',
}: DataTableProps<T>) {
  const [search, setSearch] = React.useState('')

  const handleSearch = (e: React.ChangeEvent<HTMLInputElement>) => {
    setSearch(e.target.value)
    onSearch?.(e.target.value)
  }

  const getValue = (row: T, key: string) => {
    const keys = key.split('.')
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    return keys.reduce<any>((acc, k) => acc?.[k], row)
  }

  return (
    <div className="space-y-4">
      {searchable && (
        <div className="relative max-w-sm">
          <Search className="absolute left-2.5 top-2.5 h-4 w-4 text-(--muted-foreground)" />
          <Input
            placeholder={searchPlaceholder}
            value={search}
            onChange={handleSearch}
            className="pl-9"
          />
        </div>
      )}

      <div className="rounded-xl border border-(--border) overflow-hidden">
        <div className="overflow-x-auto">
          <table className="w-full text-sm">
            <thead className="bg-(--muted)/50 border-b border-(--border)">
              <tr>
                {columns.map((col) => (
                  <th
                    key={col.key as string}
                    className={cn(
                      'text-left px-4 py-3 font-semibold text-(--foreground)',
                      col.className
                    )}
                  >
                    {col.header}
                  </th>
                ))}
                {actions && (
                  <th className="text-right px-4 py-3 font-semibold text-(--foreground)">
                    Actions
                  </th>
                )}
              </tr>
            </thead>
            <tbody className="divide-y divide-(--border)">
              {isLoading ? (
                <tr>
                  <td colSpan={columns.length + (actions ? 1 : 0)} className="px-4 py-12 text-center">
                    <Loader2 className="h-6 w-6 animate-spin mx-auto text-(--muted-foreground)" />
                  </td>
                </tr>
              ) : data.length === 0 ? (
                <tr>
                  <td
                    colSpan={columns.length + (actions ? 1 : 0)}
                    className="px-4 py-12 text-center text-(--muted-foreground)"
                  >
                    {emptyMessage}
                  </td>
                </tr>
              ) : (
                data.map((row, idx) => (
                  <tr
                    key={(row.id as string) ?? idx}
                    className="hover:bg-(--muted)/30 transition-colors"
                  >
                    {columns.map((col) => (
                      <td key={col.key as string} className={cn('px-4 py-3', col.className)}>
                        {col.render ? col.render(row) : String(getValue(row, col.key as string) ?? '-')}
                      </td>
                    ))}
                    {actions && (
                      <td className="px-4 py-3">
                        <div className="flex items-center justify-end gap-2">{actions(row)}</div>
                      </td>
                    )}
                  </tr>
                ))
              )}
            </tbody>
          </table>
        </div>
      </div>

      {totalPages > 1 && (
        <div className="flex items-center justify-between text-sm text-(--muted-foreground)">
          <span>
            Page {currentPage} of {totalPages}
          </span>
          <div className="flex items-center gap-2">
            <Button
              variant="outline"
              size="sm"
              onClick={() => onPageChange?.(currentPage - 1)}
              disabled={currentPage <= 1}
            >
              <ChevronLeft className="h-4 w-4" />
            </Button>
            <Button
              variant="outline"
              size="sm"
              onClick={() => onPageChange?.(currentPage + 1)}
              disabled={currentPage >= totalPages}
            >
              <ChevronRight className="h-4 w-4" />
            </Button>
          </div>
        </div>
      )}
    </div>
  )
}

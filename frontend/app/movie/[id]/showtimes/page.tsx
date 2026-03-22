"use client"

import * as React from "react"
import Link from "next/link"
import { Calendar as CalendarIcon, MapPin, Search } from "lucide-react"
import { Button } from "@/components/ui/Button"
import { Badge } from "@/components/ui/Badge"
import { Input } from "@/components/ui/Input"
import { cn } from "@/lib/utils"
import { mockShowtimesData, getMovieDetails } from "@/lib/mockData"

export default function ShowtimesPage({ params }: { params: { id: string } }) {
  const [selectedDate, setSelectedDate] = React.useState(0)
  const movie = getMovieDetails(params.id)

  // Generate mock dates
  const dates = Array.from({ length: 7 }).map((_, i) => {
    const d = new Date()
    d.setDate(d.getDate() + i)
    return {
      day: d.toLocaleDateString('en-US', { weekday: 'short' }),
      date: d.getDate(),
      month: d.toLocaleDateString('en-US', { month: 'short' })
    }
  })

  return (
    <div className="container mx-auto px-4 py-8 md:px-8 max-w-5xl min-h-[calc(100vh-14rem)]">
      
      {/* Movie Basic Info Header */}
      <div className="mb-8 flex flex-col md:flex-row md:items-end justify-between border-b border-(--border) pb-6">
        <div>
          <h1 className="text-3xl font-extrabold tracking-tight md:text-4xl text-(--foreground)">
            {movie.title}
          </h1>
          <div className="flex flex-wrap items-center gap-2 mt-3">
            <Badge variant="outline" className="rounded-md">{movie.genre.join(", ")}</Badge>
            <Badge variant="outline" className="rounded-md">UA</Badge>
            <span className="text-sm font-medium text-(--muted-foreground) ml-2">{movie.duration}</span>
          </div>
        </div>
        
        <div className="mt-6 md:mt-0 flex flex-col md:items-end space-y-3">
          <div className="flex items-center text-(--primary) text-sm font-semibold hover:underline cursor-pointer">
            <MapPin className="h-4 w-4 mr-1" />
            Mumbai
          </div>
          <div className="relative w-full md:w-64">
            <Search className="absolute left-2.5 top-2.5 h-4 w-4 text-(--muted-foreground)" />
            <Input className="pl-9 h-9" placeholder="Search theatre..." />
          </div>
        </div>
      </div>

      {/* Date Selector */}
      <div className="mb-10 w-full overflow-x-auto no-scrollbar pb-2">
        <div className="flex space-x-3 w-max">
          {dates.map((d, index) => {
            const isSelected = selectedDate === index
            return (
              <button
                key={index}
                onClick={() => setSelectedDate(index)}
                className={cn(
                  "flex flex-col items-center justify-center w-[72px] h-[80px] rounded-xl border transition-all shrink-0",
                  isSelected 
                    ? "border-(--primary) bg-(--primary) text-(--primary-foreground) shadow-md" 
                    : "border-(--border) hover:border-(--primary)/50 bg-(--background) text-(--foreground)"
                )}
              >
                <span className={cn("text-xs font-medium uppercase tracking-wider", isSelected ? "text-(--primary-foreground)/80" : "text-(--muted-foreground)")}>
                  {d.day}
                </span>
                <span className="text-2xl font-bold mt-0.5">{d.date}</span>
                <span className={cn("text-[10px] uppercase font-bold", isSelected ? "text-(--primary-foreground)/80" : "text-(--muted-foreground)")}>
                  {d.month}
                </span>
              </button>
            )
          })}
        </div>
      </div>

      {/* Showtimes List */}
      <div className="space-y-6">
        <div className="flex items-center p-3 text-xs font-semibold uppercase tracking-wider text-(--muted-foreground) bg-(--muted)/50 rounded-lg">
          <CalendarIcon className="h-4 w-4 mr-2" />
          Showing times for {dates[selectedDate].date} {dates[selectedDate].month}
        </div>

        {mockShowtimesData.map(theatre => (
          <div key={theatre.theatreId} className="border border-(--border) rounded-2xl bg-(--background) overflow-hidden">
            <div className="p-4 md:p-6 border-b border-(--border) bg-(--muted)/10 flex flex-col md:flex-row md:items-center justify-between">
              <h3 className="font-bold text-lg">{theatre.name}</h3>
              <div className="flex items-center space-x-2 mt-2 md:mt-0">
                <Button variant="ghost" size="sm" className="text-(--muted-foreground) h-8 text-xs">
                  Info
                </Button>
                <Button variant="ghost" size="sm" className="text-(--muted-foreground) h-8 text-xs">
                  Food & Bev
                </Button>
              </div>
            </div>
            
            <div className="p-4 md:p-6 space-y-6">
              {theatre.formats.map(format => (
                <div key={format.name} className="flex flex-col md:flex-row gap-4">
                  <div className="md:w-32 shrink-0">
                    <span className="text-sm font-semibold text-(--muted-foreground) flex items-center">
                      {format.name}
                    </span>
                  </div>
                  <div className="flex flex-wrap gap-3 flex-1">
                    {format.times.map(time => (
                      <Link key={time} href={`/movie/${params.id}/seats`}>
                        <Button
                          variant="outline"
                          className="hover:bg-(--primary)/10 hover:text-(--primary) border-(--border) hover:border-(--primary)/50 transition-all font-medium whitespace-nowrap"
                        >
                          {time}
                        </Button>
                      </Link>
                    ))}
                  </div>
                </div>
              ))}
            </div>
          </div>
        ))}

        {/* Legend */}
        <div className="pt-6 flex flex-wrap gap-4 items-center text-xs text-(--muted-foreground)">
          <div className="flex items-center"><span className="w-3 h-3 rounded-full bg-(--primary)/20 border border-(--primary) mr-2"></span> Fast Filling</div>
          <div className="flex items-center"><span className="w-3 h-3 rounded-full bg-green-500/20 border border-green-500 mr-2"></span> Available</div>
        </div>
      </div>
    </div>
  )
}

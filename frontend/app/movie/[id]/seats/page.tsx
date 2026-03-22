"use client"

import * as React from "react"
import Link from "next/link"
import { ArrowLeft, Monitor } from "lucide-react"
import { Button } from "@/components/ui/Button"
import { cn } from "@/lib/utils"
import { defaultSeatCategories, getMovieDetails } from "@/lib/mockData"

export default function SeatSelectionPage({ params }: { params: { id: string } }) {
  const [selectedSeats, setSelectedSeats] = React.useState<string[]>([])
  const movie = getMovieDetails(params.id)
  
  const getSeatCategory = (row: string) => {
    return defaultSeatCategories.find(c => c.rows.includes(row)) || defaultSeatCategories[2]
  }

  const toggleSeat = (seatId: string) => {
    setSelectedSeats(prev => 
      prev.includes(seatId) ? prev.filter(s => s !== seatId) : [...prev, seatId]
    )
  }

  const getTotalAmount = () => {
    return selectedSeats.reduce((total, seat) => {
      const row = seat.charAt(0)
      return total + getSeatCategory(row).price
    }, 0)
  }

  // Generate grid
  const renderRow = (rowLabel: string, seatCount: number, gapAfter: number[]) => {
    const seats = []
    for (let i = 1; i <= seatCount; i++) {
      const seatId = `${rowLabel}${i}`
      const isSelected = selectedSeats.includes(seatId)
      // Randomly mock booked seats based on string char code
      const isBooked = (seatId.charCodeAt(0) + i) % 7 === 0 

      seats.push(
        <React.Fragment key={seatId}>
          <button
            disabled={isBooked}
            onClick={() => toggleSeat(seatId)}
            className={cn(
              "h-8 w-8 text-xs font-semibold rounded-t-md transition-all flex items-center justify-center border",
              isBooked 
                ? "bg-(--muted) border-transparent text-transparent cursor-not-allowed opacity-50" 
                : isSelected
                  ? "bg-(--primary) border-(--primary) text-(--primary-foreground) scale-110 shadow-lg"
                  : "bg-(--background) border-(--border) hover:border-(--primary) text-(--foreground) hover:bg-(--primary)/10"
            )}
          >
            {isSelected ? i : ""}
          </button>
          
          {/* Add walkway gaps */}
          {gapAfter.includes(i) && <div className="w-8 shrink-0" />}
        </React.Fragment>
      )
    }

    return (
      <div key={rowLabel} className="flex items-center gap-2">
        <div className="w-6 text-sm font-semibold text-(--muted-foreground) shrink-0 text-center">{rowLabel}</div>
        <div className="flex gap-2 flex-1 justify-center">
          {seats}
        </div>
      </div>
    )
  }

  return (
    <div className="flex flex-col min-h-screen bg-(--background)">
      
      {/* Top Header */}
      <header className="sticky top-0 z-40 bg-(--background) border-b border-(--border)">
        <div className="container mx-auto px-4 md:px-8 h-16 flex items-center justify-between">
          <div className="flex items-center space-x-4">
            <Link href={`/movie/${params.id}/showtimes`}>
              <Button variant="ghost" size="icon" className="rounded-full">
                <ArrowLeft className="h-5 w-5" />
              </Button>
            </Link>
            <div>
              <h1 className="font-bold">{movie.title}</h1>
              <p className="text-xs text-(--muted-foreground)">Cinépolis: VIP, Thane • Today, 07:30 PM</p>
            </div>
          </div>
          
          <div className="hidden sm:flex text-sm space-x-4">
            <div className="flex items-center"><div className="w-4 h-4 border border-(--border) rounded-t-sm mr-2"></div> Available</div>
            <div className="flex items-center"><div className="w-4 h-4 bg-(--primary) rounded-t-sm mr-2"></div> Selected</div>
            <div className="flex items-center"><div className="w-4 h-4 bg-(--muted) rounded-t-sm mr-2"></div> Sold</div>
          </div>
        </div>
      </header>

      {/* Main Seat Grid Layout */}
      <div className="flex-1 flex flex-col lg:flex-row overflow-hidden">
        
        {/* Interactive Grid Map */}
        <div className="flex-1 p-4 md:p-8 overflow-auto flex flex-col items-center custom-scrollbar">
          
          {/* Screen Indicator */}
          <div className="w-full max-w-4xl mx-auto mb-16 relative flex flex-col items-center">
            <div className="w-3/4 h-24 border-t-4 border-(--primary) rounded-[50%] opacity-20 drop-shadow-[0_-15px_15px_rgba(var(--primary),0.3)]"></div>
            <div className="absolute top-4 text-xs font-semibold tracking-widest text-(--muted-foreground) flex items-center">
              <Monitor className="h-4 w-4 mr-2" /> ALL EYES THIS WAY
            </div>
          </div>

          <div className="w-full max-w-4xl mx-auto space-y-8 pb-32">
            {[...defaultSeatCategories].reverse().map(category => (
              <div key={category.name} className="space-y-4 pt-4 border-t border-(--border)/50 first:border-0 first:pt-0">
                <div className="text-xs font-semibold text-(--muted-foreground) tracking-wider text-center">
                  Rs. {category.price} {category.name}
                </div>
                <div className="space-y-3">
                  {category.rows.map(row => renderRow(row, 20, [4, 16]))}
                </div>
              </div>
            ))}
          </div>
        </div>

        {/* Action Sidebar / Bottom Bar */}
        <div className={cn(
          "w-full lg:w-96 bg-(--background) border-t lg:border-t-0 lg:border-l border-(--border) shadow-2xl lg:shadow-none p-6 transition-all duration-300 z-50 fixed lg:relative bottom-0",
          selectedSeats.length > 0 ? "translate-y-0" : "translate-y-full lg:translate-y-0"
        )}>
          {selectedSeats.length > 0 ? (
            <div className="h-full flex flex-col justify-between space-y-6">
              <div>
                <h3 className="text-lg font-bold mb-4">Booking Summary</h3>
                <div className="space-y-3 text-sm">
                  <div className="flex justify-between items-center text-(--muted-foreground)">
                    <span>Tickets ({selectedSeats.length})</span>
                    <span className="font-medium text-(--foreground)">{selectedSeats.join(", ")}</span>
                  </div>
                  <div className="flex justify-between items-center bg-(--muted)/50 p-3 rounded-md border border-(--border)">
                    <span className="font-semibold text-(--foreground)">Subtotal</span>
                    <span className="font-bold text-lg">Rs. {getTotalAmount()}</span>
                  </div>
                </div>
              </div>
              <Link href="/checkout" className="w-full">
                <Button size="lg" className="w-full text-base font-bold shadow-xl">
                  Proceed to Checkout
                </Button>
              </Link>
            </div>
          ) : (
            <div className="h-full flex flex-col items-center justify-center text-(--muted-foreground) min-h-[200px] text-center">
              <div className="w-16 h-16 rounded-full bg-(--muted) flex items-center justify-center mb-4">
                <Monitor className="h-8 w-8 opacity-50" />
              </div>
              <p className="font-medium">Please select your seats above</p>
              <p className="text-sm mt-1">Select up to 10 seats</p>
            </div>
          )}
        </div>
      </div>
    </div>
  )
}

import { Download, ExternalLink, QrCode, Ticket } from "lucide-react"
import { Button } from "@/components/ui/Button"
import { userActiveTickets, userPastTickets, getMovieDetails } from "@/lib/mockData"

export default function TicketsPage() {
  
  const getPoster = (ticketMovieTitle: string) => {
    return getMovieDetails("1").posterUrl
  }

  const renderTicket = (ticket: any, isActive: boolean) => (
    <div key={ticket.id} className={`flex flex-col md:flex-row border rounded-2xl overflow-hidden ${isActive ? 'border-(--border) bg-(--background)' : 'border-(--border)/50 bg-(--muted)/20 opacity-70'}`}>
      
      {/* Poster Section */}
      <div className="w-full md:w-32 h-40 bg-(--muted) shrink-0 overflow-hidden">
        <img src={getPoster(ticket.movie)} alt="Poster" className="w-full h-full object-cover" />
      </div>

      {/* Info Section */}
      <div className="flex-1 p-5 flex flex-col justify-between">
        <div>
          <div className="flex justify-between items-start">
            <h3 className="font-bold text-lg leading-tight line-clamp-1">{ticket.movie}</h3>
            {isActive && <span className="bg-green-500/10 text-green-500 text-xs font-bold px-2 py-1 rounded">UPCOMING</span>}
          </div>
          <p className="text-sm text-(--muted-foreground) mt-1">{ticket.date} • {ticket.time}</p>
          <p className="text-sm text-(--muted-foreground)">{ticket.location}</p>
        </div>

        <div className="grid grid-cols-2 lg:grid-cols-4 gap-4 mt-4 text-sm bg-(--muted)/50 p-3 rounded-xl border border-(--border)">
          <div>
            <p className="text-xs text-(--muted-foreground) uppercase mb-1">Seats</p>
            <p className="font-semibold">{ticket.seats}</p>
          </div>
          <div>
            <p className="text-xs text-(--muted-foreground) uppercase mb-1">Screen</p>
            <p className="font-semibold">{ticket.screen}</p>
          </div>
          <div className="hidden lg:block">
            <p className="text-xs text-(--muted-foreground) uppercase mb-1">Booking ID</p>
            <p className="font-semibold">{ticket.id}</p>
          </div>
          <div className="hidden lg:block text-right">
            <p className="text-xs text-(--muted-foreground) uppercase mb-1">Total</p>
            <p className="font-semibold">Rs. {ticket.amount}</p>
          </div>
        </div>
      </div>

      {/* QR & Action Section */}
      <div className={`w-full md:w-48 p-5 flex flex-col items-center justify-center border-t md:border-t-0 md:border-l ${isActive ? 'border-dashed border-(--muted-foreground)/30 relative' : 'border-(--border)'}`}>
        
        {isActive && (
          <>
            <div className="absolute -left-3 top-1/2 -translate-y-1/2 w-6 h-6 rounded-full bg-(--background) hidden md:block"></div>
            <div className="absolute -top-3 left-1/2 -translate-x-1/2 w-6 h-6 rounded-full bg-(--background) md:hidden"></div>
            <div className="absolute -bottom-3 left-1/2 -translate-x-1/2 w-6 h-6 rounded-full bg-(--background) md:hidden"></div>
          </>
        )}

        <div className="w-24 h-24 bg-white rounded-lg p-2 flex items-center justify-center mb-4">
          <QrCode className="w-full h-full text-black" strokeWidth={1.5} />
        </div>
        
        {isActive ? (
          <Button variant="outline" size="sm" className="w-full text-xs h-8">
            <Download className="w-3 h-3 mr-2" /> Download E-Ticket
          </Button>
        ) : (
          <Button variant="ghost" size="sm" className="w-full text-xs h-8 text-(--muted-foreground)">
            <ExternalLink className="w-3 h-3 mr-2" /> View Details
          </Button>
        )}
      </div>
    </div>
  )

  return (
    <div className="space-y-8">
      <div>
        <h1 className="text-2xl font-bold tracking-tight">Active Bookings</h1>
        <p className="text-(--muted-foreground) text-sm mt-1">View and manage your upcoming movie plans.</p>
      </div>

      <div className="space-y-4">
        {userActiveTickets.map(ticket => renderTicket(ticket, true))}
        {userActiveTickets.length === 0 && (
          <div className="text-center py-12 border border-(--border) border-dashed rounded-2xl">
            <Ticket className="w-12 h-12 text-(--muted-foreground) mx-auto mb-4 opacity-50" />
            <h3 className="font-medium text-lg">No active bookings</h3>
            <p className="text-(--muted-foreground) text-sm mb-4">Look likes you haven't booked a movie recently.</p>
            <Button>Book a ticket now</Button>
          </div>
        )}
      </div>

      <div className="pt-8">
        <h2 className="text-xl font-bold tracking-tight mb-4">Past Bookings</h2>
        <div className="space-y-4">
          {userPastTickets.map(ticket => renderTicket(ticket, false))}
        </div>
      </div>
    </div>
  )
}

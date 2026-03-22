"use client"

import Link from "next/link"
import { useSearchParams } from "next/navigation"
import { CheckCircle2, XCircle, ArrowRight, Download, Share2 } from "lucide-react"
import { Button } from "@/components/ui/Button"

export default function CheckoutStatusPage() {
  const searchParams = useSearchParams()
  const status = searchParams.get('status')
  
  const isSuccess = status !== "failure"

  if (!isSuccess) {
    return (
      <div className="flex flex-col items-center justify-center min-h-[calc(100vh-14rem)] px-4 text-center">
        <div className="w-24 h-24 bg-red-500/10 text-red-500 rounded-full flex items-center justify-center mb-6">
          <XCircle className="w-12 h-12" />
        </div>
        <h1 className="text-3xl font-bold tracking-tight mb-2">Payment Failed</h1>
        <p className="text-(--muted-foreground) max-w-md mx-auto mb-8">
          We couldn't process your payment. Please try again with a different payment method or contact your bank.
        </p>
        <div className="flex items-center space-x-4">
          <Link href="/checkout"><Button size="lg">Try Again</Button></Link>
          <Link href="/"><Button variant="outline" size="lg">Back to Home</Button></Link>
        </div>
      </div>
    )
  }

  // Success Layout
  return (
    <div className="flex flex-col items-center justify-center min-h-[calc(100vh-14rem)] py-12 px-4">
      <div className="w-full max-w-lg bg-(--background) border border-(--border) rounded-3xl p-8 shadow-2xl relative overflow-hidden">
        
        {/* Decorative background element */}
        <div className="absolute top-0 left-0 w-full h-32 bg-gradient-to-b from-green-500/20 to-transparent" />
        
        <div className="relative z-10 text-center mb-8 pt-4">
          <div className="w-20 h-20 bg-green-500 text-white shadow-lg shadow-green-500/30 rounded-full flex items-center justify-center mx-auto mb-6">
            <CheckCircle2 className="w-10 h-10" />
          </div>
          <h1 className="text-3xl font-extrabold tracking-tight">Booking Confirmed!</h1>
          <p className="text-sm font-medium text-(--muted-foreground) mt-2">Booking ID: #MTK-892401-BK</p>
        </div>

        <div className="relative z-10 bg-(--muted)/30 rounded-2xl p-6 border border-(--border) border-dashed mb-8">
          <h2 className="font-bold text-xl mb-4 text-center">Godzilla x Kong: The New Empire</h2>
          
          <div className="grid grid-cols-2 gap-y-6 text-sm">
            <div>
              <p className="text-(--muted-foreground) text-xs uppercase tracking-wider mb-1">Date</p>
              <p className="font-semibold">Today, 29 Mar</p>
            </div>
            <div>
              <p className="text-(--muted-foreground) text-xs uppercase tracking-wider mb-1">Time</p>
              <p className="font-semibold">07:30 PM (1h 55m)</p>
            </div>
            <div className="col-span-2">
              <p className="text-(--muted-foreground) text-xs uppercase tracking-wider mb-1">Theatre</p>
              <p className="font-semibold">Cinépolis: VIP, Viviana Mall, Thane</p>
            </div>
            <div className="col-span-2">
              <p className="text-(--muted-foreground) text-xs uppercase tracking-wider mb-1">Seats (3)</p>
              <div className="flex flex-wrap gap-2 mt-1">
                <span className="px-2 py-1 bg-(--primary) text-(--primary-foreground) rounded-md text-xs font-bold shadow-sm">RECLINER-P13</span>
                <span className="px-2 py-1 bg-(--primary) text-(--primary-foreground) rounded-md text-xs font-bold shadow-sm">RECLINER-P14</span>
                <span className="px-2 py-1 bg-(--primary) text-(--primary-foreground) rounded-md text-xs font-bold shadow-sm">RECLINER-P15</span>
              </div>
            </div>
          </div>
        </div>

        <div className="relative z-10 flex flex-col space-y-3">
          <Link href="/dashboard/tickets">
            <Button size="lg" className="w-full text-base h-12 shadow-md">
              <Download className="w-5 h-5 mr-2" /> View M-Ticket
            </Button>
          </Link>
          <div className="flex gap-3">
            <Button variant="outline" className="flex-1 h-12">
              <Share2 className="w-4 h-4 mr-2" /> Share Details
            </Button>
            <Link href="/" className="flex-1">
              <Button variant="ghost" className="w-full h-12 hover:bg-(--muted)">
                Go to Home
              </Button>
            </Link>
          </div>
        </div>

        {/* Perforation detail for ticket aesthetic */}
        <div className="absolute top-[40%] -left-4 w-8 h-8 rounded-full bg-black/50 md:bg-(--background)" />
        <div className="absolute top-[40%] -right-4 w-8 h-8 rounded-full bg-black/50 md:bg-(--background)" />
        <div className="absolute top-[40%] left-4 right-4 h-px border-b-2 border-dashed border-(--muted)" />
      </div>
    </div>
  )
}

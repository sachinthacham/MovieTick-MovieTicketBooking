"use client"

import * as React from "react"
import Link from "next/link"
import { ArrowLeft, Clock, MapPin, Tag, CreditCard, Apple, FileText, CheckCircle2 } from "lucide-react"
import { Button } from "@/components/ui/Button"
import { Input } from "@/components/ui/Input"
import { getMovieDetails } from "@/lib/mockData"

export default function CheckoutPage() {
  const movie = getMovieDetails("1")
  const [promoCode, setPromoCode] = React.useState("")
  const [isPromoApplied, setIsPromoApplied] = React.useState(false)

  const ticketPrice = 1350 // (3 tickets * 450)
  const convenienceFee = 120
  const discount = isPromoApplied ? 200 : 0
  const totalAmount = ticketPrice + convenienceFee - discount

  const applyPromo = () => {
    if (promoCode.trim().length > 3) {
      setIsPromoApplied(true)
    }
  }

  return (
    <div className="container mx-auto px-4 py-8 md:px-8 max-w-6xl min-h-[calc(100vh-14rem)]">
      
      {/* Header */}
      <div className="flex items-center space-x-4 mb-8">
        <Button variant="ghost" size="icon" className="rounded-full hidden md:flex" asChild>
          <Link href="/movie/1/seats"><ArrowLeft className="h-5 w-5" /></Link>
        </Button>
        <h1 className="text-3xl font-extrabold tracking-tight">Checkout</h1>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
        
        {/* Left Column: Summary & Promos */}
        <div className="lg:col-span-2 space-y-8">
          
          {/* Booking Summary Section */}
          <section className="bg-(--background) border border-(--border) rounded-2xl p-6 md:p-8">
            <h2 className="text-xl font-bold mb-6">Booking Details</h2>
            
            <div className="flex flex-col sm:flex-row gap-6">
              <div className="w-24 h-36 bg-(--muted) rounded-lg shrink-0 overflow-hidden">
                <img 
                  src={movie.posterUrl} 
                  alt="Poster" 
                  className="w-full h-full object-cover" 
                />
              </div>
              
              <div className="space-y-4 flex-1">
                <div>
                  <h3 className="text-2xl font-bold">{movie.title}</h3>
                  <div className="flex flex-wrap items-center gap-x-4 gap-y-2 mt-2 text-sm text-(--muted-foreground)">
                    <span className="flex items-center font-medium bg-(--muted) px-2 py-1 rounded text-(--foreground)"><span className="px-1 border border-(--border) rounded text-[10px] mr-2">UA</span> English, 2D</span>
                  </div>
                </div>

                <div className="grid grid-cols-2 gap-4 pt-4 border-t border-(--border)">
                  <div className="space-y-1">
                    <p className="text-xs text-(--muted-foreground) uppercase tracking-wide">Date & Time</p>
                    <p className="font-semibold text-sm flex items-center pr-2"><Clock className="w-4 h-4 mr-2" /> Today, 07:30 PM</p>
                  </div>
                  <div className="space-y-1">
                    <p className="text-xs text-(--muted-foreground) uppercase tracking-wide">Theatre</p>
                    <p className="font-semibold text-sm flex items-center pr-2 line-clamp-2"><MapPin className="w-4 h-4 mr-2 shrink-0" /> Cinépolis: VIP, Thane</p>
                  </div>
                  <div className="space-y-1 col-span-2 mt-2">
                    <p className="text-xs text-(--muted-foreground) uppercase tracking-wide">Selected Seats</p>
                    <p className="font-semibold text-sm bg-(--primary)/10 text-(--primary) inline-flex px-3 py-1.5 rounded-md mt-1">
                      RECLINER - P13, P14, P15 (3 Tickets)
                    </p>
                  </div>
                </div>
              </div>
            </div>
          </section>

          {/* Contact Details */}
          <section className="bg-(--background) border border-(--border) rounded-2xl p-6 md:p-8 space-y-4">
            <h2 className="text-xl font-bold mb-2">Contact Details</h2>
            <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
              <div>
                <label className="text-sm font-medium">Email Address</label>
                <Input type="email" placeholder="john@example.com" defaultValue="user@example.com" className="mt-1" />
              </div>
              <div>
                <label className="text-sm font-medium">Phone Number</label>
                <Input type="tel" placeholder="+91 9876543210" defaultValue="+91 9876543210" className="mt-1" />
              </div>
            </div>
            <p className="text-xs text-(--muted-foreground) mt-2">Your tickets will be sent to these contact details.</p>
          </section>

        </div>

        {/* Right Column: Order Total & Payment */}
        <div className="lg:col-span-1 space-y-6">
          
          <div className="bg-(--background) border border-(--border) rounded-2xl p-6 sticky top-24">
            
            {/* Promo Code */}
            <div className="mb-6">
              <label className="text-sm font-medium flex items-center mb-2">
                <Tag className="w-4 h-4 mr-2" /> Offers & Promocodes
              </label>
              <div className="flex space-x-2">
                <Input 
                  placeholder="Enter Code" 
                  value={promoCode} 
                  onChange={(e) => setPromoCode(e.target.value)}
                  disabled={isPromoApplied} 
                  className="uppercase font-medium"
                />
                <Button 
                  variant={isPromoApplied ? "destructive" : "secondary"} 
                  onClick={() => isPromoApplied ? setIsPromoApplied(false) : applyPromo()}
                >
                  {isPromoApplied ? "Remove" : "Apply"}
                </Button>
              </div>
              {isPromoApplied && (
                <p className="text-green-500 text-xs mt-2 flex items-center font-medium">
                  <CheckCircle2 className="w-3 h-3 mr-1" /> Promo applied successfully (-Rs. 200)
                </p>
              )}
            </div>

            <div className="border-t border-(--border) my-6" />

            {/* Price Breakdown */}
            <h3 className="font-bold text-lg mb-4">Payment Summary</h3>
            <div className="space-y-3 text-sm">
              <div className="flex justify-between text-(--muted-foreground)">
                <span>Tickets (3 x Rs. 450)</span>
                <span className="font-medium text-(--foreground)">Rs. {ticketPrice}</span>
              </div>
              <div className="flex justify-between text-(--muted-foreground)">
                <span>Convenience Fees</span>
                <span className="font-medium text-(--foreground)">Rs. {convenienceFee}</span>
              </div>
              {isPromoApplied && (
                <div className="flex justify-between text-green-500 font-medium">
                  <span>Discount</span>
                  <span>- Rs. {discount}</span>
                </div>
              )}
            </div>
            
            <div className="border-t border-(--border) my-4 border-dashed" />
            
            <div className="flex justify-between items-center bg-(--muted)/50 p-4 rounded-xl border border-(--border)">
              <span className="font-bold text-lg">Total Amount</span>
              <span className="font-extrabold text-2xl text-(--primary)">Rs. {totalAmount}</span>
            </div>

            <div className="mt-8 space-y-3">
              <p className="text-xs uppercase tracking-wider text-(--muted-foreground) font-semibold mb-2">Select Payment Method</p>
              
              <Link href="/checkout/status?status=success" className="block w-full">
                <Button size="lg" className="w-full text-base font-bold shadow-xl h-14">
                  <CreditCard className="w-5 h-5 mr-2" /> Pay with Card
                </Button>
              </Link>
              
              <Link href="/checkout/status?status=success" className="block w-full">
                <Button size="lg" variant="outline" className="w-full text-base font-bold h-14 border-(--border)">
                  <Apple className="w-5 h-5 mr-2" /> Apple Pay
                </Button>
              </Link>
            </div>
            <p className="text-[10px] text-center text-(--muted-foreground) mt-4 px-4 leading-relaxed">
              By proceeding, you agree to MovieTick's <span className="underline cursor-pointer">Terms & Conditions</span> and <span className="underline cursor-pointer">Cancellation Policy</span>.
            </p>
          </div>
          
        </div>
      </div>
    </div>
  )
}

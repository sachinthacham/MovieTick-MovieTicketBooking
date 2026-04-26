'use client'

import * as React from 'react'
import { Suspense } from 'react'
import Link from 'next/link'
import { useRouter } from 'next/navigation'
import { ArrowLeft, Clock, MapPin, CreditCard, Loader2, CheckCircle2, Lock, Film } from 'lucide-react'
import { Button } from '@/components/ui/Button'
import { Input } from '@/components/ui/Input'
import { useAuthStore } from '@/lib/stores/authStore'
import { bookingsApi } from '@/lib/api/bookings'
import { useToast } from '@/components/ui/Toast'
import type { ShowtimeSeat, CreateBookingResult } from '@/lib/types'

interface BookingContext {
  movieId: string
  showtimeId: string
  sessionId: string
  seatIds: string[]
  seatDetails: ShowtimeSeat[]
  totalAmount: number
  movieTitle?: string
  theaterName?: string
  screenName?: string
  showtimeStart?: string
  showFormatName?: string
}

function CheckoutContent() {
  const router = useRouter()
  const { user } = useAuthStore()
  const { toast } = useToast()

  const [context, setContext] = React.useState<BookingContext | null>(null)
  const [step, setStep] = React.useState<'review' | 'payment' | 'processing'>('review')
  const [cardNumber, setCardNumber] = React.useState('')
  const [cardExpiry, setCardExpiry] = React.useState('')
  const [cardCvc, setCardCvc] = React.useState('')
  const [cardName, setCardName] = React.useState(user ? `${user.firstName} ${user.lastName}` : '')
  const [bookingResult, setBookingResult] = React.useState<CreateBookingResult | null>(null)
  const [isSubmitting, setIsSubmitting] = React.useState(false)

  React.useEffect(() => {
    const stored = sessionStorage.getItem('booking-context')
    if (!stored) {
      router.push('/')
      return
    }
    try {
      setContext(JSON.parse(stored))
    } catch {
      router.push('/')
    }
  }, [router])

  const formatCard = (val: string) => {
    const digits = val.replace(/\D/g, '').slice(0, 16)
    return digits.replace(/(.{4})/g, '$1 ').trim()
  }

  const formatExpiry = (val: string) => {
    const digits = val.replace(/\D/g, '').slice(0, 4)
    if (digits.length >= 3) return `${digits.slice(0, 2)}/${digits.slice(2)}`
    return digits
  }

  const handleCreateBooking = async () => {
    if (!context) return
    if (!cardNumber || !cardExpiry || !cardCvc || !cardName) {
      toast({ title: 'Missing card details', description: 'Please fill in all payment fields.', variant: 'destructive' })
      return
    }

    setIsSubmitting(true)
    setStep('processing')

    try {
      const response = await bookingsApi.create({
        showtimeId: context.showtimeId,
        seatIds: context.seatIds,
        sessionId: context.sessionId,
      })

      const result = response.data.data
      setBookingResult(result)

      // In a production app, you would use Stripe.js to confirmPayment with the clientSecret.
      // For this demo, we simulate payment success by showing a success state.
      // The Stripe webhook would need to be triggered in production.
      
      // Clear session storage
      sessionStorage.removeItem('booking-context')
      sessionStorage.removeItem('seat-session-id')

      // Redirect to success page with booking info
      router.push(`/checkout/status?bookingId=${result.bookingId}&reference=${result.bookingReference}&amount=${result.totalAmount}`)
    } catch (err: any) {
      setStep('payment')
      const message = err?.response?.data?.message || 'Booking failed. Please try again.'
      toast({ title: 'Booking failed', description: message, variant: 'destructive' })
    } finally {
      setIsSubmitting(false)
    }
  }

  if (!context) {
    return (
      <div className="flex items-center justify-center min-h-[calc(100vh-14rem)]">
        <Loader2 className="h-8 w-8 animate-spin text-(--muted-foreground)" />
      </div>
    )
  }

  const totalWithFee = context.totalAmount + Math.round(context.totalAmount * 0.05)

  if (step === 'processing') {
    return (
      <div className="flex flex-col items-center justify-center min-h-[calc(100vh-14rem)] space-y-6">
        <div className="w-20 h-20 rounded-full bg-(--primary)/10 flex items-center justify-center">
          <Loader2 className="h-10 w-10 animate-spin text-(--primary)" />
        </div>
        <div className="text-center">
          <h2 className="text-2xl font-bold">Processing Payment</h2>
          <p className="text-(--muted-foreground) mt-2">Please wait while we confirm your booking...</p>
        </div>
      </div>
    )
  }

  const showtimeFormatted = context.showtimeStart
    ? new Date(context.showtimeStart).toLocaleString([], {
        weekday: 'short', month: 'short', day: 'numeric',
        hour: '2-digit', minute: '2-digit',
      })
    : 'N/A'

  return (
    <div className="container mx-auto px-4 py-8 md:px-8 max-w-6xl min-h-[calc(100vh-14rem)]">
      <div className="flex items-center space-x-4 mb-8">
        <Button variant="ghost" size="icon" className="rounded-full hidden md:flex" asChild>
          <Link href={`/movie/${context.movieId}/seats?showtimeId=${context.showtimeId}`}>
            <ArrowLeft className="h-5 w-5" />
          </Link>
        </Button>
        <div>
          <h1 className="text-3xl font-extrabold tracking-tight">Checkout</h1>
          <p className="text-sm text-(--muted-foreground) mt-1">Review your booking and complete payment</p>
        </div>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
        {/* Left — Booking Details & Payment Form */}
        <div className="lg:col-span-2 space-y-6">
          {/* Booking Summary */}
          <section className="bg-(--background) border border-(--border) rounded-2xl p-6 md:p-8">
            <h2 className="text-xl font-bold mb-6 flex items-center">
              <Film className="h-5 w-5 mr-2 text-(--primary)" /> Booking Details
            </h2>
            <div className="space-y-4">
              <div>
                <h3 className="text-2xl font-bold">{context.movieTitle}</h3>
                {context.showFormatName && (
                  <span className="inline-block mt-1 text-xs font-medium bg-(--muted) px-2 py-1 rounded">
                    {context.showFormatName}
                  </span>
                )}
              </div>
              <div className="grid grid-cols-2 gap-4 pt-4 border-t border-(--border)">
                <div className="space-y-1">
                  <p className="text-xs text-(--muted-foreground) uppercase tracking-wide">Date & Time</p>
                  <p className="font-semibold text-sm flex items-center">
                    <Clock className="w-4 h-4 mr-2 shrink-0" /> {showtimeFormatted}
                  </p>
                </div>
                <div className="space-y-1">
                  <p className="text-xs text-(--muted-foreground) uppercase tracking-wide">Theatre & Screen</p>
                  <p className="font-semibold text-sm flex items-start">
                    <MapPin className="w-4 h-4 mr-2 shrink-0 mt-0.5" />
                    <span>{context.theaterName}<br /><span className="text-(--muted-foreground) font-normal">{context.screenName}</span></span>
                  </p>
                </div>
                <div className="col-span-2 space-y-1">
                  <p className="text-xs text-(--muted-foreground) uppercase tracking-wide">Selected Seats</p>
                  <div className="flex flex-wrap gap-2 mt-1">
                    {context.seatDetails.map((seat) => (
                      <span key={seat.id} className="px-2 py-1 bg-(--primary)/10 text-(--primary) rounded-md text-xs font-bold">
                        {seat.seatCategoryName} - {seat.rowLabel}{seat.seatNumber}
                      </span>
                    ))}
                  </div>
                </div>
              </div>
            </div>
          </section>

          {/* Payment Form */}
          {step === 'payment' ? (
            <section className="bg-(--background) border border-(--border) rounded-2xl p-6 md:p-8 space-y-5">
              <h2 className="text-xl font-bold flex items-center">
                <Lock className="h-5 w-5 mr-2 text-(--primary)" /> Secure Payment
              </h2>
              <p className="text-sm text-(--muted-foreground)">
                Your payment is encrypted and secure. We use industry-standard SSL encryption.
              </p>
              <div className="space-y-4">
                <div>
                  <label className="text-sm font-medium">Cardholder Name</label>
                  <Input
                    value={cardName}
                    onChange={(e) => setCardName(e.target.value)}
                    placeholder="John Doe"
                    className="mt-1"
                  />
                </div>
                <div>
                  <label className="text-sm font-medium">Card Number</label>
                  <Input
                    value={cardNumber}
                    onChange={(e) => setCardNumber(formatCard(e.target.value))}
                    placeholder="1234 5678 9012 3456"
                    maxLength={19}
                    className="mt-1 font-mono"
                  />
                </div>
                <div className="grid grid-cols-2 gap-4">
                  <div>
                    <label className="text-sm font-medium">Expiry Date</label>
                    <Input
                      value={cardExpiry}
                      onChange={(e) => setCardExpiry(formatExpiry(e.target.value))}
                      placeholder="MM/YY"
                      maxLength={5}
                      className="mt-1 font-mono"
                    />
                  </div>
                  <div>
                    <label className="text-sm font-medium">CVC</label>
                    <Input
                      value={cardCvc}
                      onChange={(e) => setCardCvc(e.target.value.replace(/\D/g, '').slice(0, 4))}
                      placeholder="123"
                      maxLength={4}
                      className="mt-1 font-mono"
                    />
                  </div>
                </div>
                <div className="flex items-center text-xs text-(--muted-foreground) mt-2">
                  <Lock className="h-3 w-3 mr-1" /> All transactions are secure and encrypted.
                </div>
              </div>
              <Button
                size="lg"
                className="w-full text-base font-bold shadow-xl h-14 mt-2"
                onClick={handleCreateBooking}
                disabled={isSubmitting}
              >
                {isSubmitting ? (
                  <><Loader2 className="h-4 w-4 animate-spin mr-2" />Processing...</>
                ) : (
                  <><CreditCard className="h-5 w-5 mr-2" />Pay Rs. {totalWithFee}</>
                )}
              </Button>
            </section>
          ) : (
            <section className="bg-(--background) border border-(--border) rounded-2xl p-6 md:p-8 space-y-4">
              <h2 className="text-xl font-bold">Contact Details</h2>
              <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
                <div>
                  <label className="text-sm font-medium">Email Address</label>
                  <Input type="email" value={user?.email ?? ''} readOnly className="mt-1 bg-(--muted)/50" />
                </div>
                <div>
                  <label className="text-sm font-medium">Name</label>
                  <Input value={`${user?.firstName ?? ''} ${user?.lastName ?? ''}`} readOnly className="mt-1 bg-(--muted)/50" />
                </div>
              </div>
              <p className="text-xs text-(--muted-foreground)">Your tickets will be sent to this email address.</p>
            </section>
          )}
        </div>

        {/* Right — Order Total */}
        <div className="lg:col-span-1">
          <div className="bg-(--background) border border-(--border) rounded-2xl p-6 sticky top-24 space-y-4">
            <h3 className="font-bold text-lg">Payment Summary</h3>
            <div className="space-y-3 text-sm">
              {context.seatDetails.map((seat) => (
                <div key={seat.id} className="flex justify-between text-(--muted-foreground)">
                  <span>{seat.seatCategoryName} ({seat.rowLabel}{seat.seatNumber})</span>
                  <span className="font-medium text-(--foreground)">Rs. {seat.price}</span>
                </div>
              ))}
              <div className="flex justify-between text-(--muted-foreground)">
                <span>Convenience Fee (5%)</span>
                <span className="font-medium text-(--foreground)">Rs. {Math.round(context.totalAmount * 0.05)}</span>
              </div>
            </div>
            <div className="border-t border-dashed border-(--border) my-2" />
            <div className="flex justify-between items-center bg-(--muted)/50 p-4 rounded-xl border border-(--border)">
              <span className="font-bold text-lg">Total Amount</span>
              <span className="font-extrabold text-2xl text-(--primary)">Rs. {totalWithFee}</span>
            </div>

            {step === 'review' && (
              <Button
                size="lg"
                className="w-full text-base font-bold shadow-xl h-14 mt-4"
                onClick={() => setStep('payment')}
              >
                <CreditCard className="h-5 w-5 mr-2" /> Proceed to Pay
              </Button>
            )}

            <div className="flex items-center justify-center space-x-2 mt-4">
              <CheckCircle2 className="h-4 w-4 text-green-500" />
              <span className="text-xs text-(--muted-foreground)">Instant booking confirmation</span>
            </div>
            <p className="text-[10px] text-center text-(--muted-foreground) px-2 leading-relaxed">
              By proceeding, you agree to MovieTick's Terms & Conditions and Cancellation Policy.
            </p>
          </div>
        </div>
      </div>
    </div>
  )
}

export default function CheckoutPage() {
  return (
    <Suspense fallback={
      <div className="flex items-center justify-center min-h-[calc(100vh-14rem)]">
        <Loader2 className="h-8 w-8 animate-spin text-(--muted-foreground)" />
      </div>
    }>
      <CheckoutContent />
    </Suspense>
  )
}

import Link from "next/link"
import { Button } from "@/components/ui/Button"
import { Input } from "@/components/ui/Input"
import { ArrowLeft } from "lucide-react"

export default function ForgotPasswordPage() {
  return (
    <div className="flex min-h-[calc(100vh-14rem)] items-center justify-center py-12 px-4 sm:px-6 lg:px-8">
      <div className="w-full max-w-md space-y-8 rounded-2xl border border-(--border) bg-(--background) p-8 shadow-xl">
        <div>
          <Link href="/auth/login" className="inline-flex items-center text-sm font-medium text-(--muted-foreground) hover:text-(--foreground) mb-6 transition-colors">
            <ArrowLeft className="mr-2 h-4 w-4" />
            Back to login
          </Link>
          <h2 className="text-3xl font-bold tracking-tight text-(--foreground)">Reset password</h2>
          <p className="mt-2 text-sm text-(--muted-foreground)">
            Enter your email address and we'll send you a link to reset your password.
          </p>
        </div>

        <form className="mt-8 space-y-6">
          <div>
            <label htmlFor="email" className="block text-sm font-medium leading-6 text-(--foreground)">
              Email address
            </label>
            <div className="mt-2">
              <Input id="email" name="email" type="email" autoComplete="email" required placeholder="you@example.com" />
            </div>
          </div>

          <div>
            <Button type="button" className="w-full h-11 text-md">
              Send reset link
            </Button>
          </div>
        </form>
      </div>
    </div>
  )
}

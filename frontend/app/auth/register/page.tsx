import Link from "next/link"
import { Button } from "@/components/ui/Button"
import { Input } from "@/components/ui/Input"

export default function RegisterPage() {
  return (
    <div className="flex min-h-[calc(100vh-14rem)] items-center justify-center py-12 px-4 sm:px-6 lg:px-8">
      <div className="w-full max-w-md space-y-8 rounded-2xl border border-(--border) bg-(--background) p-8 shadow-xl">
        <div className="text-center">
          <h2 className="text-3xl font-bold tracking-tight text-(--foreground)">Create an account</h2>
          <p className="mt-2 text-sm text-(--muted-foreground)">
            Already have an account?{" "}
            <Link href="/auth/login" className="font-semibold text-(--primary) hover:underline">
              Sign in instead
            </Link>
          </p>
        </div>

        <form className="mt-8 space-y-6">
          <div className="space-y-4">
            <div>
              <label htmlFor="name" className="block text-sm font-medium leading-6 text-(--foreground)">
                Full Name
              </label>
              <div className="mt-2">
                <Input id="name" name="name" type="text" required placeholder="John Doe" />
              </div>
            </div>

            <div>
              <label htmlFor="email" className="block text-sm font-medium leading-6 text-(--foreground)">
                Email address
              </label>
              <div className="mt-2">
                <Input id="email" name="email" type="email" autoComplete="email" required placeholder="you@example.com" />
              </div>
            </div>

            <div>
              <label htmlFor="password" className="block text-sm font-medium leading-6 text-(--foreground)">
                Password
              </label>
              <div className="mt-2">
                <Input id="password" name="password" type="password" required />
              </div>
            </div>
            
            <div>
              <label htmlFor="confirm-password" className="block text-sm font-medium leading-6 text-(--foreground)">
                Confirm Password
              </label>
              <div className="mt-2">
                <Input id="confirm-password" name="confirm-password" type="password" required />
              </div>
            </div>
          </div>

          <div>
            {/* The user would normally route to OTP after this */}
            <Link href="/auth/otp" tabIndex={-1}>
              <Button type="button" className="w-full">
                Register & Verify
              </Button>
            </Link>
          </div>
        </form>
      </div>
    </div>
  )
}

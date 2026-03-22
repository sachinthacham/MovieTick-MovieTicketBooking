import Link from "next/link"
import { Button } from "@/components/ui/Button"
import { Input } from "@/components/ui/Input"

export default function LoginPage() {
  return (
    <div className="flex min-h-[calc(100vh-14rem)] items-center justify-center py-12 px-4 sm:px-6 lg:px-8">
      <div className="w-full max-w-md space-y-8 rounded-2xl border border-(--border) bg-(--background) p-8 shadow-xl">
        <div className="text-center">
          <h2 className="text-3xl font-bold tracking-tight text-(--foreground)">Welcome back</h2>
          <p className="mt-2 text-sm text-(--muted-foreground)">
            Don&apos;t have an account?{" "}
            <Link href="/auth/register" className="font-semibold text-(--primary) hover:underline">
              Create one now
            </Link>
          </p>
        </div>

        <div className="mt-8 space-y-6">
          <div className="space-y-4">
            <div>
              <label htmlFor="email" className="block text-sm font-medium leading-6 text-(--foreground)">
                Email address
              </label>
              <div className="mt-2">
                <Input id="email" name="email" type="email" autoComplete="email" required placeholder="you@example.com" />
              </div>
            </div>

            <div>
              <div className="flex items-center justify-between">
                <label htmlFor="password" className="block text-sm font-medium leading-6 text-(--foreground)">
                  Password
                </label>
                <div className="text-sm">
                  <Link href="/auth/forgot-password" className="font-semibold text-(--primary) hover:underline">
                    Forgot password?
                  </Link>
                </div>
              </div>
              <div className="mt-2">
                <Input id="password" name="password" type="password" autoComplete="current-password" required />
              </div>
            </div>
          </div>

          <div>
            <Button type="submit" className="w-full">
              Sign in
            </Button>
          </div>
        </div>

        <div className="mt-6 border-t border-(--border) pt-6">
          <div className="relative flex justify-center text-sm font-medium leading-6">
            <span className="bg-(--background) px-6 text-(--muted-foreground) absolute -top-3">Or continue with</span>
          </div>

          <div className="mt-6 grid grid-cols-2 gap-4">
            <Button variant="outline" className="w-full">
              Google
            </Button>
            <Button variant="outline" className="w-full">
              Facebook
            </Button>
          </div>
        </div>
      </div>
    </div>
  )
}

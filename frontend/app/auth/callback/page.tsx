'use client'

import * as React from 'react'
import { Suspense } from 'react'
import { useRouter, useSearchParams } from 'next/navigation'
import { Loader2 } from 'lucide-react'
import { useAuthStore } from '@/lib/stores/authStore'
import type { UserProfile } from '@/lib/types'

function CallbackContent() {
  const router = useRouter()
  const searchParams = useSearchParams()
  const { setAuth } = useAuthStore()

  React.useEffect(() => {
    const token = searchParams.get('token')
    const refreshToken = searchParams.get('refreshToken')
    const userId = searchParams.get('userId')
    const email = searchParams.get('email')
    const firstName = searchParams.get('firstName')
    const lastName = searchParams.get('lastName')
    const roles = searchParams.get('roles')?.split(',') ?? ['User']
    const profileImageUrl = searchParams.get('profileImageUrl') ?? undefined

    if (token && refreshToken && userId && email) {
      const profile: UserProfile = {
        id: userId,
        firstName: firstName ?? '',
        lastName: lastName ?? '',
        email,
        roles,
        profileImageUrl,
        createdAt: new Date().toISOString(),
      }
      setAuth(token, refreshToken, profile)
      document.cookie = `auth-token=${token}; path=/; max-age=86400`
      document.cookie = `auth-role=${roles.includes('Admin') ? 'Admin' : 'User'}; path=/; max-age=86400`
      router.replace('/')
    } else {
      router.replace('/auth/login?error=oauth_failed')
    }
  }, [searchParams, setAuth, router])

  return (
    <div className="flex min-h-[calc(100vh-14rem)] items-center justify-center">
      <div className="text-center space-y-4">
        <Loader2 className="h-10 w-10 animate-spin mx-auto text-(--primary)" />
        <p className="text-(--muted-foreground)">Signing you in...</p>
      </div>
    </div>
  )
}

export default function OAuthCallbackPage() {
  return (
    <Suspense fallback={
      <div className="flex min-h-[calc(100vh-14rem)] items-center justify-center">
        <Loader2 className="h-10 w-10 animate-spin mx-auto text-(--primary)" />
      </div>
    }>
      <CallbackContent />
    </Suspense>
  )
}

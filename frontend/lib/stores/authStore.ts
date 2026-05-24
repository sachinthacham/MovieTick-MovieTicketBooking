import { create } from 'zustand'
import { persist } from 'zustand/middleware'
import type { UserProfile } from '@/lib/types'

interface AuthState {
  token: string | null
  refreshToken: string | null
  user: UserProfile | null
  isAuthenticated: boolean
  isAdmin: boolean

  setAuth: (token: string, refreshToken: string, user: UserProfile) => void
  setUser: (user: UserProfile) => void
  logout: () => void
}

export const useAuthStore = create<AuthState>()(
  persist(
    (set) => ({
      token: null,
      refreshToken: null,
      user: null,
      isAuthenticated: false,
      isAdmin: false,

      setAuth: (token, refreshToken, user) =>
        set({
          token,
          refreshToken,
          user,
          isAuthenticated: true,
          isAdmin: user.roles?.includes('Admin') ?? false,
        }),

      setUser: (user) =>
        set((s) => ({
          user,
          isAdmin: user.roles?.includes('Admin') ?? s.isAdmin,
        })),

      logout: () =>
        set({
          token: null,
          refreshToken: null,
          user: null,
          isAuthenticated: false,
          isAdmin: false,
        }),
    }),
    { name: 'auth-storage' }
  )
)

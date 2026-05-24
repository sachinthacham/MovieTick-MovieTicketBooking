import type { UserProfile } from '@/lib/types'

/** Claim URIs used by ASP.NET Core JWT bearer (see backend JwtService). */
const CLAIM_NAME_ID = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'
const CLAIM_EMAIL = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'
const CLAIM_ROLE = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'

export function decodeJwtPayload(accessToken: string): Record<string, unknown> | null {
  try {
    const parts = accessToken.split('.')
    if (parts.length < 2) return null
    const base64 = parts[1].replace(/-/g, '+').replace(/_/g, '/')
    const padded = base64.padEnd(base64.length + ((4 - (base64.length % 4)) % 4), '=')
    const json = atob(padded)
    return JSON.parse(json) as Record<string, unknown>
  } catch {
    return null
  }
}

/** Minimal profile so the app works before /auth/profile loads (same claims as JWT). */
export function userProfileFromAccessToken(accessToken: string): UserProfile {
  const p = decodeJwtPayload(accessToken) ?? {}
  const id = String(p[CLAIM_NAME_ID] ?? p.sub ?? '')
  const email = String(p[CLAIM_EMAIL] ?? p.email ?? '')
  const role = String(p[CLAIM_ROLE] ?? p.role ?? 'User')
  return {
    id,
    firstName: '',
    lastName: '',
    email,
    roles: [role],
    createdAt: new Date().toISOString(),
  }
}

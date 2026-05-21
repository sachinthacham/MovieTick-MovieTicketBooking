import { NextResponse } from 'next/server'
import type { NextRequest } from 'next/server'

const PROTECTED_PREFIXES = ['/dashboard', '/profile']
const ADMIN_PREFIXES = ['/admin']
const AUTH_ONLY = ['/auth/login', '/auth/register', '/auth/forgot-password']

export function proxy(request: NextRequest) {
  const { pathname } = request.nextUrl

  // Read token from cookie (set by client after login)
  const token = request.cookies.get('auth-token')?.value
  const role = request.cookies.get('auth-role')?.value

  const isAuthed = Boolean(token)
  const isAdmin = role === 'Admin'

  // Redirect authenticated users away from auth pages
  if (isAuthed && AUTH_ONLY.some((p) => pathname.startsWith(p))) {
    return NextResponse.redirect(new URL('/', request.url))
  }

  // Protect user pages
  if (!isAuthed && PROTECTED_PREFIXES.some((p) => pathname.startsWith(p))) {
    const url = request.nextUrl.clone()
    url.pathname = '/auth/login'
    url.searchParams.set('redirect', pathname)
    return NextResponse.redirect(url)
  }

  // Protect admin pages
  if (ADMIN_PREFIXES.some((p) => pathname.startsWith(p))) {
    if (!isAuthed) {
      const url = request.nextUrl.clone()
      url.pathname = '/auth/login'
      url.searchParams.set('redirect', pathname)
      return NextResponse.redirect(url)
    }
    if (!isAdmin) {
      return NextResponse.redirect(new URL('/', request.url))
    }
  }

  return NextResponse.next()
}

export const config = {
  matcher: ['/dashboard/:path*', '/admin/:path*', '/profile/:path*', '/auth/:path*'],
}

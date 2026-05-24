import type { Movie } from '@/lib/types'

const PLACEHOLDER_POSTER =
  'https://images.unsplash.com/photo-1536440136628-849c177e76a1?w=500&h=750&fit=crop'

/** API origin without `/api/v1` — used to prefix relative upload paths */
export function getUploadsBase(): string {
  const api = process.env.NEXT_PUBLIC_API_URL ?? 'http://127.0.0.1:5050/api/v1'
  return api.replace(/\/api\/v1\/?$/, '')
}

/** Resolve a poster or other static asset path from the API (relative or absolute). */
export function resolveAssetUrl(
  raw: string | undefined | null,
  uploadsBase: string = getUploadsBase()
): string {
  if (raw == null || raw === '') return PLACEHOLDER_POSTER
  return raw.startsWith('http') ? raw : `${uploadsBase}${raw}`
}

/** Primary poster URL for a movie (`MoviePosterDto` uses `imageUrl`). */
export function getMoviePosterUrl(movie: Movie, uploadsBase?: string): string {
  const base = uploadsBase ?? getUploadsBase()
  const primary = movie.posters?.find((p) => p.isPrimary) ?? movie.posters?.[0]
  const raw = primary?.imageUrl ?? primary?.url
  return resolveAssetUrl(raw, base)
}

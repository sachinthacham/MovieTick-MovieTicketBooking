import type { UserProfile } from '@/lib/types'

/** Maps backend `UserProfileDto` (camelCase JSON) to frontend `UserProfile`. */
export function mapUserProfileFromApi(raw: Record<string, unknown>): UserProfile {
  const fullName = String(raw.fullName ?? '').trim()
  const parts = fullName.length > 0 ? fullName.split(/\s+/) : []
  const firstName = parts[0] ?? ''
  const lastName = parts.slice(1).join(' ') ?? ''

  const role = raw.role != null ? String(raw.role) : 'User'
  const created = raw.createdAt
  const createdAt =
    typeof created === 'string'
      ? created
      : created != null
        ? new Date(created as string | number | Date).toISOString()
        : new Date().toISOString()

  return {
    id: String(raw.id ?? ''),
    firstName,
    lastName,
    email: String(raw.email ?? ''),
    bio: raw.bio != null ? String(raw.bio) : undefined,
    dateOfBirth:
      raw.dateOfBirth != null
        ? typeof raw.dateOfBirth === 'string'
          ? raw.dateOfBirth
          : new Date(raw.dateOfBirth as string | number | Date).toISOString()
        : undefined,
    gender: raw.gender != null ? String(raw.gender) : undefined,
    profileImageUrl: raw.profileImageUrl != null ? String(raw.profileImageUrl) : undefined,
    roles: [role],
    createdAt,
  }
}

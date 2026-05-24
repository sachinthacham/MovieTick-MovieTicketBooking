import api from './axios'
import type {
  ApiResponse,
  AuthTokens,
  LoginDto,
  RegisterDto,
  RefreshTokenDto,
  RequestPasswordResetDto,
  ResetPasswordDto,
  ChangePasswordDto,
  UserProfile,
  UpdateProfileDto,
} from '@/lib/types'
import { mapUserProfileFromApi } from '@/lib/auth/mapUserProfile'

const BASE = '/auth'

function mapProfileResponse(res: ApiResponse<Record<string, unknown>>): ApiResponse<UserProfile> {
  const raw = res.data
  if (!raw || typeof raw !== 'object') {
    return { ...res, data: null as unknown as UserProfile }
  }
  return {
    ...res,
    data: mapUserProfileFromApi(raw as Record<string, unknown>),
  }
}

export const authApi = {
  login: async (dto: LoginDto): Promise<ApiResponse<AuthTokens>> => {
    const res = await api.post<ApiResponse<AuthTokens>>(`${BASE}/login`, dto).then((r) => r.data)
    return res
  },

  /**
   * Backend only returns the new user id; we sign the user in with a follow-up login.
   */
  register: async (dto: RegisterDto): Promise<ApiResponse<AuthTokens>> => {
    const fullName = `${dto.firstName} ${dto.lastName}`.trim()
    await api.post<ApiResponse<string>>(`${BASE}/register`, {
      fullName,
      email: dto.email,
      password: dto.password,
    })
    return authApi.login({ email: dto.email, password: dto.password })
  },

  refreshToken: (dto: RefreshTokenDto) =>
    api
      .post<ApiResponse<AuthTokens>>(`${BASE}/refresh-token`, { refreshToken: dto.refreshToken })
      .then((r) => r.data),

  requestPasswordReset: (dto: RequestPasswordResetDto) =>
    api.post<ApiResponse<{ resetToken: string }>>(`${BASE}/request-password-reset`, dto).then((r) => r.data),

  resetPassword: (dto: ResetPasswordDto) =>
    api.post<ApiResponse<null>>(`${BASE}/reset-password`, dto).then((r) => r.data),

  changePassword: (dto: ChangePasswordDto) =>
    api.post<ApiResponse<null>>(`${BASE}/change-password`, dto).then((r) => r.data),

  getProfile: async (): Promise<ApiResponse<UserProfile>> => {
    const res = await api.get<ApiResponse<Record<string, unknown>>>(`${BASE}/profile`).then((r) => r.data)
    return mapProfileResponse(res as ApiResponse<Record<string, unknown>>)
  },

  updateProfile: async (dto: UpdateProfileDto): Promise<ApiResponse<UserProfile>> => {
    const dob =
      dto.dateOfBirth && String(dto.dateOfBirth).trim() !== ''
        ? new Date(dto.dateOfBirth).toISOString()
        : undefined
    const res = await api
      .put<ApiResponse<Record<string, unknown>>>(`${BASE}/profile`, {
        fullName: `${dto.firstName} ${dto.lastName}`.trim(),
        bio: dto.bio || undefined,
        dateOfBirth: dob,
        gender: dto.gender || undefined,
      })
      .then((r) => r.data)
    return mapProfileResponse(res as ApiResponse<Record<string, unknown>>)
  },

  uploadProfileImage: (file: File) => {
    const form = new FormData()
    form.append('file', file)
    return api
      .post<ApiResponse<string>>(`${BASE}/profile/image`, form, {
        headers: { 'Content-Type': 'multipart/form-data' },
      })
      .then((r) => r.data)
  },
}

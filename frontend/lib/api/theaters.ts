import api from './axios'
import type {
  ApiResponse,
  Theater,
  CreateTheaterDto,
  UpdateTheaterDto,
  TheaterFacility,
  TheaterImage,
  PaginatedResult,
} from '@/lib/types'

const BASE = '/theaters'

/** Backend sends `facilityName` / `imageUrl`; UI expects `name` / `url`. */
export function normalizeTheater(raw: Record<string, unknown>): Theater {
  const facilities = (raw.facilities as Array<Record<string, unknown>> | undefined) ?? []
  const images = (raw.images as Array<Record<string, unknown>> | undefined) ?? []

  return {
    id: String(raw.id),
    name: String(raw.name ?? ''),
    address: String(raw.address ?? ''),
    city: String(raw.city ?? ''),
    state: String(raw.state ?? ''),
    country: String(raw.country ?? ''),
    zipCode: raw.zipCode as string | undefined,
    latitude: raw.latitude as number | undefined,
    longitude: raw.longitude as number | undefined,
    phoneNumber: raw.phoneNumber as string | undefined,
    email: raw.email as string | undefined,
    totalScreens: Number(raw.totalScreens ?? 0),
    averageRating: raw.averageRating != null ? Number(raw.averageRating) : 0,
    totalRatings: Number(raw.totalRatings ?? 0),
    isActive: Boolean(raw.isActive ?? true),
    facilities: facilities.map((f) => ({
      id: String(f.id),
      name: String(f.facilityName ?? f.name ?? ''),
      description: f.description as string | undefined,
      iconName: (f.icon ?? f.iconName) as string | undefined,
    })),
    images: images.map((i) => ({
      id: String(i.id),
      url: String(i.imageUrl ?? i.url ?? ''),
      caption: i.caption as string | undefined,
      displayOrder: Number(i.displayOrder ?? 0),
    })),
    createdAt: String(raw.createdAt ?? new Date().toISOString()),
  }
}

export const theatersApi = {
  /**
   * Backend returns a plain array on `data`, not `{ items, totalPages }`.
   * We normalize each row and apply search + pagination on the client.
   */
  getAll: async (params?: {
    search?: string
    city?: string
    isActive?: boolean
    pageNumber?: number
    pageSize?: number
  }): Promise<ApiResponse<PaginatedResult<Theater>>> => {
    const pageNumber = params?.pageNumber ?? 1
    const pageSize = params?.pageSize ?? 10
    const { search, city, isActive } = params ?? {}

    const resp = await api
      .get<ApiResponse<unknown[]>>(BASE, { params: { city, isActive } })
      .then((r) => r.data)

    const payload = resp.data
    const listRaw = Array.isArray(payload) ? payload : []
    let list = listRaw.map((row) => normalizeTheater(row as Record<string, unknown>))

    if (search?.trim()) {
      const q = search.trim().toLowerCase()
      list = list.filter(
        (t) =>
          t.name.toLowerCase().includes(q) ||
          t.city.toLowerCase().includes(q) ||
          t.address.toLowerCase().includes(q)
      )
    }

    const totalCount = list.length
    const totalPages = Math.max(1, Math.ceil(totalCount / pageSize) || 1)
    const start = (pageNumber - 1) * pageSize
    const items = list.slice(start, start + pageSize)

    return {
      success: resp.success,
      message: resp.message,
      data: {
        items,
        totalCount,
        pageNumber,
        pageSize,
        totalPages,
      },
      errors: resp.errors,
    }
  },

  getCities: () => api.get<ApiResponse<string[]>>(`${BASE}/cities`).then((r) => r.data),

  getById: (id: string) =>
    api.get<ApiResponse<Record<string, unknown>>>(`${BASE}/${id}`).then((r) => ({
      ...r.data,
      data: normalizeTheater(r.data.data as Record<string, unknown>),
    })),

  create: (dto: CreateTheaterDto) =>
    api.post<ApiResponse<Record<string, unknown>>>(BASE, dto).then((r) => ({
      ...r.data,
      data: normalizeTheater(r.data.data as Record<string, unknown>),
    })),

  update: (id: string, dto: UpdateTheaterDto) =>
    api.put<ApiResponse<Record<string, unknown>>>(`${BASE}/${id}`, dto).then((r) => ({
      ...r.data,
      data: normalizeTheater(r.data.data as Record<string, unknown>),
    })),

  delete: (id: string) => api.delete<ApiResponse<null>>(`${BASE}/${id}`).then((r) => r.data),

  addFacility: (theaterId: string, dto: Omit<TheaterFacility, 'id'>) =>
    api
      .post<ApiResponse<TheaterFacility>>(`${BASE}/${theaterId}/facilities`, {
        facilityName: dto.name,
        icon: dto.iconName,
      })
      .then((r) => r.data),

  deleteFacility: (_theaterId: string, facilityId: string) =>
    api.delete<ApiResponse<null>>(`${BASE}/facilities/${facilityId}`).then((r) => r.data),

  uploadImage: (theaterId: string, file: File, caption?: string) => {
    const form = new FormData()
    form.append('file', file)
    if (caption) form.append('caption', caption)
    return api
      .post<ApiResponse<TheaterImage>>(`${BASE}/${theaterId}/images`, form, {
        headers: { 'Content-Type': 'multipart/form-data' },
      })
      .then((r) => r.data)
  },

  deleteImage: (theaterId: string, imageId: string) =>
    api.delete<ApiResponse<null>>(`${BASE}/${theaterId}/images/${imageId}`).then((r) => r.data),

  rateTheater: (theaterId: string, rating: number, comment?: string) =>
    api.post<ApiResponse<null>>(`${BASE}/${theaterId}/rate`, { rating, comment }).then((r) => r.data),
}

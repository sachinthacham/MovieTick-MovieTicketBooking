import api from './axios'
import type { ApiResponse, Genre } from '@/lib/types'

const BASE = '/genres'

export const genresApi = {
  getAll: () => api.get<ApiResponse<Genre[]>>(BASE).then((r) => r.data),
  getById: (id: string) => api.get<ApiResponse<Genre>>(`${BASE}/${id}`).then((r) => r.data),
  create: (dto: { name: string; slug: string }) =>
    api.post<ApiResponse<Genre>>(BASE, dto).then((r) => r.data),
  update: (id: string, dto: { name: string; slug: string }) =>
    api.put<ApiResponse<Genre>>(`${BASE}/${id}`, dto).then((r) => r.data),
  delete: (id: string) => api.delete<ApiResponse<null>>(`${BASE}/${id}`).then((r) => r.data),
}

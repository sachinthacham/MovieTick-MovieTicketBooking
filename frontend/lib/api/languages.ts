import api from './axios'
import type { ApiResponse, Language } from '@/lib/types'

const BASE = '/languages'

export const languagesApi = {
  getAll: () => api.get<ApiResponse<Language[]>>(BASE).then((r) => r.data),
  getById: (id: string) => api.get<ApiResponse<Language>>(`${BASE}/${id}`).then((r) => r.data),
  create: (dto: { name: string; code: string }) =>
    api.post<ApiResponse<Language>>(BASE, dto).then((r) => r.data),
  update: (id: string, dto: { name: string; code: string }) =>
    api.put<ApiResponse<Language>>(`${BASE}/${id}`, dto).then((r) => r.data),
  delete: (id: string) => api.delete<ApiResponse<null>>(`${BASE}/${id}`).then((r) => r.data),
}

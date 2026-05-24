import api from './axios'
import type { ApiResponse, Screen, CreateScreenDto, UpdateScreenDto } from '@/lib/types'

const BASE = '/screens'

export const screensApi = {
  getByTheater: (theaterId: string) =>
    api.get<ApiResponse<Screen[]>>(BASE, { params: { theaterId } }).then((r) => r.data),

  getById: (id: string) =>
    api.get<ApiResponse<Screen>>(`${BASE}/${id}`).then((r) => r.data),

  create: (dto: CreateScreenDto) =>
    api.post<ApiResponse<Screen>>(BASE, dto).then((r) => r.data),

  update: (id: string, dto: UpdateScreenDto) =>
    api.put<ApiResponse<Screen>>(`${BASE}/${id}`, dto).then((r) => r.data),

  delete: (id: string) =>
    api.delete<ApiResponse<null>>(`${BASE}/${id}`).then((r) => r.data),
}

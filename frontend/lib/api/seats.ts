import api from './axios'
import type { ApiResponse, Seat, SeatCategory, CreateSeatCategoryDto, CreateBulkSeatsDto } from '@/lib/types'

export const seatCategoriesApi = {
  getByTheater: (theaterId: string) =>
    api
      .get<ApiResponse<SeatCategory[]>>('/seat-categories', { params: { theaterId } })
      .then((r) => r.data),

  create: (dto: CreateSeatCategoryDto) =>
    api.post<ApiResponse<SeatCategory>>('/seat-categories', dto).then((r) => r.data),

  update: (id: string, dto: Partial<CreateSeatCategoryDto>) =>
    api.put<ApiResponse<SeatCategory>>(`/seat-categories/${id}`, dto).then((r) => r.data),

  delete: (id: string) =>
    api.delete<ApiResponse<null>>(`/seat-categories/${id}`).then((r) => r.data),
}

export const seatsApi = {
  getByScreen: (screenId: string) =>
    api.get<ApiResponse<Seat[]>>('/seats', { params: { screenId } }).then((r) => r.data),

  createBulk: (dto: CreateBulkSeatsDto) =>
    api.post<ApiResponse<Seat[]>>('/seats/bulk', dto).then((r) => r.data),

  updateStatus: (id: string, status: Seat['status']) =>
    api.patch<ApiResponse<Seat>>(`/seats/${id}/status`, { status }).then((r) => r.data),

  delete: (id: string) =>
    api.delete<ApiResponse<null>>(`/seats/${id}`).then((r) => r.data),
}

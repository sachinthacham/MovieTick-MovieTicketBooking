import api from './axios'
import type { ApiResponse, UserAdmin, PagedUsers } from '@/lib/types'

export const adminUsersApi = {
  getAll: (params?: { page?: number; pageSize?: number; search?: string; role?: string }) =>
    api.get<ApiResponse<PagedUsers>>('/admin/users', { params }),

  getById: (id: string) =>
    api.get<ApiResponse<UserAdmin>>(`/admin/users/${id}`),

  updateRole: (id: string, role: string) =>
    api.patch<ApiResponse<boolean>>(`/admin/users/${id}/role`, { role }),

  deactivate: (id: string) =>
    api.patch<ApiResponse<boolean>>(`/admin/users/${id}/deactivate`),

  activate: (id: string) =>
    api.patch<ApiResponse<boolean>>(`/admin/users/${id}/activate`),
}

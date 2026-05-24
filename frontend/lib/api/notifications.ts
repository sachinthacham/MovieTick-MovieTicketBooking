import api from './axios'
import type { ApiResponse, PagedNotifications } from '@/lib/types'

export const notificationsApi = {
  getAll: (params?: { page?: number; pageSize?: number }) =>
    api.get<ApiResponse<PagedNotifications>>('/notifications', { params }),

  markRead: (notificationIds: string[]) =>
    api.patch<ApiResponse<boolean>>('/notifications/read', { notificationIds }),
}

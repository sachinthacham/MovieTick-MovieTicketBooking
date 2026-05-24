import api from './axios'
import type { ApiResponse, TicketDto } from '@/lib/types'

export const ticketsApi = {
  getByBooking: (bookingId: string) =>
    api.get<ApiResponse<TicketDto[]>>(`/tickets/booking/${bookingId}`),

  validate: (ticketNumber: string) =>
    api.patch<ApiResponse<TicketDto>>(`/tickets/${ticketNumber}/validate`),
}

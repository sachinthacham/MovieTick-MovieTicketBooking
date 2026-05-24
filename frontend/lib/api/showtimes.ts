import api from './axios'
import type {
  ApiResponse,
  Showtime,
  ShowtimeSeat,
  ShowFormat,
  CreateShowtimeDto,
  UpdateShowtimeDto,
  CreateShowFormatDto,
} from '@/lib/types'

/** Raw row from `GET /seats/availability/{showtimeId}` */
interface SeatAvailabilityRow {
  id: string
  seatId: string
  seatCategoryId: string
  seatNumber: string
  row: string
  column: number
  seatCategoryName: string
  color: string
  status: string
  price: number | null
}

function mapAvailabilityToShowtimeSeat(a: SeatAvailabilityRow): ShowtimeSeat {
  const statusRaw = a.status
  const status: ShowtimeSeat['status'] =
    statusRaw === 'Booked' || statusRaw === 'Blocked' || statusRaw === 'Reserved' || statusRaw === 'Available'
      ? statusRaw
      : 'Available'
  return {
    id: a.id,
    seatId: a.seatId,
    rowLabel: a.row,
    seatNumber: a.seatNumber,
    status,
    seatCategoryId: a.seatCategoryId,
    seatCategoryName: a.seatCategoryName,
    price: a.price ?? 0,
    colorCode: a.color,
  }
}

export const showtimesApi = {
  getById: (id: string) =>
    api.get<ApiResponse<Showtime>>(`/showtimes/${id}`).then((r) => r.data),

  /** `date` as `YYYY-MM-DD` (local) to filter showtimes for that calendar day; omit for all upcoming-related server behavior */
  getByMovie: (movieId: string, date?: string) =>
    api
      .get<ApiResponse<Showtime[]>>(`/showtimes/movie/${movieId}`, {
        params: date ? { date } : undefined,
      })
      .then((r) => r.data),

  getByTheater: (theaterId: string, date?: string) =>
    api
      .get<ApiResponse<Showtime[]>>(`/showtimes/theater/${theaterId}`, {
        params: date ? { date } : undefined,
      })
      .then((r) => r.data),

  getByScreen: (screenId: string, date?: string) =>
    api
      .get<ApiResponse<Showtime[]>>(`/showtimes/screen/${screenId}`, {
        params: date ? { date } : undefined,
      })
      .then((r) => r.data),

  /** Admin — bearer token required */
  getAdminRecent: (take = 20) =>
    api
      .get<ApiResponse<Showtime[]>>(`/showtimes/admin/recent`, { params: { take } })
      .then((r) => r.data),

  /** Admin — bearer token required */
  getAdminCount: () =>
    api.get<ApiResponse<number>>(`/showtimes/admin/count`).then((r) => r.data),

  getSeats: (showtimeId: string) =>
    api
      .get<ApiResponse<SeatAvailabilityRow[]>>(`/seats/availability/${showtimeId}`)
      .then((r) => ({
        ...r.data,
        data: r.data.data.map(mapAvailabilityToShowtimeSeat),
      })),

  create: async (dto: CreateShowtimeDto) => {
    const { pricing, ...body } = dto
    const res = await api
      .post<ApiResponse<Showtime>>('/showtimes', {
        movieId: body.movieId,
        screenId: body.screenId,
        showFormatId: body.showFormatId,
        languageId: body.languageId,
        startTime: body.startTime,
      })
      .then((r) => r.data)

    const showtimeId = res.data.id
    if (pricing?.length) {
      for (const p of pricing) {
        await api.post<ApiResponse<unknown>>(`/showtimes/${showtimeId}/pricing`, {
          seatCategoryId: p.seatCategoryId,
          price: p.price,
        })
      }
    }
    return res
  },

  update: (id: string, dto: UpdateShowtimeDto) =>
    api.put<ApiResponse<Showtime>>(`/showtimes/${id}`, dto).then((r) => r.data),

  /** Backend maps cancel to `DELETE /showtimes/{id}` */
  cancel: (id: string) =>
    api.delete<ApiResponse<boolean>>(`/showtimes/${id}`).then((r) => r.data),

  delete: (id: string) =>
    api.delete<ApiResponse<boolean>>(`/showtimes/${id}`).then((r) => r.data),
}

export const showFormatsApi = {
  getAll: () =>
    api.get<ApiResponse<ShowFormat[]>>('/show-formats').then((r) => r.data),

  create: (dto: CreateShowFormatDto) =>
    api.post<ApiResponse<ShowFormat>>('/show-formats', dto).then((r) => r.data),

  update: (id: string, dto: CreateShowFormatDto) =>
    api.put<ApiResponse<ShowFormat>>(`/show-formats/${id}`, dto).then((r) => r.data),

  delete: (id: string) =>
    api.delete<ApiResponse<null>>(`/show-formats/${id}`).then((r) => r.data),
}

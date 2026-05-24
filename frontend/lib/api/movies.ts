import api from './axios'
import type {
  ApiResponse,
  Movie,
  CreateMovieDto,
  UpdateMovieDto,
  MoviesFilter,
  MovieRating,
  MovieReview,
  MovieTrailer,
  MoviePoster,
  Genre,
  Language,
} from '@/lib/types'

const BASE = '/movies'

type PagedMoviesPayload = {
  items: Movie[]
  totalCount: number
  page: number
  pageSize: number
  totalPages: number
}

export const moviesApi = {
  /** Maps backend `page` ↔ filter `pageNumber` for callers using `PaginatedResult`. */
  getAll: (params?: MoviesFilter) =>
    api
      .get<ApiResponse<PagedMoviesPayload>>(BASE, {
        params: {
          page: params?.pageNumber ?? 1,
          pageSize: params?.pageSize,
          search: params?.search,
          genreId: params?.genreId,
          languageId: params?.languageId,
          isFeatured: params?.isFeatured,
          isComingSoon: params?.isComingSoon,
          isActive: params?.isActive,
          sortBy: params?.sortBy,
          city: params?.city,
          formatId: params?.formatId,
        },
      })
      .then((r) => ({
        success: r.data.success,
        message: r.data.message,
        errors: r.data.errors,
        data: {
          items: r.data.data.items,
          totalCount: r.data.data.totalCount,
          pageNumber: r.data.data.page,
          pageSize: r.data.data.pageSize,
          totalPages: r.data.data.totalPages,
        },
      })),

  getById: (id: string) =>
    api.get<ApiResponse<Movie>>(`${BASE}/${id}`).then((r) => r.data),

  create: (dto: CreateMovieDto) =>
    api.post<ApiResponse<Movie>>(BASE, dto).then((r) => r.data),

  update: (id: string, dto: UpdateMovieDto) =>
    api.put<ApiResponse<Movie>>(`${BASE}/${id}`, dto).then((r) => r.data),

  delete: (id: string) =>
    api.delete<ApiResponse<null>>(`${BASE}/${id}`).then((r) => r.data),

  toggleFeatured: (id: string) =>
    api.patch<ApiResponse<Movie>>(`${BASE}/${id}/toggle-featured`).then((r) => r.data),

  toggleComingSoon: (id: string) =>
    api.patch<ApiResponse<Movie>>(`${BASE}/${id}/toggle-coming-soon`).then((r) => r.data),

  // Genres
  addGenre: (movieId: string, genreId: string) =>
    api.post<ApiResponse<null>>(`${BASE}/${movieId}/genres/${genreId}`).then((r) => r.data),

  removeGenre: (movieId: string, genreId: string) =>
    api.delete<ApiResponse<null>>(`${BASE}/${movieId}/genres/${genreId}`).then((r) => r.data),

  // Languages
  addLanguage: (movieId: string, languageId: string) =>
    api.post<ApiResponse<null>>(`${BASE}/${movieId}/languages/${languageId}`).then((r) => r.data),

  removeLanguage: (movieId: string, languageId: string) =>
    api.delete<ApiResponse<null>>(`${BASE}/${movieId}/languages/${languageId}`).then((r) => r.data),

  // Trailers
  addTrailer: (movieId: string, dto: Omit<MovieTrailer, 'id'>) =>
    api.post<ApiResponse<MovieTrailer>>(`${BASE}/${movieId}/trailers`, dto).then((r) => r.data),

  deleteTrailer: (movieId: string, trailerId: string) =>
    api.delete<ApiResponse<null>>(`${BASE}/${movieId}/trailers/${trailerId}`).then((r) => r.data),

  // Posters
  uploadPoster: (movieId: string, file: File, type: MoviePoster['type'], isPrimary: boolean) => {
    const form = new FormData()
    form.append('file', file)
    form.append('type', type)
    form.append('isPrimary', String(isPrimary))
    return api
      .post<ApiResponse<MoviePoster>>(`${BASE}/${movieId}/posters`, form, {
        headers: { 'Content-Type': 'multipart/form-data' },
      })
      .then((r) => r.data)
  },

  deletePoster: (movieId: string, posterId: string) =>
    api.delete<ApiResponse<null>>(`${BASE}/${movieId}/posters/${posterId}`).then((r) => r.data),

  // Ratings
  rateMovie: (movieId: string, rating: number) =>
    api.post<ApiResponse<MovieRating>>(`${BASE}/${movieId}/ratings`, { rating }).then((r) => r.data),

  // Reviews
  addReview: (movieId: string, content: string) =>
    api.post<ApiResponse<MovieReview>>(`${BASE}/${movieId}/reviews`, { content }).then((r) => r.data),

  approveReview: (movieId: string, reviewId: string) =>
    api.patch<ApiResponse<null>>(`${BASE}/${movieId}/reviews/${reviewId}/approve`).then((r) => r.data),

  deleteReview: (movieId: string, reviewId: string) =>
    api.delete<ApiResponse<null>>(`${BASE}/${movieId}/reviews/${reviewId}`).then((r) => r.data),
}

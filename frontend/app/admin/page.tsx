'use client'

import { useQuery } from '@tanstack/react-query'
import { Film, Building2, CalendarRange, Star } from 'lucide-react'
import { moviesApi } from '@/lib/api/movies'
import { theatersApi } from '@/lib/api/theaters'
import { showtimesApi } from '@/lib/api/showtimes'

interface StatCardProps {
  title: string
  value: string | number
  sub?: string
  icon: React.ReactNode
  color: string
}

function StatCard({ title, value, sub, icon, color }: StatCardProps) {
  return (
    <div className={`rounded-2xl border border-(--border) p-6 bg-(--background) flex items-center gap-4`}>
      <div className={`h-12 w-12 rounded-xl flex items-center justify-center ${color}`}>
        {icon}
      </div>
      <div>
        <p className="text-(--muted-foreground) text-sm">{title}</p>
        <p className="text-2xl font-bold mt-0.5">{value}</p>
        {sub && <p className="text-xs text-(--muted-foreground) mt-0.5">{sub}</p>}
      </div>
    </div>
  )
}

export default function AdminDashboard() {
  const { data: moviesData } = useQuery({
    queryKey: ['admin-movies-count'],
    queryFn: () => moviesApi.getAll({ pageSize: 1 }),
  })

  const { data: theatersData } = useQuery({
    queryKey: ['admin-theaters-count'],
    queryFn: () => theatersApi.getAll({ pageSize: 1 }),
  })

  const { data: showtimesData } = useQuery({
    queryKey: ['admin-showtimes-count'],
    queryFn: () => showtimesApi.getAdminCount(),
  })

  const { data: featuredData } = useQuery({
    queryKey: ['admin-featured-count'],
    queryFn: () => moviesApi.getAll({ isFeatured: true, pageSize: 1 }),
  })

  const stats = [
    {
      title: 'Total Movies',
      value: moviesData?.data?.totalCount ?? '—',
      icon: <Film className="h-6 w-6 text-blue-600" />,
      color: 'bg-blue-500/10',
      sub: 'In database',
    },
    {
      title: 'Theaters',
      value: theatersData?.data?.totalCount ?? '—',
      icon: <Building2 className="h-6 w-6 text-green-600" />,
      color: 'bg-green-500/10',
      sub: 'Active venues',
    },
    {
      title: 'Showtimes',
      value: showtimesData?.data ?? '—',
      icon: <CalendarRange className="h-6 w-6 text-purple-600" />,
      color: 'bg-purple-500/10',
      sub: 'Scheduled',
    },
    {
      title: 'Featured Movies',
      value: featuredData?.data?.totalCount ?? '—',
      icon: <Star className="h-6 w-6 text-yellow-600" />,
      color: 'bg-yellow-500/10',
      sub: 'On homepage',
    },
  ]

  return (
    <div className="space-y-8">
      <div>
        <h1 className="text-3xl font-bold">Admin Dashboard</h1>
        <p className="text-(--muted-foreground) mt-1">Overview of your movie booking platform</p>
      </div>

      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4">
        {stats.map((stat) => (
          <StatCard key={stat.title} {...stat} />
        ))}
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        <RecentMovies />
        <RecentShowtimes />
      </div>
    </div>
  )
}

function RecentMovies() {
  const { data } = useQuery({
    queryKey: ['admin-recent-movies'],
    queryFn: () => moviesApi.getAll({ pageSize: 5 }),
  })

  const movies = data?.data?.items ?? []

  return (
    <div className="border border-(--border) rounded-2xl p-6">
      <h2 className="font-bold text-lg mb-4">Recent Movies</h2>
      {movies.length === 0 ? (
        <p className="text-(--muted-foreground) text-sm text-center py-6">No movies yet.</p>
      ) : (
        <div className="space-y-3">
          {movies.map((movie) => (
            <div key={movie.id} className="flex items-center justify-between py-2 border-b border-(--border)/50 last:border-0">
              <div>
                <p className="font-medium text-sm">{movie.title}</p>
                <p className="text-xs text-(--muted-foreground)">
                  {movie.genres.map((g) => g.name).join(', ')}
                </p>
              </div>
              <div className="text-right">
                {movie.isFeatured && (
                  <span className="text-xs px-2 py-0.5 rounded-full bg-yellow-500/20 text-yellow-600 font-medium">
                    Featured
                  </span>
                )}
                {movie.isComingSoon && (
                  <span className="text-xs px-2 py-0.5 rounded-full bg-blue-500/20 text-blue-600 font-medium">
                    Coming Soon
                  </span>
                )}
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  )
}

function RecentShowtimes() {
  const { data } = useQuery({
    queryKey: ['admin-recent-showtimes'],
    queryFn: () => showtimesApi.getAdminRecent(5),
  })

  const showtimes = data?.data ?? []

  return (
    <div className="border border-(--border) rounded-2xl p-6">
      <h2 className="font-bold text-lg mb-4">Recent Showtimes</h2>
      {showtimes.length === 0 ? (
        <p className="text-(--muted-foreground) text-sm text-center py-6">No showtimes yet.</p>
      ) : (
        <div className="space-y-3">
          {showtimes.map((st) => (
            <div key={st.id} className="flex items-center justify-between py-2 border-b border-(--border)/50 last:border-0">
              <div>
                <p className="font-medium text-sm">{st.movieTitle}</p>
                <p className="text-xs text-(--muted-foreground)">{st.theaterName}</p>
              </div>
              <div className="text-right text-xs text-(--muted-foreground)">
                {new Date(st.startTime).toLocaleDateString()} <br />
                {new Date(st.startTime).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  )
}

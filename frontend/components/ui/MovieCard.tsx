import * as React from "react"
import Link from "next/link"
import { Star, Clock } from "lucide-react"
import { Card, CardContent } from "@/components/ui/Card"
import { Badge } from "@/components/ui/Badge"
import { Button } from "@/components/ui/Button"

export interface Movie {
  id: string
  title: string
  posterUrl: string
  rating: number
  votes: string
  genre: string[]
  format: string[]
  duration?: string
  releaseDate?: string
  isComingSoon?: boolean
}

export function MovieCard({ movie }: { movie: Movie }) {
  return (
    <Link href={`/movies/${movie.id}`} className="group block h-full">
      <Card className="h-full overflow-hidden border-transparent bg-transparent shadow-none transition-all hover:scale-[1.02]">
        <CardContent className="p-0">
          <div className="relative aspect-[2/3] w-full overflow-hidden rounded-xl bg-(--muted)">
            {/* Using a standard img tag instead of next/image since we are mocking full URL placeholder images */}
            <img
              src={movie.posterUrl}
              alt={movie.title}
              className="h-full w-full object-cover transition-transform duration-300 group-hover:scale-105"
            />
            {movie.isComingSoon ? (
              <div className="absolute bottom-0 left-0 right-0 bg-gradient-to-t from-black/90 to-transparent p-4 text-white">
                <span className="text-sm font-medium">{movie.releaseDate}</span>
              </div>
            ) : (
              <div className="absolute bottom-0 left-0 right-0 bg-gradient-to-t from-black/90 to-transparent p-4 text-white">
                <div className="flex items-center space-x-1">
                  <Star className="h-4 w-4 fill-current text-yellow-500" />
                  <span className="text-sm font-bold">{movie.rating}</span>
                  <span className="text-xs opacity-70">({movie.votes})</span>
                </div>
              </div>
            )}
          </div>
          
          <div className="mt-3 space-y-1">
            <h3 className="line-clamp-1 text-lg font-bold text-(--foreground) group-hover:text-(--primary)">
              {movie.title}
            </h3>
            <p className="line-clamp-1 text-sm text-(--muted-foreground)">
              {movie.genre.join(", ")}
            </p>
          </div>
        </CardContent>
      </Card>
    </Link>
  )
}

import Link from "next/link"
import { Play, Star, Clock, Share2, Heart, Calendar } from "lucide-react"
import { Button } from "@/components/ui/Button"
import { Badge } from "@/components/ui/Badge"
import { Tabs, TabsList, TabsTrigger, TabsContent } from "@/components/ui/Tabs"
import { getMovieDetails } from "@/lib/mockData"

export default async function MovieDetailsPage({ params }: { params: { id: string } }) {
  // Mock data for currently selected movie
  const movie = getMovieDetails(params.id)

  return (
    <div className="flex flex-col min-h-screen bg-(--background)">
      {/* Movie Banner Container */}
      <section className="relative w-full overflow-hidden bg-black/90 pt-16 pb-12 md:py-24">
        {/* Blurred Backdrop */}
        <div className="absolute inset-0 z-0 opacity-40">
          <div 
            className="h-full w-full bg-cover bg-center blur-xl transform scale-110"
            style={{ backgroundImage: `url(${movie.backdropUrl})` }}
          />
          <div className="absolute inset-0 bg-gradient-to-r from-black via-black/80 to-transparent" />
        </div>

        <div className="container relative z-10 mx-auto px-4 md:px-8">
          <div className="flex flex-col md:flex-row items-center md:items-start gap-8">
            
            {/* Poster */}
            <div className="shrink-0">
              <div className="overflow-hidden rounded-xl shadow-2xl bg-(--muted) w-64 md:w-72 aspect-[2/3]">
                <img src={movie.posterUrl} alt={movie.title} className="w-full h-full object-cover" />
              </div>
            </div>

            {/* Movie Info */}
            <div className="flex-1 text-white space-y-6 text-center md:text-left">
              <div>
                <h1 className="text-3xl md:text-5xl font-extrabold tracking-tight">{movie.title}</h1>
              </div>

              {/* Rating & Action */}
              <div className="flex flex-col sm:flex-row items-center gap-4 bg-white/10 p-4 rounded-xl backdrop-blur-md w-fit mx-auto md:mx-0">
                <div className="flex items-center space-x-2">
                  <Star className="h-6 w-6 text-yellow-500 fill-yellow-500" />
                  <span className="text-2xl font-bold">{movie.rating}/10</span>
                  <span className="text-sm opacity-80">{movie.votes} Votes</span>
                </div>
                <div className="hidden sm:block w-px h-8 bg-white/20" />
                <Button variant="secondary" className="whitespace-nowrap rounded-full">
                  Rate now
                </Button>
              </div>

              {/* Badges */}
              <div className="flex flex-wrap items-center justify-center md:justify-start gap-2">
                <Badge variant="secondary" className="bg-white text-black hover:bg-white/90">{movie.format.join(", ")}</Badge>
                <Badge variant="secondary" className="bg-white/20 text-white hover:bg-white/30">{movie.languages.join(", ")}</Badge>
              </div>

              {/* Meta info */}
              <div className="flex flex-wrap items-center justify-center md:justify-start gap-4 text-sm opacity-90">
                <span className="flex items-center"><Clock className="mr-1 h-4 w-4" /> {movie.duration}</span>
                <span className="flex items-center">• {movie.genre.join(", ")}</span>
                <span className="flex items-center"><Calendar className="mr-1 h-4 w-4" /> {movie.releaseDate}</span>
              </div>

              {/* Actions */}
              <div className="pt-4 flex items-center justify-center md:justify-start space-x-4">
                <Link href={`/movie/${movie.id}/showtimes`}>
                  <Button size="lg" className="rounded-full px-8 bg-(--primary) text-(--primary-foreground) hover:bg-(--primary)/90">
                    Book Tickets
                  </Button>
                </Link>
                <Button size="icon" variant="outline" className="rounded-full bg-white/10 border-none hover:bg-white/20">
                  <Heart className="h-5 w-5" />
                </Button>
                <Button size="icon" variant="outline" className="rounded-full bg-white/10 border-none hover:bg-white/20">
                  <Share2 className="h-5 w-5" />
                </Button>
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* Details Tabs Layout */}
      <section className="container mx-auto px-4 md:px-8 py-8 flex-1 max-w-5xl">
        <Tabs defaultValue="overview">
          <TabsList className="mb-8 w-full justify-start border-b border-(--border) bg-transparent p-0 rounded-none h-auto">
            <TabsTrigger value="overview" className="rounded-none border-b-2 border-transparent data-[state=active]:border-(--primary) data-[state=active]:bg-transparent data-[state=active]:shadow-none px-4 py-3">Overview</TabsTrigger>
            <TabsTrigger value="cast" className="rounded-none border-b-2 border-transparent data-[state=active]:border-(--primary) data-[state=active]:bg-transparent data-[state=active]:shadow-none px-4 py-3">Cast & Crew</TabsTrigger>
            <TabsTrigger value="reviews" className="rounded-none border-b-2 border-transparent data-[state=active]:border-(--primary) data-[state=active]:bg-transparent data-[state=active]:shadow-none px-4 py-3">Reviews</TabsTrigger>
          </TabsList>

          <TabsContent value="overview" className="space-y-8 animate-in fade-in slide-in-from-bottom-2">
            <div>
              <h3 className="text-xl font-bold mb-4">About the Movie</h3>
              <p className="text-(--muted-foreground) leading-relaxed">
                {movie.description}
              </p>
            </div>

            {/* Embedded Trailer Placeholder */}
            <div>
              <h3 className="text-xl font-bold mb-4">Trailer</h3>
              <div className="relative aspect-video w-full rounded-2xl overflow-hidden bg-(--muted) flex items-center justify-center group cursor-pointer border border-(--border)">
                <img 
                  src={movie.backdropUrl} 
                  alt="Trailer Thumbnail" 
                  className="absolute inset-0 w-full h-full object-cover opacity-60 group-hover:opacity-80 transition-opacity"
                />
                <div className="relative z-10 w-16 h-16 rounded-full bg-white/20 backdrop-blur flex items-center justify-center group-hover:scale-110 transition-transform">
                  <Play className="h-8 w-8 text-white fill-white ml-1" />
                </div>
              </div>
            </div>
          </TabsContent>

          <TabsContent value="cast" className="animate-in fade-in slide-in-from-bottom-2">
            <h3 className="text-xl font-bold mb-6">Cast</h3>
            <div className="grid grid-cols-2 md:grid-cols-4 lg:grid-cols-5 gap-6">
              {movie.cast.map((actor, idx) => (
                <div key={idx} className="flex flex-col items-center text-center space-y-3">
                  <div className="w-24 h-24 rounded-full overflow-hidden bg-(--muted) border border-(--border)">
                    <img src={actor.image} alt={actor.name} className="w-full h-full object-cover" />
                  </div>
                  <div>
                    <p className="font-semibold text-sm">{actor.name}</p>
                    <p className="text-xs text-(--muted-foreground)">as {actor.role}</p>
                  </div>
                </div>
              ))}
            </div>
          </TabsContent>

          <TabsContent value="reviews" className="animate-in fade-in slide-in-from-bottom-2">
            <div className="flex items-center justify-between mb-6">
              <h3 className="text-xl font-bold">Top Reviews</h3>
              <Button variant="outline">Write a Review</Button>
            </div>
            
            <div className="space-y-6">
              {[1, 2, 3].map((i) => (
                <div key={i} className="border border-(--border) p-6 rounded-xl space-y-4">
                  <div className="flex justify-between items-start">
                    <div className="flex items-center space-x-3">
                      <div className="h-10 w-10 rounded-full bg-(--muted) flex items-center justify-center font-bold">
                        U{i}
                      </div>
                      <div>
                        <p className="font-semibold text-sm">User {i}</p>
                        <p className="text-xs text-(--muted-foreground)">2 days ago</p>
                      </div>
                    </div>
                    <div className="flex items-center text-yellow-500">
                      <Star className="h-4 w-4 fill-current" />
                      <span className="ml-1 text-sm font-bold">9/10</span>
                    </div>
                  </div>
                  <p className="text-sm text-(--muted-foreground)">
                    Visually stunning and action-packed! The CGI is top-notch and the story keeps you engaged throughout the runtime. A must watch in IMAX!
                  </p>
                </div>
              ))}
            </div>
          </TabsContent>
        </Tabs>
      </section>
    </div>
  )
}

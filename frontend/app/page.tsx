import Link from "next/link"
import { Button } from "@/components/ui/Button"
import { MovieCard } from "@/components/ui/MovieCard"
import { Play } from "lucide-react"
import { nowShowingMovies, comingSoonMovies } from "@/lib/mockData"

export default function HomePage() {
  return (
    <div className="flex flex-col min-h-screen">
      {/* Hero Section */}
      <section className="relative w-full overflow-hidden bg-(--background)">
        <div className="absolute inset-0 z-0">
          <div className="h-full w-full bg-[url('https://images.unsplash.com/photo-1440404653325-ab127d49abc1?w=1600&h=600&fit=crop')] bg-cover bg-center opacity-30 blur-sm" />
          <div className="absolute inset-0 bg-gradient-to-t from-(--background) via-(--background)/80 to-transparent" />
        </div>
        
        <div className="container relative z-10 mx-auto px-4 py-20 md:px-8 lg:py-32">
          <div className="max-w-3xl space-y-6">
            <div className="inline-flex rounded-full bg-(--primary)/20 px-3 py-1 text-sm font-semibold text-(--primary)">
              🔥 Premiering Today
            </div>
            <h1 className="text-4xl font-extrabold tracking-tight sm:text-5xl md:text-6xl lg:text-7xl">
              Godzilla x Kong: The New Empire
            </h1>
            <p className="max-w-xl text-lg text-(--muted-foreground) md:text-xl">
              Two ancient titans clash in an epic battle as humans unravel their intertwined origins and connection to Skull Island's mysteries.
            </p>
            <div className="flex flex-col space-y-4 sm:flex-row sm:space-x-4 sm:space-y-0">
              <Button size="lg" className="rounded-full px-8 text-base">
                Book Tickets
              </Button>
              <Button size="lg" variant="outline" className="rounded-full px-8 text-base backdrop-blur-md">
                <Play className="mr-2 h-4 w-4" />
                Watch Trailer
              </Button>
            </div>
          </div>
        </div>
      </section>

      {/* Filters Bar */}
      <section className="sticky top-16 z-30 w-full border-y border-(--border) bg-(--background)/95 backdrop-blur py-3">
        <div className="container mx-auto px-4 md:px-8 flex items-center justify-between overflow-x-auto no-scrollbar">
          <div className="flex space-x-2">
            {["English", "Hindi", "Tamil", "Telugu", "Malayalam"].map((lang) => (
              <Button key={lang} variant="outline" size="sm" className="rounded-full">
                {lang}
              </Button>
            ))}
          </div>
          <div className="hidden md:flex space-x-2 ml-4 border-l border-(--border) pl-4">
            {["Action", "Comedy", "Drama", "Sci-Fi", "Thriller"].map((genre) => (
              <Button key={genre} variant="ghost" size="sm" className="rounded-full">
                {genre}
              </Button>
            ))}
          </div>
        </div>
      </section>

      <main className="container mx-auto px-4 py-12 md:px-8 space-y-16">
        
        {/* Now Showing */}
        <section>
          <div className="flex items-end justify-between mb-6">
            <h2 className="text-2xl font-bold tracking-tight md:text-3xl">Now Showing</h2>
            <Link href="/movies" className="text-sm font-medium text-(--primary) hover:underline">
              See all
            </Link>
          </div>
          <div className="grid grid-cols-2 gap-4 sm:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5 2xl:grid-cols-6 pl-0">
            {nowShowingMovies.map((movie) => (
              <MovieCard key={movie.id} movie={movie} />
            ))}
          </div>
        </section>

        {/* Coming Soon */}
        <section>
          <div className="flex items-end justify-between mb-6">
            <h2 className="text-2xl font-bold tracking-tight md:text-3xl">Coming Soon</h2>
            <Link href="/movies/upcoming" className="text-sm font-medium text-(--primary) hover:underline">
              Explore all
            </Link>
          </div>
          <div className="grid grid-cols-2 gap-4 sm:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5 2xl:grid-cols-6 pl-0">
            {comingSoonMovies.map((movie) => (
              <MovieCard key={movie.id} movie={movie} />
            ))}
          </div>
        </section>

      </main>
    </div>
  )
}

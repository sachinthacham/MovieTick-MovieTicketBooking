import Link from "next/link"
import { Search, MapPin, Navigation, Star } from "lucide-react"
import { Button } from "@/components/ui/Button"
import { Input } from "@/components/ui/Input"
import { Badge } from "@/components/ui/Badge"
import { mockTheatres } from "@/lib/mockData"

export default function TheatresPage() {

  return (
    <div className="container mx-auto px-4 py-8 md:px-8 max-w-6xl min-h-[calc(100vh-14rem)]">
      {/* Header */}
      <div className="flex flex-col md:flex-row md:items-center justify-between gap-4 mb-8">
        <div>
          <h1 className="text-3xl font-bold tracking-tight">Theatres near you</h1>
          <p className="text-(--muted-foreground) mt-1 flex items-center">
            <MapPin className="mr-1 h-4 w-4" /> Mumbai, Maharashtra
          </p>
        </div>
        
        <div className="flex items-center gap-3 w-full md:w-auto">
          <div className="relative flex-1 md:w-80">
            <Search className="absolute left-3 top-2.5 h-4 w-4 text-(--muted-foreground)" />
            <Input 
              placeholder="Search for theatres, locations..." 
              className="pl-9 w-full bg-(--background)" 
            />
          </div>
          <Button variant="outline" size="icon">
            <Navigation className="h-4 w-4" />
          </Button>
        </div>
      </div>

      {/* Content Grid */}
      <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
        
        {/* Filters Sidebar */}
        <div className="space-y-6 lg:col-span-1">
          <div className="border border-(--border) rounded-2xl p-6 bg-(--background) sticky top-24">
            <h3 className="font-semibold text-lg mb-4">Filters</h3>
            
            <div className="space-y-4">
              <div>
                <h4 className="text-sm font-medium mb-3 text-(--muted-foreground)">Facilities</h4>
                <div className="space-y-2">
                  {["Food Court", "Parking", "Wheelchair Accessible", "Dolby Atmos", "IMAX"].map(facility => (
                    <label key={facility} className="flex items-center space-x-3 cursor-pointer">
                      <input type="checkbox" className="rounded border-(--input) text-(--primary) focus:ring-(--ring) w-4 h-4" />
                      <span className="text-sm">{facility}</span>
                    </label>
                  ))}
                </div>
              </div>

              <div className="pt-4 border-t border-(--border)">
                <h4 className="text-sm font-medium mb-3 text-(--muted-foreground)">Distance</h4>
                <input type="range" min="0" max="50" className="w-full accent-(--primary)" />
                <div className="flex justify-between text-xs text-(--muted-foreground) mt-2">
                  <span>0 km</span>
                  <span>50 km</span>
                </div>
              </div>
            </div>
            
            <Button className="w-full mt-6">Apply Filters</Button>
          </div>
        </div>

        {/* Theatre List */}
        <div className="lg:col-span-2 space-y-4">
          {mockTheatres.map(theatre => (
            <div key={theatre.id} className="border border-(--border) rounded-2xl p-6 bg-(--background) hover:shadow-md transition-shadow">
              <div className="flex justify-between items-start gap-4">
                <div>
                  <h3 className="text-xl font-bold">{theatre.name}</h3>
                  <p className="text-sm text-(--muted-foreground) mt-1">{theatre.address}</p>
                </div>
                <div className="text-right">
                  <Badge variant="secondary" className="mb-2">
                    {theatre.distance}
                  </Badge>
                  <div className="flex items-center justify-end text-sm font-medium text-yellow-500">
                    <Star className="h-4 w-4 fill-current mr-1" />
                    {theatre.rating}
                  </div>
                </div>
              </div>

              <div className="mt-6 pt-4 border-t border-(--border) flex flex-wrap gap-2">
                {theatre.facilities.map((fac, i) => (
                  <span key={i} className="text-xs px-2 py-1 rounded-md bg-(--muted) text-(--muted-foreground)">
                    {fac}
                  </span>
                ))}
              </div>
              
              <div className="mt-4 flex justify-end">
                <Button variant="outline" size="sm">View Showtimes</Button>
              </div>
            </div>
          ))}
        </div>
        
      </div>
    </div>
  )
}

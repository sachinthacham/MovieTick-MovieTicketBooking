import { Movie } from "@/components/ui/MovieCard"

export const nowShowingMovies: Movie[] = [
  {
    id: "1",
    title: "Dune: Part Two",
    posterUrl: "https://images.unsplash.com/photo-1534447677768-be436bb09401?w=500&h=750&fit=crop",
    rating: 8.8,
    votes: "124K+",
    genre: ["Action", "Adventure", "Sci-Fi"],
    format: ["IMAX", "4DX", "2D"],
  },
  {
    id: "2",
    title: "Kung Fu Panda 4",
    posterUrl: "https://images.unsplash.com/photo-1585951237318-9ea5e175b891?w=500&h=750&fit=crop",
    rating: 7.5,
    votes: "45K+",
    genre: ["Animation", "Action", "Comedy"],
    format: ["3D", "2D"],
  },
  {
    id: "3",
    title: "Godzilla x Kong: The New Empire",
    posterUrl: "https://images.unsplash.com/photo-1440404653325-ab127d49abc1?w=500&h=750&fit=crop",
    rating: 6.9,
    votes: "89K+",
    genre: ["Action", "Sci-Fi", "Thriller"],
    format: ["IMAX 3D", "4DX"],
  },
  {
    id: "4",
    title: "Ghostbusters: Frozen Empire",
    posterUrl: "https://images.unsplash.com/photo-1536440136628-849c177e76a1?w=500&h=750&fit=crop",
    rating: 7.1,
    votes: "32K+",
    genre: ["Comedy", "Fantasy", "Sci-Fi"],
    format: ["2D", "4DX"],
  },
]

export const comingSoonMovies: Movie[] = [
  {
    id: "5",
    title: "Furiosa: A Mad Max Saga",
    posterUrl: "https://images.unsplash.com/photo-1542204165-65bf26472b9b?w=500&h=750&fit=crop",
    rating: 0,
    votes: "0",
    genre: ["Action", "Adventure", "Sci-Fi"],
    format: [],
    isComingSoon: true,
    releaseDate: "23 May, 2024"
  },
  {
    id: "6",
    title: "Deadpool & Wolverine",
    posterUrl: "https://images.unsplash.com/photo-1608889175123-8ee362201f81?w=500&h=750&fit=crop",
    rating: 0,
    votes: "0",
    genre: ["Action", "Comedy", "Sci-Fi"],
    format: [],
    isComingSoon: true,
    releaseDate: "26 Jul, 2024"
  },
]

export const getMovieDetails = (id: string) => {
  return {
    id: id,
    title: "Godzilla x Kong: The New Empire",
    backdropUrl: "https://images.unsplash.com/photo-1440404653325-ab127d49abc1?w=1600&h=600&fit=crop",
    posterUrl: "https://images.unsplash.com/photo-1440404653325-ab127d49abc1?w=500&h=750&fit=crop",
    rating: 8.8,
    votes: "124K+",
    duration: "1h 55m",
    releaseDate: "29 Mar, 2024",
    genre: ["Action", "Sci-Fi", "Thriller"],
    format: ["IMAX 3D", "4DX"],
    languages: ["English", "Hindi", "Tamil", "Telugu"],
    description: "Two ancient titans, Godzilla and Kong, clash in an epic battle as humans unravel their intertwined origins and connection to Skull Island's mysteries. The epic new film will delve further into the histories of these Titans, their origins, and the mysteries of Skull Island and beyond.",
    cast: [
      { name: "Rebecca Hall", role: "Ilene Andrews", image: "https://images.unsplash.com/photo-1438761681033-6461ffad8d80?w=150&h=150&fit=crop" },
      { name: "Brian Tyree Henry", role: "Bernie Hayes", image: "https://images.unsplash.com/photo-1500648767791-00dcc994a43e?w=150&h=150&fit=crop" },
      { name: "Dan Stevens", role: "Trapper", image: "https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?w=150&h=150&fit=crop" },
    ]
  }
}

export const mockTheatres = [
  {
    id: "1",
    name: "PVR ICON: Infinity Mall, Andheri",
    distance: "2.4 km",
    rating: 4.5,
    address: "Infinity Mall, Link Road, Andheri West",
    facilities: ["Food Court", "Parking", "Wheelchair Accessible"],
  },
  {
    id: "2",
    name: "INOX: Megaplex, Malad",
    distance: "4.1 km",
    rating: 4.2,
    address: "Inorbit Mall, Link Road, Malad West",
    facilities: ["Recliner Seats", "Laser Projection", "Parking"],
  },
  {
    id: "3",
    name: "Cinépolis: VIP, Thane",
    distance: "12.0 km",
    rating: 4.6,
    address: "Viviana Mall, Eastern Express Hwy, Thane",
    facilities: ["IMAX", "Dolby Atmos", "Food Court"],
  },
  {
    id: "4",
    name: "Carnival Cinemas: Imax, Wadala",
    distance: "15.3 km",
    rating: 4.0,
    address: "Bhakti Park, Wadala East",
    facilities: ["IMAX", "Parking"],
  }
]

export const mockShowtimesData = [
  {
    theatreId: "1",
    name: "Cinépolis: VIP, Thane",
    formats: [
      { name: "IMAX 3D", times: ["10:30 AM", "01:15 PM", "04:45 PM", "08:00 PM"] },
      { name: "2D", times: ["11:00 AM", "02:30 PM", "06:00 PM", "09:30 PM", "11:15 PM"] }
    ]
  },
  {
    theatreId: "2",
    name: "PVR ICON: Infinity Mall, Andheri",
    formats: [
      { name: "4DX 3D", times: ["12:00 PM", "03:45 PM", "07:30 PM", "10:45 PM"] },
      { name: "2D", times: ["09:15 AM", "12:45 PM", "04:00 PM"] }
    ]
  },
  {
    theatreId: "3",
    name: "INOX: Megaplex, Malad",
    formats: [
      { name: "IMAX 2D", times: ["11:30 AM", "03:00 PM", "06:45 PM"] }
    ]
  }
]

export const defaultSeatCategories = [
  { name: "RECLINER", price: 450, color: "bg-purple-500/20 text-purple-600 border-purple-500", rows: ["P", "Q", "R"] },
  { name: "PRIME", price: 250, color: "bg-blue-500/20 text-blue-600 border-blue-500", rows: ["H", "I", "J", "K", "L", "M", "N", "O"] },
  { name: "CLASSIC", price: 150, color: "bg-green-500/20 text-green-600 border-green-500", rows: ["A", "B", "C", "D", "E", "F", "G"] }
]

export const userActiveTickets = [
  {
    id: "MTK-892401-BK",
    movie: "Godzilla x Kong: The New Empire",
    date: "29 Mar 2024",
    time: "07:30 PM",
    seats: "P13, P14, P15",
    category: "RECLINER",
    location: "Cinépolis: VIP, Viviana Mall, Thane",
    screen: "Screen 3",
    amount: "1470"
  }
]

export const userPastTickets = [
  {
    id: "MTK-123456-BK",
    movie: "Dune: Part Two",
    date: "05 Mar 2024",
    time: "04:15 PM",
    seats: "J12, J13",
    category: "PRIME",
    location: "PVR ICON: Infinity Mall, Andheri",
    screen: "IMAX Screen 1",
    amount: "620"
  }
]

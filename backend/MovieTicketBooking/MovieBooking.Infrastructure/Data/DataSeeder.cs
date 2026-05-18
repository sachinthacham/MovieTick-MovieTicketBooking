using Microsoft.EntityFrameworkCore;
using MovieBooking.Domain.Entities;
using MovieBooking.Domain.Enums;

namespace MovieBooking.Infrastructure.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(ApplicationDbContext db)
    {
        // Only seed once — skip if any user exists
        if (await db.Users.AnyAsync()) return;

        // ── 1. USERS ─────────────────────────────────────────────────────────
        var adminId  = Guid.NewGuid();
        var user1Id  = Guid.NewGuid();
        var user2Id  = Guid.NewGuid();

        var users = new List<User>
        {
            new() {
                Id = adminId,
                FullName = "Admin User",
                Email = "admin@movietick.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123456"),
                Role = "Admin",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new() {
                Id = user1Id,
                FullName = "John Doe",
                Email = "john.doe@movietick.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("User@123456"),
                Role = "User",
                IsActive = true,
                Bio = "Movie enthusiast and popcorn lover.",
                CreatedAt = DateTime.UtcNow
            },
            new() {
                Id = user2Id,
                FullName = "Sara Wilson",
                Email = "sara.wilson@movietick.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("User@123456"),
                Role = "User",
                IsActive = true,
                Bio = "I watch every movie on opening day!",
                CreatedAt = DateTime.UtcNow
            },
        };
        await db.Users.AddRangeAsync(users);

        // ── 2. GENRES ────────────────────────────────────────────────────────
        var genreIds = new Dictionary<string, Guid>();
        var genreNames = new[] { "Action", "Drama", "Comedy", "Horror", "Sci-Fi", "Romance", "Thriller", "Animation", "Adventure", "Fantasy" };
        var genres = genreNames.Select(name =>
        {
            var id = Guid.NewGuid();
            genreIds[name] = id;
            return new Genre { Id = id, Name = name, CreatedAt = DateTime.UtcNow };
        }).ToList();
        await db.Genres.AddRangeAsync(genres);

        // ── 3. LANGUAGES ─────────────────────────────────────────────────────
        var langIds = new Dictionary<string, Guid>();
        var langData = new[] {
            ("English", "en"), ("Sinhala", "si"), ("Tamil", "ta"), ("Hindi", "hi")
        };
        var languages = langData.Select(l =>
        {
            var id = Guid.NewGuid();
            langIds[l.Item1] = id;
            return new Language { Id = id, Name = l.Item1, Code = l.Item2, CreatedAt = DateTime.UtcNow };
        }).ToList();
        await db.Languages.AddRangeAsync(languages);

        // ── 4. SHOW FORMATS ──────────────────────────────────────────────────
        var fmtIds = new Dictionary<string, Guid>();
        var fmtData = new[] {
            ("2D", "Standard digital projection"),
            ("3D", "Three-dimensional with glasses"),
            ("IMAX", "Large-format immersive experience"),
            ("4DX", "Motion seats with environmental effects"),
        };
        var formats = fmtData.Select(f =>
        {
            var id = Guid.NewGuid();
            fmtIds[f.Item1] = id;
            return new ShowFormat { Id = id, Name = f.Item1, Description = f.Item2, CreatedAt = DateTime.UtcNow };
        }).ToList();
        await db.ShowFormats.AddRangeAsync(formats);

        // ── 5. SEAT CATEGORIES (global) ──────────────────────────────────────
        var catIds = new Dictionary<string, Guid>();
        var catData = new[] {
            ("Recliner", 1200m, "#8b5cf6", "Premium recliner seats with extra legroom"),
            ("Prime",    700m,  "#3b82f6", "Premium seats in the best viewing zone"),
            ("Classic",  400m,  "#22c55e", "Standard comfortable seats"),
        };
        var seatCategories = catData.Select(c =>
        {
            var id = Guid.NewGuid();
            catIds[c.Item1] = id;
            return new SeatCategory { Id = id, Name = c.Item1, DefaultPrice = c.Item2, Color = c.Item3, Description = c.Item4, CreatedAt = DateTime.UtcNow };
        }).ToList();
        await db.SeatCategories.AddRangeAsync(seatCategories);

        await db.SaveChangesAsync();

        // ── 6. MOVIES ────────────────────────────────────────────────────────
        var now = DateTime.UtcNow;
        var movieData = new[]
        {
            new {
                Title = "Avengers: Endgame",
                Desc = "The Avengers and their allies must be willing to sacrifice all in an attempt to defeat the powerful Thanos before his blitz of devastation and ruin puts an end to the universe.",
                Duration = 181, Released = new DateTime(2019, 4, 26),
                Director = "Anthony & Joe Russo",
                Cast = "Robert Downey Jr., Chris Evans, Mark Ruffalo, Chris Hemsworth, Scarlett Johansson",
                Cert = "PG-13", Featured = true, ComingSoon = false,
                Genres = new[]{"Action","Sci-Fi","Adventure"},
                Poster = "https://image.tmdb.org/t/p/w500/or06FN3Dka5tukK1e9sl16pB3iy.jpg",
                Banner = "https://image.tmdb.org/t/p/original/7RyHsO4yDXtBv1zUU3mTpHeQ0d5.jpg",
                Trailer = "https://www.youtube.com/watch?v=TcMBFSGVi1c",
                Rating = 8.4m, Ratings = 1240
            },
            new {
                Title = "The Dark Knight",
                Desc = "When the menace known as the Joker wreaks havoc and chaos on the people of Gotham, Batman must accept one of the greatest psychological and physical tests of his ability to fight injustice.",
                Duration = 152, Released = new DateTime(2008, 7, 18),
                Director = "Christopher Nolan",
                Cast = "Christian Bale, Heath Ledger, Aaron Eckhart, Michael Caine, Maggie Gyllenhaal",
                Cert = "PG-13", Featured = false, ComingSoon = false,
                Genres = new[]{"Action","Drama","Thriller"},
                Poster = "https://image.tmdb.org/t/p/w500/qJ2tW6WMUDux911r6m7haRef0WH.jpg",
                Banner = "https://image.tmdb.org/t/p/original/hkBaDkMWbLaf8B1lsWsKX7Ew3Xq.jpg",
                Trailer = "https://www.youtube.com/watch?v=EXeTwQWrcwY",
                Rating = 9.0m, Ratings = 2800
            },
            new {
                Title = "Inception",
                Desc = "A thief who steals corporate secrets through the use of dream-sharing technology is given the inverse task of planting an idea into the mind of a C.E.O.",
                Duration = 148, Released = new DateTime(2010, 7, 16),
                Director = "Christopher Nolan",
                Cast = "Leonardo DiCaprio, Joseph Gordon-Levitt, Elliot Page, Tom Hardy, Ken Watanabe",
                Cert = "PG-13", Featured = false, ComingSoon = false,
                Genres = new[]{"Sci-Fi","Thriller","Action"},
                Poster = "https://image.tmdb.org/t/p/w500/9gk7adHYeDvHkCSEqAvQNLV5Uge.jpg",
                Banner = "https://image.tmdb.org/t/p/original/s2bT29y0ngXxxu2IA8AOzzXTRhd.jpg",
                Trailer = "https://www.youtube.com/watch?v=YoHD9XEInc0",
                Rating = 8.8m, Ratings = 2100
            },
            new {
                Title = "Interstellar",
                Desc = "A team of explorers travel through a wormhole in space in an attempt to ensure humanity's survival.",
                Duration = 169, Released = new DateTime(2014, 11, 7),
                Director = "Christopher Nolan",
                Cast = "Matthew McConaughey, Anne Hathaway, Jessica Chastain, Michael Caine",
                Cert = "PG", Featured = false, ComingSoon = false,
                Genres = new[]{"Sci-Fi","Drama","Adventure"},
                Poster = "https://image.tmdb.org/t/p/w500/gEU2QniE6E77NI6lCU6MxlNBvIx.jpg",
                Banner = "https://image.tmdb.org/t/p/original/xJHokMbljvjADYdit5fK5VQsXEG.jpg",
                Trailer = "https://www.youtube.com/watch?v=zSWdZVtXT7E",
                Rating = 8.6m, Ratings = 1890
            },
            new {
                Title = "Spider-Man: No Way Home",
                Desc = "With Spider-Man's identity now revealed, Peter Parker asks Doctor Strange for help. When a spell goes wrong, dangerous foes from other worlds start to appear, forcing Peter to discover what it truly means to be Spider-Man.",
                Duration = 148, Released = new DateTime(2021, 12, 17),
                Director = "Jon Watts",
                Cast = "Tom Holland, Zendaya, Benedict Cumberbatch, Jacob Batalon, Jamie Foxx",
                Cert = "PG-13", Featured = true, ComingSoon = false,
                Genres = new[]{"Action","Adventure","Sci-Fi"},
                Poster = "https://image.tmdb.org/t/p/w500/uJYYizSuA9Y3DCs0qS4qWvHfZg4.jpg",
                Banner = "https://image.tmdb.org/t/p/original/iQFcwSGbZXMkeyKrxbPnwnRo5fl.jpg",
                Trailer = "https://www.youtube.com/watch?v=JfVOs4VSpmA",
                Rating = 8.3m, Ratings = 1560
            },
            new {
                Title = "Oppenheimer",
                Desc = "The story of J. Robert Oppenheimer's role in the development of the atomic bomb during World War II.",
                Duration = 180, Released = new DateTime(2023, 7, 21),
                Director = "Christopher Nolan",
                Cast = "Cillian Murphy, Emily Blunt, Matt Damon, Robert Downey Jr., Florence Pugh",
                Cert = "R", Featured = true, ComingSoon = false,
                Genres = new[]{"Drama","Thriller"},
                Poster = "https://image.tmdb.org/t/p/w500/8Gxv8gSFCU0XGDykEGv7zR1n2ua.jpg",
                Banner = "https://image.tmdb.org/t/p/original/fm6KqXpk3M2HVveHwCrBSSBaO0V.jpg",
                Trailer = "https://www.youtube.com/watch?v=uYPbbksJxIg",
                Rating = 8.9m, Ratings = 980
            },
            new {
                Title = "Top Gun: Maverick",
                Desc = "After more than thirty years of service as one of the Navy's top aviators, Pete 'Maverick' Mitchell is where he belongs, pushing the envelope as a courageous test pilot.",
                Duration = 130, Released = new DateTime(2022, 5, 27),
                Director = "Joseph Kosinski",
                Cast = "Tom Cruise, Jennifer Connelly, Miles Teller, Jon Hamm, Glen Powell",
                Cert = "PG-13", Featured = false, ComingSoon = false,
                Genres = new[]{"Action","Drama"},
                Poster = "https://image.tmdb.org/t/p/w500/62HCnUTHOWT7PODk4jBSfxXbJgm.jpg",
                Banner = "https://image.tmdb.org/t/p/original/odJ4hx6g6vBt4lBWKFD1tI8WS4x.jpg",
                Trailer = "https://www.youtube.com/watch?v=giXco2jaZ_4",
                Rating = 8.3m, Ratings = 1450
            },
            new {
                Title = "Dune: Part Two",
                Desc = "Paul Atreides unites with Chani and the Fremen while seeking revenge against the conspirators who destroyed his family. Facing a choice between the love of his life and the fate of the universe.",
                Duration = 166, Released = new DateTime(2024, 3, 1),
                Director = "Denis Villeneuve",
                Cast = "Timothée Chalamet, Zendaya, Rebecca Ferguson, Josh Brolin, Austin Butler",
                Cert = "PG-13", Featured = true, ComingSoon = false,
                Genres = new[]{"Sci-Fi","Adventure","Drama"},
                Poster = "https://image.tmdb.org/t/p/w500/1pdfLvkbY9ohJlCjQH2CZjjYVvJ.jpg",
                Banner = "https://image.tmdb.org/t/p/original/xOMo8BRK7PfcJv9JCnx7s5hj0PX.jpg",
                Trailer = "https://www.youtube.com/watch?v=Way9Dexny3w",
                Rating = 8.5m, Ratings = 860
            },
            new {
                Title = "Deadpool & Wolverine",
                Desc = "Deadpool is offered a chance to join the Time Variance Authority. He decides to recruit a variant of Logan — Wolverine — and together they face powerful adversaries threatening the multiverse.",
                Duration = 128, Released = new DateTime(2024, 7, 26),
                Director = "Shawn Levy",
                Cast = "Ryan Reynolds, Hugh Jackman, Emma Corrin, Jennifer Garner, Wesley Snipes",
                Cert = "R", Featured = false, ComingSoon = false,
                Genres = new[]{"Action","Comedy","Sci-Fi"},
                Poster = "https://image.tmdb.org/t/p/w500/8cdWjvZQUExUUTzyp4t6EDMubfO.jpg",
                Banner = "https://image.tmdb.org/t/p/original/yDHYTfA3R0jFYba16jBB1ef8oIt.jpg",
                Trailer = "https://www.youtube.com/watch?v=73_1biulkYk",
                Rating = 7.8m, Ratings = 720
            },
            new {
                Title = "Avatar 3",
                Desc = "Jake Sully and Ney'tiri continue their journey on Pandora, discovering new clans and confronting threats that challenge the balance of their world in ways they never imagined.",
                Duration = 175, Released = new DateTime(2025, 12, 19),
                Director = "James Cameron",
                Cast = "Sam Worthington, Zoe Saldana, Sigourney Weaver, Stephen Lang",
                Cert = "PG-13", Featured = false, ComingSoon = true,
                Genres = new[]{"Sci-Fi","Adventure","Fantasy"},
                Poster = "https://image.tmdb.org/t/p/w500/t6HIqrRAclMCA60NsSmeqe9RmNV.jpg",
                Banner = "https://image.tmdb.org/t/p/original/AmR3JfkerDef6a4srViCwx6DLWL.jpg",
                Trailer = "https://www.youtube.com/watch?v=d9MyW72ELq0",
                Rating = 0m, Ratings = 0
            },
        };

        var movieEntities = new List<Movie>();
        var movieIds = new Dictionary<string, Guid>();

        foreach (var m in movieData)
        {
            var id = Guid.NewGuid();
            movieIds[m.Title] = id;
            movieEntities.Add(new Movie
            {
                Id = id,
                Title = m.Title,
                Description = m.Desc,
                DurationMinutes = m.Duration,
                ReleaseDate = m.Released,
                Director = m.Director,
                Cast = m.Cast,
                CertificateRating = m.Cert,
                IsFeatured = m.Featured,
                IsComingSoon = m.ComingSoon,
                IsActive = true,
                AverageRating = m.Rating > 0 ? m.Rating : null,
                TotalRatings = m.Ratings,
                CreatedAt = DateTime.UtcNow,
            });
        }
        await db.Movies.AddRangeAsync(movieEntities);

        // Movie-Genre links
        var movieGenreLinks = new List<MovieGenre>();
        foreach (var m in movieData)
        {
            foreach (var g in m.Genres)
            {
                if (genreIds.TryGetValue(g, out var gid))
                    movieGenreLinks.Add(new MovieGenre { MovieId = movieIds[m.Title], GenreId = gid });
            }
        }
        await db.MovieGenres.AddRangeAsync(movieGenreLinks);

        // Movie-Language links (all in English + a few in other languages)
        var movieLangLinks = new List<MovieLanguage>();
        foreach (var m in movieData)
        {
            movieLangLinks.Add(new MovieLanguage { MovieId = movieIds[m.Title], LanguageId = langIds["English"] });
        }
        // A few movies also in Tamil and Hindi
        foreach (var title in new[] { "Avengers: Endgame", "Spider-Man: No Way Home", "Oppenheimer", "Dune: Part Two" })
        {
            movieLangLinks.Add(new MovieLanguage { MovieId = movieIds[title], LanguageId = langIds["Tamil"] });
            movieLangLinks.Add(new MovieLanguage { MovieId = movieIds[title], LanguageId = langIds["Hindi"] });
        }
        await db.MovieLanguages.AddRangeAsync(movieLangLinks);

        // Movie Posters
        var posters = new List<MoviePoster>();
        foreach (var m in movieData)
        {
            posters.Add(new MoviePoster {
                Id = Guid.NewGuid(),
                MovieId = movieIds[m.Title],
                ImageUrl = m.Poster,
                IsPrimary = true,
                PosterType = PosterType.Portrait,
                CreatedAt = DateTime.UtcNow
            });
            posters.Add(new MoviePoster {
                Id = Guid.NewGuid(),
                MovieId = movieIds[m.Title],
                ImageUrl = m.Banner,
                IsPrimary = false,
                PosterType = PosterType.Landscape,
                CreatedAt = DateTime.UtcNow
            });
        }
        await db.MoviePosters.AddRangeAsync(posters);

        // Movie Trailers
        var trailers = new List<MovieTrailer>();
        foreach (var m in movieData)
        {
            trailers.Add(new MovieTrailer {
                Id = Guid.NewGuid(),
                MovieId = movieIds[m.Title],
                Title = $"{m.Title} - Official Trailer",
                Url = m.Trailer,
                IsPrimary = true,
                CreatedAt = DateTime.UtcNow
            });
        }
        await db.MovieTrailers.AddRangeAsync(trailers);

        await db.SaveChangesAsync();

        // ── 7. THEATERS ──────────────────────────────────────────────────────
        var theaterData = new[]
        {
            new {
                Name = "Savoy 3D Cinema",
                Desc = "Colombo's premier luxury cinema with state-of-the-art IMAX and recliner seating.",
                Address = "No. 69, Galle Road",
                City = "Colombo", State = "Western Province", Country = "Sri Lanka",
                Phone = "+94 11 256 7890", Email = "info@savoy.lk",
                Lat = 6.9271, Lon = 79.8612,
                Facilities = new[] { "Free Parking", "Food Court", "Dolby Atmos", "IMAX Screen", "Wheelchair Accessible", "VIP Lounge", "Online Booking" },
                Image = "https://images.unsplash.com/photo-1489599849927-2ee91cede3ba?w=1200&h=600&fit=crop",
            },
            new {
                Name = "Liberty Cinema",
                Desc = "Kandy's most beloved cinema, offering a wide selection of local and international films.",
                Address = "5 Sangaraja Mawatha",
                City = "Kandy", State = "Central Province", Country = "Sri Lanka",
                Phone = "+94 81 222 3456", Email = "liberty@cinema.lk",
                Lat = 7.2906, Lon = 80.6337,
                Facilities = new[] { "Parking", "Canteen", "Air Conditioned", "Online Booking" },
                Image = "https://images.unsplash.com/photo-1517604931442-7e0c8ed2963c?w=1200&h=600&fit=crop",
            },
            new {
                Name = "Scope Cinemas",
                Desc = "The south's largest multiplex bringing blockbusters to Galle with premium comfort.",
                Address = "Closenberg Road, Magalle",
                City = "Galle", State = "Southern Province", Country = "Sri Lanka",
                Phone = "+94 91 222 9900", Email = "scope@cinemas.lk",
                Lat = 6.0535, Lon = 80.2210,
                Facilities = new[] { "Parking", "Refreshment Bar", "Air Conditioned", "Wheelchair Accessible", "Online Booking" },
                Image = "https://images.unsplash.com/photo-1503095396549-807759245b35?w=1200&h=600&fit=crop",
            },
        };

        var theaterIds = new Dictionary<string, Guid>();
        var theaterEntities = new List<Theater>();
        foreach (var t in theaterData)
        {
            var id = Guid.NewGuid();
            theaterIds[t.Name] = id;
            theaterEntities.Add(new Theater {
                Id = id, Name = t.Name, Description = t.Desc, Address = t.Address,
                City = t.City, State = t.State, Country = t.Country,
                PhoneNumber = t.Phone, Email = t.Email,
                Latitude = t.Lat, Longitude = t.Lon, IsActive = true,
                CreatedAt = DateTime.UtcNow
            });
        }
        await db.Theaters.AddRangeAsync(theaterEntities);

        // Theater Facilities
        var facilities = new List<TheaterFacility>();
        foreach (var t in theaterData)
        {
            foreach (var f in t.Facilities)
                facilities.Add(new TheaterFacility { Id = Guid.NewGuid(), TheaterId = theaterIds[t.Name], FacilityName = f, CreatedAt = DateTime.UtcNow });
        }
        await db.TheaterFacilities.AddRangeAsync(facilities);

        // Theater Images
        var theaterImages = new List<TheaterImage>();
        foreach (var t in theaterData)
        {
            theaterImages.Add(new TheaterImage { Id = Guid.NewGuid(), TheaterId = theaterIds[t.Name], ImageUrl = t.Image, IsPrimary = true, CreatedAt = DateTime.UtcNow });
        }
        await db.TheaterImages.AddRangeAsync(theaterImages);

        await db.SaveChangesAsync();

        // ── 8. SCREENS ────────────────────────────────────────────────────────
        // Savoy: 3 screens | Liberty: 2 screens | Scope: 2 screens
        var screenDefs = new[]
        {
            // (theater, name, screenType, totalSeats)
            ("Savoy 3D Cinema",  "Screen 1 – IMAX",      "IMAX",     200),
            ("Savoy 3D Cinema",  "Screen 2 – Gold Class", "Premium",  120),
            ("Savoy 3D Cinema",  "Screen 3 – Standard",   "Standard", 180),
            ("Liberty Cinema",   "Screen 1 – Premium",    "Premium",  150),
            ("Liberty Cinema",   "Screen 2 – Classic",    "Standard", 200),
            ("Scope Cinemas",    "Screen 1 – Prime",      "Premium",  160),
            ("Scope Cinemas",    "Screen 2 – Classic",    "Standard", 180),
        };

        var screenIds = new Dictionary<string, Guid>();
        var screenEntities = new List<Screen>();
        foreach (var (tName, sName, sType, seats) in screenDefs)
        {
            var id = Guid.NewGuid();
            screenIds[$"{tName}|{sName}"] = id;
            screenEntities.Add(new Screen {
                Id = id, TheaterId = theaterIds[tName], Name = sName,
                ScreenType = sType, TotalSeats = seats, IsActive = true,
                CreatedAt = DateTime.UtcNow
            });
        }
        await db.Screens.AddRangeAsync(screenEntities);
        await db.SaveChangesAsync();

        // ── 9. SEATS ──────────────────────────────────────────────────────────
        // Layout per screen: Rows A-G Classic, H-O Prime, P-R Recliner
        // Columns per screen vary based on totalSeats
        var allSeats = new List<Seat>();
        var screenSeatMap = new Dictionary<Guid, List<Guid>>(); // screenId -> seatIds

        var rowLayout = new[]
        {
            // (rowLabel, category, colStart, colEnd)
            ("A", "Classic"), ("B", "Classic"), ("C", "Classic"), ("D", "Classic"),
            ("E", "Classic"), ("F", "Classic"), ("G", "Classic"),
            ("H", "Prime"),   ("I", "Prime"),   ("J", "Prime"),   ("K", "Prime"),
            ("L", "Prime"),   ("M", "Prime"),   ("N", "Prime"),   ("O", "Prime"),
            ("P", "Recliner"),("Q", "Recliner"),("R", "Recliner"),
        };

        foreach (var screen in screenEntities)
        {
            // Determine columns based on total seats / 18 rows
            int cols = Math.Max(10, screen.TotalSeats / rowLayout.Length);
            if (cols > 20) cols = 20;

            var seatIds = new List<Guid>();
            foreach (var (row, catName) in rowLayout)
            {
                var catId = catIds[catName];
                for (int col = 1; col <= cols; col++)
                {
                    var seatId = Guid.NewGuid();
                    seatIds.Add(seatId);
                    allSeats.Add(new Seat {
                        Id = seatId,
                        ScreenId = screen.Id,
                        SeatCategoryId = catId,
                        SeatNumber = $"{row}{col}",
                        Row = row,
                        Column = col,
                        IsBlocked = false,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    });
                }
            }
            screenSeatMap[screen.Id] = seatIds;
        }

        // Batch insert seats in chunks to avoid memory issues
        const int batchSize = 500;
        for (int i = 0; i < allSeats.Count; i += batchSize)
        {
            await db.Seats.AddRangeAsync(allSeats.Skip(i).Take(batchSize));
            await db.SaveChangesAsync();
        }

        // ── 10. SHOWTIMES ────────────────────────────────────────────────────
        // Create showtimes for "now showing" movies across all screens
        // 3 showtimes per screen per day (today + tomorrow)
        var nowShowing = movieData.Where(m => !m.ComingSoon).Take(8).ToArray();
        var baseDate = DateTime.UtcNow.Date;

        var showtimes = new List<Showtime>();
        var showtimeSeats = new List<ShowtimeSeat>();
        var pricings = new List<ShowtimePricing>();

        // Time slots
        var timeSlots = new[] {
            TimeSpan.FromHours(10),   // 10:00 AM
            TimeSpan.FromHours(14),   // 2:00  PM
            TimeSpan.FromHours(18),   // 6:00  PM
            TimeSpan.FromHours(21),   // 9:00  PM
        };

        // Price multipliers per screen type
        var priceMultipliers = new Dictionary<string, decimal> {
            ["IMAX"] = 1.5m, ["Premium"] = 1.2m, ["Standard"] = 1.0m
        };

        var screenList = screenEntities.ToArray();
        var movieList = nowShowing.ToArray();

        // Assign movies to screens (round-robin)
        for (int dayOffset = 0; dayOffset <= 2; dayOffset++)
        {
            var date = baseDate.AddDays(dayOffset);
            for (int si = 0; si < screenList.Length; si++)
            {
                var screen = screenList[si];
                var movie = movieList[si % movieList.Length];
                var movieId = movieIds[movie.Title];
                var movieDuration = TimeSpan.FromMinutes(movie.Duration);
                var multiplier = priceMultipliers.GetValueOrDefault(screen.ScreenType, 1.0m);
                var formatId = screen.ScreenType == "IMAX" ? fmtIds["IMAX"]
                    : screen.ScreenType == "Premium" ? fmtIds["3D"]
                    : fmtIds["2D"];

                foreach (var slot in timeSlots)
                {
                    var startTime = date.Add(slot);
                    var showtimeId = Guid.NewGuid();

                    showtimes.Add(new Showtime {
                        Id = showtimeId,
                        MovieId = movieId,
                        ScreenId = screen.Id,
                        ShowFormatId = formatId,
                        LanguageId = langIds["English"],
                        StartTime = startTime,
                        EndTime = startTime + movieDuration,
                        Status = ShowtimeStatus.Scheduled,
                        CreatedAt = DateTime.UtcNow
                    });

                    // Pricing per category
                    pricings.Add(new ShowtimePricing { Id = Guid.NewGuid(), ShowtimeId = showtimeId, SeatCategoryId = catIds["Recliner"], Price = Math.Round(1200 * multiplier), CreatedAt = DateTime.UtcNow });
                    pricings.Add(new ShowtimePricing { Id = Guid.NewGuid(), ShowtimeId = showtimeId, SeatCategoryId = catIds["Prime"],    Price = Math.Round(700  * multiplier), CreatedAt = DateTime.UtcNow });
                    pricings.Add(new ShowtimePricing { Id = Guid.NewGuid(), ShowtimeId = showtimeId, SeatCategoryId = catIds["Classic"],  Price = Math.Round(400  * multiplier), CreatedAt = DateTime.UtcNow });

                    // ShowtimeSeats from screen's seats
                    if (screenSeatMap.TryGetValue(screen.Id, out var seatList))
                    {
                        foreach (var seatId in seatList)
                        {
                            showtimeSeats.Add(new ShowtimeSeat {
                                Id = Guid.NewGuid(),
                                ShowtimeId = showtimeId,
                                SeatId = seatId,
                                Status = SeatStatus.Available,
                                CreatedAt = DateTime.UtcNow
                            });
                        }
                    }
                }
            }
        }

        await db.Showtimes.AddRangeAsync(showtimes);
        await db.SaveChangesAsync();

        await db.ShowtimePricings.AddRangeAsync(pricings);
        await db.SaveChangesAsync();

        // Insert showtime seats in batches
        for (int i = 0; i < showtimeSeats.Count; i += batchSize)
        {
            await db.ShowtimeSeats.AddRangeAsync(showtimeSeats.Skip(i).Take(batchSize));
            await db.SaveChangesAsync();
        }

        // ── 11. SAMPLE MOVIE RATINGS ─────────────────────────────────────────
        var ratingData = new[]
        {
            (movieIds["Avengers: Endgame"], user1Id, 9),
            (movieIds["Avengers: Endgame"], user2Id, 8),
            (movieIds["The Dark Knight"],   user1Id, 10),
            (movieIds["The Dark Knight"],   user2Id, 9),
            (movieIds["Inception"],         user1Id, 9),
            (movieIds["Interstellar"],      user2Id, 8),
            (movieIds["Oppenheimer"],       user1Id, 9),
            (movieIds["Dune: Part Two"],    user2Id, 8),
        };

        var movieRatings = ratingData.Select(r => new MovieRating {
            Id = Guid.NewGuid(), MovieId = r.Item1, UserId = r.Item2,
            Rating = r.Item3, CreatedAt = DateTime.UtcNow
        }).ToList();
        await db.MovieRatings.AddRangeAsync(movieRatings);

        // ── 12. SAMPLE REVIEWS ───────────────────────────────────────────────
        var reviews = new List<MovieReview>
        {
            new() { Id = Guid.NewGuid(), MovieId = movieIds["Avengers: Endgame"], UserId = user1Id,
                ReviewText = "The perfect ending to 11 years of the MCU. Every moment was earned and the emotional payoff was absolutely worth the wait!", IsApproved = true, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), MovieId = movieIds["The Dark Knight"], UserId = user2Id,
                ReviewText = "Heath Ledger's Joker is simply one of the greatest performances in cinema history. This film transcends the superhero genre.", IsApproved = true, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), MovieId = movieIds["Inception"], UserId = user1Id,
                ReviewText = "Mind-bending storytelling that rewards multiple viewings. Nolan at his absolute peak.", IsApproved = true, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), MovieId = movieIds["Oppenheimer"], UserId = user2Id,
                ReviewText = "A cinematic masterpiece. Cillian Murphy delivers the performance of his career. This is what cinema is meant to be.", IsApproved = true, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), MovieId = movieIds["Dune: Part Two"], UserId = user1Id,
                ReviewText = "Denis Villeneuve has created a truly epic visual experience. The scale is breathtaking and the story is compelling.", IsApproved = true, CreatedAt = DateTime.UtcNow },
        };
        await db.MovieReviews.AddRangeAsync(reviews);

        // ── 13. THEATER RATINGS ──────────────────────────────────────────────
        var theaterRatings = new List<TheaterRating>
        {
            new() { Id = Guid.NewGuid(), TheaterId = theaterIds["Savoy 3D Cinema"], UserId = user1Id, Rating = 5m, Comment = "Absolutely stunning IMAX experience! Best cinema in Colombo.", CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), TheaterId = theaterIds["Savoy 3D Cinema"], UserId = user2Id, Rating = 4m, Comment = "Great screens and sound. Parking can be tight on weekends.", CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), TheaterId = theaterIds["Liberty Cinema"],  UserId = user1Id, Rating = 4m, Comment = "Classic cinema feel with modern comforts. Loved the experience!", CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), TheaterId = theaterIds["Scope Cinemas"],   UserId = user2Id, Rating = 4m, Comment = "Best cinema in the south! Great value for money.", CreatedAt = DateTime.UtcNow },
        };
        await db.TheaterRatings.AddRangeAsync(theaterRatings);

        await db.SaveChangesAsync();
    }
}

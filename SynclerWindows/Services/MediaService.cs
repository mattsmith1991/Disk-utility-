using SynclerWindows.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynclerWindows.Services
{
    public class MediaService : IMediaService
    {
        private readonly List<MediaItem> _mockMovies;
        private readonly List<MediaItem> _mockTvShows;
        private readonly Dictionary<string, List<MediaItem>> _userWatchlists = new();
        private readonly Dictionary<string, List<MediaItem>> _userContinueWatching = new();
        private readonly Dictionary<string, List<MediaItem>> _userHistory = new();

        public MediaService()
        {
            _mockMovies = GenerateMockMovies();
            _mockTvShows = GenerateMockTvShows();
        }

        public async Task<List<MediaItem>> GetTrendingAsync()
        {
            await Task.Delay(500); // Simulate API call
            
            var trending = new List<MediaItem>();
            trending.AddRange(_mockMovies.Take(10));
            trending.AddRange(_mockTvShows.Take(10));
            
            return trending.OrderBy(x => Guid.NewGuid()).ToList();
        }

        public async Task<List<MediaItem>> GetPopularAsync(MediaType type)
        {
            await Task.Delay(300);
            
            return type switch
            {
                MediaType.Movie => _mockMovies.OrderByDescending(m => m.Popularity).Take(20).ToList(),
                MediaType.TvShow => _mockTvShows.OrderByDescending(m => m.Popularity).Take(20).ToList(),
                _ => new List<MediaItem>()
            };
        }

        public async Task<List<MediaItem>> GetTopRatedAsync(MediaType type)
        {
            await Task.Delay(300);
            
            return type switch
            {
                MediaType.Movie => _mockMovies.OrderByDescending(m => m.VoteAverage).Take(20).ToList(),
                MediaType.TvShow => _mockTvShows.OrderByDescending(m => m.VoteAverage).Take(20).ToList(),
                _ => new List<MediaItem>()
            };
        }

        public async Task<List<MediaItem>> GetUpcomingAsync()
        {
            await Task.Delay(300);
            return _mockMovies.Where(m => m.ReleaseDate > DateTime.Now).Take(20).ToList();
        }

        public async Task<List<MediaItem>> GetNowPlayingAsync()
        {
            await Task.Delay(300);
            var now = DateTime.Now;
            return _mockMovies.Where(m => m.ReleaseDate <= now && m.ReleaseDate >= now.AddMonths(-2)).Take(20).ToList();
        }

        public async Task<List<MediaItem>> GetByGenreAsync(string genre, MediaType type)
        {
            await Task.Delay(300);
            
            var items = type == MediaType.Movie ? _mockMovies : _mockTvShows;
            return items.Where(m => m.Genres.Any(g => g.Name.Contains(genre, StringComparison.OrdinalIgnoreCase)))
                       .Take(20).ToList();
        }

        public async Task<MediaItem?> GetMediaDetailsAsync(int id, MediaType type)
        {
            await Task.Delay(200);
            
            var items = type == MediaType.Movie ? _mockMovies : _mockTvShows;
            return items.FirstOrDefault(m => m.Id == id);
        }

        public async Task<List<Season>> GetSeasonsAsync(int showId)
        {
            await Task.Delay(200);
            
            var show = _mockTvShows.FirstOrDefault(s => s.Id == showId);
            return show?.Seasons ?? new List<Season>();
        }

        public async Task<List<Episode>> GetEpisodesAsync(int showId, int seasonNumber)
        {
            await Task.Delay(200);
            
            var show = _mockTvShows.FirstOrDefault(s => s.Id == showId);
            var season = show?.Seasons.FirstOrDefault(s => s.SeasonNumber == seasonNumber);
            return season?.Episodes ?? new List<Episode>();
        }

        public async Task<List<StreamSource>> GetStreamSourcesAsync(MediaItem media)
        {
            await Task.Delay(1000); // Simulate provider search
            
            return GenerateMockStreamSources(media);
        }

        public async Task<List<MediaItem>> GetRecommendationsAsync(int mediaId, MediaType type)
        {
            await Task.Delay(300);
            
            var items = type == MediaType.Movie ? _mockMovies : _mockTvShows;
            return items.Where(m => m.Id != mediaId).OrderBy(x => Guid.NewGuid()).Take(10).ToList();
        }

        public async Task<List<MediaItem>> GetSimilarAsync(int mediaId, MediaType type)
        {
            await Task.Delay(300);
            
            var items = type == MediaType.Movie ? _mockMovies : _mockTvShows;
            return items.Where(m => m.Id != mediaId).OrderBy(x => Guid.NewGuid()).Take(10).ToList();
        }

        // User-specific operations
        public async Task<List<MediaItem>> GetWatchlistAsync(string userId)
        {
            await Task.Delay(100);
            return _userWatchlists.GetValueOrDefault(userId, new List<MediaItem>());
        }

        public async Task<List<MediaItem>> GetContinueWatchingAsync(string userId)
        {
            await Task.Delay(100);
            return _userContinueWatching.GetValueOrDefault(userId, new List<MediaItem>());
        }

        public async Task<List<MediaItem>> GetWatchHistoryAsync(string userId)
        {
            await Task.Delay(100);
            return _userHistory.GetValueOrDefault(userId, new List<MediaItem>());
        }

        public async Task AddToWatchlistAsync(string userId, MediaItem media)
        {
            await Task.Delay(100);
            
            if (!_userWatchlists.ContainsKey(userId))
                _userWatchlists[userId] = new List<MediaItem>();
                
            if (!_userWatchlists[userId].Any(m => m.Id == media.Id))
                _userWatchlists[userId].Add(media);
        }

        public async Task RemoveFromWatchlistAsync(string userId, MediaItem media)
        {
            await Task.Delay(100);
            
            if (_userWatchlists.ContainsKey(userId))
                _userWatchlists[userId].RemoveAll(m => m.Id == media.Id);
        }

        public async Task UpdateWatchStatusAsync(string userId, MediaItem media)
        {
            await Task.Delay(100);
            // In a real implementation, this would update the database
        }

        public async Task UpdateProgressAsync(string userId, MediaItem media, TimeSpan position)
        {
            await Task.Delay(100);
            
            if (!_userContinueWatching.ContainsKey(userId))
                _userContinueWatching[userId] = new List<MediaItem>();
                
            var existing = _userContinueWatching[userId].FirstOrDefault(m => m.Id == media.Id);
            if (existing != null)
                existing.CurrentPosition = position;
            else
            {
                media.CurrentPosition = position;
                _userContinueWatching[userId].Insert(0, media);
            }
        }

        public async Task RateMediaAsync(string userId, MediaItem media, double rating)
        {
            await Task.Delay(100);
            media.UserRating = rating;
        }

        // Provider operations
        public async Task<List<ProviderPackage>> GetProviderPackagesAsync()
        {
            await Task.Delay(200);
            return GenerateMockProviders();
        }

        public async Task<ProviderPackage?> InstallProviderAsync(string url)
        {
            await Task.Delay(2000); // Simulate download and install
            
            return new ProviderPackage
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Sample Provider",
                Version = "1.0.0",
                Author = "Provider Author",
                Description = "Sample provider package",
                Url = url,
                IsInstalled = true,
                IsEnabled = true,
                InstallDate = DateTime.Now,
                Status = ProviderStatus.Working
            };
        }

        public async Task UpdateProviderAsync(ProviderPackage provider)
        {
            await Task.Delay(1000);
            provider.LastUpdate = DateTime.Now;
        }

        public async Task RemoveProviderAsync(ProviderPackage provider)
        {
            await Task.Delay(500);
            provider.IsInstalled = false;
        }

        // Mock data generation
        private List<MediaItem> GenerateMockMovies()
        {
            var movies = new List<MediaItem>();
            var genres = new List<Genre>
            {
                new() { Id = 1, Name = "Action" },
                new() { Id = 2, Name = "Adventure" },
                new() { Id = 3, Name = "Comedy" },
                new() { Id = 4, Name = "Drama" },
                new() { Id = 5, Name = "Horror" },
                new() { Id = 6, Name = "Sci-Fi" },
                new() { Id = 7, Name = "Thriller" }
            };

            var titles = new[]
            {
                "The Matrix Resurrections", "Dune", "Spider-Man: No Way Home", "The Batman",
                "Top Gun: Maverick", "Black Panther: Wakanda Forever", "Avatar: The Way of Water",
                "Jurassic World Dominion", "Thor: Love and Thunder", "Minions: The Rise of Gru",
                "Doctor Strange in the Multiverse of Madness", "Fast X", "Indiana Jones 5",
                "John Wick: Chapter 4", "Guardians of the Galaxy Vol. 3", "The Flash",
                "Transformers: Rise of the Beasts", "Mission: Impossible 7", "Oppenheimer",
                "Barbie", "Killers of the Flower Moon", "Napoleon", "The Hunger Games",
                "Aquaman and the Lost Kingdom", "The Marvels"
            };

            for (int i = 0; i < titles.Length; i++)
            {
                movies.Add(new MediaItem
                {
                    Id = i + 1,
                    Title = titles[i],
                    Type = MediaType.Movie,
                    Overview = $"An exciting {titles[i]} adventure that will keep you on the edge of your seat.",
                    VoteAverage = Random.Shared.NextDouble() * 5 + 5, // 5-10 rating
                    VoteCount = Random.Shared.Next(1000, 50000),
                    Popularity = Random.Shared.NextDouble() * 100,
                    ReleaseDate = DateTime.Now.AddDays(Random.Shared.Next(-365, 365)),
                    Runtime = Random.Shared.Next(90, 180),
                    Genres = genres.OrderBy(x => Guid.NewGuid()).Take(Random.Shared.Next(1, 4)).ToList()
                });
            }

            return movies;
        }

        private List<MediaItem> GenerateMockTvShows()
        {
            var shows = new List<MediaItem>();
            var genres = new List<Genre>
            {
                new() { Id = 1, Name = "Drama" },
                new() { Id = 2, Name = "Comedy" },
                new() { Id = 3, Name = "Action" },
                new() { Id = 4, Name = "Crime" },
                new() { Id = 5, Name = "Fantasy" },
                new() { Id = 6, Name = "Sci-Fi" },
                new() { Id = 7, Name = "Thriller" }
            };

            var titles = new[]
            {
                "Stranger Things", "The Witcher", "Ozark", "The Crown", "Bridgerton",
                "Money Heist", "Squid Game", "Wednesday", "House of the Dragon", "The Boys",
                "Euphoria", "Succession", "The Mandalorian", "Ted Lasso", "Mare of Easttown",
                "Loki", "WandaVision", "The Falcon and the Winter Soldier", "Hawkeye",
                "Moon Knight", "She-Hulk", "Ms. Marvel", "What If...?", "The Bear", "Severance"
            };

            for (int i = 0; i < titles.Length; i++)
            {
                var seasonsCount = Random.Shared.Next(1, 6);
                var seasons = new List<Season>();
                
                for (int s = 1; s <= seasonsCount; s++)
                {
                    var episodeCount = Random.Shared.Next(6, 12);
                    var episodes = new List<Episode>();
                    
                    for (int e = 1; e <= episodeCount; e++)
                    {
                        episodes.Add(new Episode
                        {
                            Id = (i * 100) + (s * 10) + e,
                            Name = $"Episode {e}",
                            Overview = $"Episode {e} of season {s}",
                            EpisodeNumber = e,
                            SeasonNumber = s,
                            ShowId = i + 1000,
                            Runtime = Random.Shared.Next(40, 70),
                            AirDate = DateTime.Now.AddDays(-Random.Shared.Next(0, 365))
                        });
                    }
                    
                    seasons.Add(new Season
                    {
                        Id = (i * 10) + s,
                        Name = $"Season {s}",
                        Overview = $"Season {s} overview",
                        SeasonNumber = s,
                        EpisodeCount = episodeCount,
                        Episodes = episodes,
                        AirDate = DateTime.Now.AddDays(-Random.Shared.Next(0, 365))
                    });
                }

                shows.Add(new MediaItem
                {
                    Id = i + 1000,
                    Title = titles[i],
                    Type = MediaType.TvShow,
                    Overview = $"An amazing series about {titles[i]} that will captivate you.",
                    VoteAverage = Random.Shared.NextDouble() * 5 + 5,
                    VoteCount = Random.Shared.Next(5000, 100000),
                    Popularity = Random.Shared.NextDouble() * 100,
                    FirstAirDate = DateTime.Now.AddDays(-Random.Shared.Next(365, 3650)),
                    NumberOfSeasons = seasonsCount,
                    NumberOfEpisodes = seasons.Sum(s => s.EpisodeCount),
                    Seasons = seasons,
                    InProduction = Random.Shared.NextDouble() > 0.7,
                    Genres = genres.OrderBy(x => Guid.NewGuid()).Take(Random.Shared.Next(1, 3)).ToList()
                });
            }

            return shows;
        }

        private List<StreamSource> GenerateMockStreamSources(MediaItem media)
        {
            var sources = new List<StreamSource>();
            var qualities = new[] { "480p", "720p", "1080p", "4K" };
            var codecs = new[] { "H.264", "H.265", "XVID" };
            var providers = new[] { "RealDebrid", "Premiumize", "TorrentProvider", "DirectLink" };

            for (int i = 0; i < Random.Shared.Next(5, 15); i++)
            {
                var quality = qualities[Random.Shared.Next(qualities.Length)];
                var codec = codecs[Random.Shared.Next(codecs.Length)];
                var provider = providers[Random.Shared.Next(providers.Length)];

                sources.Add(new StreamSource
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = $"{media.DisplayTitle} {quality} {codec}",
                    Quality = quality,
                    VideoCodec = codec,
                    Provider = provider,
                    Type = provider.Contains("Debrid") ? SourceType.Debrid : SourceType.Torrent,
                    FileSize = Random.Shared.NextInt64(500_000_000, 15_000_000_000), // 500MB - 15GB
                    Seeds = Random.Shared.Next(5, 500),
                    Peers = Random.Shared.Next(1, 50),
                    IsVerified = Random.Shared.NextDouble() > 0.3,
                    IsCached = provider.Contains("Debrid") && Random.Shared.NextDouble() > 0.2,
                    IsHDR = quality == "4K" && Random.Shared.NextDouble() > 0.7,
                    DateAdded = DateTime.Now.AddHours(-Random.Shared.Next(1, 168))
                });
            }

            return sources.OrderByDescending(s => s.QualityScore).ToList();
        }

        private List<ProviderPackage> GenerateMockProviders()
        {
            return new List<ProviderPackage>
            {
                new()
                {
                    Id = "1",
                    Name = "Universal Provider",
                    Version = "2.1.0",
                    Author = "SynclerTeam",
                    Description = "Universal provider for movies and TV shows",
                    IsInstalled = true,
                    IsEnabled = true,
                    Status = ProviderStatus.Working,
                    SupportedTypes = new() { "movies", "tv" }
                },
                new()
                {
                    Id = "2",
                    Name = "Anime Provider",
                    Version = "1.5.0",
                    Author = "AnimeTeam",
                    Description = "Specialized provider for anime content",
                    IsInstalled = true,
                    IsEnabled = true,
                    Status = ProviderStatus.Working,
                    SupportedTypes = new() { "anime" }
                }
            };
        }
    }
}
using SynclerWindows.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynclerWindows.Services
{
    public class SearchService : ISearchService
    {
        private readonly IMediaService _mediaService;
        private readonly List<string> _popularSearches = new()
        {
            "Avengers", "Stranger Things", "Game of Thrones", "Breaking Bad", "The Office",
            "Marvel", "Star Wars", "Batman", "Spider-Man", "The Walking Dead"
        };

        public SearchService()
        {
            _mediaService = new MediaService();
        }

        public async Task<List<MediaItem>> SearchAsync(string query)
        {
            await Task.Delay(300); // Simulate search delay
            
            if (string.IsNullOrWhiteSpace(query))
                return new List<MediaItem>();

            var results = new List<MediaItem>();
            
            // Search in movies and TV shows
            var movies = await SearchMoviesAsync(query);
            var tvShows = await SearchTvShowsAsync(query);
            
            results.AddRange(movies);
            results.AddRange(tvShows);
            
            // Sort by relevance (simplified - just by title match and popularity)
            return results.OrderByDescending(m => GetRelevanceScore(m, query))
                         .ThenByDescending(m => m.Popularity)
                         .Take(50)
                         .ToList();
        }

        public async Task<List<MediaItem>> SearchMoviesAsync(string query)
        {
            await Task.Delay(200);
            
            var allMovies = await _mediaService.GetPopularAsync(MediaType.Movie);
            
            return allMovies.Where(m => 
                m.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                m.OriginalTitle.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                m.Overview.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                m.Genres.Any(g => g.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
            ).ToList();
        }

        public async Task<List<MediaItem>> SearchTvShowsAsync(string query)
        {
            await Task.Delay(200);
            
            var allShows = await _mediaService.GetPopularAsync(MediaType.TvShow);
            
            return allShows.Where(s => 
                s.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                s.OriginalTitle.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                s.Overview.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                s.Genres.Any(g => g.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
            ).ToList();
        }

        public async Task<List<MediaItem>> SearchAnimeAsync(string query)
        {
            await Task.Delay(200);
            
            // In a real implementation, this would search anime-specific sources
            var allShows = await _mediaService.GetPopularAsync(MediaType.TvShow);
            
            return allShows.Where(s => 
                (s.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                 s.OriginalTitle.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                 s.Overview.Contains(query, StringComparison.OrdinalIgnoreCase)) &&
                (s.Genres.Any(g => g.Name.Contains("anime", StringComparison.OrdinalIgnoreCase)) ||
                 s.OriginalLanguage == "ja")
            ).ToList();
        }

        public async Task<List<MediaItem>> SearchByGenreAsync(string genre, MediaType type)
        {
            await Task.Delay(200);
            return await _mediaService.GetByGenreAsync(genre, type);
        }

        public async Task<List<MediaItem>> SearchByYearAsync(int year, MediaType type)
        {
            await Task.Delay(200);
            
            var allItems = await _mediaService.GetPopularAsync(type);
            
            return allItems.Where(m => 
                (type == MediaType.Movie && m.ReleaseDate?.Year == year) ||
                (type == MediaType.TvShow && m.FirstAirDate?.Year == year)
            ).ToList();
        }

        public async Task<List<MediaItem>> GetSearchSuggestionsAsync(string query)
        {
            await Task.Delay(100);
            
            if (string.IsNullOrWhiteSpace(query) || query.Length < 2)
                return new List<MediaItem>();

            var results = await SearchAsync(query);
            return results.Take(10).ToList();
        }

        public async Task<List<string>> GetPopularSearchesAsync()
        {
            await Task.Delay(100);
            return _popularSearches.OrderBy(x => Guid.NewGuid()).Take(10).ToList();
        }

        public async Task<List<Genre>> GetGenresAsync(MediaType type)
        {
            await Task.Delay(100);
            
            return type switch
            {
                MediaType.Movie => new List<Genre>
                {
                    new() { Id = 1, Name = "Action" },
                    new() { Id = 2, Name = "Adventure" },
                    new() { Id = 3, Name = "Animation" },
                    new() { Id = 4, Name = "Comedy" },
                    new() { Id = 5, Name = "Crime" },
                    new() { Id = 6, Name = "Documentary" },
                    new() { Id = 7, Name = "Drama" },
                    new() { Id = 8, Name = "Family" },
                    new() { Id = 9, Name = "Fantasy" },
                    new() { Id = 10, Name = "History" },
                    new() { Id = 11, Name = "Horror" },
                    new() { Id = 12, Name = "Music" },
                    new() { Id = 13, Name = "Mystery" },
                    new() { Id = 14, Name = "Romance" },
                    new() { Id = 15, Name = "Science Fiction" },
                    new() { Id = 16, Name = "TV Movie" },
                    new() { Id = 17, Name = "Thriller" },
                    new() { Id = 18, Name = "War" },
                    new() { Id = 19, Name = "Western" }
                },
                MediaType.TvShow => new List<Genre>
                {
                    new() { Id = 1, Name = "Action & Adventure" },
                    new() { Id = 2, Name = "Animation" },
                    new() { Id = 3, Name = "Comedy" },
                    new() { Id = 4, Name = "Crime" },
                    new() { Id = 5, Name = "Documentary" },
                    new() { Id = 6, Name = "Drama" },
                    new() { Id = 7, Name = "Family" },
                    new() { Id = 8, Name = "Kids" },
                    new() { Id = 9, Name = "Mystery" },
                    new() { Id = 10, Name = "News" },
                    new() { Id = 11, Name = "Reality" },
                    new() { Id = 12, Name = "Sci-Fi & Fantasy" },
                    new() { Id = 13, Name = "Soap" },
                    new() { Id = 14, Name = "Talk" },
                    new() { Id = 15, Name = "War & Politics" },
                    new() { Id = 16, Name = "Western" }
                },
                _ => new List<Genre>()
            };
        }

        private static double GetRelevanceScore(MediaItem item, string query)
        {
            double score = 0;
            
            // Exact title match gets highest score
            if (item.Title.Equals(query, StringComparison.OrdinalIgnoreCase))
                score += 100;
            
            // Title starts with query
            if (item.Title.StartsWith(query, StringComparison.OrdinalIgnoreCase))
                score += 50;
            
            // Title contains query
            if (item.Title.Contains(query, StringComparison.OrdinalIgnoreCase))
                score += 25;
            
            // Original title matches
            if (item.OriginalTitle.Contains(query, StringComparison.OrdinalIgnoreCase))
                score += 15;
            
            // Overview contains query
            if (item.Overview.Contains(query, StringComparison.OrdinalIgnoreCase))
                score += 5;
            
            // Genre matches
            if (item.Genres.Any(g => g.Name.Contains(query, StringComparison.OrdinalIgnoreCase)))
                score += 10;
            
            return score;
        }
    }
}
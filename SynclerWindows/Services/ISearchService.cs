using SynclerWindows.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SynclerWindows.Services
{
    public interface ISearchService
    {
        Task<List<MediaItem>> SearchAsync(string query);
        Task<List<MediaItem>> SearchMoviesAsync(string query);
        Task<List<MediaItem>> SearchTvShowsAsync(string query);
        Task<List<MediaItem>> SearchAnimeAsync(string query);
        Task<List<MediaItem>> SearchByGenreAsync(string genre, MediaType type);
        Task<List<MediaItem>> SearchByYearAsync(int year, MediaType type);
        Task<List<MediaItem>> GetSearchSuggestionsAsync(string query);
        Task<List<string>> GetPopularSearchesAsync();
        Task<List<Genre>> GetGenresAsync(MediaType type);
    }
}
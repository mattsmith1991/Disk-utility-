using SynclerWindows.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SynclerWindows.Services
{
    public interface IMediaService
    {
        Task<List<MediaItem>> GetTrendingAsync();
        Task<List<MediaItem>> GetPopularAsync(MediaType type);
        Task<List<MediaItem>> GetTopRatedAsync(MediaType type);
        Task<List<MediaItem>> GetUpcomingAsync();
        Task<List<MediaItem>> GetNowPlayingAsync();
        Task<List<MediaItem>> GetByGenreAsync(string genre, MediaType type);
        Task<MediaItem?> GetMediaDetailsAsync(int id, MediaType type);
        Task<List<Season>> GetSeasonsAsync(int showId);
        Task<List<Episode>> GetEpisodesAsync(int showId, int seasonNumber);
        Task<List<StreamSource>> GetStreamSourcesAsync(MediaItem media);
        Task<List<MediaItem>> GetRecommendationsAsync(int mediaId, MediaType type);
        Task<List<MediaItem>> GetSimilarAsync(int mediaId, MediaType type);
        
        // User-specific operations
        Task<List<MediaItem>> GetWatchlistAsync(string userId);
        Task<List<MediaItem>> GetContinueWatchingAsync(string userId);
        Task<List<MediaItem>> GetWatchHistoryAsync(string userId);
        Task AddToWatchlistAsync(string userId, MediaItem media);
        Task RemoveFromWatchlistAsync(string userId, MediaItem media);
        Task UpdateWatchStatusAsync(string userId, MediaItem media);
        Task UpdateProgressAsync(string userId, MediaItem media, TimeSpan position);
        Task RateMediaAsync(string userId, MediaItem media, double rating);
        
        // Provider operations
        Task<List<ProviderPackage>> GetProviderPackagesAsync();
        Task<ProviderPackage?> InstallProviderAsync(string url);
        Task UpdateProviderAsync(ProviderPackage provider);
        Task RemoveProviderAsync(ProviderPackage provider);
    }
}
using System;
using System.Collections.Generic;

namespace SynclerWindows.Models
{
    public class User
    {
        public string Id { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public bool IsActive { get; set; } = true;
        public UserSubscription Subscription { get; set; } = new UserSubscription();
        public UserPreferences Preferences { get; set; } = new UserPreferences();
        public List<UserProfile> Profiles { get; set; } = new List<UserProfile>();
        public ExternalAccounts ExternalAccounts { get; set; } = new ExternalAccounts();
    }

    public class UserProfile
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public bool IsDefault { get; set; }
        public bool IsChild { get; set; }
        public string Pin { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public UserPreferences Preferences { get; set; } = new UserPreferences();
        public List<MediaItem> Watchlist { get; set; } = new List<MediaItem>();
        public List<MediaItem> ContinueWatching { get; set; } = new List<MediaItem>();
        public List<MediaItem> WatchHistory { get; set; } = new List<MediaItem>();
    }

    public class UserSubscription
    {
        public bool IsPremium { get; set; }
        public string SubscriptionTier { get; set; } = "Free";
        public DateTime? ExpiryDate { get; set; }
        public bool IsActive { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public List<string> Features { get; set; } = new List<string>();
    }

    public class UserPreferences
    {
        public string Theme { get; set; } = "Dark";
        public string Language { get; set; } = "en";
        public bool AutoPlay { get; set; } = true;
        public bool AutoNext { get; set; } = true;
        public int VideoQuality { get; set; } = 1080;
        public bool ShowAdultContent { get; set; } = false;
        public List<string> PreferredGenres { get; set; } = new List<string>();
        public string DefaultPlayer { get; set; } = "Built-in";
        public double Volume { get; set; } = 0.8;
        public bool EnableNotifications { get; set; } = true;
        public bool SyncProgress { get; set; } = true;
        public string HomeLayout { get; set; } = "Default";
        public List<HomeSection> HomeSections { get; set; } = new List<HomeSection>();
    }

    public class HomeSection
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // trending, popular, genre, etc.
        public bool IsEnabled { get; set; } = true;
        public int Order { get; set; }
        public Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();
    }

    public class ExternalAccounts
    {
        public TraktAccount? Trakt { get; set; }
        public SimklAccount? Simkl { get; set; }
        public MyAnimeListAccount? MyAnimeList { get; set; }
        public List<DebridAccount> DebridAccounts { get; set; } = new List<DebridAccount>();
    }

    public class TraktAccount
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime TokenExpiry { get; set; }
        public string Username { get; set; } = string.Empty;
        public bool IsConnected { get; set; }
        public bool SyncWatchlist { get; set; } = true;
        public bool SyncProgress { get; set; } = true;
        public bool SyncRatings { get; set; } = true;
    }

    public class SimklAccount
    {
        public string AccessToken { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public bool IsConnected { get; set; }
        public bool SyncWatchlist { get; set; } = true;
        public bool SyncProgress { get; set; } = true;
    }

    public class MyAnimeListAccount
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime TokenExpiry { get; set; }
        public string Username { get; set; } = string.Empty;
        public bool IsConnected { get; set; }
        public bool SyncWatchlist { get; set; } = true;
        public bool SyncProgress { get; set; } = true;
    }

    public class DebridAccount
    {
        public string Id { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // RealDebrid, Premiumize, AllDebrid, etc.
        public string ApiKey { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime? ExpiryDate { get; set; }
        public long? BytesUsed { get; set; }
        public long? BytesLimit { get; set; }
        public int Priority { get; set; } = 1;
    }
}
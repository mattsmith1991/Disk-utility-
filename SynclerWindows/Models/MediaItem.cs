using System;
using System.Collections.Generic;

namespace SynclerWindows.Models
{
    public class MediaItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string OriginalTitle { get; set; } = string.Empty;
        public string Overview { get; set; } = string.Empty;
        public string PosterPath { get; set; } = string.Empty;
        public string BackdropPath { get; set; } = string.Empty;
        public DateTime? ReleaseDate { get; set; }
        public DateTime? FirstAirDate { get; set; }
        public double VoteAverage { get; set; }
        public int VoteCount { get; set; }
        public double Popularity { get; set; }
        public List<string> GenreIds { get; set; } = new List<string>();
        public List<Genre> Genres { get; set; } = new List<Genre>();
        public string OriginalLanguage { get; set; } = string.Empty;
        public bool Adult { get; set; }
        public MediaType Type { get; set; }
        public int? Runtime { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Tagline { get; set; } = string.Empty;
        public List<ProductionCompany> ProductionCompanies { get; set; } = new List<ProductionCompany>();
        public List<ProductionCountry> ProductionCountries { get; set; } = new List<ProductionCountry>();
        public List<SpokenLanguage> SpokenLanguages { get; set; } = new List<SpokenLanguage>();
        
        // TV Show specific properties
        public int? NumberOfSeasons { get; set; }
        public int? NumberOfEpisodes { get; set; }
        public List<Season> Seasons { get; set; } = new List<Season>();
        public List<Episode> Episodes { get; set; } = new List<Episode>();
        public bool InProduction { get; set; }
        public List<string> Networks { get; set; } = new List<string>();
        public List<string> CreatedBy { get; set; } = new List<string>();
        
        // User tracking data
        public WatchStatus WatchStatus { get; set; } = WatchStatus.Unwatched;
        public DateTime? WatchedDate { get; set; }
        public double? UserRating { get; set; }
        public bool IsInWatchlist { get; set; }
        public TimeSpan? CurrentPosition { get; set; }
        public TimeSpan? TotalDuration { get; set; }
        public double ProgressPercentage => TotalDuration?.TotalSeconds > 0 && CurrentPosition.HasValue 
            ? (CurrentPosition.Value.TotalSeconds / TotalDuration.Value.TotalSeconds) * 100 
            : 0;
        
        // External IDs
        public ExternalIds ExternalIds { get; set; } = new ExternalIds();
        
        // Full poster and backdrop URLs
        public string FullPosterUrl => !string.IsNullOrEmpty(PosterPath) 
            ? $"https://image.tmdb.org/t/p/w500{PosterPath}" 
            : string.Empty;
        
        public string FullBackdropUrl => !string.IsNullOrEmpty(BackdropPath) 
            ? $"https://image.tmdb.org/t/p/w1280{BackdropPath}" 
            : string.Empty;
        
        public string DisplayTitle => !string.IsNullOrEmpty(Title) ? Title : OriginalTitle;
        public string DisplayDate => Type == MediaType.Movie 
            ? ReleaseDate?.Year.ToString() ?? string.Empty
            : FirstAirDate?.Year.ToString() ?? string.Empty;
    }

    public enum MediaType
    {
        Movie,
        TvShow,
        Episode,
        Season,
        Person
    }

    public enum WatchStatus
    {
        Unwatched,
        Watching,
        Watched,
        OnHold,
        Dropped
    }

    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class ProductionCompany
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LogoPath { get; set; } = string.Empty;
        public string OriginCountry { get; set; } = string.Empty;
    }

    public class ProductionCountry
    {
        public string Iso31661 { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }

    public class SpokenLanguage
    {
        public string Iso6391 { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string EnglishName { get; set; } = string.Empty;
    }

    public class Season
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Overview { get; set; } = string.Empty;
        public DateTime? AirDate { get; set; }
        public int EpisodeCount { get; set; }
        public string PosterPath { get; set; } = string.Empty;
        public int SeasonNumber { get; set; }
        public List<Episode> Episodes { get; set; } = new List<Episode>();
        
        public string FullPosterUrl => !string.IsNullOrEmpty(PosterPath) 
            ? $"https://image.tmdb.org/t/p/w500{PosterPath}" 
            : string.Empty;
    }

    public class Episode
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Overview { get; set; } = string.Empty;
        public DateTime? AirDate { get; set; }
        public int EpisodeNumber { get; set; }
        public int SeasonNumber { get; set; }
        public string StillPath { get; set; } = string.Empty;
        public double VoteAverage { get; set; }
        public int VoteCount { get; set; }
        public int? Runtime { get; set; }
        public int ShowId { get; set; }
        
        // User tracking
        public WatchStatus WatchStatus { get; set; } = WatchStatus.Unwatched;
        public DateTime? WatchedDate { get; set; }
        public TimeSpan? CurrentPosition { get; set; }
        public TimeSpan? TotalDuration { get; set; }
        
        public string FullStillUrl => !string.IsNullOrEmpty(StillPath) 
            ? $"https://image.tmdb.org/t/p/w500{StillPath}" 
            : string.Empty;
    }

    public class ExternalIds
    {
        public string ImdbId { get; set; } = string.Empty;
        public string TvdbId { get; set; } = string.Empty;
        public string FacebookId { get; set; } = string.Empty;
        public string InstagramId { get; set; } = string.Empty;
        public string TwitterId { get; set; } = string.Empty;
    }
}
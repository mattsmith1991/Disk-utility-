using System;
using System.Collections.Generic;

namespace SynclerWindows.Models
{
    public class StreamSource
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Quality { get; set; } = string.Empty; // 720p, 1080p, 4K, etc.
        public string Resolution { get; set; } = string.Empty; // 1920x1080, etc.
        public long FileSize { get; set; } // in bytes
        public string Provider { get; set; } = string.Empty;
        public string ProviderType { get; set; } = string.Empty; // debrid, torrent, direct, etc.
        public SourceType Type { get; set; }
        public string VideoCodec { get; set; } = string.Empty; // H.264, H.265, etc.
        public string AudioCodec { get; set; } = string.Empty; // AAC, AC3, DTS, etc.
        public string Language { get; set; } = string.Empty;
        public List<string> AudioLanguages { get; set; } = new List<string>();
        public List<Subtitle> Subtitles { get; set; } = new List<Subtitle>();
        public bool IsHDR { get; set; }
        public bool IsDolbyVision { get; set; }
        public bool IsDolbyAtmos { get; set; }
        public int Bitrate { get; set; } // in kbps
        public int Seeds { get; set; } // for torrents
        public int Peers { get; set; } // for torrents
        public double Rating { get; set; }
        public bool IsVerified { get; set; }
        public bool IsCached { get; set; } // for debrid services
        public DateTime DateAdded { get; set; }
        public TimeSpan? Duration { get; set; }
        public int Priority { get; set; } = 0;
        public Dictionary<string, object> Headers { get; set; } = new Dictionary<string, object>();
        public string ReleaseGroup { get; set; } = string.Empty;
        public string Hash { get; set; } = string.Empty; // torrent hash
        public List<StreamFile> Files { get; set; } = new List<StreamFile>(); // for multi-file torrents
        
        // Quality scoring for auto-selection
        public int QualityScore => CalculateQualityScore();
        
        // Display formatting
        public string DisplayTitle => FormatDisplayTitle();
        public string FormattedFileSize => FormatFileSize(FileSize);
        public string QualityInfo => $"{Quality} • {VideoCodec} • {FormattedFileSize}";
        
        private int CalculateQualityScore()
        {
            int score = 0;
            
            // Base quality score
            score += Quality switch
            {
                "4K" or "2160p" => 1000,
                "1080p" => 800,
                "720p" => 600,
                "480p" => 400,
                _ => 200
            };
            
            // Codec bonus
            if (VideoCodec.Contains("265") || VideoCodec.Contains("HEVC"))
                score += 100;
            
            // HDR bonus
            if (IsHDR) score += 150;
            if (IsDolbyVision) score += 200;
            if (IsDolbyAtmos) score += 50;
            
            // Source type bonus
            score += Type switch
            {
                SourceType.Debrid => 500,
                SourceType.Direct => 300,
                SourceType.Torrent => IsCached ? 400 : Seeds > 10 ? 200 : 100,
                _ => 50
            };
            
            // Verification bonus
            if (IsVerified) score += 100;
            
            return score;
        }
        
        private string FormatDisplayTitle()
        {
            var parts = new List<string>();
            
            if (!string.IsNullOrEmpty(Title))
                parts.Add(Title);
            
            if (!string.IsNullOrEmpty(Quality))
                parts.Add(Quality);
                
            if (!string.IsNullOrEmpty(VideoCodec))
                parts.Add(VideoCodec);
                
            if (IsHDR)
                parts.Add("HDR");
                
            if (IsDolbyVision)
                parts.Add("DV");
                
            if (!string.IsNullOrEmpty(ReleaseGroup))
                parts.Add($"[{ReleaseGroup}]");
            
            return string.Join(" • ", parts);
        }
        
        private static string FormatFileSize(long bytes)
        {
            if (bytes == 0) return "Unknown";
            
            string[] suffixes = { "B", "KB", "MB", "GB", "TB" };
            int suffixIndex = 0;
            double size = bytes;
            
            while (size >= 1024 && suffixIndex < suffixes.Length - 1)
            {
                size /= 1024;
                suffixIndex++;
            }
            
            return $"{size:0.##} {suffixes[suffixIndex]}";
        }
    }

    public enum SourceType
    {
        Direct,
        Torrent,
        Debrid,
        Magnet,
        Stream,
        Cache
    }

    public class Subtitle
    {
        public string Id { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public string LanguageCode { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Format { get; set; } = string.Empty; // SRT, VTT, ASS, etc.
        public bool IsDefault { get; set; }
        public bool IsForced { get; set; }
        public bool IsHearingImpaired { get; set; }
        public string Name { get; set; } = string.Empty;
        public SubtitleSource Source { get; set; }
    }

    public enum SubtitleSource
    {
        Embedded,
        External,
        OpenSubtitles,
        Subscene,
        Provider
    }

    public class StreamFile
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public long Size { get; set; }
        public bool IsSelected { get; set; }
        public int Index { get; set; }
    }

    public class ProviderPackage
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public bool IsInstalled { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime InstallDate { get; set; }
        public DateTime LastUpdate { get; set; }
        public List<string> SupportedTypes { get; set; } = new List<string>(); // movies, tv, anime
        public Dictionary<string, object> Settings { get; set; } = new Dictionary<string, object>();
        public ProviderStatus Status { get; set; } = ProviderStatus.Unknown;
    }

    public enum ProviderStatus
    {
        Unknown,
        Working,
        Limited,
        Down,
        Maintenance
    }
}
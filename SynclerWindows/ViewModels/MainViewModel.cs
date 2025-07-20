using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using SynclerWindows.Models;
using SynclerWindows.Services;
using SynclerWindows.Views;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Controls;

namespace SynclerWindows.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;
        private readonly IUserService _userService;
        private readonly IMediaService _mediaService;
        private readonly ISearchService _searchService;

        [ObservableProperty]
        private User? currentUser;

        [ObservableProperty]
        private string searchQuery = string.Empty;

        [ObservableProperty]
        private UserControl? currentView;

        [ObservableProperty]
        private string currentPage = "Home";

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private string statusMessage = string.Empty;

        public ObservableCollection<MediaItem> SearchResults { get; } = new();
        public ObservableCollection<MediaItem> TrendingContent { get; } = new();
        public ObservableCollection<MediaItem> ContinueWatching { get; } = new();
        public ObservableCollection<MediaItem> Watchlist { get; } = new();

        public ICommand NavigateCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand ShowProfileCommand { get; }
        public ICommand ShowSettingsCommand { get; }
        public ICommand PlayMediaCommand { get; }
        public ICommand AddToWatchlistCommand { get; }
        public ICommand MarkAsWatchedCommand { get; }

        public MainViewModel()
        {
            _navigationService = new NavigationService();
            _userService = new UserService();
            _mediaService = new MediaService();
            _searchService = new SearchService();

            NavigateCommand = new RelayCommand<string>(OnNavigate);
            SearchCommand = new AsyncRelayCommand(OnSearchAsync);
            ShowProfileCommand = new RelayCommand(OnShowProfile);
            ShowSettingsCommand = new RelayCommand(OnShowSettings);
            PlayMediaCommand = new RelayCommand<MediaItem>(OnPlayMedia);
            AddToWatchlistCommand = new RelayCommand<MediaItem>(OnAddToWatchlist);
            MarkAsWatchedCommand = new RelayCommand<MediaItem>(OnMarkAsWatched);

            Initialize();
        }

        private async void Initialize()
        {
            IsLoading = true;
            StatusMessage = "Initializing Syncler...";

            try
            {
                // Load user data
                CurrentUser = await _userService.GetCurrentUserAsync() ?? CreateDefaultUser();
                
                // Load home content
                await LoadHomeContentAsync();
                
                // Navigate to home
                OnNavigate("Home");
                
                StatusMessage = "Ready";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error initializing: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private User CreateDefaultUser()
        {
            return new User
            {
                Id = Guid.NewGuid().ToString(),
                Name = "User",
                Username = "user",
                Email = "user@example.com",
                CreatedDate = DateTime.Now,
                LastLoginDate = DateTime.Now,
                Profiles = new()
                {
                    new UserProfile
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Default Profile",
                        IsDefault = true,
                        CreatedDate = DateTime.Now
                    }
                }
            };
        }

        private async Task LoadHomeContentAsync()
        {
            try
            {
                // Load trending content
                var trending = await _mediaService.GetTrendingAsync();
                TrendingContent.Clear();
                foreach (var item in trending)
                {
                    TrendingContent.Add(item);
                }

                // Load continue watching
                var continueWatching = await _mediaService.GetContinueWatchingAsync(CurrentUser?.Id ?? string.Empty);
                ContinueWatching.Clear();
                foreach (var item in continueWatching)
                {
                    ContinueWatching.Add(item);
                }

                // Load watchlist
                var watchlist = await _mediaService.GetWatchlistAsync(CurrentUser?.Id ?? string.Empty);
                Watchlist.Clear();
                foreach (var item in watchlist)
                {
                    Watchlist.Add(item);
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error loading content: {ex.Message}";
            }
        }

        private void OnNavigate(string? page)
        {
            if (string.IsNullOrEmpty(page)) return;

            CurrentPage = page;
            CurrentView = _navigationService.NavigateTo(page);
        }

        private async Task OnSearchAsync()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery)) return;

            IsLoading = true;
            StatusMessage = $"Searching for '{SearchQuery}'...";

            try
            {
                var results = await _searchService.SearchAsync(SearchQuery);
                SearchResults.Clear();
                foreach (var item in results)
                {
                    SearchResults.Add(item);
                }

                OnNavigate("Search");
                StatusMessage = $"Found {results.Count} results";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Search error: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void OnShowProfile()
        {
            OnNavigate("Profile");
        }

        private void OnShowSettings()
        {
            OnNavigate("Settings");
        }

        private void OnPlayMedia(MediaItem? media)
        {
            if (media == null) return;

            try
            {
                _navigationService.NavigateToPlayer(media);
                
                // Update continue watching
                if (!ContinueWatching.Contains(media))
                {
                    ContinueWatching.Insert(0, media);
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error playing media: {ex.Message}";
            }
        }

        private async void OnAddToWatchlist(MediaItem? media)
        {
            if (media == null || CurrentUser == null) return;

            try
            {
                media.IsInWatchlist = !media.IsInWatchlist;
                
                if (media.IsInWatchlist)
                {
                    await _mediaService.AddToWatchlistAsync(CurrentUser.Id, media);
                    if (!Watchlist.Contains(media))
                    {
                        Watchlist.Add(media);
                    }
                    StatusMessage = $"Added '{media.DisplayTitle}' to watchlist";
                }
                else
                {
                    await _mediaService.RemoveFromWatchlistAsync(CurrentUser.Id, media);
                    Watchlist.Remove(media);
                    StatusMessage = $"Removed '{media.DisplayTitle}' from watchlist";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error updating watchlist: {ex.Message}";
                media.IsInWatchlist = !media.IsInWatchlist; // Revert on error
            }
        }

        private async void OnMarkAsWatched(MediaItem? media)
        {
            if (media == null || CurrentUser == null) return;

            try
            {
                media.WatchStatus = media.WatchStatus == WatchStatus.Watched 
                    ? WatchStatus.Unwatched 
                    : WatchStatus.Watched;
                
                media.WatchedDate = media.WatchStatus == WatchStatus.Watched 
                    ? DateTime.Now 
                    : null;

                await _mediaService.UpdateWatchStatusAsync(CurrentUser.Id, media);
                
                StatusMessage = media.WatchStatus == WatchStatus.Watched
                    ? $"Marked '{media.DisplayTitle}' as watched"
                    : $"Marked '{media.DisplayTitle}' as unwatched";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error updating watch status: {ex.Message}";
            }
        }

        partial void OnSearchQueryChanged(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                SearchResults.Clear();
            }
        }
    }
}
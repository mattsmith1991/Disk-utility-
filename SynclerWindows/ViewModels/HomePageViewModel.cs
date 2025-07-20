using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using SynclerWindows.Models;
using SynclerWindows.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SynclerWindows.ViewModels
{
    public partial class HomePageViewModel : ObservableObject
    {
        private readonly IMediaService _mediaService;
        private readonly INavigationService _navigationService;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private bool hasContinueWatching;

        public ObservableCollection<MediaItem> TrendingContent { get; } = new();
        public ObservableCollection<MediaItem> ContinueWatching { get; } = new();
        public ObservableCollection<MediaItem> PopularMovies { get; } = new();
        public ObservableCollection<MediaItem> PopularTVShows { get; } = new();

        public ICommand BrowsePopularCommand { get; }
        public ICommand ViewAllTrendingCommand { get; }
        public ICommand ViewAllContinueWatchingCommand { get; }
        public ICommand ViewAllMoviesCommand { get; }
        public ICommand ViewAllTVShowsCommand { get; }
        public ICommand ViewDetailsCommand { get; }
        public ICommand PlayMediaCommand { get; }

        public HomePageViewModel()
        {
            _mediaService = new MediaService();
            _navigationService = new NavigationService();

            BrowsePopularCommand = new RelayCommand(OnBrowsePopular);
            ViewAllTrendingCommand = new RelayCommand(OnViewAllTrending);
            ViewAllContinueWatchingCommand = new RelayCommand(OnViewAllContinueWatching);
            ViewAllMoviesCommand = new RelayCommand(OnViewAllMovies);
            ViewAllTVShowsCommand = new RelayCommand(OnViewAllTVShows);
            ViewDetailsCommand = new RelayCommand<MediaItem>(OnViewDetails);
            PlayMediaCommand = new RelayCommand<MediaItem>(OnPlayMedia);

            LoadContentAsync();
        }

        private async void LoadContentAsync()
        {
            IsLoading = true;

            try
            {
                // Load all content in parallel
                var trendingTask = LoadTrendingContentAsync();
                var moviesTask = LoadPopularMoviesAsync();
                var tvShowsTask = LoadPopularTVShowsAsync();
                var continueWatchingTask = LoadContinueWatchingAsync();

                await Task.WhenAll(trendingTask, moviesTask, tvShowsTask, continueWatchingTask);
            }
            catch (Exception ex)
            {
                // Handle error - in real app, show user-friendly message
                System.Diagnostics.Debug.WriteLine($"Error loading home content: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task LoadTrendingContentAsync()
        {
            var trending = await _mediaService.GetTrendingAsync();
            TrendingContent.Clear();
            foreach (var item in trending.Take(10))
            {
                TrendingContent.Add(item);
            }
        }

        private async Task LoadPopularMoviesAsync()
        {
            var movies = await _mediaService.GetPopularAsync(MediaType.Movie);
            PopularMovies.Clear();
            foreach (var movie in movies.Take(10))
            {
                PopularMovies.Add(movie);
            }
        }

        private async Task LoadPopularTVShowsAsync()
        {
            var shows = await _mediaService.GetPopularAsync(MediaType.TvShow);
            PopularTVShows.Clear();
            foreach (var show in shows.Take(10))
            {
                PopularTVShows.Add(show);
            }
        }

        private async Task LoadContinueWatchingAsync()
        {
            // Mock user ID - in real app, get from user service
            var continueWatching = await _mediaService.GetContinueWatchingAsync("user123");
            ContinueWatching.Clear();
            foreach (var item in continueWatching.Take(8))
            {
                ContinueWatching.Add(item);
            }
            HasContinueWatching = ContinueWatching.Count > 0;
        }

        private void OnBrowsePopular()
        {
            _navigationService.NavigateTo("Movies");
        }

        private void OnViewAllTrending()
        {
            // Navigate to trending page or show more trending content
            _navigationService.NavigateTo("Movies");
        }

        private void OnViewAllContinueWatching()
        {
            _navigationService.NavigateTo("Continue");
        }

        private void OnViewAllMovies()
        {
            _navigationService.NavigateTo("Movies");
        }

        private void OnViewAllTVShows()
        {
            _navigationService.NavigateTo("TVShows");
        }

        private void OnViewDetails(MediaItem? media)
        {
            if (media == null) return;

            // In a real implementation, this would navigate to a details page
            // For now, just simulate playing the media
            OnPlayMedia(media);
        }

        private void OnPlayMedia(MediaItem? media)
        {
            if (media == null) return;

            try
            {
                // Add to continue watching if not already there
                if (!ContinueWatching.Contains(media))
                {
                    // Mock progress for demo
                    media.CurrentPosition = TimeSpan.FromMinutes(15);
                    media.TotalDuration = TimeSpan.FromMinutes(120);
                    
                    ContinueWatching.Insert(0, media);
                    HasContinueWatching = true;
                    
                    // Limit continue watching to 8 items
                    while (ContinueWatching.Count > 8)
                    {
                        ContinueWatching.RemoveAt(ContinueWatching.Count - 1);
                    }
                }

                _navigationService.NavigateToPlayer(media);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error playing media: {ex.Message}");
            }
        }
    }
}
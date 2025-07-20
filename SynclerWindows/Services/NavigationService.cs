using SynclerWindows.Models;
using SynclerWindows.Views.Pages;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace SynclerWindows.Services
{
    public class NavigationService : INavigationService
    {
        private readonly Stack<string> _navigationHistory = new();
        private readonly Dictionary<string, Func<UserControl>> _pageFactories = new();

        public string CurrentPage { get; private set; } = string.Empty;
        public bool CanNavigateBack => _navigationHistory.Count > 1;

        public NavigationService()
        {
            RegisterPages();
        }

        private void RegisterPages()
        {
            _pageFactories["Home"] = () => new HomePage();
            _pageFactories["Movies"] = () => new MoviesPage();
            _pageFactories["TVShows"] = () => new TVShowsPage();
            _pageFactories["Anime"] = () => new AnimePage();
            _pageFactories["Search"] = () => new SearchPage();
            _pageFactories["Continue"] = () => new ContinueWatchingPage();
            _pageFactories["Watchlist"] = () => new WatchlistPage();
            _pageFactories["Downloads"] = () => new DownloadsPage();
            _pageFactories["History"] = () => new HistoryPage();
            _pageFactories["Cloud"] = () => new CloudPage();
            _pageFactories["Transfers"] = () => new TransfersPage();
            _pageFactories["Profile"] = () => new ProfilePage();
            _pageFactories["Settings"] = () => new SettingsPage();
            _pageFactories["Player"] = () => new PlayerPage();
        }

        public UserControl NavigateTo(string page)
        {
            if (string.IsNullOrEmpty(page) || !_pageFactories.ContainsKey(page))
            {
                page = "Home";
            }

            if (CurrentPage != page)
            {
                if (!string.IsNullOrEmpty(CurrentPage))
                {
                    _navigationHistory.Push(CurrentPage);
                }
                CurrentPage = page;
            }

            return _pageFactories[page]();
        }

        public void NavigateToPlayer(MediaItem media)
        {
            // For now, just navigate to player page
            // In a real implementation, this would pass the media to the player
            NavigateTo("Player");
        }

        public void NavigateBack()
        {
            if (CanNavigateBack)
            {
                CurrentPage = _navigationHistory.Pop();
            }
        }
    }
}
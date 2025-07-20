using SynclerWindows.Models;
using System.Windows.Controls;

namespace SynclerWindows.Services
{
    public interface INavigationService
    {
        UserControl NavigateTo(string page);
        void NavigateToPlayer(MediaItem media);
        void NavigateBack();
        bool CanNavigateBack { get; }
        string CurrentPage { get; }
    }
}
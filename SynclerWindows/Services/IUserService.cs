using SynclerWindows.Models;
using System.Threading.Tasks;

namespace SynclerWindows.Services
{
    public interface IUserService
    {
        Task<User?> GetCurrentUserAsync();
        Task<User?> LoginAsync(string username, string password);
        Task<User?> RegisterAsync(string username, string email, string password);
        Task LogoutAsync();
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(string userId);
        Task<UserProfile?> CreateProfileAsync(string userId, UserProfile profile);
        Task<bool> UpdateProfileAsync(UserProfile profile);
        Task<bool> DeleteProfileAsync(string profileId);
        Task<bool> SetActiveProfileAsync(string userId, string profileId);
        Task<bool> ConnectTraktAsync(string userId, string accessToken, string refreshToken);
        Task<bool> DisconnectTraktAsync(string userId);
        Task<bool> ConnectDebridAsync(string userId, DebridAccount account);
        Task<bool> DisconnectDebridAsync(string userId, string debridId);
        Task<bool> SyncWithTraktAsync(string userId);
    }
}
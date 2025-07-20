using SynclerWindows.Models;
using System;
using System.Threading.Tasks;

namespace SynclerWindows.Services
{
    public class UserService : IUserService
    {
        private User? _currentUser;

        public async Task<User?> GetCurrentUserAsync()
        {
            await Task.Delay(100);
            return _currentUser;
        }

        public async Task<User?> LoginAsync(string username, string password)
        {
            await Task.Delay(500); // Simulate API call
            
            // Mock login - in real implementation, validate against API
            _currentUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                Username = username,
                Email = $"{username}@example.com",
                Name = username,
                CreatedDate = DateTime.Now.AddDays(-30),
                LastLoginDate = DateTime.Now,
                Profiles = new()
                {
                    new UserProfile
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Default Profile",
                        IsDefault = true,
                        CreatedDate = DateTime.Now.AddDays(-30)
                    }
                }
            };
            
            return _currentUser;
        }

        public async Task<User?> RegisterAsync(string username, string email, string password)
        {
            await Task.Delay(1000); // Simulate registration
            
            _currentUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                Username = username,
                Email = email,
                Name = username,
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
            
            return _currentUser;
        }

        public async Task LogoutAsync()
        {
            await Task.Delay(100);
            _currentUser = null;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            await Task.Delay(200);
            if (_currentUser?.Id == user.Id)
            {
                _currentUser = user;
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            await Task.Delay(300);
            if (_currentUser?.Id == userId)
            {
                _currentUser = null;
                return true;
            }
            return false;
        }

        public async Task<UserProfile?> CreateProfileAsync(string userId, UserProfile profile)
        {
            await Task.Delay(200);
            
            if (_currentUser?.Id == userId)
            {
                profile.Id = Guid.NewGuid().ToString();
                profile.CreatedDate = DateTime.Now;
                _currentUser.Profiles.Add(profile);
                return profile;
            }
            return null;
        }

        public async Task<bool> UpdateProfileAsync(UserProfile profile)
        {
            await Task.Delay(200);
            
            if (_currentUser != null)
            {
                var existingProfile = _currentUser.Profiles.Find(p => p.Id == profile.Id);
                if (existingProfile != null)
                {
                    existingProfile.Name = profile.Name;
                    existingProfile.AvatarUrl = profile.AvatarUrl;
                    existingProfile.IsChild = profile.IsChild;
                    existingProfile.Pin = profile.Pin;
                    existingProfile.Preferences = profile.Preferences;
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> DeleteProfileAsync(string profileId)
        {
            await Task.Delay(200);
            
            if (_currentUser != null)
            {
                return _currentUser.Profiles.RemoveAll(p => p.Id == profileId) > 0;
            }
            return false;
        }

        public async Task<bool> SetActiveProfileAsync(string userId, string profileId)
        {
            await Task.Delay(100);
            
            if (_currentUser?.Id == userId)
            {
                // Reset all profiles to non-default
                foreach (var profile in _currentUser.Profiles)
                {
                    profile.IsDefault = false;
                }
                
                // Set the selected profile as default
                var selectedProfile = _currentUser.Profiles.Find(p => p.Id == profileId);
                if (selectedProfile != null)
                {
                    selectedProfile.IsDefault = true;
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> ConnectTraktAsync(string userId, string accessToken, string refreshToken)
        {
            await Task.Delay(300);
            
            if (_currentUser?.Id == userId)
            {
                _currentUser.ExternalAccounts.Trakt = new TraktAccount
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    TokenExpiry = DateTime.Now.AddDays(90),
                    Username = "trakt_user",
                    IsConnected = true
                };
                return true;
            }
            return false;
        }

        public async Task<bool> DisconnectTraktAsync(string userId)
        {
            await Task.Delay(200);
            
            if (_currentUser?.Id == userId)
            {
                _currentUser.ExternalAccounts.Trakt = null;
                return true;
            }
            return false;
        }

        public async Task<bool> ConnectDebridAsync(string userId, DebridAccount account)
        {
            await Task.Delay(300);
            
            if (_currentUser?.Id == userId)
            {
                account.Id = Guid.NewGuid().ToString();
                _currentUser.ExternalAccounts.DebridAccounts.Add(account);
                return true;
            }
            return false;
        }

        public async Task<bool> DisconnectDebridAsync(string userId, string debridId)
        {
            await Task.Delay(200);
            
            if (_currentUser?.Id == userId)
            {
                return _currentUser.ExternalAccounts.DebridAccounts.RemoveAll(d => d.Id == debridId) > 0;
            }
            return false;
        }

        public async Task<bool> SyncWithTraktAsync(string userId)
        {
            await Task.Delay(2000); // Simulate sync process
            
            if (_currentUser?.Id == userId && _currentUser.ExternalAccounts.Trakt?.IsConnected == true)
            {
                // In real implementation, sync watchlist, progress, etc. with Trakt
                return true;
            }
            return false;
        }
    }
}
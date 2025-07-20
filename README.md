# Syncler Windows

A Windows WPF application that replicates the features, look, and feel of Syncler 2.0 Android. This is a fully functional media streaming and organization app with a modern dark UI, multi-profile support, and comprehensive media management capabilities.

## Features

### 🎬 Core Features
- **Modern Dark UI** - Syncler-inspired design with purple accent colors
- **Media Browsing** - Movies, TV Shows, and Anime discovery
- **Search Functionality** - Advanced search with filters and suggestions
- **Continue Watching** - Track viewing progress across all content
- **Watchlist Management** - Save favorites for later viewing
- **Multi-Profile Support** - Multiple user profiles with individual preferences

### 🔧 Advanced Features
- **Provider System** - Extensible provider packages for content sources
- **Debrid Integration** - Support for Real-Debrid, Premiumize, All-Debrid
- **External Services** - Trakt.tv and Simkl synchronization
- **Progress Tracking** - Automatic playback position saving
- **Cloud Management** - Debrid cloud file management
- **Download Management** - Transfer monitoring and downloads

### 🎯 Syncler 2.0 Compatibility
- **Home Dashboard** - Customizable home screen sections
- **Trending Content** - Real-time trending movies and shows
- **Quality Sources** - HD/4K content with quality indicators
- **Stream Sources** - Multiple streaming sources with quality sorting
- **User Preferences** - Comprehensive settings and customization
- **Account Sync** - Settings sync across devices (simulated)

## Screenshots

The application features a modern dark theme with:
- **Sidebar Navigation** - Easy access to all sections
- **Card-based Layout** - Beautiful media cards with posters and info
- **Progress Indicators** - Visual progress bars for continue watching
- **Quality Badges** - Rating and quality indicators
- **Responsive Design** - Adapts to different window sizes

## Requirements

### System Requirements
- **Windows 10** version 1903 or later
- **Windows 11** (recommended)
- **.NET 8.0 Runtime** (Windows Desktop)
- **2 GB RAM** minimum, 4 GB recommended
- **1 GB free disk space**

### Development Requirements
- **Visual Studio 2022** (17.0 or later)
- **.NET 8.0 SDK**
- **Windows App SDK** (included with VS 2022)

## Installation

### Option 1: Download Release (Recommended)
1. Go to the [Releases](../../releases) page
2. Download the latest `SynclerWindows-Setup.exe`
3. Run the installer and follow the setup wizard
4. Launch Syncler Windows from Start Menu

### Option 2: Build from Source

#### Prerequisites
```bash
# Install .NET 8.0 SDK
winget install Microsoft.DotNet.SDK.8

# Or download from: https://dotnet.microsoft.com/download/dotnet/8.0
```

#### Build Steps
```bash
# Clone the repository
git clone https://github.com/your-username/syncler-windows.git
cd syncler-windows

# Restore NuGet packages
dotnet restore

# Build the application
dotnet build --configuration Release

# Run the application
dotnet run --project SynclerWindows
```

#### Using Visual Studio
1. Open `SynclerWindows.sln` in Visual Studio 2022
2. Set `SynclerWindows` as the startup project
3. Select **Release** configuration
4. Build > Build Solution (Ctrl+Shift+B)
5. Debug > Start Without Debugging (Ctrl+F5)

## Usage

### First Launch
1. The app will create a default user profile
2. Browse trending content on the home screen
3. Use the search bar to find specific movies/shows
4. Click on any media item to simulate playback

### Navigation
- **Sidebar Menu** - Navigate between different sections
- **Search Bar** - Global search for movies and TV shows
- **Profile Icon** - Access user settings and profiles
- **Settings Gear** - Configure app preferences

### Key Features
- **Home** - Dashboard with trending and recommended content
- **Movies/TV Shows** - Browse by category
- **Continue Watching** - Resume partially watched content
- **Watchlist** - Your saved favorites
- **Search** - Find specific content
- **Settings** - Configure providers, accounts, and preferences

## Architecture

### Project Structure
```
SynclerWindows/
├── Models/           # Data models (MediaItem, User, StreamSource)
├── Services/         # Business logic services
├── ViewModels/       # MVVM view models
├── Views/           # XAML views and pages
├── Styles/          # UI styles and themes
└── Assets/          # Images and resources
```

### Key Technologies
- **WPF (Windows Presentation Foundation)** - UI framework
- **MVVM Pattern** - Clean separation of concerns
- **ModernWpfUI** - Modern UI components and theming
- **LibVLCSharp** - Media playback capabilities
- **RestSharp** - HTTP client for API calls
- **Newtonsoft.Json** - JSON serialization

### Services Architecture
- **MediaService** - Content discovery and management
- **UserService** - User authentication and profiles
- **SearchService** - Search functionality
- **NavigationService** - Page navigation

## Development

### Adding New Features
1. Create models in `Models/` folder
2. Implement service interfaces in `Services/`
3. Create view models in `ViewModels/`
4. Design views in `Views/` folder
5. Update navigation in `NavigationService`

### Styling Guidelines
- Use Syncler color scheme (purple primary, dark background)
- Follow card-based layout patterns
- Implement proper hover and focus states
- Ensure responsive design principles

### Mock Data
The application currently uses mock data for demonstration. To integrate with real APIs:
1. Update `MediaService` to use TMDB API
2. Implement real provider packages
3. Add actual debrid service integration
4. Connect to Trakt.tv API

## Configuration

### Provider Packages
The app supports extensible provider packages. Add new providers by:
1. Implementing the provider interface
2. Installing via URL in Settings
3. Configuring provider-specific settings

### External Services
Configure external services in Settings:
- **Trakt.tv** - Progress sync and recommendations
- **Simkl** - Alternative sync service
- **Real-Debrid** - Premium streaming sources
- **Premiumize** - Cloud storage and streaming

## Troubleshooting

### Common Issues

**App won't start**
- Ensure .NET 8.0 Runtime is installed
- Check Windows version compatibility
- Run as administrator if needed

**Performance Issues**
- Close other media applications
- Check available disk space
- Update graphics drivers

**UI Issues**
- Reset window position in Settings
- Check display scaling settings
- Restart the application

### Logs and Debugging
- Application logs: `%APPDATA%/SynclerWindows/logs/`
- Enable debug mode in Settings
- Use Visual Studio debugger for development

## Contributing

### How to Contribute
1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

### Development Guidelines
- Follow MVVM patterns
- Write unit tests for services
- Update documentation for new features
- Follow existing code style conventions

### Reporting Issues
Please report issues using the GitHub issue tracker:
- Provide detailed reproduction steps
- Include system information
- Attach relevant logs if possible

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Disclaimer

This is an unofficial Windows application inspired by Syncler 2.0 Android. It is not affiliated with or endorsed by the original Syncler developers. This project is for educational and demonstration purposes.

## Acknowledgments

- **Syncler Team** - Original Android application inspiration
- **ModernWpfUI** - Modern UI components for WPF
- **TMDB** - Movie and TV show database
- **Trakt.tv** - Media tracking and recommendations
- **Community Contributors** - Bug reports and feature suggestions

## Support

For support and questions:
- Check the [Wiki](../../wiki) for detailed guides
- Search [Issues](../../issues) for existing solutions
- Create a new issue for bugs or feature requests
- Join the community discussions

---

**Made with ❤️ for the Windows community**

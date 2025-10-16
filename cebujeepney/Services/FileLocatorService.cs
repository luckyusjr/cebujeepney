using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Storage;
using System;
using System.IO;
using System.Linq;

namespace cebujeepney.Services
{
    public partial class FileLocatorService : ObservableObject
    {
        [ObservableProperty] private string projectDirectory = string.Empty;
        [ObservableProperty] private string jsonDirectory = string.Empty;

        public FileLocatorService()
        {
            ProjectDirectory = GetBaseDirectory();
            JsonDirectory    = Path.Combine(ProjectDirectory, "json");

            Directory.CreateDirectory(JsonDirectory);
        }

        public string GetCommuterDirectory()
        {
            var path = Path.Combine(JsonDirectory, "Commuters");
            Directory.CreateDirectory(path);
            return path;
        }

        public string GetAdminDirectory()
        {
            var path = Path.Combine(JsonDirectory, "Admins");
            Directory.CreateDirectory(path);
            return path;
        }

        /// <summary>
        /// Returns a writable base folder:
        /// - Mobile/desktop sandbox: FileSystem.AppDataDirectory
        /// - Windows dev: try to find the solution root (contains *.sln); fallback to LocalAppData\cebujeepney
        /// </summary>
        private static string GetBaseDirectory()
        {
#if ANDROID || IOS || MACCATALYST
            // App-private sandbox, e.g. /data/user/0/<pkg>/files (Android)
            return FileSystem.AppDataDirectory;
#elif WINDOWS
            // In Windows desktop/debug you might want to read next to the solution.
            // If not found (store app, release, etc.), fall back to LocalAppData.
            return FindSolutionRoot()
                   ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                   "cebujeepney");
#else
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                "cebujeepney");
#endif
        }

#if WINDOWS
        private static string? FindSolutionRoot()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                try
                {
                    if (dir.EnumerateFiles("*.sln").Any())
                        return dir.FullName;
                }
                catch
                {
                    // ignore protected folders
                }
                dir = dir.Parent;
            }
            return null;
        }
#endif
    }
}

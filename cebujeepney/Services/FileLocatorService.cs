// Services/FileLocatorService.cs
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Storage;
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
#if ANDROID || IOS
            ProjectDirectory = FileSystem.AppDataDirectory;                // <- sandbox
#else
            // desktop: walk up to project folder that contains the .csproj
            var d = new DirectoryInfo(AppContext.BaseDirectory);
            while (d != null && !d.EnumerateFiles("*.csproj").Any()) d = d.Parent;
            ProjectDirectory = d?.FullName ?? AppContext.BaseDirectory;
#endif
            JsonDirectory = Path.Combine(ProjectDirectory, "json");
            Directory.CreateDirectory(JsonDirectory);
        }

        public string GetAdminDirectory()
        {
            var p = Path.Combine(JsonDirectory, "Admins");
            Directory.CreateDirectory(p);
            return p;
        }

        public string GetCommuterDirectory()
        {
            var p = Path.Combine(JsonDirectory, "Commuters");
            Directory.CreateDirectory(p);
            return p;
        }
    }
}

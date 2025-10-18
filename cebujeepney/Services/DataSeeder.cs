// Services/DataSeeder.cs
using Microsoft.Maui.Storage;
using System.IO;
using System.Threading.Tasks;

namespace cebujeepney.Services;

public static class DataSeeder
{
    public static async Task EnsureAsync(FileLocatorService paths)
    {
        await CopyIfMissing(paths, "json/Admins/A001.json");
        // add others if you want:
        // await CopyIfMissing(paths, "json/Commuters/S001.json");
    }

    static async Task CopyIfMissing(FileLocatorService paths, string assetPath)
    {
        var relative = assetPath.Replace("json/", ""); // "Admins/A001.json"
        var dest = Path.Combine(paths.JsonDirectory, relative);
        Directory.CreateDirectory(Path.GetDirectoryName(dest)!);
        if (File.Exists(dest)) return;

        using var src = await FileSystem.OpenAppPackageFileAsync(assetPath);
        using var dst = File.Create(dest);
        await src.CopyToAsync(dst);
    }
}

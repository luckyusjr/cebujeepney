// Services/DataSeeder.cs
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace cebujeepney.Services;

public static class DataSeeder
{
    public static async Task EnsureAdminAsync(FileLocatorService paths)
    {
        var dir = paths.GetAdminDirectory();
        var file = Path.Combine(dir, "A001.json");
        if (File.Exists(file)) return;

        var admin = new cebujeepney.Models.Admin
        {
            AccountType = "Admin",
            Email = "admin@mail.com",
            Password = "password"
        };

        var json = JsonSerializer.Serialize(admin, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(file, json);
    }
}

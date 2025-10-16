using cebujeepney.Models;
using cebujeepney.Services;
using Microsoft.Maui.Controls;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace cebujeepney.Services
{
    public class AccountFileService
    {
        private readonly string _commuterDir;
        private readonly string _adminDir;

        public AccountFileService(FileLocatorService locatorService)
        {
            _commuterDir = locatorService.GetCommuterDirectory();
            _adminDir = locatorService.GetAdminDirectory();

            Directory.CreateDirectory(_commuterDir);
            Directory.CreateDirectory(_adminDir);
        }

        public void SaveCommuter(Commuter user)
        {
            string id = GenerateNextId(_commuterDir, "S");
            string path = Path.Combine(_commuterDir, $"{id}.json");
            File.WriteAllText(path, JsonSerializer.Serialize(user, new JsonSerializerOptions { WriteIndented = true }));
        }

        public void SaveAdmin(Admin admin)
        {
            string id = GenerateNextId(_adminDir, "A"); // A001, A002, etc.
            string path = Path.Combine(_adminDir, $"{id}.json");
            File.WriteAllText(path, JsonSerializer.Serialize(admin, new JsonSerializerOptions { WriteIndented = true }));
        }

        private string GenerateNextId(string directory, string prefix)
        {
            var files = Directory.GetFiles(directory, $"{prefix}*.json");
            int max = files.Select(f =>
            {
                var name = Path.GetFileNameWithoutExtension(f);
                return int.TryParse(name.Substring(1), out int num) ? num : 0;
            }).DefaultIfEmpty(0).Max();

            return $"{prefix}{(max + 1):D3}";
        }
    }
}

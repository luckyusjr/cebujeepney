using CommunityToolkit.Mvvm.ComponentModel;
using cebujeepney.Models;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace cebujeepney.Services
{
    public partial class AuthenticationResult : ObservableObject
    {
        [ObservableProperty] private bool isAuthenticated;
        [ObservableProperty] private string accountType = string.Empty;  // "Admin" | "Commuter"
        [ObservableProperty] private string errorMessage = string.Empty;
    }

    public partial class AuthenticationService : ObservableObject
    {
        private readonly FileLocatorService _fileLocatorService;
        private static readonly JsonSerializerOptions _json =
            new() { PropertyNameCaseInsensitive = true };

        public AuthenticationService()
        {
            _fileLocatorService = new FileLocatorService();
        }

        public FileLocatorService FileLocator => _fileLocatorService;

        public async Task<AuthenticationResult> AuthenticateAsync(string email, string password)
        {
            try
            {
                var e = (email ?? string.Empty).Trim();
                var p = password ?? string.Empty;

                // --- Admins (A*.json) ---
                var adminDir = _fileLocatorService.GetAdminDirectory();
                var adminHit = await TryMatchUserAsync<Admin>(
                    dir: adminDir,
                    glob: "A*.json",
                    expectedType: "Admin",
                    email: e,
                    password: p);

                if (adminHit.IsAuthenticated)
                    return adminHit;

                // --- Commuters (S*.json) ---
                var commuterDir = _fileLocatorService.GetCommuterDirectory();
                var commuterHit = await TryMatchUserAsync<Commuter>(
                    dir: commuterDir,
                    glob: "S*.json",
                    expectedType: "Commuter",
                    email: e,
                    password: p);

                if (commuterHit.IsAuthenticated)
                    return commuterHit;

                return new AuthenticationResult
                {
                    IsAuthenticated = false,
                    ErrorMessage = "Invalid email, password, or account type."
                };
            }
            catch (Exception ex)
            {
                return new AuthenticationResult
                {
                    IsAuthenticated = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        // ---------- helpers ----------

        private static async Task<AuthenticationResult> TryMatchUserAsync<T>(
            string dir,
            string glob,
            string expectedType,
            string email,
            string password)
        {
            if (!Directory.Exists(dir))
                return new AuthenticationResult { IsAuthenticated = false };

            foreach (var file in Directory.EnumerateFiles(dir, glob))
            {
                try
                {
                    var json = await File.ReadAllTextAsync(file);
                    var user = JsonSerializer.Deserialize<T>(json, _json);
                    if (user is null) continue;

                    // Extract fields in a type-safe way
                    string? uType = null, uEmail = null, uPassword = null;

                    switch (user)
                    {
                        case Admin a:
                            uType = a.AccountType;
                            uEmail = a.Email;
                            uPassword = a.Password;   // for testing; later replace with a hash
                            break;

                        case Commuter c:
                            uType = c.AccountType;
                            uEmail = c.Email;
                            uPassword = c.Password;
                            break;
                    }

                    if (!EqualsIgnoreCase(uType, expectedType)) continue;
                    if (!EqualsIgnoreCase(uEmail, email)) continue;
                    if (!string.Equals(uPassword ?? string.Empty, password ?? string.Empty, StringComparison.Ordinal))
                        continue;

                    // Success
                    return new AuthenticationResult
                    {
                        IsAuthenticated = true,
                        AccountType = expectedType   // normalize: "Admin" or "Commuter"
                    };
                }
                catch
                {
                    // Malformed JSON or read error: skip this file and continue
                }
            }

            return new AuthenticationResult { IsAuthenticated = false };
        }

        private static bool EqualsIgnoreCase(string? a, string? b) =>
            string.Equals(a?.Trim(), b?.Trim(), StringComparison.OrdinalIgnoreCase);
    }
}

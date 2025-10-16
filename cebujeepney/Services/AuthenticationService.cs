using CommunityToolkit.Mvvm.ComponentModel;
using cebujeepney.Models;
using Microsoft.Maui.Controls;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace cebujeepney.Services
{
    public partial class AuthenticationResult : ObservableObject
    {
        [ObservableProperty]
        private bool isAuthenticated;

        [ObservableProperty]
        private string accountType;

        [ObservableProperty]
        private string errorMessage;
    }

    public partial class AuthenticationService : ObservableObject
    {
        private readonly FileLocatorService _fileLocatorService;

        public AuthenticationService()
        {
            _fileLocatorService = new FileLocatorService();
        }
        public FileLocatorService FileLocator => _fileLocatorService;


        public async Task<AuthenticationResult> AuthenticateAsync(string email, string password)
        {
            try
            {
                // Check Admins
                var adminFiles = Directory.GetFiles(_fileLocatorService.GetAdminDirectory(), "A*.json");
                foreach (var file in adminFiles)
                {
                    var json = await File.ReadAllTextAsync(file);
                    var admin = JsonSerializer.Deserialize<Admin>(json);

                    if (admin != null &&
                        admin.AccountType == "Admin" &&
                        email.Equals(admin.Email, StringComparison.OrdinalIgnoreCase) &&
                        admin.Password == password)
                    {
                        return new AuthenticationResult
                        {
                            IsAuthenticated = true,
                            AccountType = admin.AccountType
                        };
                    }
                }

                // Check Commuters
                var commuterFiles = Directory.GetFiles(_fileLocatorService.GetCommuterDirectory(), "S*.json");
                foreach (var file in commuterFiles)
                {
                    var json = await File.ReadAllTextAsync(file);
                    var commuter = JsonSerializer.Deserialize<Commuter>(json);

                    if (commuter != null &&
                        commuter.AccountType == "Commuter" &&
                        email.Equals(commuter.Email, StringComparison.OrdinalIgnoreCase) &&
                        commuter.Password == password)
                    {
                        return new AuthenticationResult
                        {
                            IsAuthenticated = true,
                            AccountType = commuter.AccountType
                        };
                    }
                }

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
    }
}

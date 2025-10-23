using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using cebujeepney.Models;
using cebujeepney.Services;
using cebujeepney.Views;
using Microsoft.Maui.Controls;

namespace cebujeepney.ViewModels
{
    public partial class RegisterVM : ObservableObject
    {
        [ObservableProperty]
        private string username = string.Empty;

        [ObservableProperty]
        private string email = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;

        [ObservableProperty]
        private string confirmPassword = string.Empty;

        private AccountFileService _accountFileService;
        private readonly FileLocatorService _fileLocatorService;
        private readonly DisplayAlertService _displayAlertService;

        public RegisterVM()
        {
            _fileLocatorService = new FileLocatorService();
            _accountFileService = new AccountFileService(_fileLocator_service());
            _displayAlertService = new DisplayAlertService();
        }

        // helper to pass FileLocatorService instance (keeps original types intact)
        private FileLocatorService _fileLocator_service()
        {
            return _fileLocatorService;
        }

        [RelayCommand]
        private async Task Register()
        {
            if (string.IsNullOrWhiteSpace(Username) ||
                string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                await _displayAlertService.ShowAlert("Failed to Sign Up", "Please fill up the required fields.", "OK");
                return;
            }

            if (Password != ConfirmPassword)
            {
                await _displayAlertService.ShowAlert("Failed to Sign Up", "Confirm Password does not match.", "OK");
                return;
            }

            if (!Email.Contains("@"))
            {
                await _displayAlertService.ShowAlert("Failed to Sign Up", "Please enter a valid email.", "OK");
                return;
            }

            var commuterDir = _fileLocatorService.GetCommuterDirectory();

            if (Directory.Exists(commuterDir))
            {
                var files = Directory.GetFiles(commuterDir, "S*.json");
                foreach (var file in files)
                {
                    var json = await File.ReadAllTextAsync(file);
                    var existingUser = JsonSerializer.Deserialize<Commuter>(json);

                    if (existingUser != null &&
                        existingUser.Email.Equals(Email, StringComparison.OrdinalIgnoreCase))
                    {
                        await _displayAlertService.ShowAlert("Email Already Used", "Email has already been registered.", "OK");
                        return; // Stop registration
                    }
                }
            }

            var newUser = new Commuter
            {
                Email = Email,
                Password = Password,
                AccountType = "Commuter"
            };

            try
            {
                _accountFileService.SaveCommuter(newUser);

                // Set session and navigate to commuter main view
                SessionService.Instance.SetCommuter(newUser);

                await _displayAlertService.ShowAlert("Success", "Account registered successfully!", "OK");

                // Clear inputs
                Username = Email = Password = ConfirmPassword = string.Empty;
            }
            catch (Exception ex)
            {
                await _displayAlert_service().ShowAlert("Error", $"Failed to save account: {ex.Message}", "OK");
            }
        }

        private DisplayAlertService _displayAlert_service()
        {
            return _displayAlertService;
        }
    }
}

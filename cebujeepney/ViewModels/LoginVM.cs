using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using cebujeepney.Models;
using cebujeepney.Services;
using cebujeepney.Views;
using Microsoft.Maui.Controls;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace cebujeepney.ViewModels
{
    public partial class LoginVM : ObservableObject
    {
        // Initialize to avoid nulls on mobile
        [ObservableProperty] private string email = string.Empty;
        [ObservableProperty] private string password = string.Empty;
        [ObservableProperty] private bool isErrorVisible;
        [ObservableProperty] private string errorMessage = string.Empty;
        [ObservableProperty] private Color textColor = Colors.Red;

        private readonly AuthenticationService _authService;
        private static readonly JsonSerializerOptions _json = new() { PropertyNameCaseInsensitive = true };

        public LoginVM()
        {
            _authService = new AuthenticationService();
        }

        [RelayCommand]
        private async Task Login()
        {
            await DataSeeder.EnsureAdminAsync(_authService.FileLocator);
            // Basic input checks
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                ShowError("Please enter both email and password.");
                return;
            }

            if (!Email.Contains("@"))
            {
                ShowError("Please enter a valid email.");
                return;
            }

            var result = await _authService.AuthenticateAsync(Email, Password);
            if (!result.IsAuthenticated)
            {
                ShowError(result.ErrorMessage ?? "Login failed.");
                return;
            }

            // Normalize role for downstream logic
            // Accepts "User"/"Commuter" → "commuter"
            var role = (result.AccountType ?? string.Empty).Trim().ToLowerInvariant();
            if (role == "user") role = "commuter";

            // Load the current account into session (from your JSON store)
            try
            {
                string? directory = role switch
                {
                    "commuter" => _authService.FileLocator.GetCommuterDirectory(),
                    "admin" => _authService.FileLocator.GetAdminDirectory(),
                    _ => null
                };

                if (!string.IsNullOrEmpty(directory) && Directory.Exists(directory))
                {
                    foreach (var file in Directory.EnumerateFiles(directory, "*.json"))
                    {
                        try
                        {
                            var json = await File.ReadAllTextAsync(file);
                            if (role == "commuter")
                            {
                                var commuter = JsonSerializer.Deserialize<Commuter>(json, _json);
                                if (commuter?.Email.Equals(Email, StringComparison.OrdinalIgnoreCase) == true)
                                {
                                    SessionService.Instance.SetCommuter(commuter);
                                    break;
                                }
                            }
                            else if (role == "admin")
                            {
                                var admin = JsonSerializer.Deserialize<Admin>(json, _json);
                                if (admin?.Email.Equals(Email, StringComparison.OrdinalIgnoreCase) == true)
                                {
                                    SessionService.Instance.SetAdmin(admin);
                                    break;
                                }
                            }
                        }
                        catch
                        {
                            // skip malformed file, keep trying others
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError($"Could not load account: {ex.Message}");
                return;
            }

            // Clear UI fields
            Email = string.Empty;
            Password = string.Empty;
            IsErrorVisible = false;

            // Navigate
            await NavigateByRole(role);
        }

        private void ShowError(string message)
        {
            ErrorMessage = message;
            TextColor = Colors.Red;
            IsErrorVisible = true;
        }

        private async Task NavigateByRole(string role)
        {
            try
            {
                if (role == "commuter")
                {
                    if (Shell.Current is not null)
                        await Shell.Current.GoToAsync(nameof(CommuterMV));     // requires route registration
                    else
                        await Application.Current.MainPage.Navigation.PushAsync(new CommuterMV());
                }
                else if (role == "admin")
                {
                    if (Shell.Current is not null)
                        await Shell.Current.GoToAsync(nameof(AdminMV));        // requires route registration
                    else
                        await Application.Current.MainPage.Navigation.PushAsync(new AdminMV());
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Unknown account type.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Navigation error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        private async Task GoToRegister()
        {
            try
            {
                if (Shell.Current is not null)
                    await Shell.Current.GoToAsync(nameof(RegisterView));       // register route once in AppShell
                else
                    await Application.Current.MainPage.Navigation.PushAsync(new RegisterView());
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Navigation error", ex.Message, "OK");
            }
        }
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using cebujeepney.Models;
using cebujeepney.Services;
using cebujeepney.Views;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics.Text;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace cebujeepney.ViewModels
{
    public partial class LoginVM : ObservableObject
    {
        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private bool isErrorVisible;

        [ObservableProperty]
        private string errorMessage;

        [ObservableProperty]
        private Color textColor;

        private readonly AuthenticationService _authService;

        public LoginVM()
        {
            _authService = new AuthenticationService();
        }

        [RelayCommand]
        private async Task Login()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Please enter both email and password.";
                ShowError();
                await Application.Current.MainPage.DisplayAlert("Error", ErrorMessage, "OK");
                return;
            }

            if (!Email.Contains("@"))
            {
                ErrorMessage = "Please enter a valid email.";
                ShowError();
                await Application.Current.MainPage.DisplayAlert("Error", ErrorMessage, "OK");
                return;
            }

            var result = await _authService.AuthenticateAsync(Email, Password);

            if (!result.IsAuthenticated)
            {
                ErrorMessage = result.ErrorMessage;
                ShowError();
                await Application.Current.MainPage.DisplayAlert("Error", ErrorMessage, "OK");
                return;
            }

            // Clear error and prepare to store session
            IsErrorVisible = false;
            string role = result.AccountType?.ToLower() ?? string.Empty;
            string directory = role switch
            {
                "commuter" => _authService.FileLocator.GetCommuterDirectory(),
                "admin" => _authService.FileLocator.GetAdminDirectory(),
                _ => null
            };

            if (directory != null)
            {
                var files = Directory.GetFiles(directory, "*.json");
                foreach (var file in files)
                {
                    var json = await File.ReadAllTextAsync(file);

                    switch (role)
                    {
                        case "commuter":
                            var commuter = JsonSerializer.Deserialize<Commuter>(json);
                            if (commuter?.Email.Equals(Email, StringComparison.OrdinalIgnoreCase) == true)
                            {
                                SessionService.Instance.SetCommuter(commuter);
                                break;
                            }
                            break;

                        case "admin":
                            var admin = JsonSerializer.Deserialize<Admin>(json);
                            if (admin?.Email.Equals(Email, StringComparison.OrdinalIgnoreCase) == true)
                            {
                                SessionService.Instance.SetAdmin(admin);
                                break;
                            }
                            break;
                    }
                }
            }

            // Clear input fields
            Email = string.Empty;
            Password = string.Empty;

            // Show success and Navigate
            await Application.Current.MainPage.DisplayAlert("Success", "LOG IN SUCCESSFULLY", "OK");
            await NavigateByRole(role);
        }

        private void ShowError()
        {
            IsErrorVisible = true;
            TextColor = Colors.Red;
        }

        private async Task NavigateByRole(string role)
        {
            Page nextPage = role switch
            {
                "commuter" => new CommuterMV(),
                "admin" => new AdminMV(),
                _ => null
            };

            if (nextPage is null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Unknown account type.", "OK");
                return;
            }

            // RESET the navigation stack: no “back” possible.
            Application.Current.MainPage = new NavigationPage(nextPage);

            // Optional: style the bar
            // ((NavigationPage)Application.Current.MainPage).BarTextColor = Colors.White;
            // ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.FromArgb("#2E43D9");
        }


        [RelayCommand]
        private async Task GoToRegister()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new RegisterView());
        }
    }
}

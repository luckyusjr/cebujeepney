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
                return;
            }

            if (!Email.Contains("@"))
            {
                ErrorMessage = "Please enter a valid email.";
                ShowError();
                return;
            }

            var result = await _authService.AuthenticateAsync(Email, Password);

            if (!result.IsAuthenticated)
            {
                ErrorMessage = result.ErrorMessage;
                ShowError();
                return;
            }

            // Clear error and prepare to store session
            IsErrorVisible = false;
            string role = result.AccountType.ToLower();
            string directory = role switch
            {
                "user" => _authService.FileLocator.GetCommuterDirectory(),
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
                        case "user":
                            var commuter = JsonSerializer.Deserialize<Commuter>(json);
                            if (commuter?.Email.Equals(Email, StringComparison.OrdinalIgnoreCase) == true)
                            {
                                /*if (!commuter.IsActivated)
                                {
                                    ErrorMessage = "This account has been deactivated. Please contact admin.";
                                    ShowError();
                                    return;
                                }*/

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

            // Navigate
            await NavigateByRole(role);
        }

        private void ShowError()
        {
            IsErrorVisible = true;
            TextColor = Colors.Red;
        }

        private async Task NavigateByRole(string role)
        {
            switch (role)
            {
                case "user":
                    await Application.Current.MainPage.Navigation.PushAsync(new CommuterMV());
                    break;

                case "admin":
                    await Application.Current.MainPage.Navigation.PushAsync(new AdminMV());
                    break;

                default:
                    await Application.Current.MainPage.DisplayAlert("Error", "Unknown account type.", "OK");
                    break;
            }
        }

        [RelayCommand]
        private async Task GoToRegister()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new RegisterView());
        }
    }
}

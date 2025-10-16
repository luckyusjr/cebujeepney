using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cebujeepney.Models;
using cebujeepney.Services;
using System.Text.Json;

namespace cebujeepney.ViewModels
{
    public partial class RegisterVM : ObservableObject
    {

        /*[ObservableProperty]
        private string newFirstName = string.Empty;

        [ObservableProperty]
        private string newMiddleName = string.Empty;

        [ObservableProperty]
        private string newLastName = string.Empty;

        [ObservableProperty]
        private string newEmail = string.Empty;

        [ObservableProperty]
        private string newPassword = string.Empty;

        [ObservableProperty]
        private string newConfirmPassword = string.Empty;

        [ObservableProperty]
        private bool isChecked;

        [ObservableProperty]
        private int numberOfUsersRegistered;

        private readonly IDisplayAlertService _displayAlertService;
        private readonly AccountFileService _accountFileService;
        private readonly FileLocatorService _fileLocatorService;
        public RegisterVM(IDisplayAlertService displayAlertService)
        {
            _displayAlertService = displayAlertService;
            _fileLocatorService = new FileLocatorService();
            _accountFileService = new AccountFileService(_fileLocatorService);

        }

        [RelayCommand]
        private async Task SignUp()
        {
            if (!string.IsNullOrWhiteSpace(NewFirstName) &&
                !string.IsNullOrWhiteSpace(NewLastName) &&
                !string.IsNullOrWhiteSpace(NewEmail) &&
                !string.IsNullOrWhiteSpace(NewPassword) &&
                !string.IsNullOrWhiteSpace(NewConfirmPassword))
            {
                if (NewConfirmPassword == NewPassword)
                {
                    if (NewEmail.Contains("@"))
                    {
                        if (IsChecked)
                        {
                            // Check for existing email
                            var employerDir = _fileLocatorService.GetCommuterDirectory();


                            if (Directory.Exists(employerDir))
                            {
                                var files = Directory.GetFiles(employerDir, "S*.json");
                                foreach (var file in files)
                                {
                                    var json = await File.ReadAllTextAsync(file);
                                    var existingUser = JsonSerializer.Deserialize<Commuter>(json);

                                    if (existingUser != null &&
                                        existingUser.Email.Equals(NewEmail, StringComparison.OrdinalIgnoreCase))
                                    {
                                        await _displayAlertService.ShowAlert("Email Already Used", "Email has already been registered.", "OK");
                                        return; // Stop registration
                                    }
                                }
                            }

                            // Create and save new employer
                            var newUser = new Commuter
                            {
                                FirstName = NewFirstName,
                                MiddleName = NewMiddleName,
                                LastName = NewLastName,
                                Email = NewEmail,
                                Password = NewPassword,
                                AccountType = "Employer"
                            };

                            try
                            {
                                _accountFileService.SaveEmployer(newUser);

                                NumberOfUsersRegistered++;

                                await _displayAlertService.ShowAlert("Success", "Account registered successfully!", "OK");

                                // Clear inputs
                                NewFirstName = NewMiddleName = NewLastName = NewEmail = NewPassword = NewConfirmPassword = string.Empty;
                                IsChecked = false;
                            }
                            catch (Exception ex)
                            {
                                await _displayAlertService.ShowAlert("Error", $"Failed to save account: {ex.Message}", "OK");
                            }
                        }
                        else
                        {
                            await _displayAlertService.ShowAlert("Failed to Sign Up", "Please accept the Terms & Conditions", "OK");
                        }
                    }
                    else
                    {
                        await _displayAlertService.ShowAlert("Failed to Sign Up", "Please enter a valid email.", "OK");
                    }
                }
                else
                {
                    await _displayAlertService.ShowAlert("Failed to Sign Up", "Confirm Password does not match.", "OK");
                }
            }
            else
            {
                await _displayAlertService.ShowAlert("Failed to Sign Up", "Please fill up the required fields.", "OK");
            }
        }*/

    }
}

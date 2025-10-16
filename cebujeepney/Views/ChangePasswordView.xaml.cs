using cebujeepney.Services;
using Microsoft.Maui.Controls;
using System.Text.Json;
using System.IO;
using System.Threading.Tasks;

namespace cebujeepney.Views
{
    public partial class ChangePasswordView : ContentPage
    {
        private readonly FileLocatorService _locator;

        public ChangePasswordView()
        {
            InitializeComponent();
            _locator = new FileLocatorService();
        }

        // Keep existing app logic untouched; provide wiring for UI buttons.
        private async void OnChangePasswordClicked(object sender, System.EventArgs e)
        {
            ErrorLabel.IsVisible = false;

            var current = CurrentPasswordEntry.Text ?? string.Empty;
            var npw = NewPasswordEntry.Text ?? string.Empty;
            var confirm = ConfirmPasswordEntry.Text ?? string.Empty;

            if (string.IsNullOrWhiteSpace(current) || string.IsNullOrWhiteSpace(npw) || string.IsNullOrWhiteSpace(confirm))
            {
                ShowError("Please fill all fields.");
                return;
            }

            if (npw != confirm)
            {
                ShowError("New password and confirm password do not match.");
                return;
            }

            // Determine whether current session is admin or commuter
            var session = SessionService.Instance;

            // Try update file for admin
            if (session.AccountType == "Admin" && session.AdminAccount != null)
            {
                var admin = session.AdminAccount;
                if (admin.Password != current)
                {
                    ShowError("Current password is incorrect.");
                    return;
                }

                admin.Password = npw;
                SaveAdmin(admin);
                await DisplayAlert("Success", "Password changed successfully.", "OK");
                await Navigation.PopAsync();
                return;
            }

            // Try update file for commuter
            if (session.AccountType == "Commuter" && session.CommuterAccount != null)
            {
                var comm = session.CommuterAccount;
                if (comm.Password != current)
                {
                    ShowError("Current password is incorrect.");
                    return;
                }

                comm.Password = npw;
                SaveCommuter(comm);
                await DisplayAlert("Success", "Password changed successfully.", "OK");
                await Navigation.PopAsync();
                return;
            }

            ShowError("No active session found.");
        }

        private void ShowError(string text)
        {
            ErrorLabel.Text = text;
            ErrorLabel.IsVisible = true;
        }

        private void SaveAdmin(Models.Admin admin)
        {
            var adminDir = _locator.GetAdminDirectory();
            // Find the file that matches the current admin email
            var files = Directory.GetFiles(adminDir, "A*.json");
            foreach (var file in files)
            {
                var json = File.ReadAllText(file);
                var a = JsonSerializer.Deserialize<Models.Admin>(json);
                if (a != null && a.Email == admin.Email)
                {
                    File.WriteAllText(file, JsonSerializer.Serialize(admin, new JsonSerializerOptions { WriteIndented = true }));
                    SessionService.Instance.SetAdmin(admin);
                    return;
                }
            }
        }

        private void SaveCommuter(Models.Commuter comm)
        {
            var commDir = _locator.GetCommuterDirectory();
            var files = Directory.GetFiles(commDir, "S*.json");
            foreach (var file in files)
            {
                var json = File.ReadAllText(file);
                var c = JsonSerializer.Deserialize<Models.Commuter>(json);
                if (c != null && c.Email == comm.Email)
                {
                    File.WriteAllText(file, JsonSerializer.Serialize(comm, new JsonSerializerOptions { WriteIndented = true }));
                    SessionService.Instance.SetCommuter(comm);
                    return;
                }
            }
        }

        private async void OnCancelClicked(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
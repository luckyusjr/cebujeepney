using cebujeepney.Services;
using Microsoft.Maui.Controls;

namespace cebujeepney.Views
{
    public partial class ProfileView : ContentPage
    {
        public ProfileView()
        {
            InitializeComponent();
            var s = SessionService.Instance;
            if (s.AccountType == "Admin" && s.AdminAccount != null)
                EmailLabel.Text = s.AdminAccount.Email;
            else if (s.AccountType == "Commuter" && s.CommuterAccount != null)
                EmailLabel.Text = s.CommuterAccount.Email;
            else
                EmailLabel.Text = "not logged in";
        }

        private async void OnProfileImageClicked(object sender, System.EventArgs e)
        {
            await DisplayAlert("Profile", "Profile image clicked (placeholder).", "OK");
        }

        private async void OnEditProfileClicked(object sender, System.EventArgs e)
        {
            // Navigate to change password for now (profile edit placeholder)
            await Navigation.PushAsync(new ChangePasswordView());
        }

        private async void OnUpdateInfoClicked(object sender, System.EventArgs e)
        {
            await DisplayAlert("Update", "Update info clicked (placeholder).", "OK");
        }

        private async void OnLogoutClicked(object sender, System.EventArgs e)
        {
            SessionService.Instance.Clear();
            await Application.Current.MainPage.Navigation.PopToRootAsync();
        }
    }
}
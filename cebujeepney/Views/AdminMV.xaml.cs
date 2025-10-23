using cebujeepney.ViewModels; 
namespace cebujeepney.Views
{
    public partial class AdminMV : ContentPage
    {
        public AdminMV()
        {
            InitializeComponent();
            BindingContext = new CommuterVM(Navigation); 
        }

        private async void OnChangePasswordClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new ChangePasswordView());
        }

        private async void OnRoutePricingClicked(object sender, System.EventArgs e)
        {
            await DisplayAlert("Route Pricing", "Open Route Pricing page (placeholder).", "OK");
        }

        private async void OnEditAddJeepClicked(object sender, System.EventArgs e)
        {
            await DisplayAlert("Edit/Add Jeep", "Open Edit/Add Jeep page (placeholder).", "OK");
        }

        private async void OnAddDeleteTypeClicked(object sender, System.EventArgs e)
        {
            await DisplayAlert("Add/Delete Jeep Type", "Open Add/Delete Jeep Type page (placeholder).", "OK");
        }
    }
}

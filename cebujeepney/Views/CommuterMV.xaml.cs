using cebujeepney.ViewModels;

namespace cebujeepney.Views
{
    public partial class CommuterMV : ContentPage
    {
        public CommuterMV()
        {
            InitializeComponent();
            BindingContext = new CommuterVM(Navigation);
        }

        private async void OnChangePasswordClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new ChangePasswordView());
        }
    }
}
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using cebujeepney.Views;
using cebujeepney.Services;

namespace cebujeepney.ViewModels
{
    public partial class CommuterVM : ObservableObject
    {
        private readonly INavigation _navigation;

        public CommuterVM(INavigation navigation)
        {
            _navigation = navigation;
        }

        [RelayCommand]
        private async Task OpenSettings()
        {
            await _navigation.PushAsync(new CommuterSettingsView());
        }

        [RelayCommand]
        private Task Logout()
        {
            SessionService.Instance.Clear();
            Application.Current.MainPage = new NavigationPage(new LoginView());
            return Task.CompletedTask;
        }

        [RelayCommand]
        private async Task ChangePass()
        {
            await _navigation.PushAsync(new ChangePasswordView());
        }
    }
}
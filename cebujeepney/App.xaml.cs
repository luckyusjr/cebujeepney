using cebujeepney.Views;

namespace cebujeepney
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            // Show login first so the app prompts for account + password on launch
            MainPage = new NavigationPage(new LoginView());
        }
    }
}


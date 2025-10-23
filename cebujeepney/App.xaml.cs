using cebujeepney.Views;
namespace cebujeepney
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new LoginView())
            {
                BarBackgroundColor = Color.FromArgb("#2E43D9"), // Blue background
                BarTextColor = Colors.White // White text
            };
        }
    }
}


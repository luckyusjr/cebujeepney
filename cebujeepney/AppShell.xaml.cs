using cebujeepney.Views;
namespace cebujeepney
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(CommuterSettingsView), typeof(CommuterSettingsView));
        }
    }
}




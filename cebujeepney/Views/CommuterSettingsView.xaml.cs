using cebujeepney.ViewModels;
namespace cebujeepney.Views
{ 
	public partial class CommuterSettingsView : ContentPage
	{
		public CommuterSettingsView()
		{
			InitializeComponent();
			BindingContext = new CommuterVM(Navigation);
        }
	}
}



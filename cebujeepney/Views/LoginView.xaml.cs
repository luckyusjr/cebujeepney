using cebujeepney.ViewModels;
using cebujeepney.Services;

namespace cebujeepney.Views
{
  public partial class LoginView : ContentPage
   {
        public LoginView()
        {
            InitializeComponent();
            BindingContext = new LoginVM();
        }
   }
}



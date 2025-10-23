using cebujeepney.ViewModels;
using cebujeepney.Services;

namespace cebujeepney.Views
{
    public partial class RegisterView : ContentPage
    {
        public RegisterView()
        {
            InitializeComponent();
            BindingContext = new RegisterVM();
        }
    }
}



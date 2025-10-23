using cebujeepney.ViewModels;
using cebujeepney.Services;
using cebujeepney.Models;

namespace cebujeepney.Views
{
  public partial class LoginView : ContentPage
   {
        public LoginView()
        {
            InitializeComponent();
            BindingContext = new LoginVM();
        }

        /*private void OnCreateAdminClicked(object sender, EventArgs e)
        {
            try
            {
                var fileLocator = new FileLocatorService();
                var accountService = new AccountFileService(fileLocator);

                var admin = new Admin
                {
                    Email = "admin@mail.com",
                    Password = "password",
                    AccountType = "Admin"
                };

                accountService.SaveAdmin(admin);

                DisplayAlert("Success", "Admin account created!\nEmail: admin@cebujeepney.com\nPassword: admin123", "OK");
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message, "OK");
            }
        }*/
    }
}



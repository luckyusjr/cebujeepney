using cebujeepney.Models;
using Microsoft.Maui.Controls;

namespace cebujeepney.Services
{
    public class SessionService
    {
        private static SessionService _instance;
        public static SessionService Instance => _instance ??= new SessionService();

        // General user info
        public string Email { get; private set; }
        /*public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string MiddleName { get; private set; }*/
        public string AccountType { get; private set; }

        // Optional full model
        public Commuter CommuterAccount { get; private set; }
        public Admin AdminAccount { get; private set; }

        private SessionService() { }

        public void SetCommuter(Commuter user)
        {
            CommuterAccount = user;
            Email = user.Email;
            AccountType = "Commuter";
        }

        public void SetAdmin(Admin admin)
        {
            AdminAccount = admin;
            Email = admin.Email;
            AccountType = "Admin";
        }

        public void Clear()
        {
            Email = AccountType = null;
            CommuterAccount = null;
            AdminAccount = null;
        }
    }
}

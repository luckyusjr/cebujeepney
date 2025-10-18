using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cebujeepney.Models
{
    public class Admin : BaseUser
    {
        public string AccountType { get; set; } = "Admin";
        public string Password { get; set; }
        
    }
}


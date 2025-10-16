using cebujeepney.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace cebujeepney.Models
{
    public class Commuter : BaseUser
    {
        public string Password { get; set; }
        public string AccountType { get; set; } = "Commuter";
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cebujeepney.Services
{
    public interface IDisplayAlertService
    {
        Task ShowAlert(string title, string message, string cancel);
    }
}

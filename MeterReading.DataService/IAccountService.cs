using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MeterReading.Data.Model;

namespace MeterReading.DataService
{
    public interface IAccountService
    {
        public bool CreateAccount(int id, string firstname, string lastname);
    }
}

using MeterReading.Data.Context;
using MeterReading.Data.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace MeterReading.DataService
{
    public class AccountService : IAccountService
    {
        DbContextOptions<MeterReadingDataContext> _options;

        public AccountService(DbContextOptions<MeterReadingDataContext> options)
        {
            _options = options;
        }
        public bool CreateAccount(int id, string firstname, string lastname)
        {
            using (MeterReadingDataContext meterReadingDataContext = new MeterReadingDataContext(_options))
            {
                // Only create account if it doesn't already exist
                var account  = meterReadingDataContext.Accounts.Find(id);

                if (account == null)
                {
                    meterReadingDataContext.Accounts.Add(new Account()
                    {
                        AccountId = id,
                        FirstName = firstname,
                        LastName = lastname
                    });
                    meterReadingDataContext.SaveChanges();
                }
            }
            return true;
        }
    }
}

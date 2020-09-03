using MeterReading.Data.Context;
using MeterReading.Data.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MeterReading.DataService
{
    public class ReadingService : IReadingService
    {
        DbContextOptions<MeterReadingDataContext> _options;

        public ReadingService(DbContextOptions<MeterReadingDataContext> options)
        {
            _options = options;
        }
        public bool CreateReading(string inAccountId, string inReadingDate, string inReading)
        {
            // validate input details
            int accountId;
            if (!int.TryParse(inAccountId, out accountId))
            {
                throw new Exception("Invalid account id");
            }

            DateTime readingDate;
            if (!DateTime.TryParse(inReadingDate, out readingDate))
            {
                throw new Exception("Invalid reading date");   
            }

            Regex rg = new Regex(@"^\d{5}$");
            if (!rg.IsMatch(inReading))
            {
                throw new Exception("Invalid meter reading");
            }

            int reading = int.Parse(inReading);
 
            // Check account exists
            using (MeterReadingDataContext meterReadingDataContext = new MeterReadingDataContext(_options))
            {
                var account = meterReadingDataContext.Accounts.Find(accountId);

                if (account == null)
                {
                    throw new Exception("Account not found");
                }
            }

            return CreateReading(accountId, readingDate, reading);
        }


        public bool CreateReading(int accountId, DateTime readingDate, int reading)
        {
            using (MeterReadingDataContext meterReadingDataContext = new MeterReadingDataContext(_options))
            {
                meterReadingDataContext.Readings.Add(new Reading()
                {
                    AccountId = accountId,
                    MeterReadingDateTime = readingDate,
                    MeterReadValue = reading
                });
                meterReadingDataContext.SaveChanges();
            }
            return true;
        }

        public bool DeleteReading(Reading reading)
        {
            throw new NotImplementedException();
        }

        public Reading GetReading(int AccountId, DateTime readingDateTime)
        {
            throw new NotImplementedException();
        }

        public bool UpdateReading(Reading reading)
        {
            throw new NotImplementedException();
        }
    }
}

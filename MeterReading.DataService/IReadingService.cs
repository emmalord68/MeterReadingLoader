using MeterReading.Data.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MeterReading.DataService
{
    public interface IReadingService
    {
        public bool CreateReading(string inAccountId, string inReadingDate, string inReading);

        public bool CreateReading(int accountId, DateTime readingDate, int reading);

        public Reading GetReading(int AccountId, DateTime readingDateTime);
        public bool UpdateReading(Reading reading);
        public bool DeleteReading(Reading reading);
    }
}

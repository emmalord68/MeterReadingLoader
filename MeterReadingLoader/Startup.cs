using System;
using System.Threading.Tasks;
using MeterReading.Data.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using MeterReading.DataService;
using System.IO;

namespace MeterReadingLoader
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<MeterReadingDataContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("MeterReadingDataContextConnection")));

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IReadingService, ReadingService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider, IAccountService accountService)
        {
            try
            { 
                Task seedDatabase = SeedDatabase(serviceProvider, accountService);
                seedDatabase.Wait();
            }
            catch (Exception ex)
            {
                // Add logging
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        /// <summary>
        /// Add accounts to the database.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="accountService"></param>
        /// <returns></returns>
        private async Task SeedDatabase(IServiceProvider serviceProvider, IAccountService accountService )
        {
            //Get the account csv files.  Hardcoded here, but add location & filename to appsettings.json
            string path = @"../TestAccounts/Test_Accounts.csv";

            string[] lines = File.ReadAllLines(path);

            // Don't process the first line - it contains headers.
            for (int i = 1; i < lines.Length; i++)
            {
                string[] columns = lines[i].Split(',');
                accountService.CreateAccount(int.Parse(columns[0]),columns[1],columns[2]);
            }
        }
    }
}

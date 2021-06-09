using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleTracking.DataMigration.Identity.App {
    class Program {
        public static void Main(string[] args) {
            string connStr = Environment.GetEnvironmentVariable("VT_IDENTITY_SQL_CONNECTION");

            if (String.IsNullOrWhiteSpace(connStr)) {
                Console.WriteLine("No VT_IDENTITY_SQL_CONNECTION environment variable found.");
                Console.ReadKey();
                return;
            }

            using (var context = new IdentityDbContextFactory().CreateDbContext(connStr))
            using (var dbConn = context.Database.GetDbConnection()) {
                Console.WriteLine($"This will migrate {dbConn.DataSource}\\{dbConn.Database}. Continue?");

                if (Console.ReadLine().ToLower() == "y") {
                    Console.WriteLine(context.Database.GetMigrations().Count() + " migration(s) found.");
                    Console.WriteLine(context.Database.GetPendingMigrations().Count() + " to be applied.");
                    context.Database.Migrate();
                    Console.WriteLine("Done");
                }
                else {
                    Console.WriteLine("Cancelled");
                }
                Console.ReadKey();
            }
        }
    }
}

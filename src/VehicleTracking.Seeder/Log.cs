using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleTracking.Seeder {
    public class Log {
        public static void Info(string message) {
            if (!String.IsNullOrWhiteSpace(message)) {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(message);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
        public static void InfoInSameLine(string message) {
            if (!String.IsNullOrWhiteSpace(message)) {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("\r{0}   ", message);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
        public static void Error(string message) {
            if (!String.IsNullOrWhiteSpace(message)) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"\nError: ");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(message);
            }
        }
    }
}

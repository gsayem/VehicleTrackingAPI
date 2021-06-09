using System;
using System.Security.Cryptography;

namespace VehicleTracking.IdentityServer.Services {
    public static class SecretGenerator {
        public static string Generate() {
            var length = 32;
            RandomNumberGenerator cryptoRandomDataGenerator = RandomNumberGenerator.Create();
            byte[] buffer = new byte[length];
            cryptoRandomDataGenerator.GetBytes(buffer);
            return Convert.ToBase64String(buffer);
        }
    }
}

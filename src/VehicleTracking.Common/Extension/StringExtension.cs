using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace VehicleTracking.Common.Extension
{
    public static class StringExtension
    {
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }
        public static bool IsValidEmailAddress(this string value)
        {
            return new EmailAddressAttribute().IsValid(value);
        }
        public static Guid? ToGuid(this string value)
        {
            try
            {
                if (Guid.TryParse(value, out Guid guid))
                {
                    return guid;
                }
            }
            catch (Exception)
            {

            }
            return null;
        }

        public static string ToPasswordHash(this string password)
        {
            using (var sha1 = new SHA1Managed())
            {
                var hash = Encoding.UTF8.GetBytes(password);
                var generatedHash = sha1.ComputeHash(hash);
                var generatedHashString = Convert.ToBase64String(generatedHash);
                return generatedHashString;
            }
        }
    }
}

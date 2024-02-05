using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using static LoginAttemptshwcore.Program;
namespace LoginAttemptshwcore.Models
{
    public static class SecurityHelper
    {
        public static string GenerateSalt()
        {
            byte[] salt = new byte[16];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }

        public static string HashPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = password + salt;
                var bytes = Encoding.UTF8.GetBytes(saltedPassword);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
        static bool AuthenticateUser(string username, string password)
        {
            using (var context = new ApplicationContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Username == username);

                if (user == null || user.IsLocked)
                {
                    Console.WriteLine("User not found or locked.");
                    return false;
                }

                var hashedPassword = SecurityHelper.HashPassword(password, user.Salt); // Изменено на SecurityHelper

                if (hashedPassword == user.HashedPassword)
                {
                    user.LoginAttempts = 0;
                    context.SaveChanges();
                    Console.WriteLine("User authenticated successfully.");
                    return true;
                }
                else
                {
                    user.LoginAttempts++;
                    if (user.LoginAttempts >= 3)
                    {
                        user.IsLocked = true;
                    }
                    context.SaveChanges();
                    Console.WriteLine("Invalid password.");
                    return false;
                }
            }
        }
    }
}



using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using LoginAttemptshwcore.Models;
using Microsoft.Extensions.Logging;
using static LoginAttemptshwcore.Program;
namespace LoginAttemptshwcore;

class Program
{
    static void Main()
    {
        using (var db = new ApplicationContext())
        {

            // Инициализация данных книг
            //        var books = new Book[]
            //        {
            //        new Book { Title= "Book1", Author="Author1", PageCount=250 },
            //        new Book { Title= "Book2", Author="Author2", PageCount=340 },
            //        new Book { Title= "Book3", Author="Author3", PageCount=432 },
            //        new Book { Title= "Book4", Author="Author4", PageCount=124 },
            //        new Book { Title= "Book5", Author="Author5", PageCount=562 },
            //        };

            //        // Добавление книг в базу данных
            //        db.Books.AddRange(books);
            //        db.SaveChanges();
            
            //}
            Console.WriteLine("Enter username:");
            var username = Console.ReadLine();

            Console.WriteLine("Enter password:");
            var password = Console.ReadLine();

            //    //// Создаем нового пользователя и сохраняем его в базе данных
            //    var salt = SecurityHelper.GenerateSalt();
            //    var hashedPassword = SecurityHelper.HashPassword(password, salt);

            //    var newUser = new User
            //    {
            //        Username = username,
            //        HashedPassword = hashedPassword,
            //        Salt = salt,
            //        LoginAttempts = 0,
            //        IsLocked = false
            //    };

            //    db.Users.Add(newUser);
            //    db.SaveChanges();

            //    Console.WriteLine("User registered successfully.");

            // Теперь можно провести несколько попыток авторизации
            for (int i = 1; i < 3; i++)
            {
                Console.WriteLine($"Attempt {i}:");
                Console.WriteLine("Enter password:");
                var loginAttemptPassword = Console.ReadLine();

                bool isAuthenticated = AuthenticationHandler.HandleAuthentication(username, loginAttemptPassword);

                if (isAuthenticated)
                {
                    Console.WriteLine("User authenticated successfully.");
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid password.");
                }
            }
        }
    }
}








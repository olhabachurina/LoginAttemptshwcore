using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LoginAttemptshwcore.Program;

namespace LoginAttemptshwcore.Models
{
    public class AuthenticationHandler
    {
        public static bool HandleAuthentication(string username, string password)
        {
            // Проверка логина и пароля
            if (!IsUserAuthenticated(username, password))
            {
                return false;
            }

            ShowBookMenu();

            return true;
        }

        private static void ShowBookMenu()
        {
            Console.WriteLine("Book Menu:");
            Console.WriteLine("1. View All Books");
            Console.WriteLine("2. Search for a Book");
            Console.WriteLine("3. Pagination");
            Console.WriteLine("Enter your choice:");

            int choice;
            if (int.TryParse(Console.ReadLine(), out choice))
            {
                switch (choice)
                {
                    case 1:
                        ViewAllBooks();
                        break;
                    case 2:
                        SearchForBook();
                        break;
                    case 3:
                        Pagination();
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid choice.");
            }
        }

        private static void ViewAllBooks()
        {
            using (var context = new BookContext()) 
            {
                var bookRepository = new BookRepository(context);

                var allBooks = bookRepository.GetAllBooks();

                if (allBooks.Any())
                {
                    Console.WriteLine("List of All Books:");
                    foreach (var book in allBooks)
                    {
                        Console.WriteLine($"Title: {book.Title}, Author: {book.Author}, Page Count: {book.PageCount}");
                    }
                }
                else
                {
                    Console.WriteLine("No books found.");
                }
            }
        }

        private static void SearchForBook()
        {
            Console.WriteLine("Enter search term:");
            var searchTerm = Console.ReadLine();

            using (var context = new BookContext()) 
            {
                var bookRepository = new BookRepository(context);

                var searchResults = bookRepository.SearchBooks(searchTerm);

                if (searchResults.Any())
                {
                    Console.WriteLine("Search Results:");
                    foreach (var book in searchResults)
                    {
                        Console.WriteLine($"Title: {book.Title}, Author: {book.Author}, Page Count: {book.PageCount}");
                    }
                }
                else
                {
                    Console.WriteLine("No matching books found.");
                }
            }
        }

        private static void Pagination()
        {
            Console.WriteLine("Enter page number:");
            if (!int.TryParse(Console.ReadLine(), out int pageNumber) || pageNumber <= 0)
            {
                Console.WriteLine("Invalid page number.");
                return;
            }

            const int pageSize = 5;

            using (var context = new BookContext()) 
            {
                var bookRepository = new BookRepository(context);

                var pageBooks = bookRepository.GetPage(pageNumber, pageSize);

                if (pageBooks.Any())
                {
                    Console.WriteLine($"Page {pageNumber} Books:");
                    foreach (var book in pageBooks)
                    {
                        Console.WriteLine($"Title: {book.Title}, Author: {book.Author}, Page Count: {book.PageCount}");
                    }
                }
                else
                {
                    Console.WriteLine("No books found on this page.");
                }
            }
        }

        private static bool IsUserAuthenticated(string username, string password)
        {
            using (var db = new ApplicationContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Username == username);

                if (user == null || user.IsLocked)
                {
                    Console.WriteLine("User not found or locked.");
                    return false;
                }

                var hashedPassword = SecurityHelper.HashPassword(password, user.Salt);

                if (hashedPassword == user.HashedPassword)
                {
                    user.LoginAttempts = 0; // Сбрасываем счетчик неудачных попыток после успешной аутентификации
                    db.SaveChanges();
                    Console.WriteLine("User authenticated successfully.");
                    return true;
                }
                else
                {
                    user.LoginAttempts++;
                    if (user.LoginAttempts >= 3)
                    {
                        user.IsLocked = true; // Блокируем пользователя после третьей неудачной попытки
                    }
                    db.SaveChanges();
                    Console.WriteLine("Invalid password.");
                    return false;
                }
            }
        }
    }
}
    
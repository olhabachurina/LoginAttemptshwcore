using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LoginAttemptshwcore.Models.Book;

namespace LoginAttemptshwcore.Models
{
    public class BookRepository
    {
        private readonly BookContext _context;

        public BookRepository(BookContext context)
        {
            _context = context;
        }

        public IEnumerable<Book> GetAllBooks()
        {
            return _context.Books.ToList();
        }

        public IEnumerable<Book> SearchBooks(string searchTerm)
        {
            return _context.Books.Where(b => b.Title.Contains(searchTerm) || b.Author.Contains(searchTerm)).ToList();
        }

        public IEnumerable<Book> GetPage(int page, int pageSize)
        {
            return _context.Books.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }
    }

    public class BookContext : DbContext
    {
        public DbSet<Book> Books { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-4PCU5RA\\SQLEXPRESS;Database=Login;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }
    }
}
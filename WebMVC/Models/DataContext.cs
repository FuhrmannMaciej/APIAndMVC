using Microsoft.EntityFrameworkCore;
using BooksApi.Models;

namespace WebMVC.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }

        public DbSet<Book> Book { get; set; }
    }
}

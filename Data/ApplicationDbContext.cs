using LibraryWebApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LibraryWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<UserModel>  
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        
        public DbSet<BooksModel> Books { get; set; }
        public DbSet<BorrowedBooksModel> BorrowedBooks { get; set; }
    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BooksModel>().HasData(
                new BooksModel
                {
                    BookId = 1,
                    ImageUrl = "/img/cleancode.jpg",
                    BookName = "Clean Code",
                    Author = "Robert C. Martin",
                    Count = 3,
                },
                new BooksModel
                {
                    BookId = 2,
                    ImageUrl = "/img/grokkingalgo.jpg",
                    BookName = "Grokking Algorithm",
                    Author = "Robert C. Martin",
                    Count = 2,
                },
                new BooksModel
                {
                    BookId = 3,
                    ImageUrl = "/img/speakingjs.jpg",
                    BookName = "Speaking JavaScript",
                    Author = "Robert C. Martin",
                    Count = 0,
                }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}

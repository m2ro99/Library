using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LibraryWebApp.Models
{

    public class BorrowedBooksModel
    {
        [Key]
        public int Id { get; set; }
        // [ForeignKey("Book")]
        public int BookId { get; set; }
        public BooksModel? Book { get; set; } // navigation property ef will use it to get the specific data from db without going to BooksModel it will store it here
                                              // [ForeignKey("User")]
        public string? UserId { get; set; }
        public UserModel? User { get; set; } // navigation property ef will use it to get the specific data from db without going to UserModel it will store it here
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool IsReturned { get; set; } = false;

        [NotMapped]
        public TimeSpan? Duration => IsReturned ? ReturnDate - BorrowDate : null;
    }
}
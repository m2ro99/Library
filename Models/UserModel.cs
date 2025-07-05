using LibraryWebApp.Models;
using Microsoft.AspNetCore.Identity;

namespace LibraryWebApp.Models
{
    public class UserModel : IdentityUser
    {
        public ICollection<BorrowedBooksModel>? BorrowedBooks { get; set; }
    }
}
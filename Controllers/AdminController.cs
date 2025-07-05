using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LibraryWebApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace LibraryWebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var borrowedBooks = _context.BorrowedBooks
                .Include(b => b.Book)
                .Include(b => b.User)
                .ToList();

            var users = _context.Users.ToList();

            ViewBag.Users = users;
            return View(borrowedBooks);
        }

        [HttpPost]
        public IActionResult ReturnBook(int borrowedBookId)
        {
            var borrowedBook = _context.BorrowedBooks.Include(b => b.Book).FirstOrDefault(b => b.Id == borrowedBookId);
            if (borrowedBook != null && !borrowedBook.IsReturned)
            {
                borrowedBook.IsReturned = true;
                borrowedBook.ReturnDate = DateTime.Now;
                if (borrowedBook.Book != null)
                {
                    borrowedBook.Book.Count++;
                }
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UndoReturnBook(int borrowedBookId)
        {
            var borrowedBook = _context.BorrowedBooks.FirstOrDefault(b => b.Id == borrowedBookId);
            var borrowedBook2 = _context.BorrowedBooks.Include(b => b.Book).FirstOrDefault(b => b.Id == borrowedBookId);

            if (borrowedBook != null && borrowedBook.IsReturned && borrowedBook2?.Book != null)
            {
                borrowedBook.IsReturned = false;
                borrowedBook.ReturnDate = null;
                borrowedBook2.Book.Count--;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteUser(string userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                // Remove all borrowed books associated with this user first (if needed)
                var borrowedBooks = _context.BorrowedBooks.Where(b => b.UserId == userId).ToList();
                _context.BorrowedBooks.RemoveRange(borrowedBooks);
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryWebApp.Data;
using LibraryWebApp.Models;

[Authorize(Roles = "User,Admin")]
public class BorrowController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<UserModel> _userManager;

    public BorrowController(ApplicationDbContext context, UserManager<UserModel> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> BorrowedBooks()
    {
        var user = await _userManager.GetUserAsync(User);

        var borrowedBooks = await _context.BorrowedBooks
            .Include(b => b.Book)
            .Where(b => b.UserId == user!.Id && !b.IsReturned)
            .ToListAsync();

        return View(borrowedBooks);
    }
    [HttpPost]
    public async Task<IActionResult> Borrow(int bookId)
    {
        var book = await _context.Books.FindAsync(bookId);
        if (book == null || book.Count <= 0)
        {
            TempData["Error"] = "This book is not available.";
            return RedirectToAction("Index", "Books");
        }

        var user = await _userManager.GetUserAsync(User);

        var borrow = new BorrowedBooksModel
        {
            BookId = bookId,
            UserId = user?.Id,
            BorrowDate = DateTime.Now,
            ReturnDate = DateTime.Now.AddDays(14),
            IsReturned = false
        };

        book.Count--;

        _context.BorrowedBooks.Add(borrow);
        _context.Books.Update(book);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Book borrowed successfully.";
        return RedirectToAction("BorrowedBooks", "Borrow");
    }
    [HttpPost]
    public async Task<IActionResult> ReturnBook(int id)
    {
        var borrowedBook = await _context.BorrowedBooks
            .Include(b => b.Book) // include the Book model
            .FirstOrDefaultAsync(b => b.Id == id);

        if (borrowedBook == null || borrowedBook.IsReturned)
            return NotFound();

        // Mark as returned
        borrowedBook.IsReturned = true;

        // Set return date to now (actual return)
        borrowedBook.ReturnDate = DateTime.Now;

        // Increment book count if Book exists
        if (borrowedBook.Book != null)
        {
            borrowedBook.Book.Count++;
        }

        await _context.SaveChangesAsync();

        return RedirectToAction("BorrowedBooks", "Borrow");
    }
    [Authorize(Roles = "User,Admin")]
    [HttpGet]
    public async Task<IActionResult> BorrowWithRedirect(int bookId)
    {
        var user = await _userManager.GetUserAsync(User);

        var book = await _context.Books.FindAsync(bookId);
        if (book == null || book.Count <= 0)
        {
            TempData["Error"] = "This book is not available.";
            return RedirectToAction("Index", "Home");
        }

        var borrow = new BorrowedBooksModel
        {
            BookId = bookId,
            UserId = user?.Id,
            BorrowDate = DateTime.Now,
            ReturnDate = DateTime.Now.AddDays(14),
            IsReturned = false
        };

        book.Count--;

        _context.BorrowedBooks.Add(borrow);
        _context.Books.Update(book);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Book borrowed successfully.";

        return RedirectToAction("BorrowedBooks");
    }

}

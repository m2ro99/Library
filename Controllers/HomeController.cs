using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LibraryWebApp.Data;
using LibraryWebApp.Models;

namespace repos.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _db;
    public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    // Index Page
    public IActionResult Index()
    {
        // Getting Books from db and converting them to a list
        List<BooksModel> data = _db.Books.ToList();
        return View(data);
    }
    // Create Form
    public IActionResult Create()
    {
        return View();
    }
    public IActionResult Details(int Id)
    {
        var book = _db.Books.FirstOrDefault(b => b.BookId == Id);
        if (book == null)
        {
            return NotFound();
        }
        return View(book);

    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(BooksModel book)
    {
        // Server-side: Require image upload
        if (book.ImageFile == null)
        {
            ModelState.AddModelError("ImageFile", "Please upload an image for this book");
        }
        if (ModelState.IsValid)
        {
            if (book.ImageFile != null)
            {
                // ✅ Validate file size (max 5MB)
                if (book.ImageFile.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError("ImageFile", "Image size must be 5MB or less.");
                    return View(book);
                }

                // ✅ Validate file type (allow only images)
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(book.ImageFile.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("ImageFile", "Only image files (.jpg, .jpeg, .png, .gif) are allowed.");
                    return View(book);
                }

                // ✅ Save the image
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img");
                string uniqueFileName = Guid.NewGuid().ToString() + extension;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    book.ImageFile.CopyTo(fileStream);
                }

                book.ImageUrl = "/img/" + uniqueFileName;
            }

            _db.Books.Add(book);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        return View(book);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}


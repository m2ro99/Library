using LibraryWebApp.Models;
using LibraryWebApp.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.IO;

namespace LibraryWebApp.Controllers
{
    public class ModifyController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ModifyController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Modify/Edit/5
        public IActionResult Edit(int id)
        {
            var book = _context.Books.FirstOrDefault(b => b.BookId == id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Modify/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("BookId,BookName,Author,Count,ImageFile")] BooksModel model)
        {
            if (id != model.BookId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var book = _context.Books.FirstOrDefault(b => b.BookId == id);
                if (book == null)
                {
                    return NotFound();
                }

                // Update fields
                book.BookName = model.BookName;
                book.Author = model.Author;
                book.Count = model.Count;

                // Handle image upload
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    // Delete old image if exists
                    if (!string.IsNullOrEmpty(book.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", book.ImageUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Save new image
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var extension = Path.GetExtension(model.ImageFile.FileName).ToLower();
                    if (!allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("ImageFile", "Only image files (.jpg, .jpeg, .png, .gif) are allowed.");
                        return View(model);
                    }
                    if (model.ImageFile.Length > 5 * 1024 * 1024)
                    {
                        ModelState.AddModelError("ImageFile", "Image size must be 5MB or less.");
                        return View(model);
                    }
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);
                    string uniqueFileName = Guid.NewGuid().ToString() + extension;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        model.ImageFile.CopyTo(fileStream);
                    }
                    book.ImageUrl = "/img/" + uniqueFileName;
                }

                _context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            // If we got this far, something failed; reload the book for the view
            var originalBook = _context.Books.FirstOrDefault(b => b.BookId == id);
            if (originalBook != null)
            {
                model.ImageUrl = originalBook.ImageUrl;
            }
            return View(model);
        }

        // GET: Modify/Delete/5
        public IActionResult Delete(int id)
        {
            var book = _context.Books.FirstOrDefault(b => b.BookId == id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Modify/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var book = _context.Books.FirstOrDefault(b => b.BookId == id);
            if (book == null)
            {
                return NotFound();
            }
            _context.Books.Remove(book);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}

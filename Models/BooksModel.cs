using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryWebApp.Models
{

    public class BooksModel
    {
        [Key]
        public int BookId { get; set; }
        public string? ImageUrl { get; set; }
        [Required(ErrorMessage = "Book's Name can't be empty")]
        [MinLength(10, ErrorMessage = "Can't be less than 10 letters")]
        [MaxLength(60, ErrorMessage = "Maximum letters are 60")]
        public string? BookName { get; set; }

        [Required(ErrorMessage = "Author's Name can't be empty")]
        [MinLength(8, ErrorMessage = "Can't be less than 8 letters")]
        [MaxLength(50, ErrorMessage = "Maximum letters are 50")]
        public string? Author { get; set; }

        [Required(ErrorMessage = "Count is required")]
        [Range(0, 100, ErrorMessage = "Number must be between 0 & 100")]
        public int? Count { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

    }
}
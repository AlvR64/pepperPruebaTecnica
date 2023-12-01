using System.ComponentModel.DataAnnotations;

namespace LibraryPepper.API.Books.Models
{
    public class BookPublishRQ
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public DateTime PublicationDate { get; set; }
        [Required]
        public string AuthorName { get; set; } = string.Empty;
    }
}

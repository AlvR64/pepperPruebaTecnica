namespace LibraryPepper.Domain.Dtos.Books
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime PublicationDate { get; set; }
        public string AuthorName { get; set; } = string.Empty;
    }
}

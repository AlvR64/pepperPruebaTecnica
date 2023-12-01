namespace LibraryPepper.Domain.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime PublicationDate { get; set; }

        //Se ha valorado la opción de añadir una entidad Author ya que por ejemplo, a futuro se podría sacar un listado de los libros por autores pero ese caso de uso no está contemplado en el enunciado.
        public string AuthorName { get; set; } = string.Empty;
    }
}

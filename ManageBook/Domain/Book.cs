namespace ManageBook.Domain
{
    public class Book
    {
        public Guid ISBN { get; set; }
        public string Title { get; set; }
        public int NumberOfPages { get; set; }
        public string Author { get; set; }
        public string PublicationYear { get; set; }
    }
}

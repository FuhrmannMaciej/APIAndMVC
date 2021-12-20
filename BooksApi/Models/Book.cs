

namespace BooksApi.Models
{
    public class Book
    {
        public long Id { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public int NumberOfPages { get; set; }
        public long CopiesSold { get; set; }
        public bool IsAwarded { get; set; }

    }
}

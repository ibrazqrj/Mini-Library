namespace WASMLibrary.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public int YearPublished { get; set; }
        public bool IsAvailable { get; set; }
        public int BorrowedCount { get; set; }
    }
}

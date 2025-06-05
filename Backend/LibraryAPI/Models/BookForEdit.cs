namespace LibraryAPI.Models
{
    public class BookForEdit
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public int YearPublished { get; set; }
        public bool IsAvailable { get; set; }
    }
}

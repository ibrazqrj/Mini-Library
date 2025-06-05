using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Data
{
    public class BookEntity
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Title { get; set; }
        [MaxLength(50)]
        public string Author { get; set; }
        [MaxLength(17)]
        public string ISBN { get; set; }
        [Range(0, 2025)]
        public int YearPublished { get; set; }
        public bool IsAvailable { get; set; }
        public int BorrowedCount { get; set; }
        public List<BorrowingsEntity> Borrowings { get; set; } = new();
    }
}
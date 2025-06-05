namespace LibraryAPI.Data
{
    public class BorrowingsEntity
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public BookEntity Book { get; set; }
        public string UserOid { get; set; }
        public DateTime BorrowedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ReturnedAt { get; set; } 
    }
}

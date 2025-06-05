using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Data
{
    public class LibraryDbContext(DbContextOptions<LibraryDbContext> options) : DbContext(options)
    {
        public DbSet<BookEntity> Books { get; set; } = null!;

        public DbSet<UserEntity> Users { get; set; } = null!;

        public DbSet<BorrowingsEntity> Borrowings { get; set; }

    }
}

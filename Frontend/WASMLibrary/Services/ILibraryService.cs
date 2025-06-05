using WASMLibrary.Models;
using WASMLibrary.Models.Requests;

namespace WASMLibrary.Services
{
    public interface ILibraryService
    {
        Task<bool> RegisterAsync(RegisterUserRequest request);
        Task<bool> CheckExistingUser();
        Task<ChangeUserDataRequest?> GetUserInfo();
        Task<bool> UpdateUserInfo(ChangeUserDataRequest newUserData);
        Task<bool> CheckRegistrationState();
        Task<bool> SetRegistrationStateToTrue();
        Task<bool> IsAdressValidAsync(string fullAddress);
        Task<bool> LoginAsync(LoginUserRequest request);
        Task<List<Book>> GetBooksAsync();
        Task<List<Book>> GetBorrowedBooksByOidAsync();
        Task<EditBookRequest?> GetBookByIdAsync(int id);
        Task<bool> AddBookAsync(AddBookRequest request);
        Task<bool> EditBookAsync(EditBookRequest request);
        Task<bool> DeleteBookAsync(int id);
        Task<bool> BorrowBookAsync(BorrowBookRequest request);
        Task<bool> ReturnBookAsync(ReturnBookRequest request);
        Task<string> ShowMostBorrowedBookTitle();
        Task<string> ShowMostBorrowedBookTimes();
        Task<string> ShowTotalUserAmount();
        Task<string> ShowTotalBorrowedBooksAmount();
        Task<string> ShowCurrentlyBorrowedBooksAmount();
    }
}

using WASMLibrary.API;
using WASMLibrary.Models;
using WASMLibrary.Models.Requests;

namespace WASMLibrary.Services
{
    public class LibraryService : ILibraryService
    {
        private readonly ApiClient _apiClient;

        public LibraryService(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        // Book-related
        public async Task<List<Book>> GetBooksAsync() =>
            await _apiClient.GetBooksAsync();

        public async Task<EditBookRequest?> GetBookByIdAsync(int id) =>
            await _apiClient.GetBookByIdAsync(id);

        public async Task<bool> AddBookAsync(AddBookRequest request) =>
            await _apiClient.AddBookAsync(request);

        public async Task<bool> EditBookAsync(EditBookRequest request) =>
            await _apiClient.EditBookAsync(request);

        public async Task<bool> DeleteBookAsync(int id) =>
            await _apiClient.DeleteBookAsync(id);

        // User-related
        public async Task<bool> CheckExistingUser() =>
            await _apiClient.CheckExistingUser();

        public async Task<bool> RegisterAsync(RegisterUserRequest request) =>
            await _apiClient.RegisterAsync(request);

        public async Task<bool> LoginAsync(LoginUserRequest request) =>
            await _apiClient.LoginAsync(request);

        public async Task<bool> CheckRegistrationState() =>
            await _apiClient.CheckRegistrationState();

        public async Task<bool> SetRegistrationStateToTrue() =>
            await _apiClient.SetRegistrationStateToTrue();

        public async Task<ChangeUserDataRequest?> GetUserInfo() =>
            await _apiClient.GetUserInfo();

        public async Task<bool> UpdateUserInfo(ChangeUserDataRequest newUserData) =>
            await _apiClient.UpdateUserInfo(newUserData);

        public async Task<bool> IsAdressValidAsync(string fullAddress) =>
            await _apiClient.IsAdressValidAsync(fullAddress);

        // Borrowing-related
        public async Task<List<Book>> GetBorrowedBooksByOidAsync() =>
            await _apiClient.GetBorrowedBooksByOidAsync();

        public async Task<bool> BorrowBookAsync(BorrowBookRequest request) =>
            await _apiClient.BorrowBookAsync(request);

        public async Task<bool> ReturnBookAsync(ReturnBookRequest request) =>
            await _apiClient.ReturnBookAsync(request);

        // Analytics-related
        public async Task<string> ShowMostBorrowedBookTitle() =>
            await _apiClient.ShowMostBorrowedBookTitle();

        public async Task<string> ShowMostBorrowedBookTimes() =>
            await _apiClient.ShowMostBorrowedBookTimes();

        public async Task<string> ShowTotalUserAmount() =>
            await _apiClient.ShowTotalUserAmount();

        public async Task<string> ShowTotalBorrowedBooksAmount() =>
            await _apiClient.ShowTotalBorrowedBooksAmount();

        public async Task<string> ShowCurrentlyBorrowedBooksAmount() =>
            await _apiClient.ShowCurrentlyBorrowedBooksAmount();
    }
}

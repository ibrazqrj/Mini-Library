using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;
using WASMLibrary.Models;
using WASMLibrary.Models.Requests;

namespace WASMLibrary.API
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private string _baseUrl;

        public ApiClient(IConfiguration configuration, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["Api:BaseUrl"] ?? throw new InvalidOperationException("BaseUrl not defined");
        }

        public async Task<bool> RegisterAsync(RegisterUserRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{_baseUrl}/User/Registration", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> CheckExistingUser()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/User/CheckExistingUser/");

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        public async Task<ChangeUserDataRequest> GetUserInfo()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/User/GetUserInfo/");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ChangeUserDataRequest>();
            }

            return null;
        }

        public async Task<bool> UpdateUserInfo(ChangeUserDataRequest newUserData)
        {
            var json = JsonConvert.SerializeObject(newUserData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{_baseUrl}/User/UpdateUserInfo", content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> CheckRegistrationState()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/User/GetUserRegistrationState/");

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> SetRegistrationStateToTrue()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/User/SetRegistrationStateToTrue/");

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> IsAdressValidAsync(string fullAddress)
        {
            string url = $"https://nominatim.openstreetmap.org/search?q={Uri.EscapeDataString(fullAddress)}&format=json";

            _httpClient.DefaultRequestHeaders.Add("ibrazqrj", "MiniLibraryIZ/1.0 (i.zeqiraj@hotmail.com)");
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            string json = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(json);

            return result != null && result?.Count > 0;
        }

        public async Task<bool> LoginAsync(LoginUserRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/User/LoginUser", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<Book>> GetBooksAsync()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/Book/GetBooks");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"API error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                return new List<Book>();
            }

            var json = await response.Content.ReadAsStringAsync();
            var books = JsonConvert.DeserializeObject<List<Book>>(json);

            return books ?? new List<Book>();
        }

        public async Task<List<Book>> GetBorrowedBooksByOidAsync()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/Borrowing/GetBorrowedBooksByOid");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"API error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                return new List<Book>();
            }

            var json = await response.Content.ReadAsStringAsync();
            var books = JsonConvert.DeserializeObject<List<Book>>(json);

            return books ?? new List<Book>();
        }

        public async Task<EditBookRequest> GetBookByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/Book/GetBookById/{id}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<EditBookRequest>();
            }

            return null;
        }

        public async Task<bool> AddBookAsync(AddBookRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/Book/AddBook", content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EditBookAsync(EditBookRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{_baseUrl}/Book/EditBook/{request.Id}", content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/Book/DeleteBook/{id}");

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> BorrowBookAsync(BorrowBookRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{_baseUrl}/Borrowing/BorrowBook", content);

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> ReturnBookAsync(ReturnBookRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{_baseUrl}/Borrowing/ReturnBook", content);

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            return true;
        }

        public async Task<string> ShowMostBorrowedBookTitle()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/LibraryAnalytics/MostBorrowedBookTitle");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"API error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                return string.Empty;
            }

            var message = await response.Content.ReadAsStringAsync();

            return message ?? string.Empty;
        }

        public async Task<string> ShowMostBorrowedBookTimes()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/LibraryAnalytics/MostBorrowedBookTimes");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"API error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                return string.Empty;
            }

            var message = await response.Content.ReadAsStringAsync();

            return message ?? string.Empty;
        }

        public async Task<string> ShowTotalUserAmount()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/LibraryAnalytics/TotalUserAmount");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"API error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                return string.Empty;
            }

            var message = await response.Content.ReadAsStringAsync();

            return message ?? string.Empty;
        }

        public async Task<string> ShowTotalBorrowedBooksAmount()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/LibraryAnalytics/TotalBorrowedBooks");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"API error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                return string.Empty;
            }

            var message = await response.Content.ReadAsStringAsync();

            return message ?? string.Empty;
        }

        public async Task<string> ShowCurrentlyBorrowedBooksAmount()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/LibraryAnalytics/CurrentlyBorrowedBooks");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"API error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                return string.Empty;
            }

            var message = await response.Content.ReadAsStringAsync();

            return message ?? string.Empty;
        }
    }
}

# 📚 MiniLibrary (Blazor WebAssembly + ASP.NET Core API)

## Overview
**MiniLibrary** is a modern full-stack application for managing books and users in a digital library system. Built with **Blazor WebAssembly** for the frontend and **ASP.NET Core** for the backend, this project supports user registration, authentication, book borrowing, and analytics. It follows a clean architecture using services, interfaces, and an ApiClient as a central gateway to the API.

---

## ✨ Features
- **🔐 Authentication with Microsoft Login**
  - Secure authentication using **MSAL** and **Microsoft Identity Platform**
- **📚 Book Management**
  - Add, edit, delete, and view books
- **👥 User Profile Management**
  - Register, update user data, and check registration state
- **📖 Book Borrowing System**
  - Borrow and return books based on the current user
- **📊 Analytics Dashboard**
  - View total users, total borrowed books, most borrowed book, etc.
- **📍 Address Validation**
  - Validates addresses using **OpenStreetMap (Nominatim API)**

---

## 🖼 UI Preview
> Add your own screenshots here (AllBooks page, Profile page, Analytics dashboard, etc.)

---

## 🛠 Installation & Running

### 🔧 Prerequisites
- [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- Visual Studio 2022 or newer (with Blazor/WebAssembly workload)

### 🚀 Run the App (Development)
1. **📥 Clone the Repository**
   ```bash
   git clone https://github.com/your-username/MiniLibrary.git
   cd MiniLibrary
   ```
### ⚙️ Run the Backend (API)

- Open the API project (e.g. `LibraryAPI`) in Visual Studio  
- Press `F5` or click **Run**

### 💻 Run the Frontend (Blazor WASM)

- Open the Blazor WebAssembly project (e.g. `WASMLibrary`)  
- Press `F5` or click **Run**

## 📁 Project Structure

   ```graphql
    MiniLibrary
    ├── 📦 LibraryAPI                       # Backend (ASP.NET Core Web API)
    │   ├── Controllers/                   # API Endpoints (Book, Borrowing, User, Analytics)
    │   ├── Data/                          # DbContext, AutoMapper, Entities
    │   ├── Middleware/                    # Custom Middleware (e.g. ObjectId handling)
    │   ├── Migrations/                    # EF Core Migrations
    │   ├── Models/                        # DTOs & Request Models
    │   ├── Services/                      # Service Layer (UserService, LibraryService, etc.)
    │   ├── appsettings.json               # Configuration files
    │   └── Program.cs                     # Application Startup

    ├── 📦 WASMLibrary                      # Frontend (Blazor WebAssembly)
    │   ├── API/                           # ApiClient, Auth Handler, Extensions
    │   ├── Configuration/                 # Central API Configuration
    │   ├── Index/                         # Index page
    │   ├── Layout/                        # Layout components (NavMenu, MainLayout, etc.)
    │   ├── Models/
    │   │   ├── Requests/                  # Frontend DTOs for requests
    │   │   └── Book.cs / User.cs
    │   ├── Pages/                         # Razor Pages (AddBook, EditBook, Home, etc.)
    │   ├── Services/                      # Shared LibraryService & Interface
    │   ├── Shared/                        # Shared components & App.razor
    │   ├── Program.cs                     # Frontend setup
    │   └── wwwroot/                       # Static files

    ├── 📦 WASMLibrary.Client.Test         # Unit Test Project for Frontend
    │   ├── BorrowTests.cs
    │   ├── LibraryTests.cs
    │   └── UserTests.cs
   ```

---

## 📦 Used Technologies

- **Blazor WebAssembly** – Frontend SPA  
- **ASP.NET Core** – Backend API  
- **MudBlazor** – UI Components  
- **MSAL / Microsoft Identity** – Authentication  
- **AutoMapper** – DTO mapping  
- **Entity Framework Core** – Database ORM  
- **OpenStreetMap API** – Address validation  

---

## 🤝 Contributing

Feel free to contribute by submitting issues, pull requests, or feedback.

```bash
# Fork it 🍴
# Create your feature branch
git checkout -b feature/amazing-feature

# Commit your changes
git commit -m 'Add amazing feature'

# Push to the branch
git push origin feature/amazing-feature

# Open a Pull Request
```

---

## 📧 Contact

Created by [Ibrahim Zqrj](https://github.com/ibrazqrj) – reach out via GitHub or open an issue.


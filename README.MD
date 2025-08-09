# 📒 Ledgerly

**Ledgerly** is a personal finance management system designed to help users track income and expenses with clarity and control. Built with a modular architecture, Ledgerly provides secure APIs for managing accounts, categories, transactions, and users.

---

## 🚀 Features

- ✅ Create and manage user accounts
- ✅ Define custom income/expense categories
- ✅ Record transactions with timestamps and metadata
- ✅ Associate transactions with accounts and categories
- 🔐 Secure API endpoints with authentication (JWT-ready)
- 📊 Designed for future reporting and analytics

---

## 🧱 Tech Stack

| Layer       | Technology               |
| ----------- | ------------------------ |
| Backend API | ASP.NET Core Web API     |
| Database    | SQL Server               |
| Frontend    | Next.js (planned)        |
| Mobile App  | React Native (planned)   |
| Auth        | JWT-based authentication |

---

## 📦 API Endpoints

### 💸 Transaction

- `POST /api/v1/transaction/create-transaction` – Record income/expense
- `POST /api/v1/transaction/get-transactions` – List transactions
- `POST /api/v1/transaction/get-transaction` – get transaction
- `POST /api/v1/transaction/update-transaction` – Update transaction
- `POST /api/v1/transaction/delete-transaction` – Delete transaction

---

## 🛠️ Setup Instructions

1. Clone the repo:
   ```bash
   git clone https://github.com/vechet/Ledgerly.git
   cd Ledgerly
   ```
2. Configure your database connection in appsettings.json.
3. Run the backend:
   ```bash
   dotnet run
   ```

## 🧮 Database Migration Steps

1. Ledgerly uses Entity Framework Core for database access.
2. Apply Existing Migrations:
   ```bash
   dotnet ef database update
   ```

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
| Database    | SQL Server / PostgreSQL  |
| Frontend    | Next.js (planned)        |
| Mobile App  | React Native (planned)   |
| Auth        | JWT-based authentication |

---

## 📦 API Endpoints

### 🔐 Authentication

- `POST /api/auth/register` – Create a new user
- `POST /api/auth/login` – Authenticate and receive JWT

### 👤 User

- `GET /api/users/{id}` – Get user profile
- `PUT /api/users/{id}` – Update user info

### 💼 Account

- `POST /api/accounts` – Create account
- `GET /api/accounts` – List accounts

### 🗂️ Category

- `POST /api/categories` – Create category
- `GET /api/categories` – List categories

### 💸 Transaction

- `POST /api/transactions` – Record income/expense
- `GET /api/transactions` – List transactions
- `PUT /api/transactions/{id}` – Update transaction
- `DELETE /api/transactions/{id}` – Delete transaction

---

## 🛠️ Setup Instructions

1. Clone the repo:
   ```bash
   git clone https://github.com/yourusername/ledgerly.git
   cd ledgerly
   ```

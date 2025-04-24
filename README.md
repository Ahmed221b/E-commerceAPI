# 🛒 E-Commerce Backend API – .NET 8

This is a powerful and secure e-commerce backend built with **.NET 8**, featuring modern design patterns, robust authentication, and seamless integration with external services.

## 🚀 Features

### ✅ Authentication & Authorization
- Implemented using **ASP.NET Identity**, **JWT Access Tokens**, and **Refresh Tokens**
- Role-based authorization
- Email verification for user registration
- Password management:
  - Change password
  - Reset password via email

### 🛡️ Role Management
- Admins can assign roles to users
- Roles determine access to features and endpoints

### 🛍️ Product Management (Admin)
- Full **CRUD operations** for:
  - Products
  - Categories
  - Colors
- Admins can manage user accounts and assign roles

### 🛒 Shopping Cart (Customer)
- Add, update, or remove items from cart
- Proceed to checkout with **Stripe Payment Integration**

### ❤️ Wishlist
- Customers can **add**, **view**, **update**, and **delete** wishlist items

### ✍️ Product Reviews
- Customers can leave and manage reviews for products

### 🧱 Architecture
- **Generic Repository Pattern**
- **Unit of Work** for clean and maintainable data access
- Layered architecture: API layer, Application services, Domain models, and Infrastructure

---

## 📦 Tech Stack

- **.NET 8**
- **Entity Framework Core**
- **ASP.NET Core Identity**
- **JWT & Refresh Token Authentication**
- **Stripe API**
- **SQL Server** (or any supported RDBMS)
- **Automapper**, **FluentValidation**, etc. (if applicable)

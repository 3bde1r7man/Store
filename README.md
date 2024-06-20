# Store

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Installation](#installation)
- [Usage](#usage)
- [Technologies](#technologies)

## Overview

**Store** is an ASP.NET-based web application that provides a complete e-commerce experience. Users can register with email confirmation, log in, browse products, manage their shopping cart, and complete purchases. Admins have control over product and category management, enabling them to add, edit, delete, and view details.

## Features

### User Features

- **Registration & Authentication**
    - Register with email confirmation
    - Secure login system
- **Shopping**
    - Browse products
    - Add products to the shopping cart
    - Checkout process
- **Order Management**
    - View order history
    - View order details

### Admin Features

- **Category Management**
    - Add, edit, delete, and view details of categories
- **Product Management**
    - Add, edit, delete, and view details of products
- **Order Management**
    - Edit, Delete, and view details of orders

## Installation

To set up the Store project locally, follow these steps:

1. **Clone the Repository**

```bash
git clone https://github.com/3bde1r7man/Store.git
```

2. **Navigate to the Project Directory**

```bash
cd store
```

3. **Restore NuGet Packages**

```bash
dotnet restore
```

6. **Run the Application**

```bash
dotnet run
```

## Usage

### As a User

1. **Register and Confirm Email**
2. **Login to Your Account**
3. **Browse Products**
4. **Add Products to Cart**
5. **Proceed to Checkout**
6. **View Order History and Details**

### As an Admin

1. **Login as Admin**
2. **Manage Categories and Products**
    - Add, edit, delete, and view details
3. **Manage All Orders**
    - Edit order status, delete, and view details

## Technologies

- **Backend:** ASP.NET Core
- **Database:** Entity Framework Core
- **Frontend:** Razor Pages
- **Authentication:** ASP.NET Identity
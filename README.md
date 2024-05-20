# Furniture Store API
Welcome to the Furniture Store Project! This API allows users to manage products and sales, and handle e-commerce functionalities of a furniture store.

## Table of Contents
- [Introduction](#introduction)
- [Features](#features)
- [Technologies Used](#technologies-used)
- [Installation](#installation)
- [Usage](#usage)
- [API Endpoints](#api-endpoints)

## Introduction

This project provides a comprehensive API for a furniture store, enabling users to add and manage products, handle sales, and manage e-commerce functionalities. The API is designed to be flexible and easy to use,
supporting various operations needed to run an online furniture store efficiently.

## Features

- **Product Management**: Add, update, delete and view products.
- **Sales Management**: Hanlde customer orders, track sales, and manage order statuses.
- **User Authentication**: Secure authentication for users.
- **E-Commerce Integration**: Integrate with payment gateways and manage transactions.
- **Inventory Management**: Track product inventory and manage stock levels.

## Technologies Used

- **Backend**: ASP.NET Core
- **Database**: PostgreSQL
- **Authentication**: JWT(Json Web Tokens)
- **Containerization**: Docker
- **Version Control**: Git and Github
- **Testing**: XUnit

## Installation
1. **Clone the repository:**
   ```bash
   git clone https://github.com/my-furniture-store/furniturestore.api.git
   cd FurnitureStore

2. **Set up the database:**
   
   Ensure you have PostgreSQL database installed and configured.
   
   ```bash
   cd ./FurnitureStore.API/
   dotnet user-secrets init
   dotnet user-secrets set "ConnectionStrings:FurnitureStoreDB" "your-connection-string"
  
3. **Intall Dependencies:**
   ```bash
   dotnet restore
   
4. **Run the application:**
   ```bash
   cd ./FurnitureStore.API/
   dotnet run 

5. **Run the application with docker:**
    ```bash
    cd ./FurnitureStore.API/
    docker-compose build
    docker-compose up

## Usage
### Postman
You can use Postman to test the API endpoints.

### Swagger
The API includes Swagge UI for easy testing and exploration. Once the application is running, navigate to https://localhost:7000/swagger/index.html   
to access the Swagger UI.

## API Endpoints
### Authentication *(Coming soon)*
- **POST** `api/auth/register` - Register a new user
- **POST** `api/auth/login` - Authentication a user and retrieve token.

### Products
#### Products *(Coming soon)*
- **GET** `/api/products` - Retrieve all products. 
- **GET** `/api/products/{id}` - Retrieve a product by ID. 
- **POST** `/api/products` - Add a new product. 
- **PUT** `/api/products/{id}` - Update an existing product.
- **DELETE** `/api/products/{id}` - Delete a product.
#### Categories & Sub-Categories
- **GET** `/api/categories` - Retrieve all categories. 
- **GET** `/api/categories/{id}` - Retrieve a category by ID. 
- **POST** `/api/categories` - Add a new category. 
- **PUT** `/api/categories/{id}` - Update an existing category.
- **DELETE** `/api/categories/{id}` - Delete a category.
- **GET** `/api/categories/{categoryId}/subcategories` - Retrieve all sub-categories of specified category. 
- **GET** `/api/categories/{categoryId}/subcategories/{id}` - Retrieve a sub-category by ID. 
- **POST** `/api/categories/{categoryId}/subcategories` - Add a new sub-category to a category specified by category ID. 
- **PUT** `/api/categories/{categoryId}/subcategories/{id}` - Update an existing sub-category.
- **DELETE** `/api/categories/{categoryId}/subcategories/{id}` - Delete a subcategory.

### Sales *(Coming soon)*
- **GET** `/api/sales` - Retrieve all sales.
- **GET** `/api/sales/{id}` - Retrieve a sale by ID.
- **POST** `/api/sales` - Create a new sale.
- **PUT** `/api/sales/{id}` - Update an existing sale.
- **DELETE** `/api/sales/{id}` - Delete a sale.
  
### Inventory *(Coming soon)*
- **GET** `/api/inventory` - Retrieve inventory levels. *(Coming Soon)*
- **PUT** `/api/inventory/{id}` - Update inventory for a product. *(Coming Soon)*

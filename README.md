# StockFlow ERP API

StockFlow ERP API is a lightweight but structured ERP backend built with ASP.NET Core Web API. It manages products, categories, warehouses, suppliers, customers, purchase orders, sales orders, invoices, stock movements, and dashboard summaries without using an external database.

Data is stored locally using JSON files.

## Features

- Product management
- Category management
- Warehouse management
- Supplier management
- Customer management
- Purchase order flow
- Sales order flow
- Automatic invoice generation
- Stock movement tracking
- Low stock monitoring
- Dashboard summary
- Local JSON file storage
- Clean service-based architecture

## Tech Stack

- .NET 8
- ASP.NET Core Web API
- C#
- JSON file storage

## Project Structure

```txt
StockFlowERP/
├── Controllers/
├── Contracts/
├── Data/
├── Models/
├── Repositories/
├── Services/
├── Program.cs
├── StockFlowERP.csproj
└── README.md
```

## Run Project

```bash
dotnet run
```

## Main Endpoints

```txt
GET     /api/dashboard/summary

GET     /api/categories
POST    /api/categories
GET     /api/categories/{id}
PUT     /api/categories/{id}
DELETE  /api/categories/{id}

GET     /api/warehouses
POST    /api/warehouses
GET     /api/warehouses/{id}
PUT     /api/warehouses/{id}
DELETE  /api/warehouses/{id}

GET     /api/products
POST    /api/products
GET     /api/products/{id}
PUT     /api/products/{id}
DELETE  /api/products/{id}

GET     /api/suppliers
POST    /api/suppliers
GET     /api/suppliers/{id}
PUT     /api/suppliers/{id}
DELETE  /api/suppliers/{id}

GET     /api/customers
POST    /api/customers
GET     /api/customers/{id}
PUT     /api/customers/{id}
DELETE  /api/customers/{id}

GET     /api/purchase-orders
POST    /api/purchase-orders
GET     /api/purchase-orders/{id}

GET     /api/sales-orders
POST    /api/sales-orders
GET     /api/sales-orders/{id}

GET     /api/invoices
GET     /api/invoices/{id}
```

## Learning Purpose

This project was created to learn ASP.NET Core Web API through a more complex backend structure without requiring Docker or a database server.

It focuses on:

- API controller structure
- Service layer
- Repository pattern
- Dependency injection
- Local JSON persistence
- Business flow handling
- Inventory transaction logic
- Clean backend organization

## License

MIT

# Product Catalog API

A .NET 8 Web API that exposes three endpoints for browsing a product catalog, built with Entity Framework Core (in-memory) and Swagger/OpenAPI.

---

## Project Structure

```
ProductCatalogApi/
├── Controllers/
│   └── ProductsController.cs   # All three API endpoints
├── Data/
│   └── AppDbContext.cs          # EF Core context + seed data
├── DTOs/
│   └── ProductDtos.cs           # Response shape for each endpoint
├── Models/
│   ├── Product.cs               # Product entity
│   └── ProductAttribute.cs      # ProductAttribute entity (many-to-one)
├── Program.cs                   # DI registration, middleware pipeline
└── ProductCatalogApi.csproj
```

---

## Prerequisites

| Tool | Minimum Version |
|------|----------------|
| [.NET SDK](https://dotnet.microsoft.com/download) | 8.0 |

No external database is required – the app uses an **in-memory EF Core database** seeded with sample data on startup.

---

## Running Locally

```bash
# 1. Clone / download the repository
git clone <your-repo-url>
cd ProductCatalogApi

# 2. Restore packages
dotnet restore

# 3. Run
dotnet run
```

The API starts on **https://localhost:5001** (and http://localhost:5000 as a fallback).  
Swagger UI is served at the root: **https://localhost:5001/**

---

## API Endpoints

All endpoints are under `/api/products`.

### 1. `GET /api/products/{id}` — GetProduct

Returns the basic fields for a **single product**.

**Path parameter**

| Name | Type | Description |
|------|------|-------------|
| `id` | `int` | ProductId |

**Responses**

| Status | Description |
|--------|-------------|
| 200 | `ProductDto` with `productId`, `productName`, `pricePerItem`, `averageCustomerRating` |
| 404 | Product not found |
| 500 | Unexpected server error |

**Example**

```
GET /api/products/1
```

```json
{
  "productId": 1,
  "productName": "Wireless Headphones",
  "pricePerItem": 79.99,
  "averageCustomerRating": 4.7
}
```

---

### 2. `GET /api/products` — GetProducts

Returns **all products** ordered by `averageCustomerRating` **descending** (highest first).

**Responses**

| Status | Description |
|--------|-------------|
| 200 | Array of `ProductSummaryDto` |
| 500 | Unexpected server error |

**Example**

```
GET /api/products
```

```json
[
  { "productId": 4, "productName": "Laptop Stand",        "pricePerItem": 49.99, "averageCustomerRating": 4.8 },
  { "productId": 1, "productName": "Wireless Headphones", "pricePerItem": 79.99, "averageCustomerRating": 4.7 },
  ...
]
```

---

### 3. `GET /api/products/{id}/attributes` — GetProductAttributes

Returns the **attributes** (color, width, etc.) for a given product.  
Returns an **empty array** (not 404) when the product exists but has no attributes.

**Path parameter**

| Name | Type | Description |
|------|------|-------------|
| `id` | `int` | ProductId |

**Responses**

| Status | Description |
|--------|-------------|
| 200 | Array of `ProductAttributeDto` (may be empty) |
| 404 | Product not found |
| 500 | Unexpected server error |

**Example**

```
GET /api/products/1/attributes
```

```json
[
  { "attributeId": 1, "attributeName": "Color",        "attributeValue": "Matte Black"   },
  { "attributeId": 2, "attributeName": "Connectivity", "attributeValue": "Bluetooth 5.2" },
  { "attributeId": 3, "attributeName": "Battery Life", "attributeValue": "30 hours"      }
]
```

---

## Design Decisions

- **In-memory DB** – zero setup; swap for SQL Server by changing one line in `Program.cs`.
- **DTO projection** – entities are never serialised directly; EF projects to DTOs in SQL to avoid over-fetching.
- **AsNoTracking** – all read queries use `AsNoTracking()` for better performance.
- **Structured logging** – `ILogger` with structured parameters on every endpoint.
- **Error handling** – try/catch in each action returns 500 with a safe message; real errors are logged server-side.
- **404 vs 200+[]** – `GetProductAttributes` returns 404 only when the product itself doesn't exist; an existing product with no attributes returns `200 []`.

---

## Switching to SQL Server

1. Add the SQL Server package:
   ```bash
   dotnet add package Microsoft.EntityFrameworkCore.SqlServer
   ```
2. In `Program.cs`, replace:
   ```csharp
   opt.UseInMemoryDatabase("ProductCatalogDb")
   ```
   with:
   ```csharp
   opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
   ```
3. Add a connection string to `appsettings.json`:
   ```json
   "ConnectionStrings": { "Default": "Server=...;Database=ProductCatalog;..." }
   ```
4. Run EF migrations:
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```
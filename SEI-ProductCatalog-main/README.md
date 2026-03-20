# Product Catalog API

Product catalog Web API to handle three endpoints related a product catalog with built with Entity Framework Core and Swagger.

# Project Structure

ProductCatalogApi/
├── Controllers/
│   └── ProductsController.cs    # All three API endpoints, GetProduct, GetProducts, GetProductAttributes
├── Data/
│   └── ProductDbContext.cs      # Entity Framework Core database context to configure data models relationships and inserting data
├── DTOs/
│   └── ProductDtos.cs           # Response structure of API request endpoints
├── Models/
│   ├── Product.cs               # Product data modole as an entity
│   └── ProductAttribute.cs      # ProductAttribute data model as entity to have (many-to-one)
├── Program.cs                   # Application building and registration for the executing and running the application
└── ProductCatalog.csproj        # Application project in C#


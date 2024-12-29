# Simple CRUD ASP.NET Core REST API

This project demonstrates a basic implementation of a CRUD REST API using ASP.NET Core. It includes setup instructions, code snippets, and essential configurations to get started quickly.

---

## Features

- ASP.NET Core-based RESTful API.
- CRUD operations for managing products.
- Integration with SQL Server using Entity Framework Core.
- Logger integration for better debugging.

---

## Prerequisites

Before you begin, ensure you have the following installed:

- [.NET 6 SDK or later](https://dotnet.microsoft.com/download)
- A code editor like [Visual Studio](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)
- SQL Server (local or remote instance)

---

## Getting Started

Follow these steps to set up and run the project.

### 1. Clone the Repository

Clone the repository to your local machine:

```bash
git clone https://github.com/your-username/aspdotnetcore-crud-api.git
cd aspdotnetcore-crud-api
```

# 2. Install Required NuGet Package

Install the `Microsoft.EntityFrameworkCore.SqlServer` package to enable SQL Server support in the project:

```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```

# 3. Define the Product Entity

Create a `Data/Product.cs`  file and define the `Product` entity:

```bash
namespace aspDotNetCore.Data
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Sku { get; set; }
    }
}
```

# 4. Configure the Application DbContext

Create a `Data/ApplicationDbContext.cs` file and set up the DbContext:

```csharp
using Microsoft.EntityFrameworkCore;

namespace aspDotNetCore.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().ToTable("Products");
        }

        public DbSet<Product> Products { get; set; }
    }
}
```
# 5. Update the Program.cs File

Register the `ApplicationDbContext` in the dependency injection container. Open the `Program.cs` file and add the following line:

```csharp
builder.Services.AddDbContext<ApplicationDbContext>(cfg => 
    cfg.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]));
```

# Ensure the connection string is set up in your appsettings.json file:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your-server;Database=your-database;User Id=your-username;Password=your-password;"
  }
}
```

# 6. Inject Dependencies in the Controller

In your `ProductsController`, inject the `ApplicationDbContext` and `ILogger`:

```csharp
private readonly ILogger<ProductsController> _logger;
private readonly ApplicationDbContext _dbContext;

public ProductsController(ApplicationDbContext dbContext, ILogger<ProductsController> logger)
{
    _dbContext = dbContext;
    _logger = logger;
}
```
# Running the Application

### 1. Apply migrations to set up the database schema:

```bash
dotnet ef migrations add InitialMigration
dotnet ef database update
```

### 2. Run the application:

```bash
dotnet run
```

### 3. The API will be available at `https://localhost:5001` or `http://localhost:5000`.

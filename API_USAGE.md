# CleanTemplate API - Quick Start Guide

This is a complete .NET 9.0 Web API template with a ready-to-use Product CRUD implementation following Clean Architecture principles.

## 🚀 Quick Start

### Prerequisites
- .NET 9.0 SDK or later
- IDE of your choice (Visual Studio, VS Code, Rider, etc.)

### Getting Started

1. **Clone and Run**
   ```bash
   git clone <repository-url>
   cd CleanTemplateRepository
   
   # Build the project
   ./scripts/build.sh debug
   
   # Run the API
   cd Src/CleanTemplate.API
   dotnet run
   ```

2. **Access the API**
   - Swagger UI: `http://localhost:5247` (opens automatically)
   - API Base URL: `http://localhost:5247/api`

## 📝 API Endpoints

### Product CRUD Operations

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/Product` | Get all products |
| GET | `/api/Product/{id}` | Get product by ID |
| POST | `/api/Product` | Create new product |
| PUT | `/api/Product/{id}` | Update existing product |
| DELETE | `/api/Product/{id}` | Delete product |

## 🧪 Testing the API

### Option 1: Using Swagger UI (Recommended)
1. Run the application
2. Open `http://localhost:5247` in your browser
3. Use the interactive Swagger interface to test all endpoints

### Option 2: Using HTTP Client
Use the provided `CleanTemplate.API.http` file with VS Code REST Client extension or similar tools.

### Option 3: Using curl

**Create a Product:**
```bash
curl -X POST "http://localhost:5247/api/Product" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Test Product",
    "description": "A sample product for testing",
    "price": 29.99,
    "stock": 100,
    "isActive": true,
    "createdBy": "testuser"
  }'
```

**Get All Products:**
```bash
curl -X GET "http://localhost:5247/api/Product"
```

**Get Product by ID:**
```bash
curl -X GET "http://localhost:5247/api/Product/{product-id}"
```

**Update Product:**
```bash
curl -X PUT "http://localhost:5247/api/Product/{product-id}" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Updated Product",
    "description": "Updated description",
    "price": 39.99,
    "stock": 80,
    "isActive": true,
    "createdBy": "testuser",
    "updatedBy": "testuser"
  }'
```

**Delete Product:**
```bash
curl -X DELETE "http://localhost:5247/api/Product/{product-id}"
```

## 🏗️ Project Structure

```
CleanTemplateRepository/
├── Src/
│   ├── CleanTemplate.API/          # Web API layer
│   │   ├── Controllers/            # API controllers
│   │   └── Program.cs              # Application entry point
│   ├── CleanTemplate.Application/  # Application/Business logic layer
│   │   ├── DTOs/                   # Data Transfer Objects
│   │   └── Services/               # Business services
│   ├── CleanTemplate.Domain/       # Domain layer
│   │   └── Entities/               # Domain entities
│   └── CleanTemplate.Tests/        # Unit tests
├── Solution/
│   └── CleanTemplate.sln           # Solution file
└── scripts/                        # Build and test scripts
```

## 📊 Sample Data

The API uses in-memory storage for demonstration purposes. Here are some sample products you can create:

```json
{
  "name": "Smartphone X1",
  "description": "Latest flagship smartphone",
  "price": 899.99,
  "stock": 50,
  "isActive": true,
  "createdBy": "admin"
}
```

```json
{
  "name": "Wireless Headphones",
  "description": "Premium noise-cancelling headphones",
  "price": 299.99,
  "stock": 25,
  "isActive": true,
  "createdBy": "admin"
}
```

## 🧪 Running Tests

```bash
# Run all tests
./scripts/run-tests.sh

# Or run tests directly
dotnet test Solution/CleanTemplate.sln
```

## 🔧 Build Scripts

The repository includes generic build scripts that work with any .NET solution:

```bash
# Build in Debug mode
./scripts/build.sh debug

# Build in Release mode
./scripts/build.sh release

# Run tests
./scripts/run-tests.sh
```

## 📚 API Documentation

The API includes comprehensive Swagger documentation with:
- Interactive endpoint testing
- Request/response examples
- Validation rules
- Error response formats

Access the documentation at: `http://localhost:5247` when the API is running.

## 🎯 Key Features

- ✅ **Clean Architecture**: Proper separation of concerns
- ✅ **CRUD Operations**: Complete Create, Read, Update, Delete functionality
- ✅ **Input Validation**: Comprehensive request validation
- ✅ **Error Handling**: Proper HTTP status codes and error responses
- ✅ **Swagger Documentation**: Auto-generated interactive API docs
- ✅ **Unit Tests**: Comprehensive test coverage
- ✅ **In-Memory Storage**: Ready to run without database setup
- ✅ **Generic Build Scripts**: Reusable across different projects

## 🔄 Next Steps

This template provides a solid foundation. Consider these enhancements:

1. **Database Integration**: Replace in-memory storage with Entity Framework
2. **Authentication**: Add JWT authentication
3. **Logging**: Implement structured logging
4. **Caching**: Add response caching
5. **Rate Limiting**: Implement API rate limiting
6. **Health Checks**: Add application health endpoints

## 📝 License

This template is provided as-is for educational and development purposes.
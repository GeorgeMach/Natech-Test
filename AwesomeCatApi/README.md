# Awesome Cat API

A .NET Core Web API that fetches and manages cat images with their associated temperament tags.

##  Features

- Fetch cat images from The Cat API
- Store cat images and their metadata in a SQL Server database
- Tag-based filtering system using cat temperaments
- Pagination support for listing cats
- RESTful API endpoints
- Swagger documentation

##  Getting Started

### Prerequisites

- .NET 6.0 or later
- SQL Server
- The Cat API key (get one from [The Cat API](https://thecatapi.com/))

### Configuration

## Configuring Database
This project uses Trusted connection.
* Update your `appsettings.json` with your database connection string:
 "ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=Cat;Trusted_Connection=True;TrustServerCertificate=True;"
}

You can use also the following options:

*  Standard Security
Server=localhost;Database=Cat;User Id=myUsername;Password=myPassword;

* Connection to a SQL Server instance
Server=localhost\InstanceName;Database=Cat;User Id=myUsername;Password=myPassword;

## Key
1. Update the `appsettings.json` with your API Key:
  "ApiKeys": {
    "CatApiKey": "Insert-Key-Here"
  }


### Installation

1. Clone the repository
2. Run database migrations:
	```Bash
	dotnet ef database update
    ```
3. Build and run the project

##  API Endpoints

### Cats

- `POST /api/cats/fetch` - Fetch new cats from The Cat API
- `GET /api/cats` - Get paginated list of cats
  - Query Parameters:
    - `tag` (optional): Filter by temperament tag
    - `page` (default: 1): Page number
    - `pageSize` (default: 10): Items per page
- `GET /api/cats/{id}` - Get specific cat by ID

##  Project Structure

- `Controllers/` - API endpoints
- `Services/` - Logic
- `Models/` - Data models
- `Context/` - Database context and configurations
- `Migrations/` - Database migrations

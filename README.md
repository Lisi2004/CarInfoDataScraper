# RSS Feed Car Data Scraper

This project provides a service for scraping car details from the RSS feed of `https://www.veturaneshitje.com/home/rss` and saving the data into a SQL Server database. It retrieves and processes car details such as price, vehicle type, year, and mileage, ensuring all relevant data is stored for further use.

---

## Table of Contents

1. [Features](#features)
2. [Setup](#setup)
3. [Usage](#usage)
4. [Architecture](#architecture)
5. [Configuration](#configuration)
6. [Known Issues](#known-issues)

---

## Features

- Fetches car URLs from an RSS feed.
- Scrapes car details from individual car pages using `HtmlAgilityPack`.
- Persists car details into a database.
- Includes error handling for network and parsing issues.

---

## Setup

### Prerequisites

1. [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0) or later
2. [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) instance
3. [Visual Studio Code](https://code.visualstudio.com/) or any other IDE supporting .NET development
4. NuGet packages:
   - `HtmlAgilityPack`
   - `Microsoft.EntityFrameworkCore`
   - `Microsoft.EntityFrameworkCore.SqlServer`

### Steps

1. Clone the repository:
   ```bash
   git clone https://github.com/your-username/rss-feed-car-scraper.git
   cd rss-feed-car-scraper
   ```

2. Configure your database connection string in `appsettings.json`:
   ```json
   {
       "ConnectionStrings": {
           "DevConnection": "Server=YOUR_SERVER;Database=YOUR_DATABASE;Trusted_Connection=True;"
       }
   }
   ```

3. Run database migrations:
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

4. Build and run the project:
   ```bash
   dotnet build
   dotnet run
   ```

---

## Usage

The main service, `RssService`, scrapes car data from the RSS feed and stores it into the database. You can initiate the scraping process by running the `Main` method in the `Program.cs` file.

### Running the Scraper

```bash
cd src

# Run the scraper
Dotnet run
```

---

## Architecture

### Key Components

1. **RssService**:
   - Fetches car URLs from the RSS feed.
   - Scrapes car details from individual car pages.
   - Saves processed car data into the database.

2. **Car Model**:
   - Defines the structure of the car data stored in the database.

3. **AppDbContext**:
   - Manages database interactions using Entity Framework Core.

4. **Program.cs**:
   - Configures dependencies and starts the scraping process.

### Project Structure

```plaintext
src/
  |- Program.cs
  |- RssService.cs
  |- Car.cs
  |- AppDbContext.cs
config/
  |- appsettings.json
```

---

## Configuration

The project relies on an `appsettings.json` file for database configuration. Ensure the file includes a valid connection string under `ConnectionStrings.DevConnection`.

Example:

```json
{
    "ConnectionStrings": {
        "DevConnection": "Server=localhost;Database=CarData;Trusted_Connection=True;"
    }
}
```

---

## Known Issues

1. **RSS Feed Changes**: If the structure of the RSS feed or car detail pages changes, scraping may fail. Update the `HtmlNode` selectors in `GetCarDetailsFromUrl` to resolve this issue.
2. **Database Connection**: Ensure the database connection string is correctly configured.
3. **Error Handling**: While general error handling is implemented, some edge cases may require additional handling.

---

## Contributing

Feel free to open issues or submit pull requests to improve this project. Contributions are welcome!

---

## License

This project is licensed under the MIT License. See `LICENSE` for details.


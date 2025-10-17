# Mini-Game Hub Backend

This repository contains the backend service for the Mini-Game Hub, developed using **ASP.NET Core / C#** and **Entity Framework Core (EF Core)** for database management via Migrations.

This application is configured to use **SQL Server LocalDB** for development, as specified in the `appsettings.json` file.

## Prerequisites

Before running the application, ensure you have the following installed:

1.  **[.NET SDK (Recommended version based on your project's target framework)](https://dotnet.microsoft.com/download)**
2.  **SQL Server LocalDB** (Usually included with Visual Studio or SQL Server Express installation).
3.  **`dotnet-ef` global tool** (The Entity Framework Core Command-Line tool).

### 🚀 Install `dotnet-ef` Global Tool (If not already installed)

Open your terminal and run the following command to install the EF Core CLI tool globally:

```bash
dotnet tool install --global dotnet-ef
```

## Getting Started

Follow these steps to set up and run the backend on your local machine.

### Step 1: Clone the Repository

If you haven't already, clone the repository to your local machine:

```bash
git clone [YOUR_REPO_URL]
cd mini-game-hub
```

### Step 2: Restore Dependencies

Navigate to the project's root directory (where the .sln or primary .csproj file is located) and restore the necessary NuGet packages:

```bash
dotnet restore
```

(Note: dotnet build or dotnet run usually handles this automatically, but it's good practice.)

### Step 3: Configure the Database

The backend uses EF Core Migrations to manage the database schema. The default configuration connects to (localdb)\mssqllocaldb.

Check your appsettings.json file to confirm the connection string. If the connection string is correct, apply the pending migrations. This will create the database (MiniGameHubDb) and its tables for the first time.

```bash
dotnet ef database update
```

(If your solution has multiple projects, you might need to specify the Data/Migrations project and the Web/API project: dotnet ef database update --project Your.Data.Project --startup-project Your.Web.Api.Project)

### Step 4: Run the Application

Once the database is set up, you can start the backend service:

```bash
dotnet run
```

The application will compile and start the web server. The terminal output will show the listening URLs (e.g., http://localhost:5000 or https://localhost:5001).

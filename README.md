# TodoApp — Full-Stack Todo Manager

A full-stack Todo application built with **ASP.NET Core Web API**, **Entity Framework Core**, **PostgreSQL**, and **React**. It supports creating, viewing, updating, deleting, searching, and filtering Todo items.

This repository contains two separate applications: a .NET backend API and a React frontend. Run them in separate terminals during local development.

## Features

- Create, view, update, and delete Todo items.
- Search Todo items by title or description.
- Filter Todo items by category and priority.
- Assign a priority of `Low`, `Medium`, or `High`.
- Organize Todo items by category, for example `Work` or `Personal`.
- Validate request fields using data annotations.
- Return appropriate HTTP responses from the API.
- Handle exceptions centrally through custom exception middleware.
- Persist data in PostgreSQL using Entity Framework Core migrations.

## Technology stack

### Backend
- C#
- ASP.NET Core Web API (.NET 10)
- Entity Framework Core
- Npgsql PostgreSQL provider
- PostgreSQL
- Dependency injection
- Controller / service / repository separation

### Frontend
- React
- Vite
- JavaScript
- npm

## Repository structure

```text
TodoApp/
├── TodoApp.slnx
├── TodoApi/
│   ├── Controllers/       # HTTP endpoints
│   ├── Data/              # EF Core DbContext
│   ├── DTOs/               # Request models and validation
│   ├── Exceptions/         # Application-specific exceptions
│   ├── Middleware/         # Centralized exception handling
│   ├── Migrations/         # EF Core database migrations
│   ├── Models/             # Todo model and priority enum
│   ├── Repository/         # Database access
│   ├── Services/           # Application/business logic
│   ├── Program.cs          # Dependency injection and HTTP pipeline
│   ├── appsettings.json    # Non-secret application settings
│   └── TodoApi.csproj
└── Frontend/
    └── todo-client/
        ├── src/             # React components and API calls
        ├── package.json
        ├── package-lock.json
        └── vite.config.js
```

## Prerequisites

Install the following on your machine:

1. **Git**
2. **.NET 10 SDK**
3. **Node.js** compatible with the installed Vite version (Node.js 22.12 or newer is recommended)
4. **PostgreSQL** (a local PostgreSQL installation is sufficient)

You can check your installations with:

```bash
git --version
dotnet --version
node --version
npm --version
```

## 1. Clone the repository

```bash
git clone https://github.com/hasnainsheikh15/TodoApp.git
cd TodoApp
```

## 2. Configure PostgreSQL

Start PostgreSQL and create a database named `TodoDb`.

You can create the database in pgAdmin or with `psql`:

```sql
CREATE DATABASE "TodoDb";
```

Use the PostgreSQL username and password configured on your own machine. Do not use or expect the developer's local database credentials.

### Configure the backend connection string

The committed `TodoApi/appsettings.json` intentionally leaves `DefaultConnection` empty so that database credentials are not stored in source control.

Open a terminal in the backend directory:

```bash
cd TodoApi
```

Configure the connection string using .NET User Secrets:

```bash
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=TodoDb;Username=postgres;Password=YOUR_POSTGRES_PASSWORD"
```

Replace `YOUR_POSTGRES_PASSWORD` with the password for your local PostgreSQL user. If your PostgreSQL username differs from `postgres`, update `Username` too.

User Secrets are stored outside the project directory and should not be committed to Git.

## 3. Create/update the database schema

From the `TodoApi` directory, install the EF Core command-line tool if it is not already installed:

```bash
dotnet tool install --global dotnet-ef --version 10.*
```

If `dotnet-ef` is already installed, update it to a compatible 10.x version instead:

```bash
dotnet tool update --global dotnet-ef --version 10.*
```

Restore backend dependencies and apply the committed migrations:

```bash
dotnet restore
dotnet ef database update
```

The database must be running and the connection string must be configured before applying migrations.

## 4. Run the backend API

From the `TodoApi` directory:

```bash
dotnet run --launch-profile http
```

The HTTP launch profile is expected to expose the API at:

```text
http://localhost:5264
```

Keep this terminal open while using the frontend.

The API routes use the `/api/Todo` base path. Examples:

| Method | Route | Purpose |
|---|---|---|
| `GET` | `/api/Todo` | List Todo items; accepts optional search/filter query parameters |
| `GET` | `/api/Todo/{id}` | Get one Todo item |
| `POST` | `/api/Todo` | Create a Todo item |
| `PUT` | `/api/Todo/{id}` | Update a Todo item |
| `DELETE` | `/api/Todo/{id}` | Delete a Todo item |

The list endpoint supports optional query parameters such as `search`, `category`, and `priority`. For example:

```text
http://localhost:5264/api/Todo?search=assignment&category=Work&priority=High
```

Priority values are represented as strings such as `Low`, `Medium`, and `High`.

## 5. Run the React frontend

Open a **second terminal** from the repository root and run:

```bash
cd Frontend/todo-client
npm install
npm run dev
```

Vite will print a local development URL, normally:

```text
http://localhost:5173
```

Open the displayed URL in your browser. Keep both the frontend and backend terminals running.

The frontend's API configuration currently targets `http://localhost:5264`, so run the backend using the HTTP launch profile above unless you also update the frontend API base URL.

### If PowerShell blocks `npm`

On some Windows systems, PowerShell's execution policy blocks the `npm.ps1` script. You can use the Windows command shim instead:

```powershell
npm.cmd install
npm.cmd run dev
```

This avoids changing the machine's PowerShell execution policy.

## 6. Using the application

Once both applications are running:

1. Open the Vite URL in your browser.
2. Create Todo items with a title, optional description, priority, and category.
3. Use the available controls to update completion/details or delete items.
4. Search by text and filter by category or priority.
5. Try categories such as `Work` and `Personal`, and priorities such as `High`, `Medium`, and `Low`.

Data is stored in the configured PostgreSQL database, so items remain after restarting the applications.

## Troubleshooting

### The API cannot connect to PostgreSQL
- Confirm PostgreSQL is running.
- Confirm the `TodoDb` database exists.
- Check the `DefaultConnection` User Secret, including username, password, port, and database name.
- From the `TodoApi` directory, run `dotnet user-secrets list` to check whether the connection string is configured. Do not paste secret values into public issues or commit them.

### `dotnet ef` is not recognized
Install the .NET EF tool using the command in the database migration section, then open a new terminal if necessary. Ensure the installed tool version is compatible with EF Core 10.

### The frontend loads but requests fail
- Ensure the backend is running at `http://localhost:5264`.
- Check the browser console and network panel for the request URL and error.
- Confirm the frontend API base URL still matches the backend address.
- Ensure both applications are running; starting Vite does not start the .NET API.

### PowerShell refuses to run `npm`
Use `npm.cmd` instead of `npm` as shown above.

### The database schema is missing or outdated
From `TodoApi`, configure the connection string and run:

```bash
dotnet ef database update
```

## Security notes

- Never commit database passwords, API keys, or other secrets.
- Use .NET User Secrets for local development.
- Use environment variables or a secure secret store for deployment.
- The current CORS policy is intended for local development. If deploying the frontend, restrict CORS to the intended frontend origin.

## Development notes

Run the backend and frontend in separate terminals. The React app is intentionally a separate project from the ASP.NET Core API; it does not need to appear in Visual Studio's .NET Solution Explorer to run.

---

**Author:** Hasnain Sheikh  
**Repository:** https://github.com/hasnainsheikh15/TodoApp

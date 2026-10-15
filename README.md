# ProjectNet - Orders, Customers and Products Microservice Demo

[![CI](https://github.com/AdrianPi87/ProjectNet/actions/workflows/ci.yml/badge.svg)](https://github.com/AdrianPi87/ProjectNet/actions/workflows/ci.yml)
![.NET 8](https://img.shields.io/badge/.NET-8-512BD4?logo=dotnet)
![Angular 22](https://img.shields.io/badge/Angular-22-DD0031?logo=angular&logoColor=white)

A full-stack demo aligned with a senior .NET 8 / ASP.NET Core role.

- **Backend**: C# / .NET 8, ASP.NET Core Web API, **CQRS** (MediatR), **DDD** aggregates, **SOLID**, **hexagonal architecture**.
- **Persistence**: EF Core + Microsoft SQL Server, migrations and `Database.Migrate()` on startup.
- **EDA**: MassTransit over RabbitMQ in production, in-memory transport in local development; publishes and consumes `OrderCreated`.
- **Frontend**: Angular 22, TypeScript, Angular Router, RxJS, Tailwind-inspired minimalist UI.
- **Tests**: xUnit for domain and application behavior.
- **DevSecOps / Cloud**: Dockerfile, Docker Compose, Kubernetes manifests, Azure DevOps pipeline.

## Domain model

The seeded demo database contains:

- `Customers`
- `Products`
- `Orders`
- `OrderLine`

Domain rules are enforced inside the `Order` aggregate, such as preventing invalid lines, confirming only non-empty pending orders, and shipping only confirmed orders.

## Architecture

```text
backend/
  src/
    ProjectNet.Domain          -> Entities, DDD aggregates, domain events
    ProjectNet.Application     -> Commands, queries, handlers, DTOs, validation, ports
    ProjectNet.Infrastructure  -> EF Core persistence, repositories, MassTransit bus, consumers, seed
    ProjectNet.Api             -> Controllers, middleware, Swagger, composition root
  tests/
  deploy/k8s/
frontend/
azure-pipelines.yml
```

## Prerequisites

- .NET SDK 8 or later
- Node.js 20 or later with npm
- SQL Server 2022/2025 local instance or LocalDB
- Optional: Docker Desktop

## Backend: configure SQL Server

The backend currently uses a local Windows authentication connection string:

```json
{
  "ConnectionStrings": {
    "Default": "Server=localhost;Database=ProjectNet;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Adjust this value in `backend/src/ProjectNet.Api/appsettings.json` depending on your installation:

```text
Default instance:   Server=localhost;Database=ProjectNet;Trusted_Connection=True;TrustServerCertificate=True;
Named SQLEXPRESS:   Server=localhost\\SQLEXPRESS;Database=ProjectNet;Trusted_Connection=True;TrustServerCertificate=True;
LocalDB:            Server=(localdb)\\MSSQLLocalDB;Database=ProjectNet;Trusted_Connection=True;TrustServerCertificate=True;
```

For Docker, connection details are already configured in `docker-compose.yml`.

## Run the backend

```powershell
cd backend
dotnet restore
dotnet build
dotnet test
cd src\ProjectNet.Api
dotnet run --urls "http://localhost:5000"
```

Swagger is available at:

```text
http://localhost:5000/swagger
```

## Run the frontend

```powershell
cd frontend
npm install
npm start
```

Open:

```text
http://localhost:4200
```

The frontend expects the backend at `http://localhost:5000`.

## Seed data

In `Development`, when the database is empty, the application inserts:

- 10 customers
- 10 products
- 10 orders
- related order lines

If tables already contain data, seeding is skipped to avoid duplicates. To regenerate a clean demo database:

```powershell
# SQL Server default instance
sqlcmd -S localhost -E -Q "IF DB_ID('ProjectNet') IS NOT NULL BEGIN ALTER DATABASE ProjectNet SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE ProjectNet; END"
```

Then start the backend again. It applies EF Core migrations and seeds the demo data.

## Authentication and admin users

Authentication uses JWT. In Development the seeder creates these demo accounts:

| Role  | Username | Password       | Access |
|---|---|---|---|
| Admin | admin | Admin123! | Full access, including /admin/users |
| User  | user  | User123!  | Standard application access |

Login endpoint: POST /api/auth/login
Admin user management: GET /api/users

## Frontend views

- `/` - paginated order list with status, customer and page-size filters
- `/orders/:id` - order detail, customer name, product details, confirm/ship/cancel and line management
- /login - username and password login
- /admin/users - admin-only user management page
- `/create` - create a new order
- `/products` - product catalog with category and search filters
- `/products/create` - create a new product
- `/customers/create` - create a new customer

## API endpoints

### Orders

| Method | Route | Description |
|---|---|---|
| GET | `/api/orders` | Paginated orders. Query: `page`, `pageSize`, `status`, `customerId` |
| GET | `/api/orders/{id}` | Get one order |
| POST | `/api/orders` | Create order (`{ "customerId": "guid" }`) |
| POST | `/api/orders/{id}/lines` | Add a line (`{ "productId", "quantity", "unitPrice" }`) |
| POST | `/api/orders/{id}/confirm` | Confirm pending order |
| POST | `/api/orders/{id}/ship` | Ship confirmed order |
| POST | `/api/orders/{id}/cancel` | Cancel non-shipped order |

### Authentication

| Method | Route | Description |
|---|---|---|
| POST | /api/auth/login | Username-and-password login. Returns JWT and role |
| GET | /api/users | Admin-only list of users |
| POST | /api/users | Admin-only create user |

### Customers

| Method | Route | Description |
|---|---|---|
| GET | `/api/customers` | List customers |
| GET | `/api/customers/{id}` | Get customer |
| POST | `/api/customers` | Create customer (`{ "name", "email" }`) |

### Products

| Method | Route | Description |
|---|---|---|
| GET | `/api/products` | List products |
| GET | `/api/products/{id}` | Get product |
| POST | `/api/products` | Create product (`{ "name", "sku", "category", "unitPrice" }`) |

## Switching to RabbitMQ

Set the following in `appsettings.json` or through environment variables:

```json
{
  "Messaging": {
    "UseRabbitMq": true
  },
  "RabbitMq": {
    "Host": "localhost",
    "Username": "guest",
    "Password": "guest",
    "VirtualHost": "/"
  }
}
```

When disabled, the API uses MassTransit in-memory transport so the project runs locally without RabbitMQ.

## Containers

```powershell
cd backend
docker compose up --build
```

This starts:

- SQL Server
- Orders API on `http://localhost:8080`

Apply the Kubernetes manifests on any compatible cluster:

```bash
kubectl apply -f backend/deploy/k8s/
```

## Azure DevOps

`azure-pipelines.yml` restores, builds and tests both backend and frontend, publishes build artifacts and builds the container image.

## Preview

The UI uses a clean, high-contrast, Tailwind-inspired design system. Store screenshot assets under docs/screenshots/ and reference them here.

- docs/screenshots/order-list.png - paginated orders, filters and actions
- docs/screenshots/order-detail.png - order details, customer/product data and transitions
- docs/screenshots/product-catalog.png - product catalog with category filtering
- docs/screenshots/create-order.png - order creation with customer selection

`mermaid
flowchart LR
  User[User / Browser] --> Angular[Angular 22 SPA]
  Angular --> Api[ASP.NET Core Web API]
  Api --> Orders[Orders]
  Api --> Catalog[Customers / Products]
  Api --> Sql[(SQL Server)]
  Api --> MassTransit
  MassTransit --> Consumer[OrderCreated consumer]
`

## GitHub branch protection (recommended)

- Require a pull request before merging to main.
- Require at least one review.
- Require status checks: Backend (.NET 8) and Frontend (Angular 22).
- Block force pushes and direct commits to main.
- Require branches to be up to date before merging.

## Notes for interviews

- **SOLID**: one handler per use case, repository abstractions and event bus abstraction.
- **DDD**: `Order` controls its own invariants and raises domain events.
- **CQRS**: commands and queries are separate MediatR messages.
- **Hexagonal architecture**: domain and application depend only on abstractions; EF Core and MassTransit stay in infrastructure.
- **Event-driven**: `OrderCreated` is published and consumed locally.
- **TDD**: domain aggregate and application handler tests are included.
- **UX**: paginated list, filtering, responsive layout, accessible top-level navigation and clean Tailwind-inspired design.

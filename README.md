# MicroserviceProject_SellingBuddy_youtube

A comprehensive microservices-based e-commerce solution built with .NET and modern cloud-native patterns. Inspired by distributed system architecture, this project demonstrates best practices for scalable, maintainable, and modular online selling platforms.

---

## Table of Contents

- [Project Overview](#project-overview)
- [Architecture](#architecture)
- [Technologies Used](#technologies-used)
- [Microservices](#microservices)
- [API Gateway](#api-gateway)
- [Web Client](#web-client)
- [How to Run](#how-to-run)
- [Development & Extensibility](#development--extensibility)
- [License](#license)

---

## Project Overview

SellingBuddy is a demo microservices-based e-commerce platform. It features product catalog management, shopping basket, order processing, user identity, payment, notifications, and a Blazor-based web UI. Each business functionality is encapsulated in its own microservice, communicating via REST APIs and asynchronous messaging.

---

## Architecture

- **Microservices**: Independent services for catalog, basket, orders, identity, payments, notifications.
- **API Gateway**: Central entry point for client applications, routing requests to relevant services.
- **Blazor Web Client**: Modern SPA frontend consuming APIs through the gateway.
- **Service Discovery**: Consul is used for dynamic registration and discovery.
- **Event-Driven Communication**: RabbitMQ for asynchronous integration events (e.g., order creation).

```
ServiceUrls:
- Web.ApiGateway:       http://localhost:5000
- PaymentService.Api:   http://localhost:5001
- OrderService.Api:     http://localhost:5002
- BasketService.Api:    http://localhost:5003
- CatalogService.Api:   http://localhost:5004
- IdentityService.Api:  http://localhost:5005
- NotificationService:  Console App
```

---

## Technologies Used

- **.NET 5+ / ASP.NET Core**
- **Blazor WebAssembly**
- **RabbitMQ** (event bus)
- **Redis** (basket storage)
- **Consul** (service discovery)
- **Swagger/OpenAPI** (API docs)
- **Serilog** (logging)
- **Docker** (suggested for deployment)
- **Entity Framework Core** (data persistence)

---

## Microservices

### 1. CatalogService
- Provides product information and images
- Uses EF Core for persistence
- Exposes RESTful APIs

### 2. BasketService
- Manages user shopping baskets (cart)
- Backed by Redis for fast access
- Publishes/consumes events via RabbitMQ
- Secured endpoints (JWT, IdentityServer)

### 3. OrderService
- Handles order creation, tracking, and details
- Persists order data using EF Core
- Listens to basket checkout events

### 4. IdentityService
- Manages user authentication and authorization
- Issues JWT tokens for secure communication

### 5. PaymentService
- Integrates payment gateway logic (mock or actual)
- Processes payment and order status

### 6. NotificationService
- Sends notifications (console app, extensible for email/SMS)

---

## API Gateway

- Hosts at `http://localhost:5000`
- Aggregates requests/responses to/from microservices
- Handles authentication and routing for the frontend

---

## Web Client

- **Blazor WebAssembly** SPA in `src/Clients/BlazorWebApp/WebApp`
- Features:
  - Product catalog browsing
  - Add/remove basket items
  - Checkout and order management
- Uses local storage, HttpClient, and authentication state management

---

## How to Run

Prerequisites:
- [.NET SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/) (optional, recommended)
- [RabbitMQ](https://www.rabbitmq.com/download.html)
- [Redis](https://redis.io/download)
- [Consul](https://www.consul.io/downloads)

Steps:

1. Clone the repo:
   ```sh
   git clone https://github.com/TechBuddyTR/MicroserviceProject_SellingBuddy_youtube.git
   cd MicroserviceProject_SellingBuddy_youtube
   ```

2. Start infrastructure dependencies (RabbitMQ, Redis, Consul):
   - Can use Docker Compose or manual startup.

3. Build and run microservices:
   ```sh
   # In separate terminals, run each service:
   dotnet run --project src/Services/CatalogService/CatalogService.Api
   dotnet run --project src/Services/BasketService/BasketService.Api
   dotnet run --project src/Services/OrderService/OrderService.Api
   dotnet run --project src/Services/IdentityService/IdentityService.Api
   dotnet run --project src/Services/PaymentService/PaymentService.Api
   dotnet run --project src/ApiGateways/Web.ApiGateway/Web.ApiGateway.csproj
   dotnet run --project src/Clients/BlazorWebApp/WebApp/WebApp.csproj
   ```

4. Access the web UI at `http://localhost:5000` (via API Gateway).

---

## Development & Extensibility

- New features can be added as independent microservices.
- Integration events enable communication without tight coupling.
- Each service can be scaled and deployed independently.

---

## License

This project is open source. Some included icon fonts (Open Iconic) are under MIT and SIL licenses. See individual service folders for details.

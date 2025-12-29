# 📚 Book Management API

## Overview

This project is a **Book Management Web API** built using **ASP.NET Core**.  
It demonstrates **Clean Architecture**, **SOLID principles**, and an **event-driven design** while remaining easy to run and review.

The application manages:
- **Books** (Create, Update, Delete, Query)
- **History** (Audit trail of book changes)

The system runs as **a single ASP.NET Core API with one Swagger UI**, but is internally designed so it can be **split into microservices later with minimal changes**.

---

## 🏗 Architecture Overview

### High-level design goals

- Clean Architecture
- Modular / Vertical Slice structure
- Event-driven communication
- Database-per-module
- Future-ready for Kafka / microservices
- Simple to run for reviewers

```
src/
└── BookManagement.API
├── Modules
│ ├── Books
│ │ ├── Api
│ │ ├── Application
│ │ ├── Domain
│ │ └── Infrastructure
│ └── History
│ ├── Api
│ ├── Application
│ ├── Domain
│ └── Infrastructure
├── Shared
├── Middlewares
└── Program.cs
```

Each module has **clear boundaries** even though everything runs in one process.

---

## 📘 Books Module

**Responsibilities**
- Manage book lifecycle (create, update, delete)
- Publish domain events after successful persistence

**Key design points**
- EF Core (latest)
- SQLite database
- Optimistic concurrency using `RowVersion`
- Domain events (`BookCreated`, `BookUpdated`, `BookDeleted`)
- No dependency on History module

---

## 🕒 History Module

**Responsibilities**
- Maintain an **append-only audit trail**
- Provide **read-only APIs** for querying history

**Key design points**
- History entries are created **only by event handlers**
- No POST / PUT / DELETE endpoints
- Stores **snapshots**, not references
- Never queries the Books database
- Fully independent read model

---

## 🔔 Event-Driven Design

### Why events?

History is a **business concern**, not an infrastructure concern.  
Domain events allow:

- Loose coupling
- Clear ownership
- Accurate audit logs
- Easy migration to Kafka/RabbitMQ later

### Events used

- `BookCreatedEvent`
- `BookUpdatedEvent`
- `BookDeletedEvent`

Each event carries a **BookSnapshot**, preserving the state at the time of change.

---

## 📸 Snapshot-Based History

Instead of only storing `BookId`, history stores:

- `BookId` (for correlation)
- `BookTitle` (snapshot)
- `Authors` (snapshot)
- Human-readable description

This avoids:
- Cross-module joins
- Temporal inconsistency
- Tight coupling

## 🧱 Persistence Strategy

- **Books DB** → SQLite
- **History DB** → SQLite
- Separate `DbContext` per module
- Database-per-service principle
- No foreign keys across modules

Design-time `DbContextFactory` is used to ensure reliable migrations.

---

## 🧪 Error Handling & Reliability

- Global exception middleware using `ProblemDetails`
- Validation errors handled at API boundary
- Failed event publishing is logged
- Failed events can be persisted and replayed later (future-ready)
---

## 🔄 Concurrency & Idempotency

- Optimistic concurrency using `RowVersion`
- History is append-only → no updates
- Safe foundation for Kafka retries
---

## 📖 API Versioning

- Uses `asp.Versioning`
- URL + query-string versioning
- Single Swagger UI for usability

---

## 🧩 Why Single API & Single Swagger?

This project is intentionally designed to be:
- Easy to run
- Easy to review
- Easy to understand

In a real production environment:
- Books and History could be separate services
- An API Gateway would front them

For this task, internal boundaries demonstrate microservice principles **without operational overhead**.

---

## 🚀 How to Run

```bash
dotnet restore
dotnet build
dotnet run


# TC Cloud Games - Common Shared Libraries

A collection of foundational shared libraries for the TC Cloud Games microservices platform, providing contracts, messaging infrastructure, and common functionality across all services.

## 🏗️ Architecture Overview

The Common libraries implement a standardized foundation for microservices communication, domain modeling, and shared infrastructure patterns across the TC Cloud Games platform.

```
common/
├── 📁 src/
│   ├── 🔌 TC.CloudGames.Contracts/
│   │   └── Events/                       # Integration event contracts
│   │       ├── BaseIntegrationEvent.cs   # Base integration event structure
│   │       ├── Games/                    # Game-related events
│   │       ├── Payments/                 # Payment-related events
│   │       └── Users/                    # User-related events
│   ├── 📨 TC.CloudGames.Messaging/
│   │   └── Extensions/                   # Messaging infrastructure utilities
│   │       ├── GameEventsRegistrationExtensions.cs
│   │       ├── MessageNameHelper.cs     # Message naming conventions
│   │       ├── PaymentEventsRegistrationExtensions.cs
│   │       └── UserEventsRegistrationExtensions.cs
│   └── 🎯 TC.CloudGames.SharedKernel/
│       ├── Api/                          # Common API patterns
│       ├── Application/                  # CQRS and application patterns
│       ├── Domain/                       # Domain-driven design building blocks
│       ├── Extensions/                   # Common extensions
│       └── Infrastructure/               # Infrastructure abstractions
           ├── Authentication/           # JWT and token management
           ├── Caching/                  # Redis caching abstractions
           ├── Database/                 # PostgreSQL helpers
           ├── MessageBroker/            # Service Bus & RabbitMQ
           ├── Messaging/                # Event envelope patterns
           └── Middleware/               # Cross-cutting concerns
```

## 🎯 Key Components

### TC.CloudGames.Contracts
**Independent contract definitions for cross-service communication**
- **BaseIntegrationEvent**: Standardized integration event structure
- **Game Events**: Game purchase, library updates, and game lifecycle events
- **Payment Events**: Payment processing and status update contracts
- **User Events**: User registration, authentication, and profile events
- **Versioned Contracts**: Backwards-compatible event evolution

### TC.CloudGames.Messaging
**Messaging infrastructure and event registration utilities**
- **Event Registration**: Automated event handler registration for each domain
- **Message Naming**: Consistent message naming conventions across services
- **Type Safety**: Strongly-typed message handling and routing
- **Extension Methods**: Simplified service registration for messaging patterns

### TC.CloudGames.SharedKernel
**Core shared functionality and architectural patterns**

#### Domain Layer
- **BaseAggregateRoot**: Event-sourced aggregate foundation
- **BaseDomainEvent**: Domain event base implementation
- **Value Objects**: Common validation and domain primitives

#### Application Layer
- **CQRS Patterns**: Base command and query interfaces
- **Behavior Pipeline**: Logging, caching, and validation behaviors
- **Handler Abstractions**: Standardized command/query handler patterns

#### Infrastructure Layer
- **Authentication**: JWT token providers and user context
- **Caching**: Redis-based caching with health checks
- **Database**: PostgreSQL connection management and helpers
- **Message Brokers**: Azure Service Bus and RabbitMQ abstractions
- **Event Envelope**: Message envelope patterns for reliable messaging
- **Middleware**: Correlation ID, logging, and cross-cutting concerns

## 🔧 Technology Stack

### Core Framework
- **.NET 9**: Modern framework with latest C# features for all libraries

### Event Sourcing & Messaging
- **Marten**: Event sourcing patterns and PostgreSQL integration
- **Wolverine**: Message handling and CQRS support
- **Azure Service Bus**: Production-ready message broker
- **RabbitMQ**: Local development messaging

### Infrastructure Components
- **PostgreSQL**: Primary database with event store capabilities
- **Redis**: Distributed caching and session management
- **JWT Authentication**: Secure token-based authentication
- **OpenTelemetry**: Distributed tracing and observability
- **Serilog**: Structured logging with correlation

## 🏛️ Architectural Patterns

### Domain-Driven Design
- **Aggregate Patterns**: Event-sourced aggregate roots with domain events
- **Value Objects**: Immutable domain primitives with validation
- **Domain Events**: Rich event model for business logic communication

### CQRS & Event Sourcing
- **Command/Query Separation**: Clear separation of write and read operations
- **Event Store**: Marten-based event persistence for audit trails
- **Projections**: Optimized read models for query scenarios

### Message-Driven Architecture
- **Integration Events**: Standardized cross-service communication contracts
- **Event Envelope**: Reliable message delivery with routing and metadata
- **Outbox Pattern**: Transactional messaging consistency
- **Dead Letter Handling**: Error recovery and message retry mechanisms

## 📦 Usage Examples

### Integration Event Contract
```csharp
public record GamePurchasedIntegrationEvent(
    Guid EventId,
    Guid AggregateId,
    DateTimeOffset OccurredOn,
    string EventName,
    Guid UserId,
    Guid GameId,
    decimal Amount,
    string GameName
) : BaseIntegrationEvent(EventId, AggregateId, OccurredOn, EventName);
```

### Domain Aggregate
```csharp
public class GameAggregate : BaseAggregateRoot
{
    public string Title { get; private set; }
    public decimal Price { get; private set; }
    
    public void Purchase(Guid userId, decimal amount)
    {
        // Business logic
        AddNewEvent(new GamePurchasedDomainEvent(Id, userId, amount));
    }
}
```

### Message Registration
```csharp
// Startup registration
services.AddGameEventHandlers()
        .AddPaymentEventHandlers()
        .AddUserEventHandlers();
```

## 🚀 Getting Started

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/) 
- [Redis](https://redis.io/) (for caching)

### Installation

1. **Add package references to your microservice**
```xml
<PackageReference Include="TC.CloudGames.Contracts" Version="1.0.0" />
<PackageReference Include="TC.CloudGames.Messaging" Version="1.0.0" />
<PackageReference Include="TC.CloudGames.SharedKernel" Version="1.0.0" />
```

2. **Configure services in your microservice**
```csharp
// Program.cs
builder.Services.AddSharedKernel(builder.Configuration)
                .AddMessaging()
                .AddContracts();
```

3. **Use in your domain models**
```csharp
public class YourAggregate : BaseAggregateRoot
{
    // Your domain logic here
}
```

## 🔄 Cross-Service Communication

### Event Flow Pattern
1. **Domain Event**: Business logic triggers domain events within aggregates
2. **Integration Event**: Domain events are published as integration events
3. **Event Envelope**: Messages are wrapped with routing and metadata
4. **Cross-Service**: Other microservices consume integration events
5. **Business Reaction**: Consuming services react with their own domain logic

## 🧪 Testing

### Unit Tests
```bash
# Run all shared library tests
dotnet test src/TC.CloudGames.SharedKernel.Tests/
dotnet test src/TC.CloudGames.Contracts.Tests/
dotnet test src/TC.CloudGames.Messaging.Tests/
```

## 📊 Monitoring & Observability

### Built-in Features
- **Health Checks**: Database, Redis, and message broker connectivity
- **Correlation IDs**: Request tracing across service boundaries
- **Structured Logging**: Consistent log formatting with Serilog
- **Metrics**: Custom metrics for domain events and message processing
- **Distributed Tracing**: OpenTelemetry integration for cross-service calls

## 🚀 Deployment

### NuGet Packages
The common libraries are distributed as NuGet packages for easy consumption across microservices.

### Versioning Strategy
- **Semantic Versioning**: Breaking changes increment major version
- **Backward Compatibility**: Integration events maintain compatibility
- **Migration Guides**: Documentation for version upgrades

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 📞 Support

For questions or issues:
- Open an [issue](https://github.com/rdpresser/tc-cloudgames-common/issues)
- Check the [documentation](./docs/)
- Review architectural patterns and examples

---

**TC Cloud Games Common** - Foundational libraries for building scalable, event-driven microservices in the cloud gaming platform.
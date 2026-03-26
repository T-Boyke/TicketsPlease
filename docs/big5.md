# TicketsPlease: The Big 5 Architecture & Model

Dieses Dokument bietet eine visuelle Übersicht über die Architektur, das Domänenmodell und die Kerninteraktionen von **TicketsPlease**.

---

## 1. System Architecture (DDD & MVC)
Die Anwendung folgt einem klassischen **Layered Architecture**-Ansatz mit strikter Trennung nach **Domain-Driven Design (DDD)**-Prinzipien.

```mermaid
graph TD
    subgraph Web_Layer ["🚀 Web Layer (ASP.NET Core MVC)"]
        Controllers["Controllers (Internal)"]
        Views["MVC Views / Razor Pages"]
        API["REST API Endpoints"]
    end

    subgraph Application_Layer ["⚙️ Application Layer"]
        Services["Application Services"]
        DTOs["Data Transfer Objects"]
        Commands["Command/Query Handlers"]
    end

    subgraph Domain_Layer ["💎 Domain Layer (Core)"]
        Entities["Domain Entities"]
        ValueObjects["Value Objects"]
        DomainEvents["Domain Events"]
        Interfaces["Repository Interfaces"]
    end

    subgraph Infrastructure_Layer ["🏦 Infrastructure Layer"]
        EFCore["EF Core Persistence"]
        Repos["Repository Implementations"]
        Auth["Identity & Authorization"]
    end

    Web_Layer --> Application_Layer
    Application_Layer --> Domain_Layer
    Infrastructure_Layer --> Domain_Layer
    Application_Layer --> Infrastructure_Layer
```

---

## 2. Domain Class Diagram
Das Herzstück des Systems: Die Beziehungen zwischen den wichtigsten Domänen-Entitäten.

```mermaid
classDiagram
    class BaseEntity {
        +Guid Id
        +byte[] RowVersion
    }
    
    class Ticket {
        +String Title
        +String Status
        +TicketType Type
        +Guid ProjectId
        +CanBeClosed() bool
        +MoveToState()
    }
    
    class Project {
        +String Title
        +DateTime StartDate
        +bool IsOpen
        +Open()
        +Close()
    }
    
    class User {
        +String FullName
        +String Email
        +String InitialLoginCode
    }
    
    class Comment {
        +String Content
        +DateTime CreatedAt
    }
    
    class Organization {
        +String Name
        +bool IsActive
    }

    BaseEntity <|-- Ticket
    BaseEntity <|-- Project
    BaseEntity <|-- User
    BaseEntity <|-- Organization

    Project "1" -- "*" Ticket : contains
    Ticket "1" -- "*" Comment : referenced by
    User "1" -- "*" Ticket : creator / assignee
    User "*" -- "1" Organization : belongs to
    Organization "1" -- "*" Project : owns
```

---

## 3. Entity Relationship Diagram (ERD)
Fokus auf die Persistenzstrategie und Fremdschlüsselbeziehungen.

```mermaid
erDiagram
    ORGANIZATION ||--o{ PROJECT : owns
    PROJECT ||--o{ TICKET : contains
    TICKET ||--o{ TICKET_TAG : categorizes
    TICKET ||--o{ COMMENT : has
    USER ||--o{ TICKET : creates
    USER ||--o{ COMMENT : writes
    USER }o--|| ORGANIZATION : member_of
    
    TICKET {
        guid id PK
        string title
        string status
        guid project_id FK
        guid creator_id FK
    }
    
    PROJECT {
        guid id PK
        string title
        datetime start_date
        bool is_open
    }
    
    USER {
        guid id PK
        string email
        string full_name
    }
```

---

## 4. Use Case Diagram
Die Rollen und ihre primären Interaktionen mit dem System.

```mermaid
usecaseDiagram
    actor "Admin" as A
    actor "Project Manager" as PM
    actor "Developer" as D
    actor "Stakeholder" as S

    package "TicketsPlease System" {
        usecase "Create Project" as UC1
        usecase "Manage Teams" as UC2
        usecase "Create/Edit Ticket" as UC3
        usecase "Change Ticket State" as UC4
        usecase "Add Comments" as UC5
        usecase "View Dashboard" as UC6
    }

    A --> UC1
    A --> UC2
    PM --> UC1
    PM --> UC3
    PM --> UC4
    D --> UC3
    D --> UC4
    D --> UC5
    S --> UC6
```

---

## 5. Sequence Diagram: Ticket Creation Flow
Demonstration des Workflows über alle Layer hinweg.

```mermaid
sequenceDiagram
    participant U as User
    participant C as TicketsController
    participant S as TicketService
    participant R as TicketRepository
    participant DB as SQL Database

    U->>C: POST Create(CreateTicketDto)
    activate C
    C->>S: CreateTicketAsync(dto)
    activate S
    S->>S: Map DTO to Entity
    S->>R: AddAsync(newTicket)
    activate R
    R->>DB: INSERT INTO Tickets
    DB-->>R: Success
    R-->>S: TicketEntity
    deactivate R
    S-->>C: Result<TicketDto>
    deactivate S
    C-->>U: RedirectToActionResult (Details)
    deactivate C
```

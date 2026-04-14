# TicketsPlease: The Big 5 Architecture & Model

Dieses Dokument bietet eine visuelle Übersicht über die Architektur, das Domänenmodell und die
Kerninteraktionen von **TicketsPlease**.

---

## 1. System Architecture (DDD & MVC)

Die Anwendung folgt einem klassischen **Layered Architecture**-Ansatz mit strikter Trennung nach
**Domain-Driven Design (DDD)**-Prinzipien.

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
graph LR
    subgraph Actors ["👥 Rollen"]
        Admin((Admin))
        PM((Project Manager))
        Dev((Developer))
        Stake((Stakeholder))
    end

    subgraph Actions ["🎯 Use Cases"]
        UC1([Projekt erstellen])
        UC2([Teams verwalten])
        UC3([Ticket erstellen/bearbeiten])
        UC4([Status ändern])
        UC5([Kommentar hinzufügen])
        UC6([Dashboard einsehen])
    end

    Admin --> UC1
    Admin --> UC2
    PM --> UC1
    PM --> UC3
    PM --> UC4
    Dev --> UC3
    Dev --> UC4
    Dev --> UC5
    Stake --> UC6
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

---

## 6. Test Strategy & Coverage

Die Qualitätssicherung erfolgt über eine mehrstufige Testpyramide mit dem Ziel der vollständigen
Abdeckung der Geschäftslogik.

```mermaid
graph LR
    subgraph Test_Pyramid ["🧪 Testing Layers"]
        Unit["Unit Tests (100% Domain)"]
        Integration["Integration Tests (EF Core/API)"]
        Architecture["Architecture Tests (StyleCop/DDD)"]
        E2E["E2E Tests (Playwright)"]
    end

    subgraph Tools ["🛠️ Tools"]
        xUnit["xUnit"]
        Fluent["FluentAssertions"]
        Coverlet["Coverlet / Cobertura"]
        Sonar["SonarCloud"]
    end

    Unit --> xUnit
    Integration --> xUnit
    Architecture --> xUnit
    E2E --> xUnit

    Unit --> Coverlet
    Coverlet --> Sonar
```

---

## 7. Ticket Lifecycle (State Diagram)

Der Lebenszyklus eines Tickets von der Erstellung bis zur endgültigen Schließung.

```mermaid
stateDiagram-v2
    [*] --> New: Created
    New --> Assigned: User assigned
    Assigned --> InProgress: Work started
    InProgress --> Review: PR/Verification
    Review --> InProgress: Changes requested
    Review --> Resolved: Verified
    Resolved --> Closed: Archived

    InProgress --> Blocked: Impediment found
    Blocked --> InProgress: Resolved

    New --> Closed: Rejected/Duplicate
    Assigned --> Closed: Cancelled
```

---

## 8. Deployment Architecture

Physische Verteilung der Komponenten in einer Standard-Umgebung.

```mermaid
graph LR
    subgraph Client ["🌐 Client Side"]
        Browser["Web Browser (Edge/Chrome)"]
    end

    subgraph Server ["💻 Application Server (IIS/Kestrel)"]
        Web["ASP.NET Core Web App"]
        DP["Data Protection API"]
    end

    subgraph Data ["🗄️ Persistence Layer"]
        SQL["SQL Server DB"]
        Redis["Optional: Redis Cache"]
    end

    Browser -- HTTPS/TLS 1.2+ --> Web
    Web -- EF Core / SQL Client --> SQL
    Web -- Keys --> DP
```

---

## 9. Component Dependency Diagram

Striktes DDD-Abhängigkeitsmodell (Onion Architecture Approach).

```mermaid
graph BT
    subgraph Layers ["🏗️ Project Dependencies"]
        Domain["TicketsPlease.Domain"]
        App["TicketsPlease.Application"]
        Infra["TicketsPlease.Infrastructure"]
        Web["TicketsPlease.Web"]
    end

    App --> Domain
    Infra --> Domain
    Infra -.-> App
    Web --> App
    Web --> Infra
```

---

## 10. Security & Data Flow (DFD)

Darstellung der Vertrauensgrenzen und des Datenflusses.

```mermaid
graph TD
    subgraph User_Zone ["External Zone (Untrusted)"]
        User((User))
        PublicUI["Public Login Pages"]
    end

    subgraph App_Zone ["Web Application Zone"]
        Auth["ASP.NET Identity / Auth"]
        Controllers["Internal Controllers"]
        CSP["CSP Headers / HSTS"]
    end

    subgraph Data_Zone ["Data Storage Zone (Protected)"]
        DB[(SQL Server)]
        Config["appsettings.json / Environment"]
    end

    User -- HTTPS --> PublicUI
    PublicUI -- Login Credentials --> Auth
    Auth -- JWT/Cookie --> Controllers
    Controllers -- EF Core --> DB
    Config -- ConnectionString --> DB
    CSP -.-> User
```

---

## 11. Business Workflow: Organization Setup

Prozessablauf bei der Einrichtung einer neuen Organisation im System.

```mermaid
flowchart TD
    Start([Start]) --> CreateOrg[Organization erstellen]
    CreateOrg --> AddProjects[Projekte definieren]
    AddProjects --> InviteUsers[Nutzer einladen]
    InviteUsers --> AssignRoles[Rollen zuweisen: Admin/Dev/PM]
    AssignRoles --> CreateWorkflow[Workflow-Stati festlegen]
    CreateWorkflow --> Ready[System bereit für Tickets]
    Ready --> End([Ende])
```

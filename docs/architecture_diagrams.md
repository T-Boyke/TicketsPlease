# System Architecture & Infrastructure

Dieses Dokument visualisiert unsere High-Level Architektur (Clean Architecture), das Deployment-Modell und den grundlegenden Datenfluss der Applikation.

## 1. Clean Architecture (Onion) Diagramm

Das folgende Diagramm zeigt die strikte Abhängigkeitsrichtung (Dependency Rule) unserer ASP.NET Core Solution. Abhängigkeiten dürfen **immer nur nach innen** (in Richtung der Domain) zeigen.

```mermaid
architecture-beta
    group api(cloud)[Presentation / UI Layer]
    group app(cloud)[Application Layer]
    group infra(cloud)[Infrastructure Layer]
    group domain(cloud)[Domain Layer (Core)]

    service mvc(internet)[ASP.NET Core MVC & API] in api
    service cqrs(server)[CQRS Handlers & Services] in app
    service efcore(database)[EF Core Repositories] in infra
    service signalr(server)[SignalR Hubs] in infra
    
    service entities(server)[Entities, VOs, Interfaces] in domain

    mvc:B --> T:cqrs
    cqrs:B --> T:entities
    
    efcore:T --> B:cqrs
    efcore:R --> L:entities
    
    signalr:L --> R:cqrs
```

## 2. Infrastructure & Deployment Architektur

Dieses UML Deployment-Diagramm veranschaulicht, wie die fertige Applikation in einer Produktionsumgebung (z.B. Azure oder ein lokaler IIS/Docker Swarm) verteilt wird.

```mermaid
flowchart TD
    %% Users
    Client[Web Browser Chrome/Edge/Safari]
    
    %% Load Balancer / Proxy
    subgraph DMZ [Reverse Proxy / WAF]
        Nginx[Nginx / YARP Load Balancer]
    end

    %% Web Application
    subgraph WebTier [ASP.NET Core 10.3 App Service]
        App1[TicketsPlease Node 1]
        App2[TicketsPlease Node 2]
    end

    %% Storage & Persistence
    subgraph DataTier [Persistence / Data Layer]
        SQL[(MS SQL Server / Azure SQL)]
        BlobStorage{{Blob Storage / S3 Für FileAssets}}
        Redis[(Redis Cache für SignalR Backplane)]
    end
    
    %% Connections
    Client == HTTPS / WebSockets ==> Nginx
    Nginx --> App1
    Nginx --> App2
    
    App1 -- EF Core (TCP) --> SQL
    App2 -- EF Core (TCP) --> SQL
    
    App1 -- Uploads --> BlobStorage
    App2 -- Uploads --> BlobStorage
    
    App1 -. Pub/Sub .-> Redis
    App2 -. Pub/Sub .-> Redis
```

## 3. CQRS & Event Flow (Ticket Creation)

Ein Sequenzdiagramm, das den typischen Fluss eines Commands (z.B. "Erstelle ein neues Ticket") durch unsere Clean Architecture Routen zeigt.

```mermaid
sequenceDiagram
    autonumber
    actor User
    participant Controller as TicketController (UI)
    participant Mediator as MediatR (App Layer)
    participant Handler as CreateTicketCommandHandler
    participant Domain as Ticket Entity
    participant Repo as ITicketRepository (Infra)
    participant DB as SQL Database

    User->>Controller: POST /Tickets/Create (Title, Desc)
    Controller->>Mediator: Send(CreateTicketCommand)
    Mediator->>Handler: Handle(CreateTicketCommand)
    
    note over Handler,Domain: Business Logic Execution
    Handler->>Domain: new Ticket(Title, Desc)
    Domain-->>Handler: Ticket Instance
    
    note over Handler,Repo: Persistence
    Handler->>Repo: AddAsync(ticket)
    Repo->>DB: INSERT INTO Tickets ...
    DB-->>Repo: 1 Row affected
    Repo-->>Handler: Task Completed
    
    Handler->>Mediator: Result (TicketId)
    Mediator->>Controller: Result (TicketId)
    Controller-->>User: HTTP 201 Created (Redirect to Board)
```

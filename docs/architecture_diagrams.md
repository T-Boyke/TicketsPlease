# System Architecture & Infrastructure

Dieses Dokument visualisiert unsere High-Level Architektur (Clean Architecture), das
Deployment-Modell und den grundlegenden Datenfluss der Applikation.

## 1. Clean Architecture (Onion) Diagramm

Das folgende Diagramm zeigt die strikte Abhängigkeitsrichtung (Dependency Rule) unserer ASP.NET Core
Solution. Abhängigkeiten dürfen **immer nur nach innen** (in Richtung der Domain) zeigen.

```mermaid
flowchart TD
    subgraph Presentation ["1. Presentation / UI Layer"]
        MVC["ASP.NET Core MVC & API"]
    end

    subgraph Application ["2. Application Layer"]
        CQRS["CQRS Handlers & Services"]
        Interfaces["Application Interfaces"]
    end

    subgraph Infrastructure ["3. Infrastructure Layer"]
        Repo["EF Core Repositories"]
        SignalR["SignalR Hubs"]
    end

    subgraph Domain ["4. Domain Layer (Core)"]
        Entities["Entities & Value Objects"]
        DomainEvents["Domain Events"]
    end

    %% Dependency Rule: Arrows point INWARDS (towards Domain)
    MVC -->|"Uses"| Application
    Infrastructure -->|"Implements"| Application
    Application -->|"Uses"| Domain
```

## 2. Infrastructure & Deployment Architektur

Dieses UML Deployment-Diagramm veranschaulicht, wie die fertige Applikation in einer
Produktionsumgebung (z.B. Azure oder ein lokaler IIS/Docker Swarm) verteilt wird.

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

Ein Sequenzdiagramm, das den typischen Fluss eines Commands (z.B. "Erstelle ein neues Ticket") durch
unsere Clean Architecture Routen zeigt.

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

## 4. Enterprise Plugin Loader Flow (Runtime Extensibility)

Dieses Diagramm zeigt, wie das System zur Laufzeit externe Module (.dll) lädt, ohne dass der
Kern-Code neu kompiliert werden muss.

```mermaid
sequenceDiagram
    autonumber
    participant Host as TicketsPlease.Web (Host)
    participant Folder as /plugins/ Verzeichnis
    participant Loader as AssemblyLoadContext
    participant DI as ServiceCollection (DI Container)
    participant Core as System Core Features

    Host->>Folder: Scanne Verzeichnis nach *.dll
    Folder-->>Host: Liste der Plugin-Assemblies

    loop Pro Plugin
        Host->>Loader: Lade Assembly in isolierten Kontext
        Loader-->>Host: Assembly geladen
        Host->>Host: Suche Klassen mit ITicketsPleasePlugin
        Host->>DI: Registriere Plugin-Services & UI-Hooks
    end

    Core->>DI: Fordere alle IPlugin-Instanzen an
    DI-->>Core: Liste der aktiven Plugins
    Core->>Core: Integriere Plugin-Features in UI & Logik
```

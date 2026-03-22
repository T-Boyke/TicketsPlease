# 📂 TicketsPlease Source Directory

Willkommen im Herzstück der **TicketsPlease** Solution. Dieses Verzeichnis folgt
strikt den Prinzipien der **Clean Architecture** (Onion Architecture) und des
**Domain-Driven Design (DDD)**.

## 🏗️ Finale Vision: Das integrierte System

Dieser Graph zeigt, wie alle Teile der Solution ineinandergreifen, um einen
Request zu verarbeiten. Von der ersten Interaktion bis zur dauerhaften
Speicherung.

```mermaid
graph TD
    %% Frontend/Entry
    subgraph "Presentation Layer (Web)"
        VC["Views / Components"]
        CTRL["MVC Controllers"]
    end

    %% Application/Process
    subgraph "Application Layer"
        CMD["Commands / Queries"]
        VAL["Validators"]
        HAND["Handlers"]
        DTO["DTOs / Mappings"]
    end

    %% Business/Core
    subgraph "Domain Layer"
        ENT["Entities (Rich Models)"]
        VO["Value Objects"]
        EVT["Domain Events"]
    end

    %% Infrastructure/External
    subgraph "Infrastructure Layer"
        DBA["AppDbContext"]
        REPO["Repositories"]
        EXT["External Services"]
    end

    %% Logic Flow
    VC -->|User Action| CTRL
    CTRL -->|Dispatch| CMD
    CMD -->|Validate| VAL
    VAL -->|Execute| HAND
    HAND -->|Query/Update| REPO
    REPO -->|EF Core Mapping| DBA
    DBA <-->|SQL Server| SQL[(Database)]
    HAND -->|State Change| ENT
    ENT -->|Trigger| EVT
    HAND -->|Return| DTO
    DTO -->|Render| VC
    
    %% Real-Time Flow
    HAND -.->|Push Update| SIG["SignalR Hub"]
    SIG -.->|Real-Time Toast| VC

    %% Styling
    style ENT fill:#2ecc71,stroke:#27ae60,color:#fff
    style CMD fill:#f1c40f,stroke:#f39c12,color:#fff
    style REPO fill:#e74c3c,stroke:#c0392b,color:#fff
    style VC fill:#3498db,stroke:#2980b9,color:#fff
    style SIG fill:#9b59b6,stroke:#8e44ad,color:#fff
```

---

## 🏗️ UI Architecture (Razor Partials)

Wir setzen massiv auf **Reusable Razor Partials**, um Design-Konsistenz und
schnelle Iterationszyklen zu garantieren. Diese befinden sich in
`src/TicketsPlease.Web/Views/Shared/`:

- **`_Avatar.cshtml`**: Standardisiertes User-Avatar (inkl. Tooltip & Online-Status).
- **`_StatusBadge.cshtml`**: Zugängliche Badges für Ticket-Zustände.
- **`_PriorityIcon.cshtml`**: Die **"Chillischoten"-Metrik 🌶️** als SVG-Partial.
- **`_KanbanCard.cshtml`**: Die zentrale Ticket-Repräsentation für das Board.
- **`_Notification.cshtml`**: Globales Toast-System mit Spring-In Animationen.

---

## ⚡ Real-Time & Interaction

- **SignalR integration**: Das System nutzt WebSockets, um Updates (neue Tickets,
  Statusänderungen) sofort in die UI zu pushen, ohne einen Page-Refresh.
- **Presence Tracking**: Echtzeit-Visualisierung der Nutzeraktivität via Hubs.

---

## 📊 Layer Metrics (Einstiegshilfe)

Hier siehst du, welcher Layer welche Herausforderungen birgt. Nutze dies als
Orientierung, wo du dich als Neuling am besten zuerst einarbeitest.

| Layer              | Schwierigkeit | Umfang    | Zeitaufwand | Typische Probleme                           |
| :----------------- | :-----------: | :-------- | :---------- | :------------------------------------------ |
| **Domain**         |     ⭐⭐      | Gering    | Hoch        | Zirkuläre Abhängigkeiten, Logik-Platzierung |
| **Application**    |    ⭐⭐⭐     | Hoch      | Mittel      | Validator-Logik vs. Handlers vs. Entities   |
| **Infrastructure** |   ⭐⭐⭐⭐    | Mittel    | Mittel      | EF Migrations, Concurrency, SQL-Performance |
| **Web**            |    ⭐⭐⭐     | Sehr Hoch | Hoch        | Tailwind-Ketten, JS-Security, View-Logik    |

---

## 🏗️ Layer & Zuständigkeiten

Hier siehst du auf einen Blick, welcher Layer für welche Aufgabe zuständig ist.
Klicke auf den Namen für die detaillierte Anleitung.

| Layer                                                        | Farbe | Kurzbeschreibung                           |                     Dokumentation                      |
| :----------------------------------------------------------- | :---: | :----------------------------------------- | :----------------------------------------------------: |
| [**Domain**](TicketsPlease.Domain/README.md)                 |  🟢   | Enterprise Logic (Entities, Value Objects) |     [Anleitung 📖](TicketsPlease.Domain/README.md)     |
| [**Application**](TicketsPlease.Application/README.md)       |  🟡   | Use Case Logic (CQRS, DTOs, Handlers)      |  [Anleitung 📖](TicketsPlease.Application/README.md)   |
| [**Infrastructure**](TicketsPlease.Infrastructure/README.md) |  🔴   | Technical Logic (DB, Email, Storage)       | [Anleitung 📖](TicketsPlease.Infrastructure/README.md) |
| [**Web**](TicketsPlease.Web/README.md)                       |  🔵   | Presentation Logic (UI, Controller, API)   |      [Anleitung 📖](TicketsPlease.Web/README.md)       |

---

## 📍 Startpunkte: "Ich möchte..."

Finde hier den direkten Einstiegspunkt für deine aktuelle Aufgabe:

| ...eine neue Eigenschaft hinzufügen                        | ...eine neue Geschäftsregel                                | ...etwas Speichern / Laden                                           |
| :--------------------------------------------------------- | :--------------------------------------------------------- | :------------------------------------------------------------------- |
| Gehe zu [Domain/Entities/](TicketsPlease.Domain/README.md) | Gehe zu [Domain/Entities/](TicketsPlease.Domain/README.md) | Gehe zu [Application/Features/](TicketsPlease.Application/README.md) |
| Füge Property mit `private set` hinzu                      | Implementiere eine Methode in der Entity                   | Erstelle Command/Query & Handler                                     |
| **Nächster Schritt:** EF Migration                         | **Nächster Schritt:** Unit Test                            | **Nächster Schritt:** Repository                                     |

---

👉 **Quick Links:** [Domain 🟢](TicketsPlease.Domain/README.md) |
[Application 🟡](TicketsPlease.Application/README.md)
[Infrastructure 🔴](TicketsPlease.Infrastructure/README.md) |
[Web 🔵](TicketsPlease.Web/README.md)

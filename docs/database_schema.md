## 🗄️ Datenbankschema & Entities (ERD)

Die Datenbankstruktur (Entity Framework Core - Code First) ist streng relational, befindet sich in der **3. Normalform (3NF)** und ist zukunftssicher (Enterprise-Grade) für hohe Datenmengen und schnelle Abfragen ausgelegt.

### Entity Relationship Diagram (3NF Enterprise Schema)

```mermaid
erDiagram
    %% --- Identity & Access Management (IAM) ---
    ROLE {
        Guid Id PK
        string Name "Owner, Admin, Mod, Teamlead, User"
        string Description
    }

    USER {
        Guid Id PK
        Guid RoleId FK
        string Username
        string Email
        string PasswordHash
        datetime CreatedAt
        datetime LastLoginAt
        boolean IsOnline "Live Status"
    }

    USER_PROFILE {
        Guid UserId PK, FK
        string FirstName
        string LastName
        string PhoneNumber
        Guid AvatarImageId FK "Nullable"
        datetime UpdatedAt
    }

    USER_ADDRESS {
        Guid UserId PK, FK
        string Street
        string City
        string ZipCode
        string Country
    }

    %% --- Assets / Files ---
    FILE_ASSET {
        Guid Id PK
        string FileName
        string ContentType
        string BlobPath "Azure/S3/Local Path"
        long SizeBytes
        datetime UploadedAt
        Guid UploadedByUserId FK
    }

    %% --- Teams & Workspaces ---
    TEAM {
        Guid Id PK
        string Name
        string Description
        string ColorCode
        datetime CreatedAt
        Guid CreatedByUserId FK
    }

    TEAM_MEMBER {
        Guid TeamId PK, FK
        Guid UserId PK, FK
        datetime JoinedAt
        boolean IsTeamLead
    }

    %% --- Ticket Core Domain ---
    TICKET {
        Guid Id PK
        string Title
        string DescriptionMarkdown
        Guid PriorityId FK
        int ChilliesDifficulty "1-5 🌶️"
        datetime StartDate
        datetime Deadline
        Guid WorkflowStateId FK
        Guid CreatorId FK
        datetime CreatedAt
        datetime UpdatedAt
    }

    TICKET_ASSIGNMENT {
        Guid TicketId PK, FK
        Guid UserId FK "Nullable"
        Guid TeamId FK "Nullable"
        datetime AssignedAt
    }

    TICKET_PRIORITY {
        Guid Id PK
        string Name "Low, Medium, High, Blocker"
        int LevelWeight
        string ColorHex
    }

    SUBTICKET {
        Guid Id PK
        Guid ParentTicketId FK
        string Title
        boolean IsCompleted
        datetime CreatedAt
        Guid CreatorId FK
    }

    TICKET_UPVOTE {
        Guid TicketId PK, FK
        Guid UserId PK, FK
        datetime VotedAt
    }

    %% --- Workflow / Kanban ---
    WORKFLOW_STATE {
        Guid Id PK
        string Name "Todo, InProgress, Review, Done"
        int OrderIndex
        string ColorHex
        boolean IsTerminalState
    }

    %% --- Communication / Messaging ---
    MESSAGE {
        Guid Id PK
        Guid SenderUserId FK
        Guid TicketId FK "Nullable (If ticket comment)"
        Guid TeamId FK "Nullable (If Team broadcast)"
        Guid ReceiverUserId FK "Nullable (If 1-on-1 DM)"
        string BodyMarkdown
        datetime SentAt
        boolean IsEdited
    }

    MESSAGE_READ_RECEIPT {
        Guid MessageId PK, FK
        Guid UserId PK, FK
        datetime ReadAt
    }

    %% --- Relationships ---
    %% IAM
    ROLE ||--o{ USER : has
    USER ||--|| USER_PROFILE : owns
    USER ||--|| USER_ADDRESS : owns
    
    %% Assets
    FILE_ASSET |o--|| USER_PROFILE : avatar_for
    USER ||--o{ FILE_ASSET : uploads

    %% Teams
    USER ||--o{ TEAM_MEMBER : joins
    TEAM ||--o{ TEAM_MEMBER : contains
    USER ||--o{ TEAM : creates

    %% Tickets & Workflow
    USER ||--o{ TICKET : creates
    WORKFLOW_STATE ||--o{ TICKET : groups
    TICKET_PRIORITY ||--o{ TICKET : categorizes
    TICKET ||--o{ SUBTICKET : contains
    
    %% Assignments (3NF resolution)
    TICKET ||--o{ TICKET_ASSIGNMENT : has
    USER |o--o{ TICKET_ASSIGNMENT : receives
    TEAM |o--o{ TICKET_ASSIGNMENT : receives

    %% Upvoting
    TICKET ||--o{ TICKET_UPVOTE : receives
    USER ||--o{ TICKET_UPVOTE : casts

    %% Messaging
    USER ||--o{ MESSAGE : sends
    TICKET ||--o{ MESSAGE : contains_comments
    TEAM ||--o{ MESSAGE : contains_broadcasts
    MESSAGE ||--o{ MESSAGE_READ_RECEIPT : tracked_by
```

### Detaillierte Entity Beschreibung (3NF & Enterprise Design)

#### 1. Identity & Profile Context (Strikte 3NF)
Um die 3. Normalform (3NF) zu gewährleisten und das System maximal flexibel zu halten (sowie DSGVO-Löschkonzepte zu vereinfachen), wurde die gigantische `USER`-Tabelle aufgespalten:
*   **User:** Enthält *ausschließlich* Kern-Authentifizierungsdaten (Ids, Hashes, Logins) sowie einen `IsOnline` Indikator für systemweite Presence-Features.
*   **UserProfile:** Eine 1:1 Erweiterung, welche die persönlichen (nicht-Login-relevanten) Daten hält. Inklusive Referenz auf einen `FILE_ASSET` Datensatz für Profilbilder.
*   **UserAddress:** Eine eigene 1:1 Tabelle, um Kontaktdaten sauber zu trennen (hilft immens beim DSGVO-Export oder Löschen spezifischer Adressdaten).
*   **Role:** Echte 1:n Rechteverwaltung für das erweiterte RBAC (Owner, Admin, Mod, Teamlead, User).

#### 2. Media & Asset Management
*   **FileAsset:** Eine zentrale Tabelle für alle unstrukturierten Dateien im System. Egal ob Profilbilder (Avatare), Ticket-Anhänge oder in Markdown-Chats eingebettete Bilder – alles verweist auf diesen Blob-Storage-Proxy.

#### 3. Team Collaboration Context
*   **Team:** Metadaten des Teams.
*   **TeamMember:** Die n:m Auflösungstabelle. *Enterprise Feature:* Enthält nun das Flag `IsTeamLead`, um Teamleiter-Rechte direkt an die Knotenpunkte zu heften (wichtig für Broadcast-Nachrichten).

#### 4. Ticket Management Context
*   **Ticket:** Das Kern-Aggregat. Unterstützt nun ausdrücklich `DescriptionMarkdown`.
*   **TicketPriority:** Prioritäten wurden aus dem Enum-Status in eine eigene Entität ausgelagert (3NF), um Level und Farben dynamisch durch Admins definierbar zu machen.
*   **TicketAssignment:** Eine eigene Tabelle (statt statischen FKs im Ticket-Table). Dies ermöglicht es, Historien zu pflegen ("Wer hatte das Ticket vorher?") und es simultan an User *und* Teams zu hängen.
*   **TicketUpvote:** Community-Voting-System. Eine klassische n:m Tabelle, die regelt, dass ein User pro Ticket maximal einmal abstimmen (upvoten) darf.

#### 5. Communication & Messaging Engine (Neu 🚀)
Ein völlig neues Bounded Context für die interne Enterprise-Kommunikation.
*   **Message:** Ein polymorphes Nachrichten-Objekt. Es versteht volles Markdown (und damit Mermaid-Diagramme). Je nachdem, welche Foreign-Keys gesetzt sind, agiert die Entität als:
    1.  Ticket-Kommentar (`TicketId` != Null)
    2.  Direct Message (DM) an Kollegen (`ReceiverUserId` != Null)
    3.  Team-Broadcast durch Teamleads (`TeamId` != Null).
*   **MessageReadReceipt:** Echte n:m "Gelesen"-Indikatoren, damit Absender (wie bei WhatsApp) sehen, wer die Nachricht bereits konsumiert hat.

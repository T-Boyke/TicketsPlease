---
description: Workflow for creating DDD Domain Entities, Value Objects, and Domain Events in the TicketsPlease project.
---

# 🧬 Domain Entity Workflow (DDD Rich Models)

Dieser Workflow beschreibt den vollständigen Ablauf zur Erstellung einer neuen
Domain-Entität nach den Prinzipien des Domain-Driven Design (DDD).

> **Referenz:** [domain_ticket.md]
> (file:///d:/DEV/Tickets/docs/domain_ticket.md) | [database_schema.md]
> (file:///d:/DEV/Tickets/docs/database_schema.md) |
> [instructions.md §3](file:///d:/DEV/Tickets/instructions.md)

---

## Fundamentale Regeln

| Regel | Beschreibung |
| :--- | :--- |
| **Zero Dependencies** | `Domain` hat keine NuGet-Referenzen (außer `MediatR`). |
| **Rich Model** | Entities sind keine Datencontainer. Sie enthalten Logik. |
| **Private Setter** | Alle Properties: `{ get; private set; }`. |
| **Kein leerer Ctor** | Pflichtfelder über Fabrikmethode erzwingen. |
| **Immutable** | Collections extern als `IReadOnlyList<T>`. |

---

## Schritte

### 1. Entity erstellen

**Pfad:** `src/TicketsPlease.Domain/Entities/[EntityName].cs`

```csharp
/// <summary>
/// Repräsentiert ein [Entityname] im System.
/// Implementiert DDD Rich Model mit geschütztem internen State.
/// </summary>
public class MyEntity : BaseEntity
{
    // ═══════════════════════ Properties ═══════════════════════

    /// <summary>Der Titel des Objekts (max. 150 Zeichen).</summary>
    public string Title { get; private set; } = string.Empty;

    /// <summary>Optimistic Concurrency Token (EF Core Timestamp).</summary>
    public byte[] RowVersion { get; private set; } = [];

    /// <summary>Zeitpunkt der Erstellung (UTC).</summary>
    public DateTimeOffset CreatedAt { get; private set; }

    // ═══════════════════════ Collections ═══════════════════════

    private readonly List<SubEntity> _subEntities = [];

    /// <summary>Zugehörige Sub-Entities (nur lesend exponiert).</summary>
    public IReadOnlyList<SubEntity> SubEntities => _subEntities.AsReadOnly();

    // ═══════════════════════ Constructors ═══════════════════════

    /// <summary>EF Core benötigt diesen Konstruktor. NICHT extern verwenden!</summary>
    private MyEntity() { }

    /// <summary>
    /// Erstellt eine neue Instanz mit den Pflichtfeldern.
    /// </summary>
    /// <param name="title">Der Titel (darf nicht leer sein).</param>
    /// <returns>Eine initialisierte Entity-Instanz.</returns>
    /// <exception cref="ArgumentException">Wenn der Titel leer ist.</exception>
    public static MyEntity Create(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Titel darf nicht leer sein.", nameof(title));

        return new MyEntity
        {
            Id = Guid.NewGuid(),
            Title = title,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }

    // ═══════════════════════ Domain Logic ═══════════════════════

    /// <summary>
    /// Aktualisiert den Titel mit Validierung.
    /// </summary>
    /// <param name="newTitle">Der neue Titel.</param>
    /// <exception cref="ArgumentException">Wenn der neue Titel leer ist.</exception>
    public void UpdateTitle(string newTitle)
    {
        if (string.IsNullOrWhiteSpace(newTitle))
            throw new ArgumentException("Titel darf nicht leer sein.", nameof(newTitle));

        Title = newTitle;
    }
}
```

### 2. Value Objects erstellen (bei Bedarf)

**Pfad:** `src/TicketsPlease.Domain/ValueObjects/[ValueObjectName].cs`

Value Objects kapseln komplexe Typen und erzwingen Validity:

```csharp
/// <summary>
/// Repräsentiert eine validierte E-Mail-Adresse als Value Object.
/// Garantiert, dass die Adresse immer gültig ist.
/// </summary>
public sealed record EmailAddress
{
    /// <summary>Die validierte E-Mail-Adresse.</summary>
    public string Value { get; }

    /// <summary>
    /// Erstellt ein neues EmailAddress Value Object.
    /// </summary>
    /// <param name="value">Die E-Mail-Adresse.</param>
    /// <exception cref="ArgumentException">Wenn die Adresse ungültig ist.</exception>
    public EmailAddress(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !value.Contains('@'))
            throw new ArgumentException("Ungültige E-Mail-Adresse.", nameof(value));

        Value = value.Trim().ToLowerInvariant();
    }
}
```

**Verwendung in Entities:**

- `EmailAddress` statt `string Email`
- `PriorityLevel` statt `int Priority`
- `Sha1Hash` statt `string Hash`

### 3. Domain Events definieren (bei Bedarf)

**Pfad:** `src/TicketsPlease.Domain/Events/[EventName].cs`

Domain Events entkoppeln Seiteneffekte (z.B. Mail-Versand, Notifications):

```csharp
/// <summary>
/// Domain Event: Wird ausgelöst, wenn ein neues Ticket erstellt wurde.
/// Handler können darauf reagieren (z.B. Notification senden).
/// </summary>
public sealed record TicketCreatedEvent(Guid TicketId, string Title) : INotification;
```

**Event im Entity auslösen:**

```csharp
/// <summary>
/// Löst ein Domain Event aus, das von MediatR-Handlern verarbeitet wird.
/// </summary>
public void RaiseDomainEvent(INotification domainEvent)
{
    _domainEvents.Add(domainEvent);
}
```

### 4. Bounded Context einordnen

| Context | Entities |
| :--- | :--- |
| **Identity & Access** | User, UserProfile, UserAddress, Role |
| **Ticket Management** | Ticket, SubTicket, Tag, Priority, TimeLog, Upvote |
| **Workflow** | WorkflowState, SlaPolicy |
| **Communication** | Message, MessageReadReceipt, Notification |
| **Asset Management** | FileAsset |

→ Neue Entity in den passenden Bounded Context einordnen.

### 5. Business-Regeln als Methoden implementieren

Alle Zustandsänderungen geschehen über Methoden:

```csharp
// ✅ RICHTIG: Verhaltens-Methode mit Business-Logik
public void Close(User actor)
{
    if (actor.Id != CreatorId && !actor.IsAdmin && !actor.IsTeamLead)
        throw new DomainException("Keine Berechtigung zum Schließen.");

    Status = TicketStatus.Closed;
    RaiseDomainEvent(new TicketClosedEvent(Id));
}

// ❌ FALSCH: Direktes Property-Setzen
ticket.Status = TicketStatus.Closed;
```

### 6. RowVersion / Concurrency (Pflicht!)

- Jede Entity bekommt `byte[] RowVersion` als `[Timestamp]`.
- EF Core nutzt dies automatisch für Optimistic Concurrency.
- Handler fangen `DbUpdateConcurrencyException`.

### 7. XML-Dokumentation (Pflicht!)

- **Alle** `public` Members der Entity müssen XML-Kommentare haben.

### 8. Unit Tests (TDD!)

- Erstelle Unit-Tests für alle Domain-Methoden **vor** der Implementierung.
- Coverage-Ziel für Domain: **100%**.

---

### Zusammenfassung

---
description: Workflow for adding and applying EF Core migrations in the TicketsPlease project.
---

# 🗄️ EF Core Migration Workflow

Dieser Workflow beschreibt den vollständigen Ablauf für Datenbankänderungen über Entity Framework Core Code-First Migrations.

> **Referenz:** [ADR-0019 (EF Core Resilience)](file:///d:/DEV/Tickets/docs/adr/0019-ef-core-resilience-concurrency.md) | [database_schema.md](file:///d:/DEV/Tickets/docs/database_schema.md) | [instructions.md §5](file:///d:/DEV/Tickets/instructions.md)

---

## Schritte

### 1. Entity ändern (Domain Layer)
- Führe die gewünschten Änderungen an der Entity in `src/TicketsPlease.Domain/Entities/` durch.
- **Pflicht-Eigenschaften** für jede Entity:
  - Erbt von `BaseEntity` (enthält `Guid Id`).
  - `byte[] RowVersion` → Optimistic Concurrency via `[Timestamp]` Attribut.
  - Properties mit **`private set`** (DDD Rich Model).
- **Value Objects** für komplexe Typen (z.B. `EmailAddress`, `PriorityLevel`).
- XML-Dokumentation für alle neuen Properties.

### 2. DbContext Mapping konfigurieren (Infrastructure Layer)
- Öffne `src/TicketsPlease.Infrastructure/Persistence/AppDbContext.cs`.
- Konfiguriere das Mapping in `OnModelCreating()` über Fluent API:

```csharp
/// <summary>
/// Konfiguriert das EF Core Mapping für die Ticket-Entität.
/// </summary>
modelBuilder.Entity<Ticket>(entity =>
{
    // Pflicht: Concurrency Token
    entity.Property(e => e.RowVersion)
        .IsRowVersion();

    // Constraints
    entity.Property(e => e.Title)
        .IsRequired()
        .HasMaxLength(150);

    // Performance: Index auf häufig gesuchte Spalten
    entity.HasIndex(e => e.CreatedAt);
    entity.HasIndex(e => e.WorkflowStateId);

    // Defaults
    entity.Property(e => e.CreatedAt)
        .HasDefaultValueSql("GETUTCDATE()");
});
```

**Checkliste für Mappings:**
- [ ] Korrekte Datentypen (`HasMaxLength`, `HasPrecision`)
- [ ] `IsRowVersion()` für Concurrency
- [ ] Indizes auf Suchspalten (Performance!)
- [ ] Default-Werte (`HasDefaultValueSql`)
- [ ] Fremdschlüssel-Beziehungen (`HasOne`, `HasMany`)
- [ ] Cascade-Delete-Verhalten explizit setzen

### 3. Migration erstellen
Führe den Befehl im **Root-Verzeichnis** der Solution aus:

```cmd
dotnet ef migrations add [MigrationName] --project src/TicketsPlease.Infrastructure --startup-project src/TicketsPlease.Web
```

**Naming-Convention:** Beschreibender Name im PascalCase (z.B. `AddRowVersionToTicket`, `CreateTeamMemberTable`).

### 4. Migration überprüfen (Pflicht!)
Kontrolliere die generierte Datei in `src/TicketsPlease.Infrastructure/Migrations/`:

| Prüfpunkt | Beschreibung |
|---|---|
| **Datentypen** | Stimmen `nvarchar(150)`, `uniqueidentifier` etc. überein? |
| **Indizes** | Sind Performance-Indizes für Suchspalten erstellt? |
| **Default-Werte** | Sind `GETUTCDATE()` oder andere Defaults gesetzt? |
| **Nullable** | Sind Nullable-Felder korrekt markiert? |
| **RowVersion** | Ist `byte[]` als `rowversion` konfiguriert? |
| **Down-Migration** | Ist die `Down()`-Methode sauber (Rollback-fähig)? |

### 5. Datenbank aktualisieren
```cmd
dotnet ef database update --project src/TicketsPlease.Infrastructure --startup-project src/TicketsPlease.Web
```

### 6. Seed Data (falls benötigt)
- **Stammdaten** (Rollen, Priorities, WorkflowStates) über `HasData()` in `OnModelCreating`.
- **Testdaten** über separate Seed-Klassen oder Migration-Scripts.
- Seed Data **immer** mit festen `Guid`-Werten definieren (Reproduzierbarkeit).

```csharp
modelBuilder.Entity<TicketPriority>().HasData(
    new TicketPriority { Id = Guid.Parse("..."), Name = "Low", LevelWeight = 1, ColorHex = "#22c55e" },
    new TicketPriority { Id = Guid.Parse("..."), Name = "Medium", LevelWeight = 2, ColorHex = "#f59e0b" },
    new TicketPriority { Id = Guid.Parse("..."), Name = "High", LevelWeight = 3, ColorHex = "#ef4444" },
    new TicketPriority { Id = Guid.Parse("..."), Name = "Blocker", LevelWeight = 4, ColorHex = "#dc2626" }
);
```

### 7. Concurrency-Handling im Handler (Write-Ops)
Jeder Write-Handler **muss** die `DbUpdateConcurrencyException` fangen:

```csharp
try
{
    await _repository.UpdateAsync(entity, cancellationToken);
}
catch (DbUpdateConcurrencyException)
{
    throw new ConcurrencyException("Die Daten wurden zwischenzeitlich von einem anderen Benutzer geändert.");
}
```

### 8. Transaction-Handling (Manuelle Transaktionen)
Falls eine manuelle Transaktion nötig ist, nutze die `DefaultExecutionStrategy`:

```csharp
var strategy = _dbContext.Database.CreateExecutionStrategy();
await strategy.ExecuteAsync(async () =>
{
    await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
    // ... Operations ...
    await transaction.CommitAsync(cancellationToken);
});
```

### 9. Dokumentation aktualisieren
- Aktualisiere [database_schema.md](file:///d:/DEV/Tickets/docs/database_schema.md) falls sich die Struktur wesentlich geändert hat.
- Aktualisiere das Mermaid ERD-Diagramm.
- Falls es eine wesentliche Design-Entscheidung war → neuen ADR erstellen.

### 10. Tests anpassen
- Aktualisiere bestehende Integration-Tests (Testcontainers) für neue Spalten/Tabellen.
- Teste `RowVersion`-Handling und `AsNoTracking()`-Effekte.

---

*Checkliste: Entity ✓ → Mapping ✓ → Migration ✓ → Review ✓ → Update ✓ → Seed ✓ → Concurrency ✓ → Docs ✓ → Tests ✓*

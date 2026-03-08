---
name: ef-core-debugging
description: Debugs and optimizes EF Core queries, detects N+1 problems,
  analyzes query plans, and troubleshoots migrations. Use when facing slow
  queries, unexpected database behavior, concurrency issues, or migration
  errors in the TicketsPlease project.
---

# 🔬 EF Core Debugging & Performance

Dieses Skill hilft bei der Diagnose und Optimierung von EF Core Problemen.

> **Referenz:** [Architecture Rules §EF Core](file:///d:/DEV/Tickets/.agent/rules/architecture.md) | [/ef-core-migration](file:///d:/DEV/Tickets/.agent/workflows/ef-core-migration.md)

---

## Wann dieses Skill verwenden

- Slow Query / Performance-Probleme
- N+1 Query Detection
- Concurrency-Fehler (`DbUpdateConcurrencyException`)
- Migration-Fehler oder Schema-Konflikte
- Unerwartetes EF Core Verhalten

---

## Diagnose-Entscheidungsbaum

```text
Problem?
├── Langsame Queries
│   ├── N+1 → Projection mit .Select() verwenden
│   ├── Fehlende Indizes → Migration mit Index hinzufügen
│   └── Zu viele Spalten → .Select() statt .Include()
├── Concurrency-Fehler
│   ├── RowVersion fehlt → Entity + Config prüfen
│   ├── Nicht gefangen → try/catch DbUpdateConcurrencyException
│   └── Retry-Logik → CreateExecutionStrategy()
├── Migration-Fehler
│   ├── Pending Migrations → dotnet ef database update
│   ├── Snapshot out of sync → Snapshot regenerieren
│   └── Schema-Konflikt → Migration manuell anpassen
└── Tracking-Probleme
    ├── Read-Query tracked → AsNoTracking() fehlt
    └── Detached Entity → Attach + State setzen
```

---

## Diagnose-Schritte

### 1. Query-Logging aktivieren

```csharp
// In appsettings.Development.json
{
  "Logging": {
    "LogLevel": {
      "Microsoft.EntityFrameworkCore.Database.Command": "Information"
    }
  }
}
```

### 2. N+1 Detection

**Symptom:** Viele kleine SELECT-Statements statt eines JOINs.

```csharp
// ❌ N+1 Problem: Jede Navigation wird einzeln geladen
var tickets = await _context.Tickets
    .Include(t => t.Tags)
    .Include(t => t.Creator)
    .ToListAsync(ct);

// ✅ Projection: Ein einziger Query mit genau den benötigten Spalten
var tickets = await _context.Tickets
    .AsNoTracking()
    .Select(t => new TicketListItemDto
    {
        Id = t.Id,
        Title = t.Title,
        CreatorName = t.Creator.DisplayName,
        TagCount = t.Tags.Count
    })
    .ToListAsync(ct);
```

### 3. AsNoTracking Check

**Regel:** ALLE Lese-Queries MÜSSEN `AsNoTracking()` verwenden.

```csharp
// Suche nach Verstößen:
// Grep: .ToListAsync( OHNE vorheriges .AsNoTracking()
// Grep: .FirstOrDefaultAsync( OHNE vorheriges .AsNoTracking()
```

### 4. Concurrency-Debugging

```csharp
/// <summary>
/// Korrekte Behandlung von Optimistic Concurrency.
/// </summary>
try
{
    await _repository.UpdateAsync(entity, cancellationToken);
}
catch (DbUpdateConcurrencyException)
{
    // Entity wurde zwischenzeitlich von anderem User geändert
    throw new ConcurrencyException(
        "Das Objekt wurde zwischenzeitlich geändert. Bitte laden Sie die Seite neu.");
}
```

### 5. Index-Analyse

```csharp
// In EntityConfiguration.cs prüfen:
builder.HasIndex(t => t.Status);                    // Filterung
builder.HasIndex(t => t.CreatedAt);                 // Sortierung
builder.HasIndex(t => new { t.Status, t.Priority }); // Composite
```

---

## Performance-Checkliste

| # | Check | Status |
| --- | --- | --- |
| 1 | `AsNoTracking()` auf allen Lese-Queries | ☐ |
| 2 | `.Select()` Projection statt `.Include()` | ☐ |
| 3 | Keine N+1 Queries (Logging prüfen) | ☐ |
| 4 | Indizes auf Filter/Sort-Spalten | ☐ |
| 5 | `CancellationToken` durchgereicht | ☐ |
| 6 | Pagination bei Listen (`Skip/Take`) | ☐ |
| 7 | `RowVersion` als `[Timestamp]` konfiguriert | ☐ |
| 8 | `CreateExecutionStrategy()` bei manuellen Transaktionen | ☐ |

---

## Nützliche EF Core Commands

```bash
# Migration erstellen
dotnet ef migrations add MigrationName \
  --project src/TicketsPlease.Infrastructure \
  --startup-project src/TicketsPlease.Web

# Migration anwenden
dotnet ef database update \
  --project src/TicketsPlease.Infrastructure \
  --startup-project src/TicketsPlease.Web

# SQL für Migration anzeigen (ohne Ausführung)
dotnet ef migrations script \
  --project src/TicketsPlease.Infrastructure \
  --startup-project src/TicketsPlease.Web

# Letzte Migration rückgängig machen
dotnet ef migrations remove \
  --project src/TicketsPlease.Infrastructure \
  --startup-project src/TicketsPlease.Web
```

---

### Skill: ef-core-debugging v1.0

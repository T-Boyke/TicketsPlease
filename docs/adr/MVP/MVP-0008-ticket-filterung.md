# MVP-0008. Ticket-Filterung via LINQ-Queries

**Datum:** 2026-03-23

**Status:** Accepted

## Kontext

Die Aufgabe (F6.1–F6.3) fordert eine Filterung der Ticket-Liste nach drei Kriterien: Projekt,
zugeordneter Benutzer und Ersteller. Die Seite soll bei Filterung neu laden (serverseitiges
Filtern).

## Entscheidung

Wir implementieren serverseitige Filterung über **Query-String-Parameter** und **EF Core
LINQ-Queries** mit dynamischer Komposition.

### Implementierung

1. **Query-Parameter:** `?projectId=&assignedUserId=&creatorId=`
2. **`GetFilteredTicketsQuery`** nimmt optionale Filter-Parameter entgegen.
3. **LINQ-Komposition:** Filter werden nur angehängt, wenn der Parameter gesetzt ist (dynamisches
   `IQueryable<T>.Where()`).
4. **UI:** Dropdown-Selects über der Ticket-Liste. Form mit `GET`-Method, Submit lädt die Seite mit
   Filtern neu.
5. **`AsNoTracking()`** für alle Lese-Queries (Performance).

```csharp
// Beispiel: Dynamische Filter-Komposition
var query = _context.Tickets.AsNoTracking();
if (projectId.HasValue)
    query = query.Where(t => t.ProjectId == projectId);
if (assignedUserId.HasValue)
    query = query.Where(t => t.AssignedUserId == assignedUserId);
// "Nicht zugeordnet" = AssignedUserId == null
```

## Konsequenzen

### Positiv

- Einfach, performant, konform.
- Server-seitig: Kein JavaScript nötig.
- Dynamische LINQ-Komposition ist erweiterbar.
- Filter über URL teilbar (Bookmarkable).

### Negativ

- Full-Page-Reload bei jeder Filteränderung (für MVP akzeptabel).
- Kein Faceted Search mit Counts (Enterprise Feature).

## Alternativen

| Alternative           | Pro              | Contra                  | Entscheidung |
| --------------------- | ---------------- | ----------------------- | ------------ |
| Server-Side LINQ      | Einfach, konform | Page-Reload             | ✅ Gewählt   |
| Client-Side JS Filter | Kein Reload      | Alle Daten ins Frontend | ❌ Abgelehnt |
| OData / GraphQL       | Flexibel         | Overkill für MVP        | ❌ Abgelehnt |

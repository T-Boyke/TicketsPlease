# MVP-0006. Startseite mit Statistik-Aggregationen

**Datum:** 2026-03-23

**Status:** Accepted

## Kontext

Die IHK-Aufgabe (F4.1, F4.2) fordert eine zentrale Startseite, die auch
ohne Login erreichbar ist. Sie soll Statistiken anzeigen: Tickets
(Gesamt/offen/geschlossen), Projekte (Gesamt/offen/beendet) und optional
Benutzer (Gesamt/pro Rolle).

## Entscheidung

1. **Startseite** unter Root-URL (`/`), erreichbar über `HomeController`.
2. **`[AllowAnonymous]`** auf der Index-Action, alle anderen Bereiche
   erfordern Login.
3. **Statistik-Query** via MediatR: `GetDashboardStatisticsQuery` liefert
   ein `DashboardStatisticsDto` mit:
   - `TotalTickets`, `OpenTickets`, `ClosedTickets`
   - `TotalProjects`, `ActiveProjects`, `CompletedProjects`
   - (Optional) `TotalUsers`, `UsersByRole` Dictionary
4. **EF Core Aggregation** via `CountAsync()` mit `AsNoTracking()`.
5. **Links/Buttons** zum Ticket- und Admin-Bereich (Admin-Link nur für
   Admins sichtbar via `User.IsInRole("Admin")`).

## Konsequenzen

### Positiv

- Klar definiertes Query-Pattern (CQRS).
- Ein einziger DB-Roundtrip für alle Statistiken (Projektion).
- Auch unangemeldete Benutzer sehen Statistiken (Information Radiator).

### Negativ

- Statistiken könnten bei vielen Tickets langsam werden (für MVP mit
  < 1000 Tickets irrelevant; für Enterprise: Redis-Caching).

## Alternativen

| Alternative            | Pro                    | Contra                    | Entscheidung |
| ---------------------- | ---------------------- | ------------------------- | ------------ |
| MediatR Query (CQRS)   | Sauber, testbar        | Etwas mehr Code           | ✅ Gewählt    |
| ViewComponent direkt   | Weniger Code           | Kein CQRS, schwer testbar | ❌ Abgelehnt  |
| Cached Counter-Tabelle | Performant             | Overkill für MVP          | ❌ Abgelehnt  |

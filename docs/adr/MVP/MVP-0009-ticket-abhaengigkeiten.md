# MVP-0009. Ticket-Abhängigkeiten (Blocking)

**Datum:** 2026-03-23

**Status:** Accepted

## Kontext

Die Aufgabe (F7.1) fordert die Möglichkeit, Tickets als „blockiert
durch andere Tickets" zu markieren. Ein Ticket kann nur geschlossen werden,
wenn alle blockierenden Tickets bereits geschlossen sind.

## Entscheidung

Wir implementieren eine **n:m-Beziehung** (Self-Referencing Many-to-Many)
über eine Join-Tabelle `TicketDependency`.

### Implementierung

1. **Neue Join-Entity:** `TicketDependency` (oder Auflösungstabelle)
   - `BlockedTicketId` (Guid, FK → Ticket) – das blockierte Ticket
   - `BlockingTicketId` (Guid, FK → Ticket) – das blockierende Ticket
   - Composite PK: (`BlockedTicketId`, `BlockingTicketId`)
2. **Domain-Methode auf Ticket:**
   - `ticket.AddBlocker(blockingTicketId)` – fügt Abhängigkeit hinzu.
   - `ticket.CanClose()` → prüft, ob alle Blocker geschlossen sind.
   - `ticket.Close(userId)` → wirft Exception, wenn `!CanClose()`.
3. **UI auf Detailseite:**
   - Multi-Select Dropdown zum Auswählen blockierender Tickets.
   - Liste der blockierenden Tickets als Links.
   - Close-Button deaktiviert, wenn offene Blocker vorhanden.

## Konsequenzen

### Positiv

- Saubere relationale Modellierung (keine JSON-Arrays im Ticket).
- Domain-Logik erzwingt Geschäftsregel „Close nur wenn alle Blocker
  geschlossen".
- Einfache Query: `ticket.Blockers.All(b => b.IsClosed)`.

### Negativ

- Neue Entity und Migration erforderlich.
- Zirkuläre Abhängigkeiten sind möglich (A blockiert B, B blockiert A) –
  für MVP keine Zykluserkennung implementiert.

## Alternativen

| Alternative          | Pro                 | Contra                  | Entscheidung |
| -------------------- | ------------------- | ----------------------- | ------------ |
| Join-Tabelle (n:m)   | Relational, 3NF     | Neue Entity/Migration   | ✅ Gewählt   |
| JSON-Array im Ticket | Kein Join nötig     | Nicht relational, no FK | ❌ Abgelehnt |
| Separate Graph-DB    | Perfekt für Graphen | Massiv Overkill         | ❌ Abgelehnt |

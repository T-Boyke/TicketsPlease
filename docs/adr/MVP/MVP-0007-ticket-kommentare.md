# MVP-0007. Ticket-Kommentarsystem

**Datum:** 2026-03-23

**Status:** Accepted

## Kontext

Die IHK-Aufgabe (F5.1) fordert ein Kommentarsystem für Tickets. Kommentare
haben Inhalt, TicketID, Ersteller und Erstellzeitpunkt. Sie werden auf der
Ticket-Detailseite angezeigt (neueste zuerst) und können dort angelegt werden.

## Entscheidung

Wir nutzen das bestehende **`Message`-Entity** (bereits im Domain Layer) als
Kommentar-Objekt. Ein Kommentar ist eine Message mit gesetztem `TicketId`-FK
(und `ReceiverUserId = null`, `TeamId = null`).

### Implementierung

1. **Keine neue Entity nötig** – Message-Entity deckt Kommentare ab.
2. **Query:** `GetTicketCommentsQuery(ticketId)` – filtert Messages
   nach `TicketId`, sortiert absteigend nach `SentAt`.
3. **Command:** `AddTicketCommentCommand(ticketId, bodyText)` – setzt
   `SenderUserId` automatisch auf den angemeldeten User.
4. **UI:** Kommentarliste auf der Ticket-Detailseite als Partial View.

## Konsequenzen

### Positiv

- Wiederverwendung des bestehenden Message-Entity (DRY).
- Polymorphes Design: Gleiche Entity für Kommentare, DMs und Broadcasts.
- Einfache Query über FK-Filter auf `TicketId`.

### Negativ

- Message-Entity enthält Felder (TeamId, ReceiverUserId), die für
  Kommentare irrelevant sind (nullable, kein Problem).
- Kein Thread/Antwort-System (für MVP nicht gefordert).

## Alternativen

| Alternative           | Pro                    | Contra                     | Entscheidung |
| --------------------- | ---------------------- | -------------------------- | ------------ |
| Message als Kommentar | DRY, bereits vorhanden | Ungenutzte nullable Felder | ✅ Gewählt    |
| Eigene Comment-Entity | Spezialisiert          | Code-Duplizierung          | ❌ Abgelehnt  |

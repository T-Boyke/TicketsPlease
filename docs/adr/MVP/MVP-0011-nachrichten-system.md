# MVP-0011. Nachrichten-System (User-to-User)

**Datum:** 2026-03-23

**Status:** Accepted

## Kontext

Die Aufgabe (F9.1) fordert ein Nachrichtensystem, über das Benutzer außerhalb von Tickets
miteinander kommunizieren können. Nachrichten bestehen aus Absender, Empfänger, Zeitstempel und
Textinhalt. Jeder Benutzer bekommt eine Nachrichten-Seite mit einer nach Absender/Empfänger
gruppierten Übersicht.

## Entscheidung

Wir nutzen das bestehende **`Message`-Entity** (bereits im Domain Layer). Eine Direktnachricht (DM)
ist eine Message mit gesetztem `ReceiverUserId` (und `TicketId = null`, `TeamId = null`).

### Implementierung

1. **Kein neues Entity nötig** – Message-Entity deckt DMs ab.
2. **`MessagesController`** mit:
   - `Index` – Übersicht aller Konversationen (gruppiert nach Partner).
   - `Conversation(userId)` – Alle Nachrichten mit einem bestimmten User.
   - `Send(receiverId, body)` – Neue Nachricht erstellen.
3. **Gruppierungs-Query:** Messages nach dem jeweils anderen User gruppieren (Sender → Empfänger
   oder Empfänger → Sender).
4. **SenderUserId** wird automatisch auf den angemeldeten User gesetzt.
5. **SentAt** wird automatisch auf `DateTimeOffset.UtcNow` gesetzt.

## Konsequenzen

### Positiv

- Wiederverwendung des Message-Entity (DRY, konsistent mit F5).
- Polymorphes Message-Pattern: TicketId→Kommentar, ReceiverUserId→DM.
- Einfache Konversations-Gruppierung via LINQ GroupBy.

### Negativ

- Kein Echtzeit-Chat (für MVP ist Full-Page-Reload akzeptabel; SignalR ist Enterprise Phase 2).
- Keine Read-Receipts im MVP (MessageReadReceipt ist Enterprise).
- Rein textbasiert, kein Markdown-Rendering (Enterprise).

## Alternativen

| Alternative            | Pro                     | Contra            | Entscheidung |
| ---------------------- | ----------------------- | ----------------- | ------------ |
| Message-Entity (DM)    | DRY, polymorphes Design | Kein Echtzeit     | ✅ Gewählt   |
| Eigene DM-Entity       | Spezialisiert           | Code-Duplizierung | ❌ Abgelehnt |
| SignalR Real-Time Chat | Echtzeit                | Overkill für MVP  | ❌ Abgelehnt |

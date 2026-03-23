# MVP-0005. Ticket-Lifecycle (Erstellen, Bearbeiten, Schließen)

**Datum:** 2026-03-23

**Status:** Accepted

## Kontext

Die IHK-Aufgabe (F3.1–F3.4) definiert den vollständigen Ticket-Lifecycle:
Erstellen, Auflisten, Detailansicht, Bearbeiten und Schließen. Strikte
Berechtigungsregeln bestimmen, wer welche Aktionen ausführen darf.

## Entscheidung

Das `Ticket`-Entity im Domain Layer wird mit folgenden MVP-Properties
implementiert:

### Properties (Rich Domain Model)

- `Title` (string, Pflicht), `Description` (string, Pflicht)
- `ProjectId` (Guid, FK → Project, Pflicht, nur offene Projekte)
- `CreatorId` (Guid, FK → User, automatisch = angemeldeter Benutzer)
- `CreatedAt` (DateTimeOffset, automatisch)
- `AssignedUserId` (Guid?, nullable)
- `AssignedAt` (DateTimeOffset?, automatisch bei Zuweisung)
- `ClosedByUserId` (Guid?, F3.4), `ClosedAt` (DateTimeOffset?, F3.4)

### Berechtigungslogik (Domain-Methoden)

- **Bearbeiten** (`ticket.Update(userId)`): Nur Admin, Ersteller oder
  Zugewiesener. Änderbar: Beschreibung, AssignedUserId.
- **Schließen** (`ticket.Close(userId)`): Nur Admin, Ersteller oder
  Zugewiesener. Setzt `ClosedByUserId` und `ClosedAt`.
- **Sortierung:** Ticket-Liste nach Projekt + Erstellungsdatum (absteigend).

## Konsequenzen

### Positiv

- Rich Domain Model: Geschäftslogik liegt im Entity, nicht im Controller.
- Klare Berechtigungsprüfung via Domain-Methoden.
- Alle IHK-Akzeptanzkriterien (F3.1–F3.4) vollständig abgedeckt.

### Negativ

- `private set` auf allen Properties erfordert Verhaltensmethoden.
- Domain-Validierung muss sowohl im Entity als auch in FluentValidation
  gespiegelt werden.

## Alternativen

| Alternative           | Pro                  | Contra                        | Entscheidung |
| --------------------- | -------------------- | ----------------------------- | ------------ |
| Rich Domain Model     | DDD, testbar         | Mehr Code im Entity           | ✅ Gewählt   |
| Anemic Model + Service | Einfacher            | Verletzt DDD, weniger testbar | ❌ Abgelehnt |

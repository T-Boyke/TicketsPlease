# MVP-0010. Workflow-Engine (Status-Verwaltung)

**Datum:** 2026-03-23

**Status:** Accepted

## Kontext

Die Aufgabe (F8.1) fordert eine Workflow-Verwaltung für Tickets. Admins
können Workflows erstellen (CRUD). Ein Workflow hat eine ID und Bezeichnung
und besteht aus einer Reihe von Status. Jedem Projekt wird genau ein
Workflow zugeordnet.

Optional: Status-Folge-Regeln (welche Status-Übergänge erlaubt sind) und
rollenbasierte Status-Vergabe.

## Entscheidung

Wir nutzen das bestehende **`WorkflowState`-Entity** (bereits im Domain
Layer) und erweitern es um ein **`Workflow`-Aggregat**.

### Implementierung

1. **Neues `Workflow`-Entity** (Aggregat Root):
   - `Id` (Guid), `Name` (string, z.B. „Standard", „Bug-Tracking")
   - `States` (Collection von `WorkflowState`)
2. **Bestehendes `WorkflowState`-Entity** erweitern:
   - `WorkflowId` (Guid, FK → Workflow)
   - `Name`, `OrderIndex`, `ColorHex`, `IsTerminalState`
3. **Project ↔ Workflow:** `Project.WorkflowId` (Guid, FK → Workflow).
4. **Admin-CRUD:** `/Admin/Workflows/` mit Create, Edit, Delete, Index.
5. **Ticket-Status:** Ticket referenziert `WorkflowStateId` statt
   String-basiertem Status.

### Optional (wenn Zeit)

- `WorkflowTransition` (bereits im Domain Layer): Definiert erlaubte
  Status-Übergänge und Rollen-Berechtigungen pro Übergang.

## Konsequenzen

### Positiv

- Dynamische Workflows pro Projekt (nicht hardcoded).
- WorkflowState-Entity bereits vorhanden, minimaler Aufwand.
- Admins können projektspezifische Prozesse definieren.
- Grundlage für Enterprise-Kanban-Board (Phase 2).

### Negativ

- Komplexer als ein simples String-Feld (`Status = "Offen"`).
- Neue Entities und Migrationen erforderlich.
- Default-Workflow muss geseeded werden.

## Alternativen

| Alternative                 | Pro               | Contra                     | Entscheidung |
| --------------------------- | ----------------- | -------------------------- | ------------ |
| Workflow-Entity (Dynamisch) | Flexibel, konform | Mehr Code, Migration       | ✅ Gewählt   |
| Status als Enum             | Extrem einfach    | Nicht dynamisch, kein CRUD | ❌ Abgelehnt |
| Status als Lookup-Table     | Einfach           | Kein Workflow-Konzept      | ❌ Abgelehnt |

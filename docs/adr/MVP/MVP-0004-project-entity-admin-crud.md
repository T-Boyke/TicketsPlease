# MVP-0004. Project-Entity und Admin-CRUD

**Datum:** 2026-03-23

**Status:** Accepted

## Kontext

Die Aufgabe (F2.1, F2.2) fordert einen eigenen Admin-Bereich mit
CRUD-Funktionalität für Projekte. Ein Projekt besteht aus Titel,
Beschreibung, Startdatum (Pflicht) und Enddatum (optional). Nur Admins
dürfen Projekte verwalten. Tickets werden später einem Projekt zugeordnet.

Das `Project`-Entity existiert aktuell **nicht** im Domain Layer und muss
neu erstellt werden.

## Entscheidung

1. **Neues `Project`-Entity** im Domain Layer mit den Properties:
   - `Title` (string, Pflicht, max. 200 Zeichen)
   - `Description` (string, Pflicht)
   - `StartDate` (DateTimeOffset, Pflicht)
   - `EndDate` (DateTimeOffset?, Optional)
   - Erbt von `BaseEntity` (Id, TenantId, IsDeleted, RowVersion)
2. **Admin-Bereich** unter `/Admin/` mit eigenem Area oder Controller-Prefix.
3. **Autorisierung:** `[Authorize(Roles = "Admin")]` auf dem gesamten
   Admin-Bereich.
4. **CRUD über MediatR-Commands:** `CreateProjectCommand`,
   `UpdateProjectCommand`, `DeleteProjectCommand` + `GetProjectsQuery`.
5. **Scaffold-Unterstützung:** Razor Views für Create/Edit/Delete/Index.

## Konsequenzen

### Positiv

- Saubere Domain-Entity mit Rich-Model-Pattern.
- Projekte sind von Tag 1 an Aggregate Roots (Tickets referenzieren sie).
- Admin-Bereich ist rollenbasiert abgesichert.
- Anforderung F2.2 vollständig abgedeckt.

### Negativ

- Neue EF Core Migration erforderlich für die `Projects`-Tabelle.
- Admin-Bereich erfordert separate Views und Navigation.

### Neutral

- Ein Projekt kann als „beendet" gelten, wenn `EndDate < DateTime.Now`.
- Nur offene Projekte (nicht beendet/gelöscht) können Tickets zugeordnet
  werden (F3.1 Anforderung).

## Alternativen

| Alternative             | Pro                   | Contra                       | Entscheidung |
| ----------------------- | --------------------- | ---------------------------- | ------------ |
| Eigene Project-Entity   | DDD-konform, flexibel | Migration nötig              | ✅ Gewählt   |
| Projekte als Enum/Liste | Einfacher             | Kein CRUD, nicht erweiterbar | ❌ Abgelehnt |
| Projekte als Tags       | Flexible Zuordnung    | Kein Admin-CRUD möglich      | ❌ Abgelehnt |

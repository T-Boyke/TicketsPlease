# ADR 0030: Database Design (3rd Normal Form)

- Status: accepted
- Deciders: Tobias
- Date: 2026-03-06

## Context and Problem Statement

Für ein Ticketsystem mit komplexen Zuweisungen (User & Teams), Time-Tracking und
Historien ist die Datenintegrität kritisch. Ein schlechtes Schema führt zu
Redundanzen und Update-Anomalien.

## Decision Drivers

- Datenkonsistenz
- Keine Redundanzen
- Skalierbarkeit (Enterprise Features)
- Einfache Reportings

## Considered Options

- Denormalized NoSQL (MongoDB)
- SQL mit 3. Normalform (3NF)
- Flat-Table Design

## Decision Outcome

Chosen option: "SQL mit 3. Normalform (3NF)", because es Redundanzen eliminiert,
die Integrität via Foreign-Keys schützt und die Basis für komplexe Relationen
(wie das n:m Plugin-Setting-System) bildet.

### Positive Consequences

- Revisionssichere Datenhaltung.
- Einfache Erweiterbarkeit (neue Tabellen stören bestehende Abfragen kaum).
- Sauberes EF Core Mapping.

### Negative Consequences

- Komplexere Queries (Joins erforderlich).
- Etwas höherer Aufwand beim initialen Anlegen der Entities.

## Pros and Cons of the Options

### SQL 3NF

- Good, because maximale Datenintegrität.
- Bad, because viele Joins bei großen Abfragen (abgelindert durch Caching).

### Denormalized Design

- Good, because schnelle Lesezugriffe ohne Joins.
- Bad, because Update-Anomalien bei User-Namensänderungen etc.

# ADR 0050: CQRS with MediatR
* Status: accepted
* Deciders: Tobias
* Date: 2026-03-06

## Context and Problem Statement
In einer Clean Architecture neigen Service-Klassen dazu, mit der Zeit anzuschwellen und zu "God-Objects" zu werden, die zu viele Verantwortlichkeiten übernehmen. Wir benötigen ein Muster, das die Logik für Lese- und Schreibvorgänge sauber trennt und die Kopplung minimiert.

## Decision Drivers
* Vermeidung von riesigen Service-Klassen
* Einhaltung des Single Responsibility Principle (SRP)
* Einfache Implementierung von Cross-Cutting Concerns (Logging, Validation)
* Testbarkeit einzelner Use-Cases

## Considered Options
* Klassische Service-Layer Pattern
* CQRS (Command Query Responsibility Segregation) mit MediatR
* Direct Repository Access in Controllern

## Decision Outcome
Chosen option: "CQRS mit MediatR", because es jeden Use-Case (z.B. "Ticket erstellen") in eine eigene Handler-Klasse isoliert. MediatR fungiert als In-Memory Message Bus, was die Kopplung zwischen Web-API und Geschäftslogik auf ein Minimum reduziert.

### Positive Consequences
* Jeder Use-Case ist in einer isolierten Datei/Klasse (SRP perfekt umgesetzt).
* Einfache Implementierung von Middlewares (MediatR Behaviors) für Caching oder Validierung.
* Hervorragende Testbarkeit der einzelnen Handler.

### Negative Consequences
* Erhöhte Anzahl an Dateien (Command, Result, Handler pro Use-Case).
* Indirektion kann für neue Entwickler anfangs verwirrend sein.

## Pros and Cons of the Options

### CQRS mit MediatR
* Good, because es erzwingt eine saubere Trennung von Befehlen und Abfragen.
* Good, because Use-Cases sind explizit sichtbar im Projektbaum.
* Bad, because Boilerplate-Code für jede Operation.

### Klassische Services
* Good, because weniger Dateien und Indirektion.
* Bad, because Services neigen dazu, hunderte Zeilen Code mit unzusammenhängender Logik zu enthalten.

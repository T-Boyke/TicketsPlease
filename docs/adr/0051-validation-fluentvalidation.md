# ADR 0051: Validation Strategy (FluentValidation)

- Status: accepted
- Deciders: Tobias
- Date: 2026-03-06

## Context and Problem Statement

Valider Input ist die erste Verteidigungslinie eines Enterprise-Systems. Wir
benötigen eine Lösung, die komplexe Validierungsregeln (z.B. Abhängigkeiten
zwischen Feldern) sauber und außerhalb der Domain-Entities oder Controller
definiert.

## Decision Drivers

- Trennung von Validierungslogik und Geschäftslogik
- Lesbarkeit der Regeln
- Wiederverwendbarkeit von Validatoren
- Integration in die MediatR-Pipeline

## Considered Options

- Data Annotations ([Required], [MaxLength] an den DTOs)
- FluentValidation Library
- Manuelle `if-else` Checks in den Handlern

## Decision Outcome

Chosen option: "FluentValidation", because es eine flüssige (Fluent) API bietet,
um auch hochkomplexe Regeln ausdrucksstark zu definieren. Die Regeln liegen in
separaten Klassen, was die DTOs sauber hält.

### Positive Consequences

- Validierungslogik ist zentralisiert und separat testbar.
- Wir können Validatoren automatisch via MediatR-Pipeline ausführen (Fail-Fast).
- Unterstützung für Lokalisierung der Fehlermeldungen ist eingebaut.

### Negative Consequences

- Zusätzliche Library-Abhängigkeit.
- Ein weiterer "Layer" (Validator-Klassen) muss gepflegt werden.

## Pros and Cons of the Options

### FluentValidation

- Good, because maximale Flexibilität und Lesbarkeit.
- Good, because kein "Pollution" der DTOs mit Attributen.
- Bad, because leicht erhöhter Boilerplate-Aufwand.

### Data Annotations

- Good, because nativ in ASP.NET Core integriert.
- Bad, because unflexibel bei komplexen, übergreifenden Regeln.
- Bad, because vermischt Schema-Definition mit Validierungs-Logik.

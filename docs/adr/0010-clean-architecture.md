# ADR 0010: Clean Architecture (Onion)

* Status: accepted
* Deciders: Tobias
* Date: 2026-03-06

## Context and Problem Statement
Wir benötigen eine Struktur, die das System testbar, unabhängig von Frameworks
und leicht erweiterbar macht. Eine eng gekoppelte Architektur führt bei
wachsenden Anforderungen (Phase 2-5) zu Wartungsproblemen.

## Decision Drivers

* Testbarkeit der Geschäftslogik
* Unabhängigkeit von externen Frameworks (UI, DB)
* Klare Trennung von Zuständigkeiten
* Unterstützung für zukünftige Enterprise-Erweiterungen

## Considered Options
* N-Tier Architecture (Classic Layers)
* Clean Architecture (Onion / Hexagonal)
* Microservices

## Decision Outcome
Chosen option: "Clean Architecture", because sie die Domain (Geschäftsregeln) in
das Zentrum stellt und Abhängigkeiten strikt nach innen (Richtung Core)
definiert. Dies ermöglicht es, die Infrastructure (DB) oder Presentation (MVC)
auszutauschen, ohne die Kern-Logik zu verändern.

### Positive Consequences
* Domain-Code ist 100% testbar ohne Datenbank-Mocking-Hölle.
* Wir können problemlos zwischen SQL Server und anderen Providern wechseln.
* Die Projektstruktur ist für das IHK-Projekt professionell und State-of-the-Art.

### Negative Consequences
* Höhere Anzahl an Projekten in der Solution (Domain, App, Infra, Web).
* Erfordert Mapping zwischen Entities und DTOs (gelöst durch Mapster).

## Pros and Cons of the Options

### Clean Architecture
* Good, because Geschäftsregeln sind isoliert und geschützt.
* Good, because Abhängigkeiten fließen nur nach innen.
* Bad, because initiale Boilerplate-Kosten (4 Projekte) sind höher als bei N-Tier.

### N-Tier Architecture
* Good, because schnelleres Initial-Setup.
* Bad, because Geschäftslogik landet oft direkt in der UI oder im Daten-Layer.
* Bad, because schlechtere Testbarkeit durch Durchgriffe auf die DB.

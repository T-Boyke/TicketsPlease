# ADR 0021: API Documentation & Visualization (Scalar)

- Status: accepted
- Deciders: Tobias
- Date: 2026-03-06

## Context and Problem Statement

Für Enterprise-Systeme und die Anbindung von Plugins ist eine erstklassige API-Dokumentation
unerlässlich. Wir benötigen ein Tool, das Swagger/OpenAPI-Definitionen nicht nur anzeigt, sondern
sie visuell ansprechend und interaktiv aufbereitet.

## Decision Drivers

- Ästhetik & moderne UI
- Interaktionsmöglichkeiten (API-Testing direkt in der Doku)
- Einfache Integration in .NET 10.3
- Unterstützung für OpenAPI 3.x

## Considered Options

- Swagger UI (Default / Swashbuckle)
- NSwag
- Scalar

## Decision Outcome

Chosen option: "Scalar", because es eine modernere, ästhetisch ansprechendere Benutzeroberfläche
bietet als das klassische Swagger UI. Es ist "Developer-First" gestaltet und integriert mächtige
Testing-Features und Client-Code-Generatoren direkt in der Dokumentations-Ansicht.

### Positive Consequences

- Professioneller "Enterprise"-Eindruck bei Drittentwicklern.
- Reduzierter Support-Aufwand durch klare, interaktive Dokumentation.
- Nahtlose Integration in das neue .NET OpenAPI Middleware-System.

### Negative Consequences

- Scalar ist neuer als Swagger UI, was potenziell weniger Community-Hilfe bei Fehlern bedeutet.

## Pros and Cons of the Options

### Scalar

- Good, because wunderschöne, moderne Oberfläche.
- Good, because integrierter Client-Code-Generator.
- Bad, because noch nicht so weit verbreitet wie Swagger UI.

### Swagger UI

- Good, because der absolute De-facto Standard.
- Bad, because Design wirkt veraltet (Web 2.0 Optik).
- Bad, because mühsames Scrollen bei sehr großen Schnittstellen-Definitionen.

# ADR 0020: Technology Stack
* Status: accepted
* Deciders: Tobias
* Date: 2026-03-06

## Context and Problem Statement
Wir müssen einen modernen, stabilen und zukunftssicheren Stack wählen, der sowohl die IHK-Anforderungen erfüllt als auch die geplante Enterprise-Skalierung unterstützt.

## Decision Drivers
* Modernität (C# 14 / .NET 10.3)
* Starke Community-Unterstützung
* Performance & Skalierbarkeit
* Cloud-Readiness

## Considered Options
* .NET Framework (Legacy)
* ASP.NET Core 10.3 (Modern .NET)
* Node.js / React (Alternative Stack)

## Decision Outcome
Chosen option: "ASP.NET Core 10.3", because es die native Plattform für C# ist, höchste Performance bietet und nahtlos mit EF Core und dem gewählten Architektur-Muster (Clean Architecture) harmoniert.

### Positive Consequences
* Zugriff auf modernste Sprachfeatures von C# 14.
* Native Unterstützung für Dependency Injection und High-Performance Web-Features.
* Einfache Deployment-Möglichkeiten auf Azure/Docker.

### Negative Consequences
* Erfordert laufende Updates, da .NET 10.3 Cutting-Edge ist.

## Pros and Cons of the Options

### ASP.NET Core 10.3
* Good, because Cross-Platform und extrem performant.
* Good, because Integration von MediatR und EF Core ist Industrie-Standard.
* Bad, because Einarbeitung in neuste Features erforderlich.

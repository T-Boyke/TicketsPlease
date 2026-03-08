# ADR 0060: Testing Strategy
* Status: accepted
* Deciders: Tobias
* Date: 2026-03-06

## Context and Problem Statement
Um eine hohe Code-Qualität und Wartbarkeit zu gewährleisten, benötigen wir eine automatisierte Test-Strategie. Integration-Tests gegen "echte" Datenbanken sind oft langsam oder schwer zu konfigurieren.

## Decision Drivers
* 100% Domain Coverage
* Schnelle Feedback-Zyklen
* Zuverlässigkeit der Integration-Tests
* Einfaches CI/CD Setup

## Considered Options
* Manuelles Testing
* Unit Tests Only
* TDD mit Unit- & Integration-Tests (Testcontainers)

## Decision Outcome
Chosen option: "TDD mit Unit- & Integration-Tests (Testcontainers)", because es die Vorteile von schnellen Unit-Tests mit der Sicherheit eines echten Datenbank-Laufes (via Docker) kombiniert, ohne dass der Entwickler lokal manuell einen SQL-Server pflegen muss.

### Positive Consequences
* "It works on my machine" ist Geschichte.
* CI/CD Gates (Lighthouse & Tests) garantieren Top-Qualität.
* Architektur-Regeln werden via Code (`NetArchTest`) erzwungen.

### Negative Consequences
* Erfordert Docker-Installation auf den Workstations.
* Längere CI-Laufzeiten durch Container-Startup.

## Pros and Cons of the Options

### Testing with Testcontainers
* Good, because echte SQL-Queries werden validiert.
* Good, because isolierte Testumgebung für jeden Lauf.
* Bad, because höhere Ressourcenlast (RAM/CPU) beim Testen.

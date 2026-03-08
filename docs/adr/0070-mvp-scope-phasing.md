# ADR 0070: MVP Scope & Phasing
* Status: accepted
* Deciders: Tobias
* Date: 2026-03-06

## Context and Problem Statement
Bei komplexen Projekten besteht die Gefahr des "Scope Creeps" (Verzettelung), besonders im Rahmen einer zeitkritischen IHK-Projektarbeit (80 Stunden).

## Decision Drivers
* Deadline-Treue (IHK-Abgabe)
* Fokus auf Kernfunktionen
* Erweiterbarkeit für spätere Versionen (Enterprise)
* Stabilität des Kerns

## Considered Options
* Alles-auf-einmal-bauen (Big Bang)
* Striktes MVP mit phasenweiser Erweiterung
* Build-as-you-go ohne Vision

## Decision Outcome
Chosen option: "Striktes MVP mit phasenweiser Erweiterung", because es sicherstellt, dass die Basisfunktionalität (Ticket, Kanban, Identity) zum Abgabetermin 100% stabil ist, während das Architektur-Design und die Dokumentation bereits den Weg für Enterprise-Features ebnen.

### Positive Consequences
* IHK-Projekt ist "Sicherer Hafen".
* Klare Trennung zwischen IHK-Basis und Bonus-Features.
* Professionelles Produktmanagement-Vorgehen.

### Negative Consequences
* Einige spannende Features (SignalR, Plugins) landen erst in Phase 2-4.

## Pros and Cons of the Options

### Phasenweiser Rollout
* Good, because kontrolliertes Projektrisiko.
* Good, because klare Meilensteine.
* Bad, because erfordert Disziplin (Nein-Sagen zu neuen Features während Phase 1).

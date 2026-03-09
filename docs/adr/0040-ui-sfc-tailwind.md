# ADR 0040: UI Architecture (SFC & Tailwind 4.2)

* Status: accepted
* Deciders: Tobias
* Date: 2026-03-06

## Context and Problem Statement

Wir brauchen ein Frontend-System, das schnelles Prototyping ermöglicht, aber auch
bei hunderten Komponenten (Tickets, Modals, Boards) wartbar bleibt und nicht in
einem "CSS-Spaghetti" endet.

## Decision Drivers

* Developer Experience (DX)
* Skalierbarkeit des CSS
* Kurze Ladezeiten
* Modularität

## Considered Options

* Globales CSS (BEM / OOCSS)
* TailwindCSS 4.2 (Utility-First)
* CSS Isolation (SFC) in ASP.NET Core

## Decision Outcome

Chosen option: "Kombination aus SFC (Isolation) und TailwindCSS 4.2", because
Tailwind pfeilschnelles Design ermöglicht, während die CSS-Isolation
sicherstellt, dass Komponenten-spezifische Anpassungen niemals das restliche
System beeinflussen.

### Positive Consequences

* Maximale Modularität (Single File Component Feeling).
* Kein "Side-Effect"-CSS.
* Moderne Optik durch Tailwind 4.2 Features.

### Negative Consequences

* CSS muss pro View/Component angelegt werden.
* Tailwind-Klassen können im HTML unübersichtlich werden (gelöst durch `@apply`
  Abstraktion).

## Pros and Cons of the Options

### SFC + Tailwind

* Good, because Best-of-both-worlds (Utility Speed + Component Isolation).
* Bad, because Setup der Tailwind-CLI/Build-Prozess erforderlich.

# ADR 0008: Plugin Architecture (Extensibility)
* Status: accepted
* Deciders: Tobias
* Date: 2026-03-06

## Context and Problem Statement
Das System soll für Drittanbieter und kundenspezifische Erweiterungen offen sein, ohne dass der Kern des Ticket-Systems modifiziert werden muss.

## Decision Drivers
* Offen-Geschlossen-Prinzip (Open-Closed)
* Keine Build-Abhängigkeit zu externen Modulen
* Dynamische Konfiguration zur Laufzeit
* Isolation von Erweiterungen

## Considered Options
* Statische Integration (Direct Projects)
* Dynamische DLL-Loading (Plugins)
* Webhooks & External APIs Only

## Decision Outcome
Chosen option: "Dynamische DLL-Loading (Plugins)", because es eine nahtlose Integration von Logik und UI direkt im Host-Prozess ermöglicht, was für Performance und UX (Notification-Channels etc.) vorteilhaft ist.

### Positive Consequences
* Marktplatz-Feeling möglich.
* Kunden können eigene SLA- oder Auth-Logik implementieren.
* Core bleibt sauber und übersichtlich.

### Negative Consequences
* Höhere Komplexität im Startvorgang der App (Assembly Discovery).
* Sicherheitsrisiken durch das Laden von Fremdcode (muss über Interfaces/Sandbox isoliert werden).

## Pros and Cons of the Options

### Plugin Architecture
* Good, because maximale Flexibilität für Enterprise-Kunden.
* Good, because saubere Trennung von Third-Party Code.
* Bad, because Fehler in Plugins können das gesamte System instabil machen (Isolations-Layer nötig).

# ADR 0061: Structured Logging (Serilog)

- Status: accepted
- Deciders: Tobias
- Date: 2026-03-06

## Context and Problem Statement

In einer modernen Enterprise-Welt sind reine Text-Logs schwer zu durchsuchen und zu analysieren.
Wir benötigen strukturierte Informationen in den Logs, um Fehler in komplexen Workflows (z.B.
Ticket-Plugin-Interaktionen) schnell identifizieren zu können.

## Decision Drivers

- Durchsuchbarkeit der Logs
- Integration mit modernen Monitoring-Tools (Seq, ELK)
- Performance
- Einfache Konfiguration

## Considered Options

- Microsoft.Extensions.Logging (Default)
- Serilog
- NLog

## Decision Outcome

Chosen option: "Serilog", because es "Structured Logging" nativ unterstützt. Anstatt Nachrichten zu
formatieren, speichert Serilog Datenobjekte (JSON), was granulare Abfragen über Log-Felder (z.B.
`TicketId`) ermöglicht.

### Positive Consequences

- Logs können nach spezifischen Properties gefiltert werden, ohne RegEx-Hölle.
- Reichhaltige Auswahl an "Sinks" (Ziele) für Logs (File, Console, SQL, Seq).
- Einfache Correlation-IDs über MediatR-Behaviors möglich.

### Negative Consequences

- Konfiguration ist etwas aufwendiger als das Standard-Logging.

## Pros and Cons of the Options

### Serilog

- Good, because Fokus auf strukturierte Daten von Anfang an.
- Good, because riesiges Ökosystem an Erweiterungen.
- Bad, because leichtes Overhead im Vergleich zum nackten Standard-Logging.

# ADR 0082: Real-Time Communication (SignalR)

- Status: accepted
- Deciders: Tobias
- Date: 2026-03-06

## Context and Problem Statement

Ein modernes Kanban-System lebt von Interaktion. Wenn ein Kollege ein Ticket verschiebt oder einen
Kommentar schreibt, sollen andere User dies sofort sehen, ohne die Seite manuell aktualisieren zu
müssen.

## Decision Drivers

- Benutzererfahrung (Interaktivität)
- Performance (Vermeidung von Polling)
- Echtzeit-Updates (Chat, Presence)

## Considered Options

- HTTP Polling (Browser fragt alle 5 Sek. nach Updates)
- WebHooks (Nur Server-seitig)
- SignalR (WebSockets / Server-Sent Events)

## Decision Outcome

Chosen option: "SignalR", because es die native Lösung für Echtzeit-Kommunikation in ASP.NET Core
ist. Es wählt automatisch den besten Transportweg (WebSockets, SSE oder Long Polling) und bietet
eine einfache Hub-Abstraktion.

### Positive Consequences

- "Live"-Gefühl in der App (Kanban, Chat, Notifications).
- Reduzierte Serverlast im Vergleich zu aggressivem Polling.
- Elegante Implementierung von Presence-Features (Wer ist online?).

### Negative Consequences

- Erfordert das Halten von offenen Verbindungen (Ressourcenlast am Server).
- Höhere Komplexität bei der Skalierung (erfordert Redis Backplane für mehrere Server).

## Pros and Cons of the Options

### SignalR

- Good, because nahtlose Integration in das .NET Ökosystem.
- Good, because Fallback-Mechanismen für ältere Browser.
- Bad, because State-Management (Verbindungsabbrüche) muss im Frontend sauber gelöst werden.

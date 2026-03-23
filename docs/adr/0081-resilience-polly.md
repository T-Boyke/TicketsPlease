# ADR 0081: Resilience & Fault Handling (Polly)

- Status: accepted
- Deciders: Tobias
- Date: 2026-03-06

## Context and Problem Statement

Verteilte Systeme (Datenbank, Mail-Server, externe Plugins) können temporär
ausfallen. Ohne eine Strategie zum Umgang mit Fehlern (Transiente Fehler) stürzt
die Applikation bei kleinen Netzwerkstörungen sofort ab.

## Decision Drivers

- Systemstabilität & Robustheit
- Fehlertoleranz
- Benutzererfahrung (UX) bei Fehlern

## Considered Options

- Keine Strategie (App stürzt ab / zeigt 500er Error)
- Polly (Resilience Framework)
- Manuelle `try-catch-retry` Logik

## Decision Outcome

Chosen option: "Polly", because es ein bewährtes .NET Framework ist, um
Politiken wie "Retry", "Circuit Breaker", "Timeout" oder "Fallback" deklarativ
zu definieren.

### Positive Consequences

- Kurze Datenbank-Hänger führen nicht mehr zum Absturz des Users.
- Circuit-Breaker schützt überlastete Subsysteme vor weiteren Anfragen.
- Sauberes Handling von API-Timeouts.

### Negative Consequences

- Komplexität bei der Definition der Politiken (Wann mache ich einen Retry? Wie
  oft?).
- Kann Fehler "verschleiern", wenn das Logging nicht sauber mitläuft.

## Pros and Cons of the Options

### Polly

- Good, because Industrie-Standard für Resilience in .NET.
- Good, because entkoppelt die Fehler-Logik vom Geschäfts-Code.
- Bad, because erfordert tiefes Verständnis der Verteilungs-Thematik.

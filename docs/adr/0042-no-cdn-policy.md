# ADR 0042: No-CDN Policy

- Status: accepted
- Deciders: Tobias
- Date: 2026-03-06

## Context and Problem Statement

Die Nutzung von CDNs (Content Delivery Networks) birgt Risiken bezüglich Datenschutz (DSGVO),
Ausfallsicherheit und Offline-Entwicklungsfähigkeit.

## Decision Drivers

- DSGVO Konformität
- Unabhängigkeit von Drittanbietern
- Performance (keine zusätzlichen DNS Lookups)
- Offline-First Developing

## Considered Options

- CDN-basierte Einbindung (Google Fonts, Tailwind CDN)
- Lokale Asset-Einbindung via Libman
- Bundling via NPM/Webpack

## Decision Outcome

Chosen option: "Lokale Asset-Einbindung via Libman", because es die einfachste und am besten
integrierte Methode in ASP.NET Core ist, um externe Bibliotheken versioniert und lokal zu verwalten,
ohne einen komplexen Node.js Build-Prozess zwingend vorauszusetzen.

### Positive Consequences

- Volle Kontrolle über geladene Dateien.
- 100% DSGVO-konform (keine IP-Weitergabe an Google/FontAwesome).
- Die App läuft auch im isolierten Firmennetzwerk ohne Internetzugang.

### Negative Consequences

- Initialer Mehraufwand beim Konfigurieren der `libman.json`.

## Pros and Cons of the Options

### Libman (Local)

- Good, because einfach und robust.
- Good, because keine Abhängigkeit von Internet-Konnektivität beim Rendern.
- Bad, because Assets müssen im Git (oder via Restore) verwaltet werden.

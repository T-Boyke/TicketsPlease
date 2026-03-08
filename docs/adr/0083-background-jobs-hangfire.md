# ADR 0083: Background Job Scheduling (Hangfire)
* Status: accepted
* Deciders: Tobias
* Date: 2026-03-06

## Context and Problem Statement
Einige Prozesse (Auto-Close von Tickets, SLA-Prüfungen, Teamlead-Broadcast-Mails) müssen zeitgesteuert oder im Hintergrund laufen, ohne den Web-Anfrageprozess (Request) zu blockieren. Wir benötigen eine Lösung, die Jobs persistiert, damit sie auch nach einem Server-Neustart nicht verloren gehen.

## Decision Drivers
* Persistenz von Hintergrund-Aufgaben
* Zuverlässigkeit (Retries bei Fehlern)
* Dashboard zur Überwachung
* Einfache Integration in ASP.NET Core

## Considered Options
* System.Threading.Timer / Task.Delay
* IHostedService / BackgroundService (Native .NET)
* Hangfire
* Quartz.NET

## Decision Outcome
Chosen option: "Hangfire", because es eine integrierte SQL-Server-Persistenz bietet, fehlgeschlagene Jobs automatisch wiederholt und über ein hervorragendes Dashboard verfügt, um den Status aller Hintergrund-Prozesse in Echtzeit zu sehen.

### Positive Consequences
* Jobs überleben Anwendungs-Neustarts (Crash-Resistence).
* Transparentes Monitoring via `/hangfire` Web-UI.
* Wir benötigen keinen separaten Windows-Dienst; Hangfire läuft direkt im Web-Host.

### Negative Consequences
* Erfordert eigene Tabellen in der Datenbank.
* Persistenz-Storage (SQL Server) erzeugt etwas Overhead.

## Pros and Cons of the Options

### Hangfire
* Good, because extrem einfach zu bedienen und sehr robust.
* Good, because native Dashboard-Unterstützung.
* Bad, because SQL-Persistenz kann bei extrem hoher Last zum Flaschenhals werden (Redis-Option existiert aber).

### IHostedService (Native)
* Good, because kein externes NuGet-Paket/Setup nötig.
* Bad, because Jobs sind flüchtig (gehen bei Restart verloren).
* Bad, because kein eingebautes Management/Dashboard.

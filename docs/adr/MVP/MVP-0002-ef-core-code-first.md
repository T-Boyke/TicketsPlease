# MVP-0002. EF Core Code-First mit SQL Server

**Datum:** 2026-03-23

**Status:** Accepted

## Kontext

Die Aufgabe (F1.2) verlangt eine SQL Server-Datenbank, angebunden über
Entity Framework mit dem Code-First-Ansatz. Offene Migrationen sollen beim
App-Start automatisch ausgeführt werden.

## Entscheidung

Wir verwenden **Entity Framework Core 10** im **Code-First-Modus** mit
Microsoft SQL Server. Migrationen werden beim App-Start automatisch über
`context.Database.Migrate()` in `Program.cs` angewendet.

## Konsequenzen

### Positiv

- Domain-Entities steuern das Datenbankschema (DDD-konform).
- Automatische Migrationen vereinfachen den Entwicklungsprozess.
- Versionierung des Schemas über Git (Migrations-Dateien).
- `localdb` oder Docker SQL Server für lokale Entwicklung.

### Negativ

- Automatische Migration bei App-Start kann in Produktion riskant sein
  (für MVP akzeptabel, für Enterprise separate Strategie nötig).
- Performance-Overhead durch ORM gegenüber Raw SQL (vernachlässigbar).

### Neutral

- Connection String wird über `appsettings.Development.json` und
  `dotnet user-secrets` verwaltet (siehe ADR-0110).

## Alternativen

| Alternative         | Pro                    | Contra                        | Entscheidung |
| ------------------- | ---------------------- | ----------------------------- | ------------ |
| EF Core Code-First  | konform, DDD       | ORM-Overhead                  | ✅ Gewählt   |
| EF Core DB-First    | Bestehende DB nutzbar  | Kein Code-First, aufwändiger  | ❌ Abgelehnt |
| Dapper (Micro-ORM)  | Performanter           | Kein EF, kein Code-First      | ❌ Abgelehnt |
| ADO.NET Raw         | Volle Kontrolle        | Viel Boilerplate, unsicher    | ❌ Abgelehnt |

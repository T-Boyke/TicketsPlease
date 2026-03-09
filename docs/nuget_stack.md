# 📦 Backend Library & NuGet Stack

Um die hochgesteckten Enterprise-Ziele (Clean Architecture, Resilience,
Security, TDD) unserer ASP.NET Core 10.3 Applikation zu erreichen,
definieren wir hier die exakten NuGet-Pakete pro Schicht.

Diese Liste dient als "Einkaufszettel" für die nächste Implementierungsphase.

## 1. 🟢 Domain Layer (Core)

In reiner Clean Architecture hat die Domain **keine** Abhängigkeiten zu
Infrastruktur-Paketen. Sie soll "nackt" sein.

* *Keine externen NuGet-Pakete (Zero-Dependency-Prinzip).*
* **`MediatR.Contracts` (Planned)**: Nur falls wir Domain-Events über das
  `INotification`-Interface von MediatR abwickeln wollen.

## 2. 🟡 Application Layer (Use Cases)

Hier liegt die Geschäftslogik, die von der UI oder APIs angestoßen wird.

* **`MediatR` (v12.4.1)**: Der De-facto Standard für C# CQRS (Command Query
  Responsibility Segregation). Das Herzstück, welches Commands und Queries aus
  dem Controller an die passenden Handler routet (In-Memory Message Bus).
* **`FluentValidation.DependencyInjectionExtensions` (v11.11.0)**: Typsichere,
  flüssige Validierungsregeln. Wird als *Pipeline Pipeline Behavior* direkt in
  MediatR eingeklinkt, sodass ungültige Commands gar nicht erst den Handler
  erreichen.
* **`Mapster` (v7.4.0)**: Zum blitzschnellen, code-reduzierten Mappen von Domain
  Entities in flache DTOs (Data Transfer Objects) oder ViewModels. *Empfehlung:
  Mapster, da es extrem perfomant ist und viel ohne Reflection (via
  Code-Generation) löst.*

## 3. 🔴 Infrastructure Layer (Persistence & Services)

Diese Schicht kümmert sich um die "Außenwelt" (Datenbanken, Dateisysteme, Mails).

* **`Microsoft.EntityFrameworkCore.SqlServer` (v10.0.3)**: Für die
  Datenbank-Kommunikation.
* **`Microsoft.AspNetCore.Identity.EntityFrameworkCore` (v10.0.3)**: Bindet das
  Rollen/User-System direkt in EF Core ein.
* **`MailKit` (v4.10.0)** / **`MimeKit` (v4.15.1)**: Der Industrie-Standard für
  das Versenden von E-Mails via SMTP (Zwingend benötigt für die
  **Teamlead-Broadcast-Mails** aus den Features).
* **`Polly` (v8.5.2)**: Eine Resilience und Transient-Fault-Handling Library.
  Falls die Datenbank kurz weg ist oder der Mail-Server stockt, kümmert sich Polly
  um automatisierte Retries (z.B. Exponential Backoff) und Circuit Breaking. Das
  ist Enterprise-Standard!
* **`Hangfire.AspNetCore` (v1.8.18)** & **`Hangfire.SqlServer` (v1.8.18)**:
  Für robuste, persistente Hintergrundaufgaben (Auto-Archivieren).
  Enthält ein eigenes Dashboard zur Überwachung der Jobs.
* **`Bogus` (v35.6.5)**: Erzeugung von Fake-Daten für Seeding und Tests.
* **`Newtonsoft.Json` (v13.0.3)**: Robuste JSON-Serialisierung (Infrastruktur-Support).
* **`Microsoft.Extensions.Caching.StackExchangeRedis` (Planned)**: Ermöglicht
  High-Performance Distributed Caching via Redis.
* **`AspNetCore.HealthChecks.SqlServer` (Planned)**: Überwacht die Vitalwerte
  der Applikation und stellt sie unter `/health` bereit.

## 4. 🔵 Presentation / Web Layer (MVC & API)

Die Eintrittspforte des Nutzers.

* **`Serilog.AspNetCore` (v9.0.0)**: Ersetzt den Standard-Microsoft-Logger
  durch **Structured Logging**. Logs (Fehler, Warnings) werden sauber im JSON
  Format weggeschrieben (perfekt für externe Tools wie Seq, Datadog oder
  ElasticSearch).
* **`Scalar.AspNetCore` (v2.0.18)**: Die modernere Alternative zu
  Swagger/NSwag. Bietet eine wunderschöne, interaktive API-Dokumentation.
* **`Asp.Versioning.Mvc.ApiExplorer` (v8.1.0)**: Ermöglicht eine saubere
  Versionierung der REST-Endpunkte (`/v1/`, `/v2/`).
* **`Microsoft.EntityFrameworkCore.Design` (v10.0.3)**: Wird für Code-First EF
  Migrations (`dotnet ef migrations add`) benötigt.
* **`Tailwind.MSBuild` (v2.0.2)**: Integriert Tailwind CSS direkt in den C#
  Build-Prozess ohne lokale Node.js Installation (Node-free).

## 5. 🧪 Testing Projects

Um die 100% Qualtity Gates in Github Actions zu erreichen:

* **`xunit` (v2.9.3)** & **`xunit.runner.visualstudio` (v3.1.4)**: Das
  Core-Testing Framework.
* **`FluentAssertions` (v8.8.0)**: Macht Asserts lesbar wie Englisch
  (`ticket.Title.Should().NotBeNullOrEmpty();`).
* **`Moq` / `NSubstitute` (Planned)**: Zum Mocken von Interfaces in Unit Tests.
* **`Microsoft.AspNetCore.Mvc.Testing` (v10.0.3)**: Infrastruktur für
  Integrationstests (WebApplicationFactory).
* **`Microsoft.EntityFrameworkCore.Sqlite` (v10.0.3)**: SQLite support für
  leichte Integrationstests (Optional neben Testcontainers).
* **`Testcontainers.MsSql` (Planned)**: **Absolute Highlight für
  Integration-Tests.** Anstatt einer fehleranfälligen lokalen
  In-Memory-Datenbank fährt Testcontainers vor jedem Testlauf unsichtbar einen
  winzigen, echten Microsoft SQL Docker-Container hoch, testet gegen die echte DB
  und wirft ihn danach weg.
* **`NetArchTest.Rules` (v1.3.2)**: Architektur-Tests! Ein Test, der
  fehlschlägt, falls ein Entwickler aus Versehen versucht, die
  Infrastructure-Schicht in der Domain-Schicht zu referenzieren.
* **`coverlet.collector` (v6.0.4)**: Code-Coverage Datensammler für CI.

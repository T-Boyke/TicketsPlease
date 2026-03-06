# 📦 Backend Library & NuGet Stack

Um die hochgesteckten Enterprise-Ziele (Clean Architecture, Resilience, Security, TDD) unserer ASP.NET Core 10.3 Applikation zu erreichen, definieren wir hier die exakten NuGet-Pakete pro Schicht.

Diese Liste dient als "Einkaufszettel" für die nächste Implementierungsphase.

## 1. 🟢 Domain Layer (Core)
In reiner Clean Architecture hat die Domain **keine** Abhängigkeiten zu Infrastruktur-Paketen. Sie soll "nackt" sein.
*   *Keine externen NuGet-Pakete (Zero-Dependency-Prinzip).*
*   *(Optional)* `MediatR.Contracts`: Nur falls wir Domain-Events über das `INotification`-Interface von MediatR abwickeln wollen.

## 2. 🟡 Application Layer (Use Cases)
Hier liegt die Geschäftslogik, die von der UI oder APIs angestoßen wird.
*   **`MediatR`**: Der De-facto Standard für C# CQRS (Command Query Responsibility Segregation). Das Herzstück, welches Commands und Queries aus dem Controller an die passenden Handler routet (In-Memory Message Bus).
*   **`FluentValidation.DependencyInjectionExtensions`**: Typsichere, flüssige Validierungsregeln. Wird als *Pipeline Pipeline Behavior* direkt in MediatR eingeklinkt, sodass ungültige Commands gar nicht erst den Handler erreichen.
*   **`Mapster` oder `AutoMapper`**: Zum blitzschnellen, code-reduzierten Mappen von Domain Entities in flache DTOs (Data Transfer Objects) oder ViewModels. *Empfehlung: Mapster, da es extrem perfomant ist und viel ohne Reflection (via Code-Generation) löst.*

## 3. 🔴 Infrastructure Layer (Persistence & Services)
Diese Schicht kümmert sich um die "Außenwelt" (Datenbanken, Dateisysteme, Mails).
*   **`Microsoft.EntityFrameworkCore.SqlServer`**: Für die Datenbank-Kommunikation.
*   **`Microsoft.AspNetCore.Identity.EntityFrameworkCore`**: Bindet das Rollen/User-System direkt in EF Core ein.
*   **`MailKit` / `MimeKit`**: Der Industrie-Standard für das Versenden von E-Mails via SMTP (Zwingend benötigt für die **Teamlead-Broadcast-Mails** aus den Features).
*   **`Polly`**: Eine Resilience und Transient-Fault-Handling Library. Falls die Datenbank kurz weg ist oder der Mail-Server stockt, kümmert sich Polly um automatisierte Retries (z.B. Exponential Backoff) und Circuit Breaking. Das ist Enterprise-Standard!
*   **`Hangfire.AspNetCore`** & **`Hangfire.SqlServer`**: Für robuste, persistente Hintergrundaufgaben (wie das Auto-Archivieren von Tickets). Enthält ein eigenes Dashboard zur Überwachung der Jobs.
*   **`Microsoft.Extensions.Caching.StackExchangeRedis`**: Ermöglicht High-Performance Distributed Caching via Redis.
*   **`AspNetCore.HealthChecks.SqlServer`** & **`AspNetCore.HealthChecks.UI`**: Überwacht die Vitalwerte der Applikation und stellt sie unter `/health` bereit.

## 4. 🔵 Presentation / Web Layer (MVC & API)
Die Eintrittspforte des Nutzers.
*   **`Serilog.AspNetCore`**: Ersetzt den Standard-Microsoft-Logger durch **Structured Logging**. Logs (Fehler, Warnings) werden sauber im JSON Format weggeschrieben (perfekt für externe Tools wie Seq, Datadog oder ElasticSearch).
*   **`Scalar.AspNetCore`**: Die modernere Alternative zu Swagger/NSwag. Bietet eine wunderschöne, interaktive API-Dokumentation.
*   **`Asp.Versioning.Mvc.ApiExplorer`**: Ermöglicht eine saubere Versionierung der REST-Endpunkte (`/v1/`, `/v2/`).
*   **`Microsoft.EntityFrameworkCore.Design`**: Wird für Code-First EF Migrations (`dotnet ef migrations add`) benötigt.
*   **`TailwindCSS.MSBuild`**: Integriert Tailwind CSS direkt in den C# Build-Prozess ohne lokale Node.js Installation (Node-free).

## 5. 🧪 Testing Projects
Um die 100% Qualtity Gates in Github Actions zu erreichen:
*   **`xunit` & `xunit.runner.visualstudio`**: Das Core-Testing Framework (bevorzugt über NUnit/MSTest).
*   **`FluentAssertions`**: Macht Asserts lesbar wie Englisch (`ticket.Title.Should().NotBeNullOrEmpty();`).
*   **`Moq` / `NSubstitute`**: Zum Mocken von Interfaces in Unit Tests.
*   **`Testcontainers.MsSql`**: **Das absolute Highlight für Integration-Tests.** Anstatt einer fehleranfälligen lokalen In-Memory-Datenbank fährt Testcontainers vor jedem Testlauf unsichtbar einen winzigen, echten Microsoft SQL Docker-Container hoch, testet gegen die echte DB und wirft ihn danach weg.
*   **`NetArchTest.Rules`**: Architektur-Tests! Ein Test, der fehlschlägt, falls ein Entwickler aus Versehen versucht, die Infrastructure-Schicht in der Domain-Schicht zu referenzieren.

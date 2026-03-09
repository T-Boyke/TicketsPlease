# 🛠️ TicketsPlease – Tech-Stack Referenz

Zusammenfassung der technischen Infrastruktur und erlaubten Abhängigkeiten.

## 📋 Table of Contents

- [🚀 Frameworks \& Core](#-frameworks--core)
- [📦 NuGet Stack (Strict Layers)](#-nuget-stack-strict-layers)
- [🎨 Frontend \& Assets](#-frontend--assets)

---

## 🚀 Frameworks & Core

- **Runtime:** .NET 10 / ASP.NET Core 10.3 / C# 14.
- **Architektur:** Clean Architecture (Onion) + DDD + CQRS.
- **Datenbank:** MS SQL Server mit EF Core Code-First.
- **Asynchronität:** Zwingende Nutzung von `CancellationToken`.

---

## 📦 NuGet Stack (Strict Layers)

Referenziert aus [nuget_stack.md](file:///d:/DEV/Tickets/docs/nuget_stack.md).

| Layer              | Erlaubte Packages (Auszug)                                      |
| ------------------ | --------------------------------------------------------------- |
| **Domain**         | `MediatR.Contracts` (Zero-Dependency Policy!)                   |
| **Application**    | `MediatR`, `FluentValidation`, `Mapster`                        |
| **Infrastructure** | `Microsoft.EntityFrameworkCore.SqlServer`, `Serilog`, `MailKit` |
| **Web**            | `Tailwind.Hosting`, `Markdig`, `DOMPurify`                      |

---

## 🎨 Frontend & Assets

- **CSS:** TailwindCSS 4.2 (Zero-Node Integration via MSBuild).
- **Icons:** FontAwesome 7.2 Pro (Local Assets).
- **LibMan:** Client-Library-Management in `wwwroot/lib` (No-CDN Policy).
- **Dynamic Theming:** `ICorporateSkinProvider` gesteuertes Design.

---

_TechRef v1.0 | 2026-03-09_

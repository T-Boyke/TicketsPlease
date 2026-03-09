# 💻 CLI Essentials

Zentrale CLI-Befehle und Werkzeuge für die TicketsPlease Entwicklung.

## 📋 Table of Contents

- [🚀 .NET CLI](#-net-cli)
- [🗄️ EF Core CLI](#-ef-core-cli)
- [🎨 Tailwind CLI](#-tailwind-cli)

---

## 🚀 .NET CLI

Standard-Operationen für die Solution:

- `dotnet build`: Kompiliert die gesamte Solution.
- `dotnet test`: Führt alle xUnit und Architecture Tests aus.
- `dotnet run --project src/TicketsPlease.Web`: Startet die Web-Applikation.
- `dotnet format`: Prüft und korrigiert Code-Style gemäß `.editorconfig`.

---

## 🗄️ EF Core CLI

Verwaltung der Datenbank-Migrationen:

- `dotnet ef migrations add [Name] --project src/TicketsPlease.Infrastructure --startup-project src/TicketsPlease.Web`
- `dotnet ef database update --project src/TicketsPlease.Infrastructure --startup-project src/TicketsPlease.Web`

---

## 🎨 Tailwind CLI

Das Projekt nutzt `tailwindcss-dotnet` (v2.0.0-beta.3) als lokales Tool:

- `dotnet tailwind init`: Initialisiert Tailwind (falls nötig).
- **Integration:** Läuft primär über MSBuild beim Build-Prozess der `TicketsPlease.Web`.

---

_CLIEssentials v1.0 | 2026-03-09_

# MVP-0001. ASP.NET Core MVC als Web-Framework

**Datum:** 2026-03-23

**Status:** Accepted

## Kontext

Die Aufgabe (F1.1) fordert eine Web-Anwendung, die mit ASP.NET Core ab
Version 8 umgesetzt wird und dem MVC-Muster folgt. Ein einheitliches Design
soll durch ein CSS-Framework gewährleistet werden.

Wir müssen entscheiden, welche Variante des ASP.NET Core Frameworks (MVC,
Razor Pages, Blazor, Minimal API) für das Ticketsystem eingesetzt wird.

## Entscheidung

Wir verwenden **ASP.NET Core 10 MVC** (Model-View-Controller) als primäres
Web-Framework mit Razor Views für die serverseitige HTML-Generierung.

## Konsequenzen

### Positiv

- Exakt die vom Prüfer geforderte MVC-Architektur.
- Serverseitiges Rendering: Kein JavaScript-Framework (React/Vue) nötig.
- Bewährtes Pattern mit klarer Trennung (Controller → View → Model).
- Perfekte Integration mit ASP.NET Core Identity für Auth (F1.3).

### Negativ

- Kein SPA-Erlebnis (Single Page Application) – jeder Seitenaufruf ist ein
  Full-Page-Reload (für MVP akzeptabel).
- Interaktive Features (Drag & Drop) erfordern zusätzliches JavaScript.

### Neutral

- Tailwind CSS 4.2.2 wird als CSS-Framework eingesetzt (siehe ADR-0040).

## Alternativen

| Alternative   | Pro                      | Contra                           | Entscheidung |
| ------------- | ------------------------ | -------------------------------- | ------------ |
| ASP.NET MVC   | konform, bewährt     | Full-Page-Reloads                | ✅ Gewählt    |
| Razor Pages   | Einfacher für CRUD-Views | Weniger Kontrolle, kein MVC      | ❌ Abgelehnt  |
| Blazor Server | SPA-Feeling              | Nicht gefordert, Komplexität | ❌ Abgelehnt  |
| Minimal API   | Leichtgewichtig          | Keine Views, nur API             | ❌ Abgelehnt  |

# TicketsPlease 🎫

Ein hochmodernes, kollaboratives und skalierbares Kanban-Ticketsystem,
entwickelt mit **C# 14, ASP.NET Core 10.3 und Entity Framework Core**.

> 💡 **Hintergrund & Entstehung:** Dieses Abschlussprojekt
> ("Einfaches Ticketsystem") basiert auf fundierten Vorarbeiten und
> Erfahrungswerten aus den Repositories
> [C-Sharp-OOP-Fundamentals](https://github.com/T-Boyke/C-Sharp-OOP-Fundamentals)
> und [C-Sharp-ASP-Fundamentals](https://github.com/T-Boyke/C-Sharp-ASP-Fundamentals).
> Es transformiert diese theoretischen Grundlagen nun in ein vollwertiges,
> cloud-ready Enterprise-Produkt.
> 👉 **Projektphasen:** Da der Auftrag "Einfaches Ticketsystem" lautet, trennen
> wir strikt zwischen dem **[IHK MVP-Kern](docs/MVP_Roadmap.md)** und der
> späteren Enterprise-Ausbaustufe!
> 🛠️ **Schnellstart:** Neu im Projekt? Folge der
> **[Einrichtungsanleitung für Dummies](docs/dev_setup_guide.md)**!
> 🤖 **AI-gestützte Entwicklung:** Dieses Projekt nutzt
> **Antigravity AI Skills** zur Automatisierung von Scaffolding, Reviews und
> Debugging. Siehe **[Antigravity Guide](docs/antigravity-guide.md)**.

---

<details>
<summary><strong>📋 Table of Contents (Inhaltsverzeichnis)</strong></summary>

- [TicketsPlease 🎫](#ticketsplease-)
  - [📊 Implementierungs-Status (MVP vs. Enterprise)](#-implementierungs-status-mvp-vs-enterprise)
  - [1. 🚀 Vision \& Projektziele](#1--vision--projektziele)
  - [2. 🛠️ Technologie-Stack](#2-️-technologie-stack)
  - [3. 📐 Architektur \& Design](#3--architektur--design)
    - [Domain-Driven Design (DDD)](#domain-driven-design-ddd)
    - [Clean Architecture (Onion / Hexagonal)](#clean-architecture-onion--hexagonal)
    - [SOLID, DRY, KISS \& YAGNI](#solid-dry-kiss--yagni)
    - [🧩 Enterprise Plugin Architektur (Extensibility)](#-enterprise-plugin-architektur-extensibility)
  - [4. 🎨 UI \& Frontend Spezifikation](#4--ui--frontend-spezifikation)
    - [Razor Architecture \& Partials (DRY)](#razor-architecture--partials-dry)
    - [Lokale Assets (No CDN Policy)](#lokale-assets-no-cdn-policy)
    - [🧭 Navigation \& Settings-Menü](#-navigation--settings-menü)
    - [Atomic CSS \& Utility First](#atomic-css--utility-first)
    - [♿ Barrierefreiheit (a11y) \& UX Standards](#-barrierefreiheit-a11y--ux-standards)
    - [🌗 Theme-Switching \& Dark Mode (UX)](#-theme-switching--dark-mode-ux)
  - [5. 🧩 Enterprise Features \& Funktionalitäten](#5--enterprise-features--funktionalitäten)
    - [🌐 Globalisierung \& Lokalisierung (I18N)](#-globalisierung--lokalisierung-i18n)
    - [🔐 Erweitertes Rollen \& Rechte System (RBAC)](#-erweitertes-rollen--rechte-system-rbac)
    - [💬 Kollaboration \& Kommunikation](#-kollaboration--kommunikation)
    - [👤 Erweiterte Benutzerprofile (IAM \& Registrierung)](#-erweiterte-benutzerprofile-iam--registrierung)
    - [👥 Teams \& Workspaces](#-teams--workspaces)
    - [🎫 Das Ticket-Core-Domain](#-das-ticket-core-domain)
    - [📋 Kanban Dashboard (Interaktiv)](#-kanban-dashboard-interaktiv)
    - [Enterprise Add-Ons (Phase 2-5) 🚀](#enterprise-add-ons-phase-2-5-)
  - [6. 🛡️ Code-Qualität \& Workflows](#6-️-code-qualität--workflows)
    - [Test-Driven Development (TDD) \& Quality Assurance](#test-driven-development-tdd--quality-assurance)
    - [💯 Google Lighthouse Tests (Performance \& SEO)](#-google-lighthouse-tests-performance--seo)
    - [Continuous Integration / Continuous Deployment (CI/CD)](#continuous-integration--continuous-deployment-cicd)
    - [🔍 Statische Code-Analyse \& Linter](#-statische-code-analyse--linter)
    - [🛡️ Enterprise Security \& Trust (Defense in Depth)](#️-enterprise-security--trust-defense-in-depth)
    - [Datenbank-Seeding](#datenbank-seeding)
    - [Extensive Dokumentation \& Architektur-Diagramme](#extensive-dokumentation--architektur-diagramme)
  - [7. 🤝 GitHub Etikette \& Branching Strategy](#7--github-etikette--branching-strategy)
  - [8. 🗺️ Implementierungs Roadmap (IHK)](#8-️-implementierungs-roadmap-ihk)
  - [📄 Lizenz \& Rechtliches](#-lizenz--rechtliches)

</details>

---

## 📊 Implementierungs-Status (MVP vs. Enterprise)

| Feature | Status | Beschreibung | Scope |
| :--- | :--- | :--- | :--- |
| **Core Entities** | ✅ Aktiv | 26 Enterprise-Entitäten (3NF) aktiv | MVP Context |
| **IAM** | ✅ Aktiv | Full Organization, User & Profile Mapping | MVP |
| **Kanban Board** | 🏗️ In Arbeit | Interaktives Board mit Drag & Drop | MVP |
| **Tailwind CSS v4** | ✅ Aktiv | Modernes Styling via nativem CLI | MVP |
| **Testing Infra** | ✅ Aktiv | Architektur- & Integrations-Tests | MVP |
| **AI Skills** | ✅ Aktiv | Automatisierung via Antigravity | MVP |
| **Team Support** | ✅ Aktiv | Persistent Teams & Member Mgt. | Enterprise |
| **SLA Policies** | ✅ Aktiv | Persistence Layer & Policies | Enterprise |
| **Plugin System** | 🗺️ Roadmap | Externe Module (Dynamic Loading) | Enterprise |

---

## 1. 🚀 Vision & Projektziele

<details>
<summary>Mehr erfahren über die Vision...</summary>

Dieses Projekt realisiert ein umfassendes Ticketverwaltungssystem mit
dynamischen Kanban-Boards, welches explizit als Abschlussprojekt des c# .net 10
kurses vom dozenten Tom Seelig gefordert und Der Gruppe bestehend aus Tobias
und x, y, Z konzipiert wurde.

Der kompromisslose Fokus liegt dabei nicht nur auf reiner Funktionalität,
sondern vor allem auf **exzellenter Softwarearchitektur**, **höchster
Code-Qualität** und **strikter Testabdeckung**. Ziel ist es, eine
Enterprise-grade Applikation zu schaffen, die als Referenz für moderne C#
Web-Entwicklung dient. Jeder Aspekt der Anwendung, vom Datenbank-Design bis zur
CI/CD-Pipeline, wird nach Best-Practices der Industrie umgesetzt und
dokumentiert.

</details>

---

## 2. 🛠️ Technologie-Stack

<details>
<summary>Mehr erfahren über die genutzten Technologien...</summary>

Das System nutzt einen hochmodernen und perfekt aufeinander abgestimmten Stack:

- **Entwicklungsumgebungen (IDEs):**
  - [Visual Studio 2026](https://visualstudio.microsoft.com/)
  - [JetBrains Rider 3.2026](https://www.jetbrains.com/rider/)
  - *(Exklusiver Support mit maßgeschneiderten `.editorconfig` und
    Plugin-Configs).*
- **Backend & Core (ASP.NET Core 10.3 / C# 14):**
  - [Offizielle C# Docs](https://learn.microsoft.com/en-us/dotnet/csharp/) |
    [ASP.NET Core Docs](https://learn.microsoft.com/en-us/aspnet/core/)
  - **Resilience:** EF Core mit `EnableRetryOnFailure` und expliziten
    Transaktions-Strategien.
  - **Concurrency:** Optimistische Nebenläufigkeitskontrolle via `RowVersion`
    (Timestamp) in allen Entitäten.
  - **Validation:** CQRS & Validation (MediatR, FluentValidation).
  - **Async Policy:** Zwingende Nutzung von `CancellationToken` in allen
    asynchronen Calls.
  - 👉 **[Detaillierte Backend-Library & NuGet Strategie](docs/nuget_stack.md)**
- **Datenbank & ORM:**
  - [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
    (Code-First Approach)
  - **Performance:** Strikte `AsNoTracking()` Policy für reine Lesezugriffe.
  - [Microsoft SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
  - *Siehe detailliertes [Datenbankschema (ERD)](docs/database_schema.md)*
- **Frontend & User Interface (UI):**
  - **No-Bootstrap Policy:** Das Projekt ist vollständig Bootstrap-frei für
    maximalen Control-Flow.
  - **TailwindCSS v4:** Neueste Evolution via nativem `tailwindcss.exe` (Kein
    Node.js / .NET 9 Runtime nötig!).
  - **Client-Side Libs:** Markdig (Markdown), SortableJS (Drag & Drop),
    FontAwesome 7.2.
  - **Corporate Identity:** Dynamisches Theming über `ICorporateSkinProvider`
    und CSS-Variablen.
  - Paketverwaltung via
    **[LibMan](https://learn.microsoft.com/en-us/aspnet/core/client-side/libman/libman-vs)**
    (`libman.json`).
- **Architektur-Pattern:**
  - [Clean Architecture (Onion)](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures#clean-architecture)
  - [Domain-Driven Design (DDD)](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/)
  - [CQRS (Command Query Responsibility Segregation)](https://learn.microsoft.com/en-us/azure/architecture/patterns/cqrs)
- **Testing-Frameworks:**

  - [xUnit](https://xunit.net/) (Unit/Integration)
  - **Architecture:** `NetArchTest.eXtend` für statische Regelprüfung.
  - **Integration:** `Microsoft.AspNetCore.Mvc.Testing` für API- & DB-Tests.
  - **Data Seeding:** [Bogus](https://github.com/bchavez/Bogus) für
    synthetische Testdaten (Locale: `de`).
  - [Playwright](https://playwright.dev/dotnet/) (E2E)
  - [Vitest](https://vitest.dev/) (JS-Frontend Tests)

</details>

---

## 3. 📐 Architektur & Design

<details open>
<summary>Architektur aufklappen...</summary>

Unsere Softwarearchitektur ist das Rückgrat des Systems. Sie sichert
Skalierbarkeit, Wartbarkeit und Erweiterbarkeit für die kommenden Jahre.

👉 **Für tiefergehende Details lesen Sie unsere [Architecture Decision Records (ADRs)](docs/adr).**

### Domain-Driven Design (DDD)

Das System nutzt striktes Domain-Driven Design (DDD) zur physischen und
logischen Trennung der Fachlichkeit. Dies verhindert "Spaghetti-Code" und sorgt
dafür, dass die Software exakt die Geschäftsrealität abbildet.

- **Bounded Contexts:** Wir definieren eindeutige Abgrenzungen der Domänen (z.B.
  *Identity & Access Context*, *Ticket Management Context*, *Team Collaboration
  Context*). Jeder Context hat sein eigenes, unantastbares Modell.
- **Ubiquitous Language:** Wir etablieren ein klares, gemeinsames Vokabular für
  Entwickler und Fachexperten. Ein "Ticket" heißt immer "Ticket", ein "Nutzer"
  immer "Nutzer". Diese Sprache spiegelt sich 1:1 im C# Code, in der Datenbank
  und in der UI wider.

### Clean Architecture (Onion / Hexagonal)

Wir erzwingen eine strikte Trennung von Zuständigkeiten
(Separation of Concerns). Abhängigkeiten zeigen *immer* nur nach innen in
Richtung der Domain-Schicht.

- **Domain Layer (Kern):** Beinhaltet ausschließlich Core-Geschäftsregeln,
  Entities (Ticket, User, Team) und Value Objects. Diese Schicht hat *null*
  Abhängigkeiten nach außen – weder zur Datenbank, noch zu Web-Technologien
  oder anderen Frameworks.
- **Application Layer:** Orchestriert die Use Cases der Anwendung. Nutzt
  Commands und Queries (CQRS-Pattern), um Lese- und Schreiboperationen strikt
  zu trennen. Definiert DTOs (Data Transfer Objects) und Interfaces für die
  Domäne.
- **Infrastructure Layer:** Implementiert die Interfaces aus der Application
  Layer. Hier leben die EF Core Repositories, Datenbank-Kontexte und die
  Anbindung an externe Dienste (z.B. Mail-Provider).
- **Web/Presentation Layer:** Die ASP.NET Core MVC/Razor-Komponenten,
  Controller, API-Endpoints und die darstellende Benutzeroberfläche.

### SOLID, DRY, KISS & YAGNI

Unser Code unterliegt höchsten Qualitätsstandards:

- **1 Class pro File:** Eine unverrückbare Regel. Jede C#-Klasse, jedes
  Interface und jedes Enum bekommt exakt eine Datei.
- **DRY (Don't Repeat Yourself):** Logik wird abstrahiert und wiederverwendet.
  Code-Duplikate werden im Code-Review rigoros abgelehnt.
- **KISS (Keep It Simple, Stupid) & YAGNI (You Aren't Gonna Need It):** Wir
  bauen keine komplexen Abstraktionen "für die Zukunft". Wir lösen das
  aktuelle Problem mit dem einfachsten, lesbarsten und verständlichsten Code.

### 🧩 Enterprise Plugin Architektur (Extensibility)

Das System ist zukunftssicher darauf ausgelegt, Features aus verschiedenen (auch
externen) Quellen nachzuladen, ohne den Core-Code anfassen zu müssen
(Offen-Geschlossen-Prinzip).

- **Abstrakte Plugin-Schnittstellen:** Im Core-Domain definieren wir Interfaces
  (z.B. `ITicketActionPlugin`, `INotificationProvider`), die von externen
  Modulen implementiert werden können.
- **Dynamisches Nachladen:** Geplant ist ein Architektur-Design, welches via
  Reflection (`Assembly.Load`) oder das C# `ManagedLoadContext` zur Laufzeit
  kompiliierte `.dll`-Dateien (Plugins) aus einem definierten Verzeichnis lädt
  und über Dependency Injection in die Applikation einklinkt.
- *Beispiele für spätere Plugins:* Externe Time-tracking-Tools (Toggl
  Integration), Custom-Auth-Provider (SAML/SSO), oder KI-gestützte
  Ticket-Zusammenfassungen.

</details>

---

## 4. 🎨 UI & Frontend Spezifikation

<details>
<summary>Frontend Details aufklappen...</summary>

Das Frontend muss blitzschnell laden, responsiv sein und eine herausragende
Developer Experience (DX) bieten. Besonderer Wert liegt auf einer **aufgeräumten
und extrem lesbaren CSHTML-Struktur**.

### Razor Architecture & Partials (DRY)

Wir nutzen eine streng modulare UI-Architektur. Mittels ASP.NET Core Razor
CSS-Isolation und dedizierten `ViewComponents` bündeln wir Template (HTML),
Logik (C#) und domänenspezifisches Styling (CSS) konsequent. Jede
UI-Komponente (Button, Card, Modal) ist völlig autark und kann ohne
Nebenwirkungen ausgetauscht oder verschoben werden.

Ziel ist es, den CSHTML-Code frei von C#-Business-Logik und ausufernden
CSS-Klassen zu halten (Trennung von Markup und Styling). Wir nutzen ein
zentrales **Barrel-File**-Konzept für das Frontend-Routing und den
Component-Export, um Import-Pfade sauber zu halten.

**Wichtige DRY-Regel:** Sobald sich ein kleines HTML-Konstrukt mehr als einmal
wiederholt (z.B. Ticket-Tags, User-Avatare), lagern wir es zwingend in
wiederverwendbare `<partial name="_Avatar" />` Views oder TagHelpers (wie einen
Custom `<icon />` Tag) aus!

### Lokale Assets (No CDN Policy)

Aus Gründen der Ausfallsicherheit, Performance und des Datenschutzes (DSGVO)
verwenden wir **keine Content Delivery Networks (CDNs)**. Sämtliche Libraries
(Tailwind, FontAwesome) werden vollständig lokal über den Microsoft Library
Manager (`libman.json`) in das Projektverzeichnis (`wwwroot/lib`) integriert.

👉 **Mehr dazu in unserer [Asset Management Strategie](docs/frontend_assets.md).**

### 🧭 Navigation & Settings-Menü

- **Dynamische Navbar:** Nach erfolgreichem Login wird in der
  Haupt-Navigation ein dediziertes "Settings" (Einstellungen) Menü sichtbar.
- **Rollenbasierte Ansichten:** Die Unterpunkte dieses Settings-Menüs filtern
  sich streng nach den Claims/Rollen des Nutzers.
  - **Benutzer (User / Teamlead):** Zugriff auf **"Mein Profil"**
    (Avatar-Upload, persönliche Daten), **"Sicherheit"** (Passwort ändern),
    **"Benachrichtigungen"** (E-Mail/App-Präferenzen) und **"Darstellung"**
    (Dark/Light Theme, Sprache/I18N, Zeitzone). *Teamleads* sehen zusätzlich
    die **"Team-Verwaltung"** für ihre zugewiesenen Squads.
  - **Administratoren (Admin / Owner):** Sehen zusätzlich zu den User-Settings
    die globale **"Benutzerverwaltung"** (Passwörter zurücksetzen), die
    **"Gruppen- & Rechteverwaltung"** (Rollen und Berechtigungen vergeben),
    **"Workflow-Konfiguration"** (Kanban-Spalten anpassen), **"SLA-Richtlinien"**,
    zentrales **"Tag-Management"**, globale **"System-Einstellungen"** (z.B.
    SMTP, Plugin-Verwaltung) und das systemweite **"Audit Log"** (Historie &
    Security Events).

### Atomic CSS & Utility First

Wir verfolgen strikt den Utility-First Ansatz, aber kapseln diesen sauber:

- **TailwindCSS 4.2:** Wir nutzen den JIT (Just-in-Time) Compiler von Tailwind
  für pfeilschnelles, utility-basiertes Design.
  [Offizielle Dokumentation](https://tailwindcss.com/docs).
- **FontAwesome 7.2:** Für Enterprise-Grade Vektor Icons.
  [Offizielle Icon-Suche](https://fontawesome.com/search).
- **Komponenten-CSS (@apply)::** Um das CSHTML nicht mit hundert
  Tailwind-Klassen pro DIV zu verschmutzen, abstrahieren wir wiederkehrende
  UI-Muster klassisch über `@apply`. Diese benutzerdefinierten, stark
  wiederverwendbaren CSS-Klassen liegen logisch getrennt im
  Frontend-Verzeichnis:
  - `/css/components/btn.css` (Alle Button-Variationen)
  - `/css/components/theme.css` (Color-Tokens und Typografie)
  - `/css/components/cards.css` (Struktur für Kanban-Cards)
  - `/css/components/form.css` (Inputs, Selects, Validation States)

### ♿ Barrierefreiheit (a11y) & UX Standards

Wir entwickeln kompromisslos nach dem **Barrierefreiheitsstärkungsgesetz
(BFSG)** und den strengen Richtlinien der [W3C ARIA Authoring Practices Guide
(APG)](https://www.w3.org/WAI/ARIA/apg/).

- **Keyboard-First Navigation:** Das gesamte Kanban-Board, alle Modals und
  Dropdowns müssen vollständig (und logisch) per `Tab`-Taste bedienbar sein.
  Focus-Traps in Modals sind Pflicht.
- **Semantik & ARIA:** Wir nutzen primär natives HTML5 (z.B. `<dialog>`,
  `<nav>`, `<main>`). Wo dies nicht ausreicht, setzen wir exakte
  `aria-`-Attribute ein (z.B. `aria-expanded`, `aria-describedby`), um
  Screenreadern den visuellen Kontext zu übersetzen.
- **Datensparsamkeit (DSGVO):** Strikte "Privacy by Design" Architektur in der
  Datenbank und im Session-Handling.

### 🌗 Theme-Switching & Dark Mode (UX)

Ein flüssiger Wechsel zwischen Dark-, Light- und System-Mode ist natives
Projekt-Feature.

- **CSS-Variablen (`theme.css`):** Anstatt Farben hart in Tailwind-Klassen zu
  kodieren (z.B. `bg-gray-800`), definieren wir in der `theme.css`
  CSS-Custom-Properties (z.B. `--color-surface`, `--color-primary`).
- **Tailwind-Integration:** Tailwind wird so konfiguriert, dass es diese
  CSS-Variablen konsumiert. Dies ermöglicht einen instantanen Theme-Wechsel
  (durch Tausch eines data-Attributes auf dem `<html>`-Tag), ganz ohne
  Page-Reload.

</details>

---

## 5. 🧩 Enterprise Features & Funktionalitäten

<details open>
<summary>Detaillierte Features aufklappen...</summary>

Das System deckt den gesamten Lifecycle einer modernen, kollaborativen
Enterprise-Ticketing-Lösung ab.

### 🌐 Globalisierung & Lokalisierung (I18N)

Die Applikation ist von Tag Eins an auf internationale Enterprise-Nutzung
ausgelegt. Wir implementieren die nativen [ASP.NET Core Localization
(I18N)](https://learn.microsoft.com/de-de/aspnet/core/fundamentals/localization)
Features:

- **Ressourcen-Dateien (`.resx`):** Sämtliche UI-Texte, Fehlermeldungen und
  E-Mail-Templates werden nicht hardcodiert, sondern über zentralisierte
  Ressourcen verwaltet.
- **Request Localization Middleware:** Das System erkennt die bevorzugte Sprache
  des Nutzers (über den `Accept-Language` HTTP-Header oder ein explizites
  User-Profile Setting) und wechselt die UI-Sprache fließend (z.B. zwischen
  Deutsch und Englisch).
- **Zeitzonen & Währungen:** Alle `DateTimeOffset` Werte werden
  nutzerspezifisch gerendert, Währungen/Zahlenformate werden der Kultur des
  Betrachters angepasst.

### 🔐 Erweitertes Rollen & Rechte System (RBAC)

Ein granulares, hierarchisches Berechtigungsobjekt-System steuert sämtliche
Zugriffe:

- **Owner (Systemherr):** Voller Zugriff, darf Admins ernennen, globale
  Systemeinstellungen ändern, Rechnungsdaten einsehen.
- **Admin:** Globale Benutzerverwaltung, Projekt- und Team-Anlage, Konfiguration
  von Workflows.
- **Moderator (Mod):** Konfliktlösung, Ticket-Bereinigung, Spam-Prävention
  (falls öffentliche Tickets aktiv).
- **Teamlead:** Kann innerhalb *seines* Teams User hinzufügen/entfernen,
  Workloads einsehen und Rundmails an das Team versenden.
- **User:** Der Standard-Entwickler/Bearbeiter. Darf Tickets anlegen,
  bearbeiten, Subtickets erstellen und kommentieren.

### 💬 Kollaboration & Kommunikation

- **Ticket-Kommentare:** Jedes Ticket besitzt einen dedizierten, chronologischen
  Kommentar-Thread. Teammitglieder können Lösungen diskutieren, Rückfragen
  stellen oder Assets anhängen.
- **Markdown & Mermaid Engine:** Überall, wo Text eingegeben wird
  (Ticket-Beschreibungen, Kommentare, Mails), wird vollständiges Markdown
  inklusive Mermaid.js (für Architekturdiagramme) gerendert.
- **Realtime Messaging System:** Nahtlose Integration von Direktnachrichten
  (1-to-1) und projektspezifischen Chat-Räumen für Teammitglieder (basierend auf
  SignalR/WebSockets).
- **Teamlead "Broadcast" Mails:** Teamleiter können offizielle Ankündigungen
  und Rundmails (formatiert in Markdown) direkt aus der Applikation an alle
  ihre Squad-Mitglieder senden.
- **Live Online-Status (Presence):** Grüne Indikatoren neben Profilbildern
  signalisieren in Echtzeit, ob ein Entwickler oder Ticket-Besitzer gerade in
  der Applikation aktiv ist.

### 👤 Erweiterte Benutzerprofile (IAM & Registrierung)

- **Zwingende Registrierungsdaten:** Bei der Kontoerstellung ist die Angabe von
  `Username`, `Vorname` (`FirstName`) und einer gültigen `Email` zwingend
  erforderlich (Hard Constraints in der Domain).
- Ausführliche Profile inklusive **Profilbild-Upload** (Avatar-Crop-Funktion),
  Kontaktdaten, Arbeitszeiten und abteilungsspezifischen Eigenschaften.
- Personalisierte Dashboards pro User (Was sind *meine* assigned Tickets?).

### 👥 Teams & Workspaces

- **Team-Erstellung:** Nutzer können eigene Teams/Squads formieren und mit
  Metadaten (Team-Kürzel, Farbe, Beschreibung) versehen.
- **Team-Management:** Einladungs- und Freigabeprozesse für den Beitritt zu
  bestehenden Teams.
- **Kollaboration:** Tickets können ganzen Teams (statt nur Einzelpersonen)
  zugewiesen werden.

### 🎫 Das Ticket-Core-Domain

Das Herzstück der Applikation. Ein Ticket ist ein hochkomplexes Objekt mit
folgenden Eigenschaften:

- **Stammdaten:** Eindeutige ID, referenzierbarer **SHA1-Hash** (zur
  systemweiten Identifizierung und Kopieren), Titel, ausführliche
  **Markdown-gestützte Beschreibung** (inkl. Mermaid-Graphen).
- **Audit & Tracking:** Jedes Ticket erfasst neben dem Zeitstempel auch
  zwingend einen **Geo/IP Timestamp** bei Erstellung und Bearbeitung aus
  Revisionsgründen.
- **Community Voting (Upvotes):** Entwickler und Teams können über Tickets
  abstimmen (Upvoting). Dies hilft Produktmanagern automatisiert zu erkennen,
  welche Features oder Bugs der Community aktuell am wichtigsten sind.
- **Zeitmanagement:** Startdatum, Deadline, geschätzter Aufwand und geloggte
  Arbeitszeit.
- **Priorisierung:** Skala (z.B. Low, Medium, High, Blocker) mit
  entsprechenden farblichen Indikatoren.
- **Die "Chillischoten"-Metrik 🌶️:** Eine visuelle, einzigartige Aufwands- und
  Schwierigkeitsbewertung (1 bis 5 Chillischoten), die auf einen Blick die
  Komplexität verdeutlicht, ohne trockene Zahlen zu verwenden.
- **Zuweisung (Assignees):** Flexibles Routing an Einzelpersonen, mehrere
  Nutzer oder komplette Teams.
- **Subtickets:** Unbegrenzte Schachtelung. Große Epics oder komplexe Tickets
  können granular in kleinere, abarbeitbare Subtickets (Tasks) unterteilt
  werden.

👉 **Exakte Definitionen der Ticket-Entität finden sich in der [Domain Model Dokumentation](docs/domain_ticket.md).**

### 📋 Kanban Dashboard (Interaktiv)

Das visuelle Zentrum der Produktivität.

- **Echtzeit-Board:** Interaktives Board, welches den Status (To Do, In
  Progress, Review, Done) visuell darstellt.
- **Custom Workflows:** Administratoren können die Spalten (Status-Stadien) des
  Boards pro Projekt anpassen.
- **Drag & Drop Matrix:** Native Drag & Drop-Funktionalität im Browser, mit der
  Tickets reibungslos durch die Workflow-Stadien gezogen werden können.

### Enterprise Add-Ons (Phase 2-5) 🚀

Unser System ist für den skalierten Betrieb konzipiert. Folgende Säulen stützen
die stabilen Phasen:

- **Observability & Monitoring:** Integrierte Endpunkte (`/health`) zur
  Echtzeit-Überwachung von DB, File-Storage und Mail-Server.
- **Infrastructure Excellence:**
  - **Hangfire / Quartz:** Persistentes Background-Processing für SLAs und
    Auto-Close.
  - **Redis Caching:** Distributed In-Memory Cache zur Minimierung von
    DB-Latenzen.
- **API Mastery:**
  - **Scalar Interactive Docs:** Eine interaktive API-Dokumentation für
    Drittanbieter.
  - **Semantic Versioning:** Versionierte API-Endpunkte für langfristige
    Stabilität.

</details>

---

## 6. 🛡️ Code-Qualität & Workflows

<details open>
<summary>Qualitätsstandards aufklappen...</summary>

Wir betrachten Code nicht als bloßen Text, sondern als beständiges Handwerk.

### Test-Driven Development (TDD) & Quality Assurance

Tests sind in diesem Projekt kein Nachgedanke, sondern treiben das Design. Wir
verfolgen den konsequenten **Red-Green-Refactor**-Zyklus.

- **100% Test Coverage-Ziel für die Domain:** Die Domain-Logik (Kern) duldet
  Zero Compromise. Jede Regel muss getestet sein.
- **Unit Tests:** Fokussiert auf Systemdienste, Helferklassen und die reinen
  Domain-Entities (geschrieben mit xUnit und Moq).
- **Integration Tests:** Validieren das Zusammenspiel mit der Datenbank (EF
  Core In-Memory oder Testcontainers) und testen komplette
  API-Routen/Controller.
- **E2E (End-to-End) Tests:** Mittels Playwright und Vitest automatisieren wir
  Browser-Klicks, um kritische User-Journeys (z.B. "Neues Ticket anlegen und
  auf Done schieben") abzusichern.

### 💯 Google Lighthouse Tests (Performance & SEO)

Als echtes Enterprise-Produkt muss das Frontend kompromisslos performen und
zugänglich sein.

- **Die CI/CD-Pipeline (GitHub Actions)** bricht ab, wenn der automatische
  **Google Lighthouse Score** in den Kategorien Performance, Accessibility,
  Best Practices und SEO unter **100/100** fällt.
- **Mobile & Desktop First:** Die Audits werden isoliert für *Desktop*
  (Breitbild Kanban) und stark optimiertem *Mobile* Viewport (Responsive
  Listen/Karten) durchlaufen.

### Continuous Integration / Continuous Deployment (CI/CD)

Wir verwenden **GitHub Actions**, um jegliche menschliche Fehler beim
Deployment auszuschließen.

- **The Pipeline:** Jeder Commit und jeder Pull Request triggert automatisch
  den Build-Prozess.
- **Quality Gates:** Der Build bricht sofort ab (Red Build), wenn:
  1. Der Code nicht kompiliert.
  2. Das Code-Formatting (Linting via `dotnet format`) von der `.editorconfig`
     abweicht.
  3. Auch nur ein einziger Test fehlschlägt.

### 🔍 Statische Code-Analyse & Linter

Wir verlassen uns nicht auf guten Willen, sondern auf harte Werkzeuge. Die in
Rider und VS 2026 integrierten **Roslyn Analyzer** steuern die Code-Qualität in
Echtzeit:

- Die Projektweite `.editorconfig` erzwingt Naming-Conventions (z.B. Interfaces
  *müssen* mit `I` beginnen, private Felder mit `_`).
- Regeln wie `IDE0005` (Unused Imports) sind auf `Error` oder `Warning`
  gestellt. Warnungen im Build werden im PR Review wie Fehler behandelt.
- Code-Smells werden von der IDE rot unterstrichen und in der CI-Pipeline
  geblockt.

### 🛡️ Enterprise Security & Trust (Defense in Depth)

Wir sichern die Applikation nach dem "Defense in Depth" (Zwiebelschalen)
Prinzip ab. Wenn eine Schicht kompromittiert wird, hält die Nächste dem
Angriff stand.

- **Secret Management:** Sensible Daten (Connection Strings, JWT-Keys,
  API-Tokens) werden **niemals** im Git-Repository via `appsettings.json`
  committet.
  - *Lokale Entwicklung:* Wir nutzen strikt den [ASP.NET Core Secret Manager
    `dotnet user-secrets`](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets).
  - *Produktion:* Integration von Systemen wie Azure Key Vault oder AWS Secrets
    Manager.
- **Data Protection & Hashing:** Passwörter werden mittels ASP.NET Core
  Identity (Pbkdf2/Argon2Id) gehasht. Cookies zwingend mit `HttpOnly` und
  `Secure` Flag versehen.
- **Input-Validation:** Kein User-Input erreicht die Business-Logik ungeprüft.
  FluentValidation und Anti-Forgery Tokens (XSRF/CSRF) sind in jedem
  Post-Request aktiv (und DOMPurify im Frontend, siehe Assets).

### Datenbank-Seeding

Im Entwicklungsmodus (`Development`) wird die Datenbank automatisch mit
realistischen, synthetischen deutschen Daten gefüllt, falls sie leer ist. Dies
wird durch die `Bogus` Bibliothek realisiert.

- **Lokalisierung:** `de` (Deutsch)
- **Datenschutz:** 100% synthetische Daten gemäß [ADR 0120](docs/adr/0120-synthetic-data-privacy.md).
- **Umfang:** Generiert automatisch Organizations, Roles, Users (200),
  Profiles, Addresses, Teams (15), Tickets (500), SubTickets (100) &
  Messages (300).

---

### Extensive Dokumentation & Architektur-Diagramme

Dokumentation veraltet nicht, wenn sie automatisiert und systematischer
Bestandteil des Workflows ist.

- **Architektur-Graphen & Diagramme:** Wir nutzen Mermaid-Diagramme für alle
  komplexen Systeme. Diese finden sich gesammelt unter:
  - 👉 **[System Architecture & Data-Flows (Big 5 UML)](docs/architecture_diagrams.md)**
  - 👉 **[Datenbankschema & Entities (ERD 3NF)](docs/database_schema.md)**
  - 👉 **[Projekt Gantt-Chart (IHK Roadmap)](docs/gantt_roadmap.md)**
- **Mockups & Screenshots:** Initiale Planungs-Mockups, UI-Entwürfe und finale
  Screenshots (z.B. für die IHK Präsentation) werden versioniert im Ordner
  **`/docs/mockups/`** abgelegt.
- **Grafiken & Assets:** Wenn im Rahmen des UI-Designs (Frontend) oder der
  Dokumentation temporäre Platzhalter-Bilder benötigt werden, nutzen wir
  vorerst [Placehold.co](https://placehold.co/) (Open Source SVG Platzhalter).
  Echte, physisch benötigte Grafiken (Logos, Icons) kommen in den Ordner
  **`/docs/assets/`** bzw. später nach `wwwroot/images/`.
- **Architectural Decision Records (ADR):** Alle wesentlichen
  Design-Entscheidungen (Architektur, Stack, Security) sind revisionssicher im
  **[ADR-Index](docs/adr/readme.md)** dokumentiert.

</details>

---

## 7. 🤝 GitHub Etikette & Branching Strategy

<details open>
<summary>Git Guidelines aufklappen...</summary>

Um bei mehreren Entwicklern Chaos zu vermeiden, herrschen strikte Git-Regeln:

- **Der `master` / `main` Branch ist HEILIG.**
  - Der Master-Branch muss zu **jeder Zeit lauffähig** sein (Compilable &
    Green Tests).
  - Niemals wird direkt in den Master-Branch gepusht (Pushes sind per
    Branch-Protection gesperrt!).
- **Feature Branching:**
  - Jedes neue Feature, jeder Bugfix beginnt mit dem Auschecken des aktuellen
    Masters: `git checkout -b feature/meine-coole-funktion`
  - Wir nutzen sprechende Präfixe: `feature/xyz`, `bugfix/xyz`, `hotfix/xyz`,
    `docs/xyz`.
- **Pull Requests (PRs):**
  - Features werden ausschließlich über Pull Requests in den Master gemerged.
  - Ein PR benötigt zwingend das grüne Licht der **CI/CD Pipeline**
    (Tests bestanden).
  - Ein PR benötigt zwingend ein **Code Review** (Approve) durch mindestens
    einen anderen Entwickler.
- **Issue Tracking:** Keine Code-Änderung ohne zugehöriges GitHub Issue.

</details>

---

## 8. 🗺️ Implementierungs Roadmap (IHK)

<details open>
<summary>Roadmap & Meilensteine (IHK) aufklappen...</summary>

Dieser Fahrplan strukturiert die Entwicklung chronologisch bis zur finalen Abgabe.

1. **Phase 1: Planung & Setup (Aktuell)**
   - UI/UX Planung und Erstellung grundlegender Sketche / Wireframes.
   - Team Onboarding in GitHub (Rechtevergabe, Branch Protections einrichten).
   - Erstellung des **GitHub Projects Boards** (Zentrale Projektverwaltung).
   - Anlage der initialen Epics, Tickets und Subtickets im GitHub Board.
   - Initiale Solution & Clean Architecture (.NET Projektmappen anlegen).
2. **Phase 2: CI/CD & Groundwork**
   - Aufsetzen der **GitHub Actions Pipeline** (Build, Test, Lint).
   - Setup Environment: EF Core konfigurieren, MSSQL Anbindung herstellen.
   - Libman & Tailwind-CLI für Frontend Assets einrichten.
3. **Phase 3: Domain Modeling & IAM**
   - User, Profil und Team Models (Entities) präzise ausarbeiten.
   - ASP.NET Core Identity & Auth implementieren.
   - Datenbank-Migrationen (Code-First) ausführen.
4. **Phase 4: UI Foundation & Theming**
   - Erstellung der globalen CSS-Architektur (`btn.css`, `cards.css`, `theme.css`).
   - Entwicklung der SFC (Single File Components) Layout-Frames und Navigation.
5. **Phase 5: Ticket Engine & Business Logic**
   - Ticket & Subticket Domains mitsamt Validierungslogik integrieren.
   - Repositories und CQRS-Commands implementieren (TDD First!).
6. **Phase 6: Kanban & Interaktivität**
   - Drag & Drop Dashboard entwickeln.
   - Die visuelle "Chillischoten"-Anzeige rendern.
   - Workflow-Logik (Status-Übergänge der Tickets) festigen.
7. **Phase 7: Finale & Dokumentation (IHK Abgabe)**
   - **FINALE IHK-DOKU:** Ausarbeitung der Projektarbeit im Google Docs Form.
   - Erstellung der finalen Präsentation für das Fachgespräch (Google Slides).
   - Letzter Review-Zyklus: Test Coverage Check und Code Cleanup.

</details>

---

## 📄 Lizenz & Rechtliches

Dieses Projekt unterliegt spezifischen Lizenzbestimmungen, die durch einen
automatisierten GitHub-Action-Job (License Header Check) in allen Quelldateien
validiert werden. Weitere Informationen zur Nutzung und Distribution finden
sich in der `LICENSE` Datei im Hauptverzeichnis.

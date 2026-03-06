# TicketsPlease 🎫

Ein hochmodernes, skalierbares Ticket- und Kanban-System. Entwickelt mit modernsten Architektur-Prinzipien, kompromissloser Code-Qualität und einer exzellenten Developer Experience.

---

<details>
<summary><strong>📋 Table of Contents (Inhaltsverzeichnis)</strong></summary>

- [1. 🚀 Vision & Projektziele](#1--vision--projektziele)
- [2. 🛠️ Technologie-Stack](#2-️-technologie-stack)
- [3. 📐 Architektur & Design](#3--architektur--design)
    - [Domain-Driven Design (DDD)](#domain-driven-design-ddd)
    - [Clean Architecture](#clean-architecture-onion--hexagonal)
    - [Clean Code Prinzipien](#solid-dry-kiss--yagni)
- [4. 🎨 UI & Frontend Spezifikation](#4--ui--frontend-spezifikation)
    - [Single File Components (SFC)](#single-file-components-sfc)
    - [Atomic CSS & Utility First](#atomic-css--utility-first)
- [5. 🧩 Features & Funktionalitäten](#5--features--funktionalitäten)
    - [Benutzer & Rollen (IAM)](#-benutzer--rollen-iam)
    - [Teams](#-teams)
    - [Tickets & Subtickets](#-tickets--subtickets)
    - [Kanban Dashboard](#-kanban-dashboard)
- [6. 🛡️ Code-Qualität & Workflows](#6-️-code-qualität--workflows)
    - [Test-Driven Development (TDD)](#test-driven-development-tdd)
    - [Continuous Integration / Continuous Deployment (CI/CD)](#continuous-integration--continuous-deployment-cicd)
    - [Extensive Dokumentation](#extensive-dokumentation)
- [7. 🤝 GitHub Etikette & Branching Strategy](#7--github-etikette--branching-strategy)
- [8. 🗺️ Implementierungs Roadmap (IHK)](#8-️-implementierungs-roadmap-ihk)

</details>

---

## 1. 🚀 Vision & Projektziele

<details>
<summary>Mehr erfahren über die Vision...</summary>

Dieses Projekt realisiert ein umfassendes Ticketverwaltungssystem mit dynamischen Kanban-Boards, welches explizit als Abschlussprojekt des c# .net 10 kurses vom dozenten Tom Seelig gefordert und Der Gruppe bestehend aus Tobias und x, y, Z  konzipiert wurde. 

Der kompromisslose Fokus liegt dabei nicht nur auf reiner Funktionalität, sondern vor allem auf **exzellenter Softwarearchitektur**, **höchster Code-Qualität** und **strikter Testabdeckung**. Ziel ist es, eine Enterprise-grade Applikation zu schaffen, die als Referenz für moderne C# Web-Entwicklung dient. Jeder Aspekt der Anwendung, vom Datenbank-Design bis zur CI/CD-Pipeline, wird nach Best-Practices der Industrie umgesetzt und dokumentiert.

</details>

---

## 2. 🛠️ Technologie-Stack

<details>
<summary>Mehr erfahren über die genutzten Technologien...</summary>

Das System nutzt einen hochmodernen und perfekt aufeinander abgestimmten Stack:

*   **Entwicklungsumgebungen (IDEs):** Visual Studio 2026 & JetBrains Rider 3.2026 (Exklusiver Support mit maßgeschneiderten `editorconfig` und Plugin-Configs).
*   **Backend & Core:** C# 14, ASP.NET Core 10.3
*   **Datenbank & ORM:** Entity Framework Core (Code-First Approach), Microsoft SQL Server
    *   *Siehe detailliertes [Datenbankschema (ERD)](docs/database_schema.md)*
*   **Frontend & User Interface (UI):**
    *   Lokales **TailwindCSS 4.2** für blitzschnelles Styling.
    *   Lokales **FontAwesome 7.2** für skalierbare Vektor-Icons.
    *   Paketverwaltung für alle statischen Client-Bibliotheken via **libman** (`libman.json`), komplett ohne externe CDNs für maximale Performance und DSGVO-Konformität.
*   **Architektur-Pattern:** Clean Architecture (Onion), Domain-Driven Design (DDD), CQRS (Command Query Responsibility Segregation).
*   **Testing-Frameworks:** xUnit (Unit/Integration), Playwright, Vitest (E2E).

</details>

---

## 3. 📐 Architektur & Design

<details open>
<summary>Architektur aufklappen...</summary>

Unsere Softwarearchitektur ist das Rückgrat des Systems. Sie sichert Skalierbarkeit, Wartbarkeit und Erweiterbarkeit für die kommenden Jahre.

👉 **Für tiefergehende Details lesen Sie unsere [Architecture Decision Records (ADRs)](docs/adr).**

### Domain-Driven Design (DDD)
Das System nutzt striktes Domain-Driven Design (DDD) zur physischen und logischen Trennung der Fachlichkeit. Dies verhindert "Spaghetti-Code" und sorgt dafür, dass die Software exakt die Geschäftsrealität abbildet.
*   **Bounded Contexts:** Wir definieren eindeutige Abgrenzungen der Domänen (z.B. *Identity & Access Context*, *Ticket Management Context*, *Team Collaboration Context*). Jeder Context hat sein eigenes, unantastbares Modell.
*   **Ubiquitous Language:** Wir etablieren ein klares, gemeinsames Vokabular für Entwickler und Fachexperten. Ein "Ticket" heißt immer "Ticket", ein "Nutzer" immer "Nutzer". Diese Sprache spiegelt sich 1:1 im C# Code, in der Datenbank und in der UI wider.

### Clean Architecture (Onion / Hexagonal)
Wir erzwingen eine strikte Trennung von Zuständigkeiten (Separation of Concerns). Abhängigkeiten zeigen *immer* nur nach innen in Richtung der Domain-Schicht.
*   **Domain Layer (Kern):** Beinhaltet ausschließlich Core-Geschäftsregeln, Entities (Ticket, User, Team) und Value Objects. Diese Schicht hat *null* Abhängigkeiten nach außen – weder zur Datenbank, noch zu Web-Technologien oder anderen Frameworks. 
*   **Application Layer:** Orchestriert die Use Cases der Anwendung. Nutzt Commands und Queries (CQRS-Pattern), um Lese- und Schreiboperationen strikt zu trennen. Definiert DTOs (Data Transfer Objects) und Interfaces für die Domäne.
*   **Infrastructure Layer:** Implementiert die Interfaces aus der Application Layer. Hier leben die EF Core Repositories, Datenbank-Kontexte und die Anbindung an externe Dienste (z.B. Mail-Provider).
*   **Web/Presentation Layer:** Die ASP.NET Core MVC/Razor-Komponenten, Controller, API-Endpoints und die darstellende Benutzeroberfläche. 

### SOLID, DRY, KISS & YAGNI
Unser Code unterliegt höchsten Qualitätsstandards:
*   **1 Class pro File:** Eine unverrückbare Regel. Jede C#-Klasse, jedes Interface und jedes Enum bekommt exakt eine Datei. 
*   **DRY (Don't Repeat Yourself):** Logik wird abstrahiert und wiederverwendet. Code-Duplikate werden im Code-Review rigoros abgelehnt.
*   **KISS (Keep It Simple, Stupid) & YAGNI (You Aren't Gonna Need It):** Wir bauen keine komplexen Abstraktionen "für die Zukunft". Wir lösen das aktuelle Problem mit dem einfachsten, lesbarsten und verständlichsten Code.

</details>

---

## 4. 🎨 UI & Frontend Spezifikation

<details>
<summary>Frontend Details aufklappen...</summary>

Das Frontend muss blitzschnell laden, responsiv sein und eine herausragende Developer Experience (DX) bieten. Besonderer Wert liegt auf einer **aufgeräumten und extrem lesbaren CSHTML-Struktur**.

### Single File Components (SFC) im ASP.NET Core
Wir nutzen eine streng modulare UI-Architektur. Mittels ASP.NET Core Razor CSS-Isolation und dedizierten `ViewComponents` bündeln wir Template (HTML), Logik (C#) und domänenspezifisches Styling (CSS) konsequent. Jede UI-Komponente (Button, Card, Modal) ist völlig autark und kann ohne Nebenwirkungen ausgetauscht oder verschoben werden.
Ziel ist es, den CSHTML-Code frei von C#-Business-Logik und ausufernden CSS-Klassen zu halten (Trennung von Markup und Styling). Wir nutzen ein zentrales **Barrel-File**-Konzept für das Frontend-Routing und den Component-Export, um Import-Pfade sauber zu halten.

### Lokale Assets (No CDN Policy)
Aus Gründen der Ausfallsicherheit, Performance und des Datenschutzes (DSGVO) verwenden wir **keine Content Delivery Networks (CDNs)**. Sämtliche Libraries (Tailwind, FontAwesome) werden vollständig lokal über den Microsoft Library Manager (`libman.json`) in das Projektverzeichnis (`wwwroot/lib`) integriert.

👉 **Mehr dazu in unserer [Asset Management Strategie](docs/frontend_assets.md).**

### Atomic CSS & Utility First
Wir verfolgen strikt den Utility-First Ansatz, aber kapseln diesen sauber:
*   **TailwindCSS 4.2:** Wir nutzen den JIT (Just-in-Time) Compiler von Tailwind für pfeilschnelles, utility-basiertes Design. [Offizielle Dokumentation](https://tailwindcss.com/docs).
*   **FontAwesome 7.2:** Für Enterprise-Grade Vektor Icons. [Offizielle Icon-Suche](https://fontawesome.com/search).
*   **Komponenten-CSS (@apply):** Um das CSHTML nicht mit hundert Tailwind-Klassen pro DIV zu verschmutzen, abstrahieren wir wiederkehrende UI-Muster klassisch über `@apply`. Diese benutzerdefinierten, stark wiederverwendbaren CSS-Klassen liegen logisch getrennt im Frontend-Verzeichnis:
    *   `/css/components/btn.css` (Alle Button-Variationen)
    *   `/css/components/theme.css` (Color-Tokens und Typografie)
    *   `/css/components/cards.css` (Struktur für Kanban-Cards)
    *   `/css/components/form.css` (Inputs, Selects, Validation States)

### ♿ Barrierefreiheit (a11y) & DSGVO Compliance
Die Applikation wird von Grund auf nach dem **Barrierefreiheitsstärkungsgesetz (BFSG)** sowie der **DSGVO** entwickelt:
*   Vollständige Tastaturnavigation im Kanban-Board.
*   Korrekte ARIA-Labels und semantisches HTML5 für Screenreader.
*   Hohe Kontrastwerte im UI-Design (Theme-System).
*   Datensparsamkeit und strikte "Privacy by Design" Architektur in der Datenbank.

</details>

---

## 5. 🧩 Enterprise Features & Funktionalitäten

<details open>
<summary>Detaillierte Features aufklappen...</summary>

Das System deckt den gesamten Lifecycle einer modernen, kollaborativen Enterprise-Ticketing-Lösung ab.

### 🔐 Erweitertes Rollen & Rechte System (RBAC)
Ein granulares, hierarchisches Berechtigungsobjekt-System steuert sämtliche Zugriffe:
*   **Owner (Systemherr):** Voller Zugriff, darf Admins ernennen, globale Systemeinstellungen ändern, Rechnungsdaten einsehen.
*   **Admin:** Globale Benutzerverwaltung, Projekt- und Team-Anlage, Konfiguration von Workflows.
*   **Moderator (Mod):** Konfliktlösung, Ticket-Bereinigung, Spam-Prävention (falls öffentliche Tickets aktiv).
*   **Teamlead:** Kann innerhalb *seines* Teams User hinzufügen/entfernen, Workloads einsehen und Rundmails an das Team versenden.
*   **User:** Der Standard-Entwickler/Bearbeiter. Darf Tickets anlegen, bearbeiten, Subtickets erstellen und kommentieren.

### 💬 Kollaboration & Kommunikation
*   **Markdown & Mermaid Engine:** Überall, wo Text eingegeben wird (Ticket-Beschreibungen, Kommentare, Mails), wird vollständiges Markdown inklusive Mermaid.js (für Architekturdiagramme) gerendert.
*   **Realtime Messaging System:** Nahtlose Integration von Direktnachrichten (1-to-1) und projektspezifischen Chat-Räumen für Teammitglieder (basierend auf SignalR/WebSockets).
*   **Teamlead "Broadcast" Mails:** Teamleiter können offizielle Ankündigungen und Rundmails (formatiert in Markdown) direkt aus der Applikation an alle ihre Squad-Mitglieder senden.
*   **Live Online-Status (Presence):** Grüne Indikatoren neben Profilbildern signalisieren in Echtzeit, ob ein Entwickler oder Ticket-Besitzer gerade in der Applikation aktiv ist.

### 👤 Erweiterte Benutzerprofile
*   Ausführliche Profile inklusive **Profilbild-Upload** (Avatar-Crop-Funktion), Kontaktdaten, Arbeitszeiten und abteilungsspezifischen Eigenschaften.
*   Personalisierte Dashboards pro User (Was sind *meine* assigned Tickets?).

### 👥 Teams & Workspaces
*   **Team-Erstellung:** Nutzer können eigene Teams/Squads formieren und mit Metadaten (Team-Kürzel, Farbe, Beschreibung) versehen.
*   **Team-Management:** Einladungs- und Freigabeprozesse für den Beitritt zu bestehenden Teams.
*   **Kollaboration:** Tickets können ganzen Teams (statt nur Einzelpersonen) zugewiesen werden.

### 🎫 Das Ticket-Core-Domain
Das Herzstück der Applikation. Ein Ticket ist ein hochkomplexes Objekt mit folgenden Eigenschaften:
*   **Stammdaten:** Eindeutige ID, Titel, ausführliche **Markdown-gestützte Beschreibung** (inkl. Mermaid-Graphen).
*   **Zeitmanagement:** Startdatum, Deadline, geschätzter Aufwand und geloggte Arbeitszeit.
*   **Priorisierung:** Skala (z.B. Low, Medium, High, Blocker) mit entsprechenden farblichen Indikatoren.
*   **Die "Chillischoten"-Metrik 🌶️:** Eine visuelle, einzigartige Aufwands- und Schwierigkeitsbewertung (1 bis 5 Chillischoten), die auf einen Blick die Komplexität verdeutlicht, ohne trockene Zahlen zu verwenden.
*   **Zuweisung (Assignees):** Flexibles Routing an Einzelpersonen, mehrere Nutzer oder komplette Teams.
*   **Subtickets:** Unbegrenzte Schachtelung. Große Epics oder komplexe Tickets können granular in kleinere, abarbeitbare Subtickets (Tasks) unterteilt werden.

👉 **Exakte Definitionen der Ticket-Entität finden sich in der [Domain Model Dokumentation](docs/domain_ticket.md).**

### 📋 Kanban Dashboard (Interaktiv)
Das visuelle Zentrum der Produktivität.
*   **Echtzeit-Board:** Interaktives Board, welches den Status (To Do, In Progress, Review, Done) visuell darstellt.
*   **Custom Workflows:** Administratoren können die Spalten (Status-Stadien) des Boards pro Projekt anpassen.
*   **Drag & Drop Matrix:** Native Drag & Drop-Funktionalität im Browser, mit der Tickets reibungslos durch die Workflow-Stadien gezogen werden können.

</details>

---

## 6. 🛡️ Code-Qualität & Workflows

<details open>
<summary>Qualitätsstandards aufklappen...</summary>

Wir betrachten Code nicht als bloßen Text, sondern als beständiges Handwerk.

### Test-Driven Development (TDD)
Tests sind in diesem Projekt kein Nachgedanke, sondern treiben das Design. Wir verfolgen den konsequenten **Red-Green-Refactor**-Zyklus. 
*   **100% Test Coverage-Ziel für die Domain:** Die Domain-Logik (Kern) duldet Zero Compromise. Jede Regel muss getestet sein.
*   **Unit Tests:** Fokussiert auf Systemdienste, Helferklassen und die reinen Domain-Entities (geschrieben mit xUnit und Moq).
*   **Integration Tests:** Validieren das Zusammenspiel mit der Datenbank (EF Core In-Memory oder Testcontainers) und testen komplette API-Routen/Controller.
*   **E2E (End-to-End) Tests:** Mittels Playwright und Vitest automatisieren wir Browser-Klicks, um kritische User-Journeys (z.B. "Neues Ticket anlegen und auf Done schieben") abzusichern.

### Continuous Integration / Continuous Deployment (CI/CD)
Wir verwenden **GitHub Actions**, um jegliche menschliche Fehler beim Deployment auszuschließen.
*   **Die Pipeline:** Jeder Commit und jeder Pull Request triggert automatisch den Build-Prozess.
*   **Quality Gates:** Der Build bricht sofort ab (Red Build), wenn:
    1.  Der Code nicht kompiliert.
    2.  Das Code-Formatting (Linting via `dotnet format`) von der `.editorconfig` abweicht.
    3.  Auch nur ein einziger Test fehlschlägt.

### Extensive Dokumentation
Dokumentation veraltet nicht, wenn sie automatisiert und systematischer Bestandteil des Workflows ist.
*   **Self-Documenting Code:** Präzise, selbsterklärende Namensgebung (Variablen, Methoden) macht 80% aller Kommentare überflüssig. 
*   **XML-Kommentare (JavaDoc Style):** Für alle öffentlichen Klassen, Interfaces (`IxyzService`) und komplexe Methoden nutzen wir zwingend die [C# XML-Dokumentationskommentare](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments). Dies generiert perfekten IntelliSense-Support.
*   **Architectural Decision Records (ADR):** Wir nutzen den Ordner `/docs/adr/`, um gravierende Architektur-Entscheidungen (Warum EF Core? Warum Tailwind?) historisch logisch festzuhalten.

</details>

---

## 7. 🤝 GitHub Etikette & Branching Strategy

<details open>
<summary>Git Guidelines aufklappen...</summary>

Um bei mehreren Entwicklern Chaos zu vermeiden, herrschen strikte Git-Regeln:

1.  **Der `master` / `main` Branch ist HEILIG.** 
    *   Der Master-Branch muss zu **jeder Zeit lauffähig** sein (Compilable & Green Tests).
    *   Niemals wird direkt in den Master-Branch gepusht (Pushes sind per Branch-Protection gesperrt!).
2.  **Feature Branching:**
    *   Jedes neue Feature, jeder Bugfix beginnt mit dem Auschecken des aktuellen Masters: `git checkout -b feature/meine-coole-funktion`
    *   Wir nutzen sprechende Präfixe: `feature/xyz`, `bugfix/xyz`, `hotfix/xyz`, `docs/xyz`.
3.  **Pull Requests (PRs):**
    *   Features werden ausschließlich über Pull Requests in den Master gemerged.
    *   Ein PR benötigt zwingend das grüne Licht der **CI/CD Pipeline** (Tests bestanden).
    *   Ein PR benötigt zwingend ein **Code Review** (Approve) durch mindestens einen anderen Entwickler.
4.  **Issue Tracking:** Keine Code-Änderung ohne zugehöriges GitHub Issue.

</details>

---

## 8. 🗺️ Implementierungs Roadmap (IHK)

<details open>
<summary>Roadmap & Meilensteine (IHK) aufklappen...</summary>

Dieser Fahrplan strukturiert die Entwicklung chronologisch bis zur finalen Abgabe.

1. **Phase 1: Planung & Setup (Aktuell)**
   - UI/UX Planung und Erstellung grundlegender Sketche / Wireframes.
   - Team Onboarding in GitHub (Rechtevergabe, Branch Protections einrichten).
   - Erstellung des **GitHub Projects Kanban-Boards** (Zentrale Verwaltung für dieses Projekt).
   - Anlage der initialen Epics, Tickets und Subtickets im GitHub Board.
   - Initiale Solution & Clean Architecture Strukturierung (.NET Projektmappen anlegen).

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
   - **FINALE IHK-Dokumentation:** Ausarbeitung der Projektarbeit im Google Docs Markdown / Google Sheet Format.
   - Erstellung der finalen Präsentation für das Fachgespräch im Google Workspace (Slides).
   - Letzter Review-Zyklus: Test Coverage Check und Code Cleanup.

</details>

---

## 📄 Lizenz & Rechtliches

Dieses Projekt unterliegt spezifischen Lizenzbestimmungen, die durch einen automatisierten GitHub-Action-Job (License Header Check) in allen Quelldateien validiert werden. Weitere Informationen zur Nutzung und Distribution finden sich in der `LICENSE` Datei im Hauptverzeichnis.

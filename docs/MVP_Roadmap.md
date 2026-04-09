# 🚀 Minimum Viable Product (MVP) vs. Enterprise Roadmap

Unser Projektauftrag der lautet: **„Ein einfaches Ticketsystem
realisieren."** (Projektantrag: 70 Stunden, 23.03.–16.04.2026)

Wir definieren hier eine strikte Abgrenzung zwischen dem **Kern-MVP**, das
exakt die **Aufgabe (Features F1–F9)** abdeckt, und den **Enterprise
Add-Ons**, die wir erst nach bestandener Prüfung umsetzen.

Die „Clean Architecture" und das „3NF Entity Schema" bilden das Fundament für
beide Phasen. Das Schema ist von Tag 1 an auf „Enterprise" ausgelegt, wir
implementieren in Phase 1 (MVP) jedoch nur die relevanten Endpunkte.

---

## 🛠️ Phase 1: Das Kern-MVP (F1–F9)

Dies ist der **exakte Prüfungsumfang**. Alle Features hier müssen 100%
funktionsfähig sein und durch Unit/Integration-Tests abgedeckt werden.

### F1: Web-Anwendung (Basis-Setup)

- [x] Clean Architecture .NET 10 Solution Struktur steht.
- [x] Entity Framework Code-First DB mit MS SQL (Docker Testcontainers).
- [x] **Antigravity AI Integration:** Vollautomatisierung via AI Skills
      (Scaffold, review, ADR).
- [x] User-Registrierung & Login (ASP.NET Core Identity). Registrierung
      erfordert zwingend **Username, Vorname und E-Mail**.
- [x] Dynamische Navbar mit kontext-sensitivem "Settings"-Menü (Profile für
      User; Gruppen- & Rechteverwaltung für Admins).
- [x] **Foundation Ready:** Alle 26 Enterprise-Entitäten (RBAC, Profiles, Teams)
      im persistence layer implementiert.

**Ticket Management (Core Funktionalität):**

- [x] Kanban Dashboard (To Do, In Progress, Review, Done) mit Vanilla HTML5 Drag
      & Drop.
- [x] Tickets erstellen (Titel, Beschreibung, Priorität). Inkl. automatischer
      SHA1-Hash Generierung (Referenzierbarkeit) und Geo/IP-Timestamp Erfassung
      pro Ticket.
- [x] Chillischoten-Schwierigkeits-Metrik (1-5 🌶️).
- [x] Einem User ein Ticket zuweisen.
- [x] **Close-Regeln:** Ein Ticket kann manuell nur von seinem _Ersteller_ oder
      einem _Admin_ geschlossen/gelöscht werden.

**Data & UI:**

- [x] Razor Views & ViewComponents (strikte Nutzung von Partials zur Vermeidung
      von Code-Duplikation `DRY`).
- [x] **Tailwind CSS v4.2.2 Stack:** Vollständige Integration via
      `tailwindcss-dotnet` (Node-free).
- [x] **Client-Side Asset Management:** `libman.json` für SortableJS, FA7.
- [x] CI/CD Pipeline (GitHub Actions) aktiv.
- [x] ASP.NET Core Identity: Login & Logout.
- [x] Eigener AccountController zur Steuerung der Authentifizierung.
- [x] Rollen: Admin, Developer, Tester (mind. je 1 User).
- [x] Nicht angemeldete Benutzer sehen nur die Startseite.

### F2: Admin-Bereich

- [x] Eigener Admin-Bereich, nur für Admins erreichbar.
- [x] Admin-Startseite mit Links zu Unterbereichen.
- [x] Button/Link zum Admin-Bereich auf Startseite (nur für Admins sichtbar).
- [x] **Projekte CRUD** (Titel, Beschreibung, Startdatum _Pflicht_,
      Enddatum _optional_).
- [x] (Optional) Benutzerverwaltung im Admin-Bereich.

### F3: Ticket-Management

- [x] Tickets anlegen: Titel, Beschreibung, Projekt, Ersteller (automatisch),
      Erstellungsdatum (automatisch), Zugewiesener, Zuweisungsdatum.
- [x] Nur offene Projekte können zugeordnet werden.
- [x] Ticket-Liste (sortiert nach Projekt + Erstellungsdatum absteigend).
- [x] Ticket-Detailseite mit allen Informationen + Zurück-Button.
- [x] Tickets bearbeiten (nur Admin, Ersteller oder Zugewiesener):
      Beschreibung, Zugewiesener änderbar. Zuweisungsdatum wird automatisch
      aktualisiert.
- [x] Tickets schließen: Attribute GeschlossenVon, GeschlossenAm. Nur Admin,
      Ersteller oder Zugewiesener darf schließen. Status (offen/geschlossen)
      in Übersicht und Detail sichtbar.
- [x] (Optional) Dateien zu Tickets hochladen.

### F4: Startseite & Statistiken

- [x] Zentrale Startseite unter Root-URL, auch ohne Login erreichbar.
- [x] Links/Buttons zum Ticket- und Admin-Bereich.
- [x] Statistiken: Tickets (Gesamt/offen/geschlossen),
      Projekte (Gesamt/offen/beendet).
- [x] (Optional) Benutzer-Statistiken (Gesamt und pro Rolle).

### F5: Kommentare

- [x] Kommentare auf Ticket-Detailseite (Inhalt, TicketID, Ersteller,
      Erstellzeitpunkt).
- [x] Anzeige neueste zuerst, Button zum Anlegen vorhanden.
- [x] Ersteller = angemeldeter User, Zeitpunkt automatisch gesetzt.

### F6: Filterung

- [x] Filterung nach Projekten („Bestimmtes Projekt" / „Alle Projekte").
- [x] Filterung nach zugeordnetem Benutzer („Benutzer" / „Alle" /
      „Nicht zugeordnet").
- [x] Filterung nach Ersteller („Benutzer" / „Alle Benutzer").
- [x] Seite lädt bei Filterung neu.

### F7: Abhängigkeiten

- [x] Auf der Detailseite Tickets auswählen, die dieses Ticket blockieren.
- [x] Blockierende Tickets als Liste auf der Detailseite anzeigen.
- [x] Ticket kann nur geschlossen werden, wenn alle blockierenden Tickets
      bereits geschlossen sind.

### F8: Workflows

- [x] Workflow-Verwaltung im Admin-Bereich (CRUD, nur Admin).
- [x] Workflow hat ID und Bezeichnung.
- [x] Workflow besteht aus Status-Reihe (die ein Ticket annehmen kann).
- [x] Jedem Projekt kann genau ein Workflow zugeordnet werden.
- [x] (Optional) Status-Folge-Regeln (welche Status als Folge möglich).
- [x] (Optional) Rollenbasierte Status-Vergabe.

### F9: Nachrichten

- [x] Nachrichten: Absender, Empfänger, Zeitstempel, Nachrichteninhalt (Text).
- [x] Jeder angemeldete Benutzer hat eine Nachrichten-Seite.
- [x] Möglichkeit, neue Nachrichten zu erstellen.
- [x] Liste aller gesendeten/erhaltenen Nachrichten, gruppiert nach
      Absender/Empfänger.
- [x] **Dateianhänge:** Unterstützung für Anhänge in Direktnachrichten (MVP+).

### Infrastruktur (bereits erledigt)

- [x] 26 Enterprise-Entitäten im Domain Layer implementiert.
- [x] `BaseEntity` mit Id, TenantId, IsDeleted, RowVersion.
- [x] `AppDbContext` und `DbInitialiser` (Bogus Seeding).
- [x] GitHub Actions CI/CD Pipeline.
- [x] Tailwind CSS v4.2.2 via MSBuild.
- [x] LibMan für Frontend-Assets.

> [!NOTE] **MVP-Entity:** Das `Project`-Entity (Titel,
> Beschreibung, Startdatum, Enddatum) ist als grundlegende Domain-Entity
> implementiert. Tickets werden Projekten zugeordnet (FK).

---

## 🚀 Phase 2: Die Enterprise Add-Ons (Nach dem MVP)

Sobald das MVP steht, der Build (CI/CD) dauerhaft grün ist und die Doku
wächst, schalten wir nach und nach folgende Module auf das bestehende 3NF (3.
Normalform) Datenbankschema frei:

**1. Erweitertes Ticket-Domain:** ✅

- [x] **Chillischoten-Metrik (1–5 🌶️):** Visuelle Schwierigkeitsbewertung
      mit dynamischem Grün-zu-Rot Farbverlauf.
- [x] **SHA1-Hash:** Globale Referenzierbarkeit für Tickets.
- [x] **GeoIP-Timestamp:** Audit-Trail bei Erstellung/Bearbeitung.
- [x] **Time-Tracking:** Start/Stop Button für Arbeitszeit (TimeLogs).
- [x] **Auto-Close Rule:** `TicketCleanupWorker` (BackgroundService) für
      Done→Archived nach 30 Tagen.
- [x] **Tags & Labels:** Globale Tags (n:m) an Tickets mit Multi-Select UI.
- [x] **Subtickets:** Verschachtelte Aufgabenlisten mit interaktivem
      Fortschrittsbalken pro Master-Ticket.

**2. Kollaboration & Echtzeit (Das „Jira/Slack" Erlebnis):**

- [x] **Kanban Dashboard:** Interaktives Board mit SortableJS.
- [x] **Markdown Engine & Mermaid:** Rendering in Ticket-Beschreibungen.
- [x] **Community Voting:** Upvoting von Tickets.
- [x] **Live Presence:** Wer arbeitet gerade an welchem Ticket? + Globale Präsenzanzeige.
- [x] **Team Chat:** Real-time HQ & Team Chat (SignalR Collaboration Hub).
- [x] Ticket Templates (F1): Erstellen/Verwalten von standardisierten Ticket-Vorlagen.
- [x] Workspace Management (F1): Erstellen/Verwalten von Workspaces/Organisationen (Enterprise Ready).

**3. Workspace Management:**

| ID  | Feature                  | Status      | Priority | Description                                             |
| --- | ------------------------ | ----------- | -------- | ------------------------------------------------------- |
| F1  | **Workspace Management** | _Pflicht_   | Hoch     | Erstellen/Verwalten von Workspaces/Organisationen       |
| F2  | **Team-Zuweisung**       | ✅ Erledigt | Mittel   | Tickets können Teams statt Individuen zugewiesen werden |

- **Broadcast Mails:** Teamleads senden via MailKit.
- **Erweiterte Profile:** Profilbild Uploader mit Avatar-Cropping.

---

## 🏆 Phase 3: Advanced Enterprise Modules

**1. Audit & Compliance:**

- [x] **Ticket History (Audit-Log):** Append-Only-Tabelle für alle Änderungen.
- [x] **SEO & Accessibility Excellence:** 100% Lighthouse Audit Ziel.
- [x] **Optimistic Concurrency (Race Conditions):** Schutz gegen parallele Bearbeitung.

**2. Benachrichtigungen & Alerts:**

- [x] **In-App Notification Center:** Glocke in der UI.
- [x] **Notification Sounds:** Konfigurierbare akustische Signale (Base64 integriert).

**3. Dokumentenmanagement:**

- [x] **Ticket Attachments:** Upload von PDFs, Screenshots (via FILE*ASSET).
      \_Infrastruktur implementiert & für Messaging freigeschaltet.*

**4. Service Level Agreements (SLAs):**

- [~] **Automatisierte Countdowns:** Response- und Resolution-Times (Real-time Badges auf Ticket-Karten).

**5. Faceted Search & Filtering (EF Core):**

- [x] **High-Performance Search:** Komplexes Filtern von Tickets (z.B. "Alle Tickets
      mit Tag #frontend, Status = In Progress, Assigned = Me").
- [x] _Architektur:_ Umsetzung über dynamische LINQ-Queries in EF Core, idealerweise
  optimiert durch definierte Datenbank-Indizes auf den Suchspalten.

**6. Ticket Templates:**

- **Vorlagen-System:** Admins können Templates (z.B. "Bug Report", "Feature
  Request") definieren. Beim Anlegen eines Tickets wird die Markdown-Description
  automatically strukturiert vorausgefüllt.

---

## 🔌 Phase 4: Das Plugin Ecosystem

Der finale Schritt zur Marktführerschaft. Das System öffnet sich für
Erweiterungen von Drittanbietern oder isolierte Kunden-Lösungen.

**1. Plugin Loader Architecture:**

- Das System scant zur Laufzeit ein `/plugins/` Verzeichnis.
- Gefundene `.dll` Dateien, die das `ITicketsPleasePlugin` Interface
  implementieren, werden dynamisch per Dependency Injection in den
  Core-Lifecycle eingehängt.

**2. Offizielle Erweiterungen:**

- **TogglSync:** Ein Plugin, welches die internen `TIME_LOG` Einträge
  automatisch per API an Toggl Track sendet.
- **AI-Summarizer:** Ein Plugin, das bei jedem Statuswechsel auf "Review" die
  Ticket-Description an OpenAI schickt und einen Zusammenfassungs-Kommentar
  generiert.
- **SAML / SSO Authenticator:** Ein Plugin, welches den Standard-Login durch
  einen Enterprise Single-Sign-On (z.B. Entra ID) ersetzt.

### Phase 2: Collaboration & Real-Time (Status: IN PROGRESS 🟢)

| ID  | Feature                              | Status       | Priority |
| --- | ------------------------------------ | ------------ | -------- |
| F10 | **SignalR Infrastructure**           | ✅ Erledigt  | Hoch     |
| F11 | **Live Collaboration** (Kanban Sync) | ✅ Erledigt  | Hoch     |
| F12 | **Notification Center** (Toasts)     | ✅ Erledigt  | Mittel   |
| F13 | **Live Presence** (Active Viewers)   | 🔄 In Arbeit | Mittel   |
| F14 | **Team-Chat / Ticket Messaging**     | ⏳ Geplant   | Mittel   |

> **Fazit:** Wir haben das Datenbankschema und die Ordnerstruktur bereits auf
> **Enterprise** Level gehievt. Alle Entitäten und Beziehungen sind im Code
> (Domain/Infrastruktur) hinterlegt und durch den `DbInitialiser` (Bogus) mit
> umfangreichen synthetischen Testdaten (500 Tickets, 200 Users) verifiziert.
> Die initialen C# Feature-Sprints können nun auf dieser soliden Basis aufbauen!

---

## 📈 Phase 5: Final Polish & Observability

Die Vorbereitung auf den skalierten Live-Betrieb (Operations).

**1. Monitoring & Health:**

- **ASP.NET Core Health Checks:** Endpunkte (`/health`), die den Status der
  Datenbank, des Filesystems und externer APIs (Mail, Toggl) in Echtzeit
  überwachen.

**2. Distributed Caching:**

- **Redis-Integration:** Häufig angefragte Daten (z.B. User-Profile,
  Team-Listen) werden im Redis zwischengespeichert (Cache-Aside Pattern), um die
  SQL-Datenbank zu entlasten.

**3. Robust Background Tasks:**

- **Hangfire / Quartz.NET:** Ein persistenter Job-Scheduler mit eigenem
  Dashboard. Garantiert, dass Auto-Close Jobs oder SLA-Mails auch dann
  ausgeführt werden, wenn der Server kurz neu startet.

**4. API Excellence:**

- **Scalar / OpenAPI:** Automatische Dokumentation aller API-Endpunkte für
  Plugin-Entwickler.
- **Versioning:** Versionierte API-Routen (`/api/v1/tickets`), um
  Breaking-Changes sicher zu managen.

> **Finales Fazit:** Das System ist nun von den Grundlagen bis zur High-End
> Enterprise-Cloud-Architektur vollständig durchgeplant. Jedes C# Projekt und
> jedes NuGet-Paket hat seinen festen Platz.

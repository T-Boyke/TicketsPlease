# 🚀 Minimum Viable Product (MVP) vs. Enterprise Roadmap

Unser Projektauftrag der IHK lautet: **„Ein einfaches Ticketsystem
realisieren."** (Projektantrag: 70 Stunden, 23.03.–16.04.2026)

Wir definieren hier eine strikte Abgrenzung zwischen dem **Kern-MVP**, das
exakt die **IHK-Aufgabe (Features F1–F9)** abdeckt, und den **Enterprise
Add-Ons**, die wir erst nach bestandener Prüfung umsetzen.

Die „Clean Architecture" und das „3NF Entity Schema" bilden das Fundament für
beide Phasen. Das Schema ist von Tag 1 an auf „Enterprise" ausgelegt, wir
implementieren in Phase 1 (MVP) jedoch nur die IHK-relevanten Endpunkte.

---

## 🛠️ Phase 1: Das Kern-MVP (IHK F1–F9)

Dies ist der **exakte IHK-Prüfungsumfang**. Alle Features hier müssen 100%
funktionsfähig sein und durch Unit/Integration-Tests abgedeckt werden.

### F1: Web-Anwendung (Basis-Setup)

- [x] Clean Architecture .NET 10 Solution Struktur steht.
- [x] Entity Framework Code-First DB mit MS SQL (Docker Testcontainers).
- [x] **Antigravity AI Integration:** Vollautomatisierung via AI Skills
      (Scaffold, review, ADR).
- [ ] User-Registrierung & Login (ASP.NET Core Identity). Registrierung
      erfordert zwingend **Username, Vorname und E-Mail**.
- [ ] Dynamische Navbar mit kontext-sensitivem "Settings"-Menü (Profile für
      User; Gruppen- & Rechteverwaltung für Admins).
- [x] **Foundation Ready:** Alle 26 Enterprise-Entitäten (RBAC, Profiles, Teams)
      im persistence layer implementiert.

**Ticket Management (Core Funktionalität):**

- [ ] Kanban Dashboard (To Do, In Progress, Review, Done) mit Vanilla HTML5 Drag
      & Drop.
- [ ] Tickets erstellen (Titel, Beschreibung, Priorität). Inkl. automatischer
      SHA1-Hash Generierung (Referenzierbarkeit) und Geo/IP-Timestamp Erfassung
      pro Ticket.
- [ ] Chillischoten-Schwierigkeits-Metrik (1-5 🌶️).
- [ ] Einem User ein Ticket zuweisen.
- [ ] **Close-Regeln:** Ein Ticket kann manuell nur von seinem _Ersteller_ oder
      einem _Admin_ geschlossen/gelöscht werden.

**Data & UI:**

- [ ] Razor Views & ViewComponents (strikte Nutzung von Partials zur Vermeidung
      von Code-Duplikation `DRY`).
- [x] **Tailwind CSS v4.2.2 Stack:** Vollständige Integration via
      `tailwindcss-dotnet` (Node-free).
- [x] **Client-Side Asset Management:** `libman.json` für SortableJS, FA7.
- [x] CI/CD Pipeline (GitHub Actions) aktiv.
- [ ] ASP.NET Core Identity: Login & Logout.
- [ ] Eigener AccountController zur Steuerung der Authentifizierung.
- [ ] Rollen: Admin, Developer, Tester (mind. je 1 User).
- [ ] Nicht angemeldete Benutzer sehen nur die Startseite.

### F2: Admin-Bereich

- [ ] Eigener Admin-Bereich, nur für Admins erreichbar.
- [ ] Admin-Startseite mit Links zu Unterbereichen.
- [ ] Button/Link zum Admin-Bereich auf Startseite (nur für Admins sichtbar).
- [ ] **Projekte CRUD** (Titel, Beschreibung, Startdatum [Pflicht],
      Enddatum [optional]).
- [ ] (Optional) Benutzerverwaltung im Admin-Bereich.

### F3: Ticket-Management

- [ ] Tickets anlegen: Titel, Beschreibung, Projekt, Ersteller (automatisch),
      Erstellungsdatum (automatisch), Zugewiesener, Zuweisungsdatum.
- [ ] Nur offene Projekte können zugeordnet werden.
- [ ] Ticket-Liste (sortiert nach Projekt + Erstellungsdatum absteigend).
- [ ] Ticket-Detailseite mit allen Informationen + Zurück-Button.
- [ ] Tickets bearbeiten (nur Admin, Ersteller oder Zugewiesener):
      Beschreibung, Zugewiesener änderbar. Zuweisungsdatum wird automatisch
      aktualisiert.
- [ ] Tickets schließen: Attribute GeschlossenVon, GeschlossenAm. Nur Admin,
      Ersteller oder Zugewiesener darf schließen. Status (offen/geschlossen)
      in Übersicht und Detail sichtbar.
- [ ] (Optional) Dateien zu Tickets hochladen.

### F4: Startseite & Statistiken

- [ ] Zentrale Startseite unter Root-URL, auch ohne Login erreichbar.
- [ ] Links/Buttons zum Ticket- und Admin-Bereich.
- [ ] Statistiken: Tickets (Gesamt/offen/geschlossen),
      Projekte (Gesamt/offen/beendet).
- [ ] (Optional) Benutzer-Statistiken (Gesamt und pro Rolle).

### F5: Kommentare

- [ ] Kommentare auf Ticket-Detailseite (Inhalt, TicketID, Ersteller,
      Erstellzeitpunkt).
- [ ] Anzeige neueste zuerst, Button zum Anlegen vorhanden.
- [ ] Ersteller = angemeldeter User, Zeitpunkt automatisch gesetzt.

### F6: Filterung

- [ ] Filterung nach Projekten („Bestimmtes Projekt" / „Alle Projekte").
- [ ] Filterung nach zugeordnetem Benutzer („Benutzer" / „Alle" /
      „Nicht zugeordnet").
- [ ] Filterung nach Ersteller („Benutzer" / „Alle Benutzer").
- [ ] Seite lädt bei Filterung neu.

### F7: Abhängigkeiten

- [ ] Auf der Detailseite Tickets auswählen, die dieses Ticket blockieren.
- [ ] Blockierende Tickets als Liste auf der Detailseite anzeigen.
- [ ] Ticket kann nur geschlossen werden, wenn alle blockierenden Tickets
      bereits geschlossen sind.

### F8: Workflows

- [ ] Workflow-Verwaltung im Admin-Bereich (CRUD, nur Admin).
- [ ] Workflow hat ID und Bezeichnung.
- [ ] Workflow besteht aus Status-Reihe (die ein Ticket annehmen kann).
- [ ] Jedem Projekt kann genau ein Workflow zugeordnet werden.
- [ ] (Optional) Status-Folge-Regeln (welche Status als Folge möglich).
- [ ] (Optional) Rollenbasierte Status-Vergabe.

### F9: Nachrichten

- [ ] Nachrichten: Absender, Empfänger, Zeitstempel, Nachrichteninhalt (Text).
- [ ] Jeder angemeldete Benutzer hat eine Nachrichten-Seite.
- [ ] Möglichkeit, neue Nachrichten zu erstellen.
- [ ] Liste aller gesendeten/erhaltenen Nachrichten, gruppiert nach
      Absender/Empfänger.

### Infrastruktur (bereits erledigt)

- [x] 26 Enterprise-Entitäten im Domain Layer implementiert.
- [x] `BaseEntity` mit Id, TenantId, IsDeleted, RowVersion.
- [x] `AppDbContext` und `DbInitialiser` (Bogus Seeding).
- [x] GitHub Actions CI/CD Pipeline.
- [x] Tailwind CSS v4.2.2 via MSBuild.
- [x] LibMan für Frontend-Assets.

> [!IMPORTANT] **Fehlende MVP-Entity:** Das `Project`-Entity (Titel,
> Beschreibung, Startdatum, Enddatum) muss als eigene Domain-Entity
> erstellt werden. Tickets werden einem Projekt zugeordnet (FK).
> Dies ist IHK-Pflicht (F2.2)!

---

## 🚀 Phase 2: Die Enterprise Add-Ons (Nach dem MVP)

Sobald das MVP steht, der Build (CI/CD) dauerhaft grün ist und die IHK-Doku
wächst, schalten wir nach und nach folgende Module auf das bestehende 3NF (3.
Normalform) Datenbankschema frei:

**1. Erweitertes Ticket-Domain:**

- **Chillischoten-Metrik (1–5 🌶️):** Visuelle Schwierigkeitsbewertung.
- **SHA1-Hash:** Globale Referenzierbarkeit für Tickets.
- **GeoIP-Timestamp:** Audit-Trail bei Erstellung/Bearbeitung.
- **Time-Tracking:** Start/Stop Button für Arbeitszeit (TimeLogs).
- **Auto-Close Rule:** BackgroundService für Done→Archived nach X Tagen.
- **Tags & Labels:** Globale Tags (n:m) an Tickets.
- **Subtickets:** Verschachtelte Aufgabenlisten pro Master-Ticket.

**2. Kollaboration & Echtzeit (Das „Jira/Slack" Erlebnis):**

- **Kanban Dashboard:** Interaktives Board mit HTML5 Drag & Drop.
- **Markdown Engine & Mermaid:** Rendering in Ticket-Beschreibungen.
- **SignalR Chat & Kommentare:** Live-Kommentare ohne Page-Reload.
- **Live-Online Presence:** Grüne/Rote „Online" Punkte.
- **Community Voting:** Upvoting von Tickets.

**3. Workspace Management:**

- **Teams:** Organisation von Usern in Squads/Teams.
- **Team-Routing:** Tickets an Teams zuweisen.
- **Broadcast Mails:** Teamleads senden via MailKit.
- **Erweiterte Profile:** Profilbild Uploader mit Avatar-Cropping.

---

## 🏆 Phase 3: Advanced Enterprise Modules

**1. Audit & Compliance:**

- **Ticket History (Audit-Log):** Append-Only-Tabelle für alle Änderungen.

**2. Benachrichtigungen & Alerts:**

- **In-App Notification Center:** Glocke in der UI.
- **Notification Sounds:** Konfigurierbare akustische Signale.

**3. Dokumentenmanagement:**

- **Ticket Attachments:** Upload von PDFs, Screenshots (via FILE_ASSET).

**4. Service Level Agreements (SLAs):**

- **Automatisierte Countdowns:** Response- und Resolution-Times.

**5. Faceted Search & Filtering (EF Core):**

- **High-Performance Search:** Komplexes Filtern von Tickets (z.B. "Alle Tickets
  mit Tag #frontend, Status = In Progress, Assigned = Me").
- _Architektur:_ Umsetzung über dynamische LINQ-Queries in EF Core, idealerweise
  optimiert durch definierte Datenbank-Indizes auf den Suchspalten.

**6. Ticket Templates:**

- **Vorlagen-System:** Admins können Templates (z.B. "Bug Report", "Feature
  Request") definieren. Beim Anlegen eines Tickets wird die Markdown-Description
  automatisch strukturiert vorausgefüllt.

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

> **Fazit:** Wir haben das Datenbankschema und die Ordnerstruktur bereits auf
> **Phase 5** Level gehievt. Alle Entitäten und Beziehungen sind im Code
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

> **Finales Fazit:** Das System ist nun von den IHK-Grundlagen bis zur High-End
> Enterprise-Cloud-Architektur vollständig durchgeplant. Jedes C# Projekt und
> jedes NuGet-Paket hat seinen festen Platz.

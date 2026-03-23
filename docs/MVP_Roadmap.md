# 🚀 Minimum Viable Product (MVP) vs. Enterprise Roadmap

Unser Projektauftrag der IHK lautet: **"Ein einfaches Ticketsystem
realisieren."**

Um sich in den 80 Projektstunden nicht in Architekturen zu verrennen, definieren
wir hier eine strikte Abgrenzung zwischen dem **Kern-MVP (Minimum Viable
Product)**, mit dem wir zwingend in die IHK Prüfung gehen, und den **Enterprise
Add-Ons**, die wir erst anflanschen, wenn die Basis grundsolide steht.

Die "Clean Architecture" und das "3NF Entity Schema" bilden das Fundament für
beide Phasen. Das Schema ist von Tag 1 an auf "Enterprise" ausgelegt, wir
implementieren in Phase 1 (MVP) jedoch noch nicht alle Endpunkte/Features davon!

---

## 🛠️ Phase 1: Das Kern-MVP (IHK Zielvorgabe)

Dies ist das absolute Minimum, das laufen _muss_, um eine "1" in der
Präsentation zu garantieren. Alle Features hier müssen 100% bugfrei sein und
durch Unit/Integration-Tests (TDD) abgedeckt werden.

**Identity & Setup:**

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
- [x] **Client-Side Asset Management:** `libman.json` für Markdig, SortableJS,
      FA7.

---

## 🚀 Phase 2: Die Enterprise Add-Ons (Nach dem MVP)

Sobald das MVP steht, der Build (CI/CD) dauerhaft grün ist und die IHK-Doku
wächst, schalten wir nach und nach folgende Module auf das bestehende 3NF (3.
Normalform) Datenbankschema frei:

**1. Erweitertes Ticket-Domain:**

- **Time-Tracking:** Entwickler können per Start/Stop Button echte Arbeitszeit
  auf ein Ticket buchen (TimeLogs).
- **Auto-Close Rule:** Ein geplanter Cronjob (BackgroundService), der gelöste
  Tickets ("Done") nach X Tagen Inaktivität automatisch ins Archiv (Closed)
  verschiebt.
- **Tags & Labels:** Globale Tags (z.B. `#frontend`, `#bug`), die an Tickets
  gehängt werden können (n:m).
- **Subtickets:** Unbegrenzt verschachtelte Aufgabenlisten pro Master-Ticket.

**2. Kollaboration & Echtzeit (Das "Jira/Slack" Erlebnis):**

- **Markdown Engine & Mermaid:** Rendering von Graphen in den
  Ticket-Beschreibungen.
- **SignalR Chat & Kommentare:** Live-Ticket-Kommentare ohne Page-Reload.
- **Live-Online Presence:** Grüne/Rote "Online" Punkte am Profilbild der
  Entwickler.
- **Community Voting:** Upvoting von Tickets durch das Team.

**3. Workspace Management:**

- **Teams:** Organisation von Usern in Squads/Teams.
- **Team-Routing:** Tickets ganzen Teams (statt nur Einzelpersonen) zuweisen.
- **Broadcast Mails:** Teamleads können via MailKit Newsletter/Notices an ihre
  Teams senden.
- **Erweiterte Profile:** Profilbild Uploader mit Avatar-Cropping (FileAsset
  Management).

---

## 🏆 Phase 3: Advanced Enterprise Modules

Diese Module katapultieren das System auf das Level von Branchenriesen wie Jira
oder Linear. Sie erfordern tiefe architektonische Planung.

**1. Audit & Compliance:**

- **Ticket History (Audit-Log):** Lückenlose Historisierung. Jeder
  Statuswechsel, jede Prioritätsänderung und Neuzuweisung wird mit Timestamp und
  Actor (User) in einer Append-Only-Tabelle (`TICKET_HISTORY`) mitgeschrieben.
  Einsehbar für Administratoren über das "Audit Log" im Settings-Menü.

**2. Benachrichtigungen & Alerts:**

- **In-App Notification Center:** Eine "Glocke" in der UI. Benachrichtigt User
  bei Zuweisungen, Erwähnungen (@User) oder SLA-Verletzungen.
- **Notification Sounds:** Einstellbare akustische Signale bei eingehenden
  Notifications oder neuen Chat-Nachrichten im Dashboard (im Benutzerprofil
  auswählbar & probehörbar).

**3. Dokumentenmanagement:**

- **Ticket Attachments:** Upload von PDFs, Log-Dateien oder Screenshots direkt
  an das Ticket (via `FILE_ASSET` Entity) mit integriertem Reader/Preview in der
  UI.

**4. Service Level Agreements (SLAs):**

- **Automatisierte Countdowns:** SLAs für Response- und Resolution-Times
  basierend auf Ticket-Priorität (z.B. "Blocker" muss in 4h gelöst sein).
  Eskalation bei Verletzung.

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

**2. Offizielle Erweiterungen (Beispiele):**

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

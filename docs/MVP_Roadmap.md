# 🚀 Minimum Viable Product (MVP) vs. Enterprise Roadmap

Unser Projektauftrag der IHK lautet: **"Ein einfaches Ticketsystem realisieren."** 

Um sich in den 80 Projektstunden nicht in Architekturen zu verrennen, definieren wir hier eine strikte Abgrenzung zwischen dem **Kern-MVP (Minimum Viable Product)**, mit dem wir zwingend in die IHK Prüfung gehen, und den **Enterprise Add-Ons**, die wir erst anflanschen, wenn die Basis grundsolide steht.

Die "Clean Architecture" und das "3NF Entity Schema" bilden das Fundament für beide Phasen. Das Schema ist von Tag 1 an auf "Enterprise" ausgelegt, wir implementieren in Phase 1 (MVP) jedoch noch nicht alle Endpunkte/Features davon!

---

## 🛠️ Phase 1: Das Kern-MVP (IHK Zielvorgabe)

Dies ist das absolute Minimum, das laufen *muss*, um eine "1" in der Präsentation zu garantieren.
Alle Features hier müssen 100% bugfrei sein und durch Unit/Integration-Tests (TDD) abgedeckt werden.

**Identity & Setup:**
*   [x] Clean Architecture .NET 10 Solution Struktur steht.
*   [x] Entity Framework Code-First DB mit MS SQL (Docker Testcontainers).
*   [ ] User-Registrierung & Login (ASP.NET Core Identity).
*   [ ] *Einfache* Rollen (Admin, User).

**Ticket Management (Core Funktionalität):**
*   [ ] Kanban Dashboard (To Do, In Progress, Review, Done) mit Vanilla HTML5 Drag & Drop.
*   [ ] Tickets erstellen (Titel, Beschreibung, Priorität).
*   [ ] Chillischoten-Schwierigkeits-Metrik (1-5 🌶️).
*   [ ] Einem User ein Ticket zuweisen.
*   [ ] **Close-Regeln:** Ein Ticket kann manuell nur von seinem *Ersteller* oder einem *Admin* geschlossen/gelöscht werden.

**Data & UI:**
*   [ ] Razor Views & ViewComponents (strikte Nutzung von Partials zur Vermeidung von Code-Duplikation `DRY`).
*   [ ] Styling via lokaler TailwindCSS/FontAwesome Stack (`libman.json`).

---

## 🚀 Phase 2: Die Enterprise Add-Ons (Nach dem MVP)

Sobald das MVP steht, der Build (CI/CD) dauerhaft grün ist und die IHK-Doku wächst, schalten wir nach und nach folgende Module auf das bestehende 3NF (3. Normalform) Datenbankschema frei:

**1. Erweitertes Ticket-Domain:**
*   **Time-Tracking:** Entwickler können per Start/Stop Button echte Arbeitszeit auf ein Ticket buchen (TimeLogs).
*   **Auto-Close Rule:** Ein geplanter Cronjob (BackgroundService), der gelöste Tickets ("Done") nach X Tagen Inaktivität automatisch ins Archiv (Closed) verschiebt.
*   **Tags & Labels:** Globale Tags (z.B. `#frontend`, `#bug`), die an Tickets gehängt werden können (n:m).
*   **Subtickets:** Unbegrenzt verschachtelte Aufgabenlisten pro Master-Ticket.

**2. Kollaboration & Echtzeit (Das "Jira/Slack" Erlebnis):**
*   **Markdown Engine & Mermaid:** Rendering von Graphen in den Ticket-Beschreibungen.
*   **SignalR Chat & Kommentare:** Live-Ticket-Kommentare ohne Page-Reload.
*   **Live-Online Presence:** Grüne/Rote "Online" Punkte am Profilbild der Entwickler.
*   **Community Voting:** Upvoting von Tickets durch das Team.

**3. Workspace Management:**
*   **Teams:** Organisation von Usern in Squads/Teams.
*   **Team-Routing:** Tickets ganzen Teams (statt nur Einzelpersonen) zuweisen.
*   **Broadcast Mails:** Teamleads können via MailKit Newsletter/Notices an ihre Teams senden.
*   **Erweiterte Profile:** Profilbild Uploader mit Avatar-Cropping (FileAsset Management).

---

## 🏆 Phase 3: Advanced Enterprise Modules

Diese Module katapultieren das System auf das Level von Branchenriesen wie Jira oder Linear. Sie erfordern tiefe architektonische Planung.

**1. Audit & Compliance:**
*   **Ticket History (Audit-Log):** Lückenlose Historisierung. Jeder Statuswechsel, jede Prioritätsänderung und Neuzuweisung wird mit Timestamp und Actor (User) in einer Append-Only-Tabelle (`TICKET_HISTORY`) mitgeschrieben.

**2. Benachrichtigungen & Alerts:**
*   **In-App Notification Center:** Eine "Glocke" in der UI. Benachrichtigt User bei Zuweisungen, Erwähnungen (@User) oder SLA-Verletzungen.
*   **Notification Sounds:** Einstellbare akustische Signale bei eingehenden Notifications oder neuen Chat-Nachrichten im Dashboard (im Benutzerprofil auswählbar & probehörbar).

**3. Dokumentenmanagement:**
*   **Ticket Attachments:** Upload von PDFs, Log-Dateien oder Screenshots direkt an das Ticket (via `FILE_ASSET` Entity) mit integriertem Reader/Preview in der UI.

**4. Service Level Agreements (SLAs):**
*   **Automatisierte Countdowns:** SLAs für Response- und Resolution-Times basierend auf Ticket-Priorität (z.B. "Blocker" muss in 4h gelöst sein). Eskalation bei Verletzung.

**5. Faceted Search & Filtering (EF Core):**
*   **High-Performance Search:** Komplexes Filtern von Tickets (z.B. "Alle Tickets mit Tag #frontend, Status = In Progress, Assigned = Me").
*   *Architektur:* Umsetzung über dynamische LINQ-Queries in EF Core, idealerweise optimiert durch definierte Datenbank-Indizes auf den Suchspalten.

**6. Ticket Templates:**
*   **Vorlagen-System:** Admins können Templates (z.B. "Bug Report", "Feature Request") definieren. Beim Anlegen eines Tickets wird die Markdown-Description automatisch strukturiert vorausgefüllt.

> **Fazit:** Wir bauen das Datenbankschema und die Ordnerstruktur heute schon für **Phase 2 und 3**, aber die initialen C# Feature-Sprints fokussieren sich eisern auf **Phase 1**!

# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [0.14.0-alpha] - 2026-04-14

### Added

- **Enterprise Governance Infrastructure**: Vollständige Implementierung von organisationsweiten SLAs, Quiet Hours, Prioritäts-Filtern und Notification Preferences.
- **SLA Background Worker**: Automatisierter, Mandanten-fähiger Hintergrunddienst zur permanenten Überwachung von SLA-Verletzungen unter Berücksichtigung von Zeitzonen.
- **Audit & Governance Log**: Unveränderlicher Audit-Trail (Append-Only) im neuen Product Owner Dashboard zur Nachverfolgung sicherheitsrelevanter Konfigurationsänderungen.
- **Stakeholder Dashboard**: Neue KPI-Übersicht für Projektsponsoren mit Team Velocity, SLA-Einhaltung und aggregierten Gesundheitsmetriken.
- **Admin System Maintenance**: Danger Zone Operations für Datenbank-Wipe und Tabellen-Truncate, geschützt durch "DELETE CONFIRM" Security Phrases.
- **Organization Invites**: Token-basierter Einladungs- und Registrierungsworkflow inklusive Profilvervollständigung (Position, Adresse).
- **Circular Dependency Detection**: Validierung beim Anlegen von Ticket-Abhängigkeiten zur Vermeidung von Endlosschleifen.

### Improved

- **User Profiles**: Erweiterung um spezifische Einstellungen (Kanban-Intervalle, Animations-Reduktion, Notification Sounds).
- **Clean Architecture Refactoring**: Restriktiver Domain-Fokus – direkte Datenbankzugriffe aus Web Controllern entfernt und durch dedizierte Application Services (z.B. `OrganizationService`) ersetzt.
- **Tenant Context Query Filter**: Globale EF Core Abfragefilter zur automatischen Isolierung von Organisationsdaten bei gleichzeitigem Admin-Bypass.

## [0.13.0-alpha] - 2026-04-12

### Added

- **Quick Chat Dropdown**: Neues Navigations-Element in der Navbar für den direkten Zugriff auf die letzten 5 Unterhaltungen inklusive unread-Indikatoren.
- **Team Join Requests**: Benutzer können nun Beitrittsanfragen für Teams ihrer Organisation stellen, die automatisch als Benachrichtigung an Teamleads/Admins gesendet werden.
- **Advanced Tag Management**: Integration von 20 vordefinierten Standard-Tags mit spezifischen FontAwesome Icons und Farbcodes.
- **Tag Creation Modal**: Implementierung eines AJAX-basierten Modals zur dynamischen Erstellung neuer Tags direkt aus der Ticket-Bearbeitungsansicht.
- **Massive Unit Test Suite**: Erweiterung der Testabdeckung um über 100 neue Unit-Tests für alle zentralen Services im Application- und Infrastructure-Layer.

### Improved

- **Collaboration Hub Overhaul**: Neugestaltung des Hub-Buttons in der Navbar (`fa-cloud-bolt`) mit modernen CSS-Animationen und verbesserter Klick-Logik.
- **High-Quality Seeding**: Der `DbInitialiser` generiert nun eine exakte Test-Infrastruktur mit 25 Benutzern (Admins, Teamleads, User), 2 Workspaces, 5 Teams und vollständigen Profilen.
- **Dashboard Multi-Tenancy**: Alle Statistiken (Projekte, Tickets, Highscores) werden nun präzise nach der `TenantId` des angemeldeten Benutzers gefiltert.
- **Workflow-Datenmodell**: Umstellung der `WorkflowTransition`-Identität auf einen eindeutigen GUID-Primärschlüssel zur Vermeidung von EF-Tracking-Konflikten bei mandantenübergreifenden Status-Übergängen.

### Fixed

- **Navbar Auth Security**: Behebung eines Absturzes (`ArgumentNullException`) bei unauthentifizierten Zugriffen auf die globale Navigationsleiste.
- **Dependency Logic Fix**: Korrektur der Inversion beim Hinzufügen von Ticket-Abhängigkeiten (Blockierer vs. Abhängiges Ticket).
- **Notification Deep-Links**: Sicherstellung, dass "View Ticket" Buttons in Benachrichtigungen direkt zur Detailansicht des betroffenen Tickets führen.
- **Seeder Stability**: Beseitigung von potenziellen `InvalidOperationException` (Empty Sequence) Fehlern durch defensive Programmierung im Seeding-Prozess.

## [0.12.0-alpha] - 2026-04-10

### Added

- **Full Notification Persistence**: Implemented `INotificationRepository` and database schema for permanent storage of system alerts.
- **Interactive Notification Center**: Replaced static dropdown with a functional component featuring "Load More" paging and "Mark all as read" capabilities.
- **Real-Time SignalR Updates**: Enhanced the notification system with live badge increments and list updates without page refresh.
- **Avatar Cropping**: Profile pictures are now dynamically cropped and centered in the navbar using `object-cover` for a consistent UI.
- **I18n Localization**: Fully localized the notification system and navbar elements into German.

### Improved

- **State Consistency Engine**: Automated synchronization between `WorkflowStateId` and `Status` fields in the Ticket entity, preventing UI-sync issues in the Kanban board.
- **Navbar Aesthetics**: Replaced the placeholder chat icon with a modern `fa-comments` icon and improved layout spacing.
- **Service Layer Robustness**: Enhanced `TicketService` to handle status-name lookups during state transitions.

### Fixed

- **Kanban "Snap-back" Bug**: Resolved the issue where tickets moved to "In Progress" would disappear or revert due to inconsistent status naming in the database.
- **Notification Direct Access**: Fixed the navbar popup links to ensure specific notification types (e.g., ticket updates) are correctly routed.

## [0.11.0-alpha] - 2026-04-09

### Added

- **Ticket Template Engine**: Implemented a comprehensive template system allowing administrators to define standardized Markdown descriptions and default priorities for different ticket types.
- **Smart Form Pre-filling**: Added a client-side injection system in the Ticket Creation flow that applied templates instantly on selection.
- **Workspace Management Hub**: New administrative module for multi-tenant organization control, including subscription tier management and global workspace status.
- **Enterprise Presence (Phase 2.3)**: Finalized the SignalR Collaboration Hub with global presence tracking and team-specific chat group support.

### Improved

- **Admin UI Aesthetics**: Updated the administrative area with a premium, glassmorphism-inspired design consistent with the core application.
- **Navigation Architecture**: Expanded the global navbar with categorized administrative actions for Templates and Workspaces.

## [0.10.0-alpha] - 2026-04-09

### Added

- **Global Faceted Search**: Implemented high-performance search across ticket titles and descriptions in Repository and Service layers.
- **Notification Dropdown**: Added a premium notification center in the navbar with mock alerts and unread indicators.
- **Dynamic Navigation**: Implemented smart active-link highlighting in the navbar based on current route data.

### Improved

- **Kanban Resilience**: Fixed Drag & Drop "snap-back" by integrating missing `RequestVerificationToken` in AJAX headers.
- **Column UX**: Added vertical scrolling (`max-h-65vh`) to Kanban list containers to handle large ticket volumes.
- **Leaderboard Logic**: Optimized the Highscore Board to hide participants with zero points or completed tickets.

### Fixed

- **Avatar Rendering**: Resolved potential NullReference/IndexOutOfRange exceptions when rendering avatars for unassigned tickets.
- **API Visibility**: Exposed `TicketsController` as public to ensure proper route discovery.
- **Workflow Seeding**: Relaxed transition rules in `DbInitialiser` to allow all users to move tickets to the "Done" state.

## [0.9.0-alpha] - 2026-04-02

### Added

- **SignalR Architecture**: Real-time infrastructure for live collaboration.
- **Notification Center**: Enterprise Toast system for instant alerts and updates.
- **Live Comments**: New comments appear instantly on the ticket details page without refresh.
- **Kanban Sync**: Real-time notification support for board status changes.
- **Presence Indicators**: Infrastructure for tracking users on active tickets.
- **Clean Concurrency**: Repository-level RowVersion management for better architecture.
- **Teams Module**: Full Clean Architecture implementation for team management and administration.
- **Project Tickets**: Eliminated ticket placeholder on project details with dynamic list rendering.

### Phase 2.2: Collaboration & Enterprise Excellence

- **📊 Interactive Kanban Board**: Implemented fluid drag-and-drop status updates using SortableJS
  and automatic AJAX persistence with Anti-Forgery protection.
- **✍️ Markdown & Mermaid Engine**: Built a high-performance client-side rendering engine using
  `marked.js`, `mermaid.js`, and `DOMPurify` for secure rich text and diagrams in tickets,
  comments, and messages.
- **📜 Ticket Audit Log**: Added comprehensive history tracking and a dedicated UI panel to
  visualize all ticket property changes (who, what, when, old vs. new value) over time.
- **❤️ Community Voting**: Integrated upvote/downvote functionality for tickets with real-time
  status badges on the board and toggle buttons in the detail view.
- **🛡️ Race Condition Protection**: Implemented optimistic concurrency control via `RowVersion`
  (Concurrency Tokens) in the domain, service, and UI to prevent accidental data overrides.
- **🔍 SEO & Accessibility Excellence**: Achieved a 100% Lighthouse target for SEO and A11y through
  dynamic meta-tags, localized descriptions, semantic HTML5, and ARIA-labeling for all
  interactive elements.

## [2026-03-27]

### Phase 2.1: Erweitertes Ticket-Domain (Enterprise)

- **🌶️ Chili Difficulty Gradient**: Implemented a dynamic green-to-red color gradient for the
  Chili difficulty metric (1-5) in Create, Edit, and Detail views.
- **🏷️ Tag Management**: Added multi-select Tag synchronization for tickets, allowing users to
  categorize tasks with globally defined labels.
- **⏱️ Time-Tracking System**: Integrated a real-time stopwatch and manual time logging.
  Tickets now track total worked time via a new dedicated `TimeLog` entity.
- **✅ Subticket Checklists**: Introduced nested subtickets with interactive toggling and a
  dynamic progress bar on the ticket detail page.
- **🧹 Auto-Close Worker**: Implemented `TicketCleanupWorker` background service to automatically
  archive tickets in 'Done' or 'Closed' states after 30 days.

## [2026-03-26]

### Phase 1 Optional Features (MVP Completion)

- **F2: Admin User Management**: Implemented `AdminUsersController` and premium Razor views
  for listing and editing users, including role management and profile updates.
- **F3: Ticket Attachments**: Fully integrated file upload and download capabilities for
  tickets, supported by `IFileStorageService` and `IFileAssetRepository`.
- **F4: Dashboard Statistics**: Created `DashboardService` to aggregate system-wide metrics
  (tickets, projects, users) and updated `Home/Index` with a modern stats grid.
- **F8: Advanced Workflow Logic**: Implemented state machine transition rules and role-based
  validation in `TicketService.MoveTicketAsync`, ensuring business process compliance.

### Added

- **100% Integration Coverage**: Expanded Integration tests for Infrastructure, Services, and
  Kern-Controllers achieving high/100% path coverage.
- **Visual Documentation**: Created a comprehensive architectural documentation suite
  (`docs/big5.md`) with 11 diagrams.
- **Internal Controller Discovery**: Implemented `InternalControllerFeatureProvider` for better
  DDD encapsulation.
- **Security & DevExperience**: Relaxed CSP in Development for Styleguide and Browser Link support.
- **Messaging Attachments**: Support for file uploads in direct messages with automatic mapping.
- **Conversation View**: Grouped messaging interface (F9) with full history and premium UI.
- **Verified Components**: Added unit tests for `LocalStorageService` and `MessageService` enhancements.

### Optimized

- **Build & Linting**: Achieved 100% compliance with `dotnet format --verify-no-changes`.
- **Ignore Strategy**: Added `.markdownlintignore` and updated `.gitignore` for coverage reports.
- **Static Analysis**: Resolved all remaining compiler & SonarQube warnings (`CA1002`, `CA1819`,
  `CS8602`, `CA1305`, `S3267`, `SA1611`, `SA1202`, `SA1402`, `CS0200`) for an immaculate codebase.
- **Structured Storage**: Implemented dated subdirectories (YYYY/MM) in `LocalStorageService` for
  better scalability and disk organization.

### Fixed

- **404 Routing**: Restored `public` access to all Web controllers to ensure proper discovery.
- **Inconsistent Accessibility**: Fixed `CS0051` by making `LoginViewModel` public.
- **Type Conversion**: Resolved `bool?` to `bool` mismatch in `AccountController`.
- **Identity Hash**: Fixed string-to-User conversion in `DbInitialiser` password hashing.
- **Dependency Cleanup**: Removed unused `Identity.Core` and `Stores` packages from Application.
- **Domain Entity**: Removed mutable property setter from Message Attachments collection to fix `CA2227`.

[0.9.0-alpha]: https://github.com/Tobia/TicketsPlease/releases/tag/v0.9.0
[2026-03-27]: https://github.com/BitLC-NE-2025-2026/TicketsPlease/compare/v0.2.0...v0.3.0
[2026-03-26]: https://github.com/BitLC-NE-2025-2026/TicketsPlease/compare/v0.1.0...v0.2.0

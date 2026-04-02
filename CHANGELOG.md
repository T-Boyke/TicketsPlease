# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [0.9.0-alpha] - 2026-04-02
#### Added
- **SignalR Architecture**: Real-time infrastructure for live collaboration.
- **Notification Center**: Enterprise Toast system for instant alerts and updates.
- **Live Comments**: New comments appear instantly on the ticket details page without refresh.
- **Kanban Sync**: Real-time notification support for board status changes.
- **Presence Indicators**: Infrastructure for tracking users on active tickets.
- **Clean Concurrency**: Repository-level RowVersion management for better architecture.

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
- **Static Analysis**: Resolved multiple SA1402, CA1515, and CS0660 warnings.
- **Structured Storage**: Implemented dated subdirectories (YYYY/MM) in `LocalStorageService` for
  better scalability and disk organization.

### Fixed

- **404 Routing**: Restored `public` access to all Web controllers to ensure proper discovery.
- **Inconsistent Accessibility**: Fixed `CS0051` by making `LoginViewModel` public.
- **Type Conversion**: Resolved `bool?` to `bool` mismatch in `AccountController`.
- **Identity Hash**: Fixed string-to-User conversion in `DbInitialiser` password hashing.
- **Dependency Cleanup**: Removed unused `Identity.Core` and `Stores` packages from Application.

[0.9.0-alpha]: https://github.com/Tobia/TicketsPlease/releases/tag/v0.9.0
[2026-04-02]: https://github.com/BitLC-NE-2025-2026/TicketsPlease/compare/v0.3.0...main
[2026-03-27]: https://github.com/BitLC-NE-2025-2026/TicketsPlease/compare/v0.2.0...v0.3.0
[2026-03-26]: https://github.com/BitLC-NE-2025-2026/TicketsPlease/compare/v0.1.0...v0.2.0

# 🧠 TicketsPlease – Project Intelligence

Dieses Dokument dient als zentrale Wissensbasis für die Mission, Vision und den aktuellen
Fortschritt des TicketsPlease-Projekts.

## 📋 Table of Contents

- [Vision & Mission](#vision--mission)
- [MVP Roadmap Status](#mvp-roadmap-status)
- [Bounded Contexts](#bounded-contexts)

---

## Vision & Mission

TicketsPlease ist ein hochmodernes Kanban-Ticketsystem, das als Referenz für modernste
C#-Entwicklung dient. Der Fokus liegt auf kompromissloser Softwarearchitektur
(Clean Architecture) und höchster Code-Qualität.

> [!IMPORTANT]
> **MVP First:** Die Einhaltung der Phase 1 (IHK MVP) hat oberste Priorität vor allen
> Enterprise-Features.

---

## MVP Roadmap Status

Das Projekt ist phasenweise nach der [MVP-Roadmap](file:///d:/DEV/Tickets/docs/MVP_Roadmap.md) aufgebaut.

| Phase  | Fokus                                    | Status       |
| ------ | ---------------------------------------- | ------------ |
| **P1** | IHK MVP Kern (Identity, Kanban, Tickets) | 🏗️ In Arbeit |
| **P2** | CI/CD & Groundwork                       | ✅ Aktiv     |
| **P3** | Domain Modeling & IAM                    | ✅ Aktiv     |

---

## Bounded Contexts

Die Domäne ist in klare, abgegrenzte Kontexte unterteilt:

- **Identity & Access:** Auth, RBAC, Profile.
- **Ticket Management:** Core Business Domain (Tickets, SubTickets).
- **Workflow:** Kanban-Stadien, SLAs.
- **Communication:** Chat, Kommentare, Benachrichtigungen.
- **Asset Management:** Blob-Storage & Dateizuordnungen.

---

_TicketsContext v1.0 | 2026-03-09_

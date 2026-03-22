# 🧬 TicketsPlease – Domain Knowledge

Geschäftsregeln und fachliche Tiefe der Kern-Domäne.

## 📋 Table of Contents

- [Ticket Domain Entity](#ticket-domain-entity)
- [Chillischoten-Metrik](#chillischoten-metrik)
- [Geschäftsregeln (Business Rules)](#geschäftsregeln-business-rules)

---

## Ticket Domain Entity

Das zentrale Domain-Aggregat.

- **Identifikation:** Eindeutiger SHA1-Hash.
- **Tracking:** Geo-Timestamp bei jeder Zustandsänderung.
- **Rich Model:** Logik lebt in der Entity (`ticket.MoveToReview()`), nicht im
  Service.

---

## Chillischoten-Metrik

Ein visuelles System zur Bewertung von Ticket-Komplexität:

- **1 Schote:** Trivial / Dokumentation.
- **3 Schoten:** Standard-Feature / Refactoring.
- **5 Schoten:** Kritischer Bug / Architektur-Umbau.

---

## Geschäftsregeln (Business Rules)

- **Close Logic:** Tickets können nur vom Ersteller, Admin oder Teamlead Final
  geschlossen
  > (`Closed`) werden.
- **State Flow:** To Do → In Progress → Review → Done → (Auto-Archiv nach X
  Tagen).

---

_DomainKnowledge v1.0 | 2026-03-09_

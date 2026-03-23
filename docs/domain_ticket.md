# Core Domain: The Ticket Entity

Die `Ticket`-Entität ist das absolut zentrale Objekt dieser Applikation. Sie
unterliegt strikten Design-Regeln aus dem _Domain-Driven Design (DDD)_.

## Eigenschaften (Properties)

Eine vollständige Ausbaustufe eines Tickets umfasst:

- **Id:** `Guid` (Global Unique Identifier)
- **Sha1Hash:** `string` (Ein Hash zur globalen, systemweiten Identifikation
  eines Tickets. Erlaubt es, das Ticket zu kopieren und überall im System per
  Hash zu referenzieren).
- **Title/Name:** `string` (Max. 150 Zeichen)
- **Description:** `string` (Markdown wird unterstützt)
- **State / Status:** `TicketStatus` Enum
  - `ToDo`
  - `InProgress`
  - `InReview`
  - `Done`
- **Priority:** `TicketPriority` Enum (`Low`, `Medium`, `High`,
  `Critical/Blocker`)
- **Complexity / Difficulty (Chillischoten 🌶️):** `byte` (1 bis 5). Dies soll
  rein visuell die Komplexität abbilden, ohne klassische "Story Points" zu
  nutzen.
- `CreatedAt`: `DateTimeOffset` (Systemgesteuert)
- `UpdatedAt`: `DateTimeOffset?` (Bei Modifikation)
- `GeoIpTimestamp`: `string` (Kombination aus IP-Adresse/Geo-Location und
  exaktem Zeitstempel der Erstellung oder Modifikation aus Audit-Gründen).
- `StartDate`: `DateTimeOffset?` (Optional, wann die Arbeit beginnt)
- `Deadline`: `DateTimeOffset?` (Optional, wann die Arbeit beendet sein muss)
- `EstimatedHours`: `decimal?` (Geschätzter Aufwand)
- **Time Tracking & Logs:**
  - Im Gegensatz zu einem simplen `LoggedHours` Feld, besitzt das Ticket eine
    Collection von `TimeLog` Objekten (`IReadOnlyList<TimeLog>`). Dies erfasst
    exakt, _welcher_ User _wann_ _wie lange_ an dem Ticket gearbeitet hat.
- **Service Level Agreements (SLAs):**
  - Über die Priorität (`TicketPriority`) ist eine `SLA_POLICY` verknüpft, die
    feste `ResponseTimeHours` und `ResolutionTimeHours` vorschreibt. Die UI
    warnt den User optisch vor Ablauf dieser Deadlines.
- **Tags & Labels:**
  - Eine Collection an Metadaten (`IReadOnlyList<Tag>`), um Tickets
    projektübergreifend zu filtern (z.B. `#frontend`, `#bug`).
- **Dateianhänge (Attachments):**
  - Eine Collection von `FileAsset` (`IReadOnlyList<FileAsset>`). Erlaubt den
    Upload von PDFs, Log-Files oder Screenshots (`DOMPurify` geprüft) direkt an
    das Ticket.
- **Assignees:**
  - `AssignedUserId`: `Guid?` (Referenz zum bearbeitenden Nutzer)
  - Fazit: Kann an eine Person oder ein ganzes `Team` delegiert werden (via
    Join-Table oder eigener Property).
- **Hierarchy:**
  - `ParentTicketId`: `Guid?`
  - `SubTickets`: `IReadOnlyList<Ticket>` (Hiermit können große Epics in Tasks
    zerlegt werden).

## Domain Logic & Encapsulation

Wir nutzen im Domain Layer **Rich Models** anstelle von anämischen
(datengetriebenen) Modellen. Dies bedeutet:

1. **Properties besitzen `private set`:** Sie können nicht einfach von außen
   manipuliert werden (`ticket.Status = TicketStatus.Done;` ist verboten!).
2. **Verhaltens-Methoden:** Zustandsänderungen geschehen ausschließlich über
   definierte Methoden der Klasse (z.B. `ticket.MoveToReview(Guid userId)`), die
   intern sicherstellen, dass alle Business-Regeln eingehalten werden.
3. **Strict Close Rules (Ticket-Schließung):**
   - Ein Ticket darf nur manuell über die Methode `ticket.Close(User actor)`
     geschlossen werden. Diese Methode prüft zwingend, ob der `actor` entweder
     der Ersteller (`CreatorId`), ein `Admin` oder ein `Teamlead` ist. Normale
     Bearbeiter dürfen Tickets nur auf "Done" verschieben, aber nicht
     wegschließen.
   - _Auto-Close:_ Ein geplanter Background-Task (Cronjob) darf Tickets, die
     länger als X Tage auf "Done" stehen, automatisiert ins Archiv/Closed
     verschieben.
4. **Konstruktoren:** Es gibt keinen leeren Parameterlosen Konstruktor für die
   Erstellung. Ein Ticket **muss** immer mit den minimalen Pflichtfeldern
   (Title, Context, GeoIpTimestamp) initialisiert werden und generiert bei
   Erstellung zwingend seinen `Sha1Hash`.

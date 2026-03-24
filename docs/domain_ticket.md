# Core Domain: The Ticket Entity

Die `Ticket`-Entität ist das absolut zentrale Objekt dieser Applikation. Sie
unterliegt strikten Design-Regeln aus dem _Domain-Driven Design (DDD)_.

## MVP-Eigenschaften (F3 Pflicht)

Die folgenden Properties sind für die Prüfung zwingend erforderlich:

- **Id:** `Guid` (Global Unique Identifier)
- **Title:** `string` (Max. 150 Zeichen, Pflicht)
- **Description:** `string` (Pflicht)
- **ProjectId:** `Guid` (FK → Project, Pflicht. Nur offene Projekte.)
- **CreatorId:** `Guid` (FK → User, automatisch = angemeldeter Benutzer)
- **CreatedAt:** `DateTimeOffset` (Systemgesteuert, automatisch)
- **AssignedUserId:** `Guid?` (Nullable, Referenz zum bearbeitenden Nutzer)
- **AssignedAt:** `DateTimeOffset?` (Automatisch bei Zuweisung gesetzt)
- **ClosedByUserId:** `Guid?` (Wer hat geschlossen? F3.4)
- **ClosedAt:** `DateTimeOffset?` (Wann geschlossen? F3.4)
- **Status:** Offen / Geschlossen (F3.4)

## Enterprise-Eigenschaften (Phase 2–5, Post-MVP)

Die folgenden Properties werden erst nach der Prüfung implementiert:

- **Sha1Hash:** `string` (Globale Identifikation)
- **State / WorkflowStateId:** `Guid` FK (Kanban-Status: ToDo, InProgress,
  Review, Done)
- **Priority:** `TicketPriority` Enum (`Low`, `Medium`, `High`, `Blocker`)
- **Complexity / Difficulty (Chillischoten 🌶️):** `byte` (1 bis 5)
- **GeoIpTimestamp:** `string` (Audit: IP/Geo + Zeitstempel)
- **StartDate:** `DateTimeOffset?` (Geplanter Beginn)
- **Deadline:** `DateTimeOffset?` (Abgabetermin)
- **EstimatedHours:** `decimal?` (Geschätzter Aufwand)
- **Time Tracking:** Collection von `TimeLog` Objekten
- **SLA Policy:** Über Priorität verknüpft
- **Tags & Labels:** Collection von `Tag` (n:m)
- **Dateianhänge:** Collection von `FileAsset`
- **Hierarchy:** `ParentTicketId` + `SubTickets` Collection

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

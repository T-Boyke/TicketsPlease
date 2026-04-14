# ADR 0080: Ticket Identification & Audit Trail (SHA1 & GeoIP)

- Status: accepted
- Deciders: Tobias
- Date: 2026-03-06

## Context and Problem Statement

Ein zentrales Merkmal unseres Systems ist es, dass Tickets einfach und eindeutig referenziert werden
müssen, ohne lange UUIDs (Guids) manuell kopieren zu müssen. Zweitens erfordert die zunehmende
Ausrichtung als Enterprise- und B2B-Tool (Rechnungsstellung und Zeiterfassung basierend auf
Ticket-Aktivitäten) einen absolut revisionssicheren Audit-Trail.

Es muss entschieden werden, wie wir Tickets für den Benutzer sichtbar referenzieren und wie wir die
Zeit-/Ortsdaten der Ticketerstellung und -bearbeitung unmanipulierbar speichern.

## Decision Drivers

- Benutzerfreundlichkeit beim Kopieren/Teilen von Tickets.
- Eindeutige systemweite Identifizierbarkeit, auch offline.
- Revisionssicherheit / Auditierbarkeit (Compliance für Enterprise-Kunden).
- Performance (Indexierbarkeit in der Datenbank).

## Considered Options

- Nutzung der reinen Datenbank-Id (Guid) für alles.
- Inkrementeller Integer basierend auf dem Projekt (`PROJ-123` wie in Jira).
- Generierung eines dedizierten SHA1 Hashes für Referenzzwecke + explizite Geo/IP Timestamp Column.

## Decision Outcome

Chosen option: "Generierung eines dedizierten SHA1 Hashes + explizite Geo/IP Timestamp Column",
because diese Kombination die beste Balance aus Dezentralität, Kollision-Sicherheit und
Revisionssicherheit bietet.

Ein `Sha1Hash` auf dem `Ticket` Objekt wird beim Konsturktor-Aufruf erzeugt und dient fassen als
universelle, gut kopierbare ID über die reinen Systemgrenzen hinweg. Gleichzeitig wird dem
Konstruktor zwingend ein `GeoIpTimestamp` Objekt übergeben.

### Positive Consequences

- Entwickler können kurze Hashes austauschen, statt komplexe URLs zu schicken.
- Extreme Revisionssicherheit für Compliance und B2B-Prozesse, da der GeoIP-Stempel explizit bei der
  Domain-Erstellung (Konstruktor) injiziert wird.
- Losgelöst von Projektabhängigkeiten (anders als `PROJ-123`, was bei Ticket-Verschiebungen zwischen
  Projekten problematisch wird).

### Negative Consequences

- Leicht erhöhter Speicherbedarf in der Datenbank.
- `Ticket` Konstruktoren werden strikter und erfordern mehr Vorarbeit in der Application-Layer
  (Ermittlung von IP und Zeit), bevor ein Ticket persistiert werden kann.

## Pros and Cons of the Options

### Inkrementeller Integer (`PROJ-123`)

- Good, because es ist der Industrie-Standard, den Nutzer von Jira kennen.
- Bad, because es erzwingt eine strikte Zuordnung zu einem Prefix. Verschiebt man das Ticket in ein
  Projekt mit anderem Prefix, bricht die Referenz oder wird verwirrend.
- Bad, because in hoch-skalierenden, verteilten System ist das Generieren einer lückenlosen
  Sequenznummer performativ oft teuer.

### SHA1 Hash + GeoIp Timestamp

- Good, because der Hash ist universell und kann weltweit als Index genutzt werden.
- Good, because der GeoIP Stempel sichert die Integrität sofort auf Domain-Ebene ab.
- Bad, because Hashes sind nicht so schön zu lesen wie kurze Zahlen.

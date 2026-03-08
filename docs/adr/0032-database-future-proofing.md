# ADR 0032: Database Future-Proofing (Enterprise Extensions)

**Datum:** 2026-03-06

**Status:** Accepted

## Kontext

Das aktuelle Schema von **TicketsPlease** befindet sich in der 3. Normalform (3NF) und deckt die Kernanforderungen ab. Um das System jedoch fit für den Enterprise-Einsatz (SaaS, Compliance, Skalierbarkeit) zu machen, fehlen Mechanismen für Mandantentrennung, Datenlöschschutz, strikte Prozesssteuerung und kundenspezifische Erweiterbarkeit.

## Entscheidung

Wir führen vier fundamentale Erweiterungen in das Kern-Design ein:

1.  **Multi-Tenancy (Mandantenfähigkeit):** Jede geschäftkritische Entität erhält eine `TenantId` (FK zu einer neuen `ORGANIZATION` Tabelle). Wir nutzen den **Shared Database, Isolated Schema** Ansatz (via Global Query Filters in EF Core).
2.  **Soft-Delete:** Alle Entitäten erben von einer `BaseEntity` mit `bool IsDeleted` und `DateTime? DeletedAt`. Physische Löschvorgänge werden durch logische ersetzt (Compliance & Revisionssicherheit).
3.  **Workflow State Machine:** Statt nur einem Status-Feld führen wir eine `WORKFLOW_TRANSITION` Tabelle ein. Diese definiert gültige Pfade (z.B. "Review" -> "Done" ist erlaubt, "Review" -> "Todo" nicht ohne Berechtigung).
4.  **Custom Field Engine (EAV-Pattern):** Wir erlauben dynamische Felder via `CUSTOM_FIELD_DEFINITION` und `TICKET_CUSTOM_VALUE`. Kunden können so eigene Metadaten (z.B. "Kundennummer") erfassen, ohne das DB-Schema zu ändern.

## Konsequenzen

### Positiv
- **Skalierbarkeit:** Eine Single-Installation kann tausende Kunden (Tenants) bedienen.
- **Sicherheit:** Global Query Filter verhindern, dass Daten zwischen Tenants "leaken".
- **Compliance:** Soft-Delete erfüllt gesetzliche Anforderungen zur Datenaufbewahrung.
- **Flexibilität:** Die Workflow-Engine erlaubt komplexe B2B-Prozesse.

### Negativ
- **Komplexität:** Entwickler müssen bei jeder Abfrage an die Global Filters denken.
- **Performance:** EAV-Tabellen (Custom Fields) sind bei Massenabfragen langsamer als Spalten.
- **Datenmenge:** Soft-Delete vergrößert das Datenvolumen in der Primärdatenbank.

## Alternativen

| Alternative | Pro | Contra | Entscheidung |
|---|---|---|---|
| Database-per-Tenant | Maximale Isolation | Extrem teuer und schwer zu warten | ❌ Abgelehnt |
| Hard Delete Only | Einfachere DB | Kein Revisionsschutz, gefährlich | ❌ Abgelehnt |
| Statischer Workflow | Schnell implementiert | Nicht konfigurierbar für Kunden | ❌ Abgelehnt |
| NoSQL für Custom Fields | Sehr flexibel | Hybrid-Join Komplexität (SQL + NoSQL) | ❌ Abgelehnt |

## Referenzen
- [Database Schema (Target)](../database_schema.md)
- [ADR 0030: Database 3NF](./0030-database-3nf.md)

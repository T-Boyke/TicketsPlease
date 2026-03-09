# ADR 0032: Database Future-Proofing (Enterprise Extensions)

- Status: accepted
- Date: 2026-03-06

## Context

Das aktuelle Schema von **TicketsPlease** befindet sich in der 3. Normalform (3NF) und deckt die
Kernanforderungen ab. Um das System jedoch fit für den Enterprise-Einsatz (SaaS, Compliance,
Skalierbarkeit) zu machen, fehlen Mechanismen für Mandantentrennung, Datenlöschschutz, strikte
Prozesssteuerung und kundenspezifische Erweiterbarkeit.

## Entscheidung

Wir führen vier fundamentale Erweiterungen in das Kern-Design ein:

1. **Multi-Tenancy (Mandantenfähigkeit):** Jede geschäftkritische Entität erhält eine `TenantId` (FK
   zu einer neuen `ORGANIZATION` Tabelle). Wir nutzen den **Shared Database, Isolated Schema**
   Ansatz (via Global Query Filters in EF Core).
2. **Soft-Delete:** Alle Entitäten erben von einer `BaseEntity` mit `bool IsDeleted` und `DateTime?
DeletedAt`. Physische Löschvorgänge werden durch logische ersetzt (Compliance &
   Revisionssicherheit).
3. **Workflow State Machine:** Statt nur einem Status-Feld führen wir eine `WORKFLOW_TRANSITION`
   Tabelle ein. Diese definiert gültige Pfade (z.B. "Review" -> "Done" ist erlaubt, "Review" ->
   "Todo" nicht ohne Berechtigung).
4. **Custom Field Engine (EAV-Pattern):** Wir erlauben dynamische Felder via
   `CUSTOM_FIELD_DEFINITION` und `TICKET_CUSTOM_VALUE`.

---

| Option              | Pro                   | Contra               | Outcome      |
| :------------------ | :-------------------- | :------------------- | :----------- |
| Statischer Workflow | Schnell implementiert | Nicht konfigurierbar | ❌ Abgelehnt |
| NoSQL Custom Fields | Sehr flexibel         | Hybrid-Join          | ❌ Abgelehnt |

## Referenzen

- [Database Schema (Target)](../database_schema.md)
- [ADR 0030: Database 3NF](./0030-database-3nf.md)

---
name: adr-writer
description: Creates Architecture Decision Records (ADRs) in the Lightweight ADR format by Michael Nygard. Use when making significant architectural decisions, introducing new technologies, or changing established patterns. Ensures proper documentation with context, decision, and consequences.
---

# 📋 ADR Writer

Erstellt Architecture Decision Records im Lightweight ADR Format (Michael Nygard).

> **Referenz:** [ADR-Index](file:///d:/DEV/Tickets/docs/adr/README.md) | [Documentation Standards](file:///d:/DEV/Tickets/.agent/workflows/documentation-standards.md)

---

## Wann dieses Skill verwenden

- Neue Technologie / NuGet-Paket wird eingeführt
- Architektur-Pattern wird geändert
- Signifikante Design-Entscheidung (z.B. Datenbank-Schema, API-Design)
- Abweichung von bestehendem ADR

---

## Wann KEIN ADR nötig

- Routine-Code-Änderungen
- Bug-Fixes
- Rein kosmetische Änderungen
- Bereits durch bestehendes ADR abgedeckt

---

## ADR erstellen

### 1. Nächste Nummer ermitteln

```bash
# Im docs/adr/ Verzeichnis die höchste Nummer finden
ls d:/DEV/Tickets/docs/adr/
# → 0001-xxx.md, 0002-xxx.md, ..., NNNN-xxx.md
# Nächste Nummer: NNNN + 1
```

### 2. Datei erstellen

**Pfad:** `docs/adr/NNNN-kebab-case-titel.md`

### 3. Template verwenden

```markdown
# NNNN. [Titel der Entscheidung]

**Datum:** YYYY-MM-DD

**Status:** Proposed | Accepted | Deprecated | Superseded by [XXXX](./XXXX-xxx.md)

## Kontext

[Welches Problem lösen wir? Warum ist eine Entscheidung nötig?]
[Technischer Hintergrund, Constraints, Anforderungen]

## Entscheidung

[Was wurde entschieden? Konkret und klar.]

## Konsequenzen

### Positiv
- [Vorteil 1]
- [Vorteil 2]

### Negativ
- [Nachteil / Trade-off 1]
- [Nachteil / Trade-off 2]

### Neutral
- [Neutrale Folge, z.B. "Team muss sich einarbeiten"]

## Alternativen

| Alternative | Pro | Contra | Entscheidung |
|---|---|---|---|
| [Option A] | ... | ... | ❌ Abgelehnt |
| [Option B] | ... | ... | ✅ Gewählt |
| [Option C] | ... | ... | ❌ Abgelehnt |
```

### 4. ADR-Index aktualisieren

Trage den neuen ADR in `docs/adr/README.md` ein:

```markdown
| NNNN | [Titel](./NNNN-kebab-case-titel.md) | Accepted | YYYY-MM-DD |
```

---

## Bestehende ADRs (Referenz)

Prüfe VOR der Erstellung eines neuen ADR, ob der Sachverhalt bereits abgedeckt ist:

```bash
# Bestehende ADRs auflisten
ls d:/DEV/Tickets/docs/adr/*.md
```

---

## Status-Lifecycle

```text
Proposed → Accepted → [Deprecated | Superseded by XXXX]
```

- **Proposed:** Zur Diskussion gestellt
- **Accepted:** Entscheidung ist gültig und bindend
- **Deprecated:** Nicht mehr relevant (z.B. Feature entfernt)
- **Superseded:** Durch neueren ADR ersetzt (Verlinkung!)

---

## Qualitäts-Kriterien

| # | Kriterium | Beschreibung |
| --- | --- | --- |
| 1 | **Klar** | Entscheidung ist eindeutig formuliert |
| 2 | **Begründet** | Kontext erklärt das "Warum" |
| 3 | **Abgewogen** | Alternativen wurden betrachtet |
| 4 | **Konsequent** | Positive UND negative Folgen benannt |
| 5 | **Verlinkt** | Superseded ADRs referenzieren den Nachfolger |

---

### Skill: adr-writer v1.0

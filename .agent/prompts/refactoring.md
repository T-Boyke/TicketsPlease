# 🔄 Prompt: Refactoring

Nutze diesen Prompt für sichere und strukturierte Refactorings.

## 📋 Refactoring Goals

- **DRY:** Duplikate eliminieren (z.B. in Partials auslagern).
- **Separation of Concerns:** Logik aus Controllern in MediatR Handler
  verschieben.
- **DDD Alignment:** Anämische Logik zurück in die Domain Entities schieben.
- **Performance:** `.AsNoTracking()` für Lesezugriffe ergänzen.

## 🧪 Safety Rules

- Führe `dotnet test` vor und nach der Änderung aus.
- Keine Breaking Changes ohne vorherige Ankündigung.
- Behalte die Testabdeckung bei oder erhöhe sie.

---

_Prompt:Refactor v1.0 | 2026-03-09_

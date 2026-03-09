# 🐞 Prompt: Bug Fix

Nutze diesen Prompt für die systematische Behebung von Fehlern.

## 📋 Debugging Flow

1. **Reproduce:** Schreibe einen fehlschlagenden Unit- oder Integration-Test.
2. **Analyze:** Nutze EF Core Debugging Skills bei DB-Problemen.
3. **Fix:** Behebe die Ursache, nicht nur das Symptom.
4. **Verify:** Stelle sicher, dass der neue Test (und alle alten) grün sind.

## 🛡️ Best Practices

- Nutze Structured Logging (`ILogger`), um Kontext zu erfassen.
- Dokumentiere den Fix im `CHANGELOG.md` oder via ADR, falls architektonisch relevant.

---

_Prompt:BugFix v1.0 | 2026-03-09_

# 🧪 TicketsPlease – Quality Assurance

Leitfaden für Testing und Code-Stabilität.

## 📋 Table of Contents

- [🔄 TDD Cycle](#-tdd-cycle)
- [🔭 Test-Struktur](#-test-struktur)
- [💯 Quality Gates](#-quality-gates)

---

## 🔄 TDD Cycle

Wir fordern den strikten TDD-Arbeitsfluss:

1. **Red:** Schreibe einen fehlschlagenden Test.
2. **Green:** Implementiere den Code, bis der Test besteht.
3. **Refactor:** Optimiere den Code unter Erhalt der Tests.

---

## 🔭 Test-Struktur

- **Architecture Tests:** NetArchTest prüft Layer-Abhängigkeiten.
- **Unit Tests:** xUnit für Domain-Logic & MediatR Handler.
- **Integration Tests:** Testcontainers (SQL Server) für Persistence Layer.
- **E2E Tests:** Playwright für kritische Web-Journeys.

---

## 💯 Quality Gates

Die CI-Pipeline bricht ab, wenn:

- **Build:** Fehler oder Warnungen (as error).
- **Tests:** Auch nur ein Test fehlschlägt.
- **Lighthouse:** Score in Performance oder Accessibility < 100.

---

_QA v1.0 | 2026-03-09_

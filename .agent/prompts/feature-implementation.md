# 🏗️ Prompt: Feature Implementation

Nutze diesen Prompt, um neue Features strukturiert in die Solution zu
integrieren.

## 📋 Steps

1. **Domain First:** Erstelle/Erweitere zuerst die Domain Entities & Value
   Objects.
2. **Application Layer:** Implementiere MediatR Commands/Queries und
   FluentValidation.
3. **Infrastructure:** Erstelle Repositories und DB-Mappings (Migrations).
4. **Presentation:** Baue die Razor Views/Components mit Tailwind 4.

## 📏 Rules

- Folge dem `/add-cqrs-feature` Workflow.
- Nutze `CancellationToken` in allen asynchronen Methoden.
- Erzwinge `RowVersion` handling bei Schreibzugriffen.
- Validiere JEDEN Input.

---

_Prompt:Feature v1.0 | 2026-03-09_

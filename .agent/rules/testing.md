# 🧪 TicketsPlease – Testing Rules

Regeln für Tests und Quality Assurance. Tests treiben das Design (TDD).

---

## TDD-Zyklus (Pflicht!)

```text
🔴 RED → Test schreiben (schlägt fehl)
🟢 GREEN → Minimalen Code schreiben (Test besteht)
🔵 REFACTOR → Code verbinden (Tests bleiben grün)
🔁 Repeat
```

## Coverage-Ziele

| Layer                      | Ziel                               |
| -------------------------- | ---------------------------------- |
| **Domain**                 | 100% – Zero Compromise             |
| **Application (Handlers)** | 90%+                               |
| **Infrastructure**         | Integration Tests (Testcontainers) |
| **Web**                    | E2E (Playwright/Vitest)            |

## Naming

- Klasse: `[ClassName]Tests`
- Methode: `[Method]_[Scenario]_[Result]`
- Beispiel: `Handle_TicketNotFound_ThrowsNotFoundException`

## AAA-Pattern (Pflicht!)

Jeder Test: **Arrange** → **Act** → **Assert**.

## Unit Tests

- Mocking via `Moq` oder `NSubstitute`.
- Teste Happy-Path UND Fehlerfälle (Validation, NotFound, Concurrency).
- CancellationToken korrekt testen.

## Integration Tests

- **Testcontainers** (echter SQL Server Docker). Kein `InMemoryDatabase`!
- Teste RowVersion-Handling und AsNoTracking-Effekte.

## Architektur Tests (NetArchTest)

- Layer-Dependencies automatisch prüfen.
- Domain darf NICHT von Infrastructure abhängen.

## Assertions

- **FluentAssertions** Pflicht:
  - `result.Should().NotBeNull()`
  - `list.Should().HaveCount(3)`
  - `act.Should().ThrowAsync<NotFoundException>()`

## Pre-Commit

- `dotnet test` muss lokal grün sein bevor ein Commit erfolgt.
- CI/CD bricht bei fehlschlagenden Tests sofort ab.

## Lighthouse

- Score muss 100/100 erreichen in: Performance, Accessibility, Best Practices, SEO.

---

## TicketsPlease Testing Rules v1.0 | 2026-03-06

---
description: Standards and workflow for writing tests (Unit & Integration) in
  the TicketsPlease project.
---

# 🧪 Testing & QA Standards

Dieser Workflow definiert die vollständigen Test-Standards für das TicketsPlease
Projekt. Tests sind **kein Nachgedanke**, sondern treiben das Design (TDD).

> **Referenz:** [ADR-0006 (Testing Strategy)](file:///d:/DEV/Tickets/docs/adr/0006-testing-strategy.md) |
> [nuget_stack.md §5](file:///d:/DEV/Tickets/docs/nuget_stack.md) |
> [instructions.md §10](file:///d:/DEV/Tickets/instructions.md)

---

## TDD-Zyklus (Red-Green-Refactor)

```text
🔴 RED: Test schreiben, der fehlschlägt
       ↓
🟢 GREEN: Minimalen Code schreiben, damit der Test besteht
       ↓
🔵 REFACTOR: Code verbessern, Tests bleiben grün
       ↓
🔁 Zurück zu RED
```

> **Regel:** Schreibe den Test **vor** der Implementierung. Nicht umgekehrt!

---

## Coverage-Ziele

| Layer | Ziel | Beschreibung |
| :--- | :--- | :--- |
| **Domain** | **100%** | Zero Compromise. Jede Business-Regel muss getestet sein. |
| **Application (Handlers)** | **90%+** | Alle Commands/Queries inkl. Fehlerfälle. |
| Layer | Ziel | Beschreibung |
| :--- | :--- | :--- |
| **Domain** | **100%** | Zero Compromise. Business-Regeln. |
| **Application** | **90%+** | Alle Commands/Queries. |
| **Infrastructure** | **Integration** | Testcontainers für SQL. |
| **Web** | **E2E** | Playwright + Vitest. |

---

## Schritte

### 1. Test-Projekt auswählen

- Wähle das passende Projekt im `tests/` Ordner.
- Erstelle ggf. einen neuen Testordner parallel zur Feature-Struktur in `Application`.

### 2. Naming Conventions (Unverletzlich!)

| Element | Convention | Beispiel |
| :--- | :--- | :--- |
| **Test-Klasse** | `[ClassName]Tests` | `CreateTicketCommandHandlerTests` |
| **Test-Methode** | `[Method]_[Scenario]_[ExpectedResult]` | `Handle_ValidCommand_ReturnsNewTicketId` |
| **Test-Methode (Fehler)** | `[Method]_[InvalidScenario]_Throws[Exception]` | `Handle_TicketNotFound_ThrowsNotFoundException` |

### 3. AAA-Pattern (Pflicht!)

Jeder Test folgt strikt dem **Arrange-Act-Assert** Muster:

```csharp
/// <summary>
/// Verifiziert, dass ein gültiger CreateTicketCommand eine neue Ticket-ID zurückgibt.
/// </summary>
[Fact]
public async Task Handle_ValidCommand_ReturnsNewTicketId()
{
    // Arrange
    var mockRepo = new Mock<ITicketRepository>();
    mockRepo.Setup(r => r.AddAsync(It.IsAny<Ticket>(), It.IsAny<CancellationToken>()))
        .Returns(Task.CompletedTask);

    var handler = new CreateTicketCommandHandler(mockRepo.Object);
    var command = new CreateTicketCommand { Title = "Test-Ticket", Description = "Beschreibung" };

    // Act
    var result = await handler.Handle(command, CancellationToken.None);

    // Assert
    result.Should().NotBeEmpty("weil eine gültige GUID zurückgegeben werden muss");
    mockRepo.Verify(r => r.AddAsync(It.IsAny<Ticket>(), It.IsAny<CancellationToken>()), Times.Once);
}
```

### 4. Unit Tests (MediatR Handler Tests)

| Regel | Beschreibung |
| :--- | :--- |
| **Mocking** | Repositories/Interfaces via `Moq`/`NSubstitute`. |
| **Isolierung** | Jeder Test ist unabhängig. Kein State-Sharing. |
| **Happy Path** | Teste den erfolgreichen Durchlauf. |
| **Fehler** | Teste `ValidationException`, `NotFoundException`. |
| **CancellationToken** | Stelle sicher, dass der Token weitergereicht wird. |
| **Domain-Logik** | Teste die Rich-Model-Methoden separat. |

### 5. Integration Tests (Repository & DB)

| Regel              | Beschreibung                                                                                            |
| :----------------- | :------------------------------------------------------------------------------------------------------ |
| **Testcontainers** | Nutze `Testcontainers.MsSql` für einen echten SQL Server Docker-Container. **Kein `InMemoryDatabase`!** |
| **Isolation**      | Jeder Test bekommt eine frische Datenbank (Container wird pro Testlauf hochgefahren).                   |
| **RowVersion**     | Teste optimistische Nebenläufigkeit (zwei gleichzeitige Updates).                                       |
| **AsNoTracking**   | Verifiziere, dass Queries kein Tracking aktivieren.                                                     |
| **Seed Data**      | Nutze Test-spezifische Seed-Daten, keine Produktions-Seeds.                                             |

```csharp
/// <summary>
/// Verifiziert, dass das Repository ein Ticket korrekt in die Datenbank persistiert.
/// </summary>
[Fact]
public async Task AddAsync_ValidTicket_PersistsToDatabase()
{
    // Arrange (Testcontainer läuft bereits via Fixture)
    var ticket = Ticket.Create("Integration Test", "Beschreibung", "127.0.0.1|2026-03-06");

    // Act
    await _repository.AddAsync(ticket, CancellationToken.None);

    // Assert
    var persisted = await _context.Tickets
        .AsNoTracking()
        .FirstOrDefaultAsync(t => t.Id == ticket.Id);

    persisted.Should().NotBeNull();
    persisted!.Title.Should().Be("Integration Test");
    persisted.RowVersion.Should().NotBeNull("weil RowVersion automatisch gesetzt werden muss");
}
```

### 6. Architektur-Tests (NetArchTest)

- Nutze `NetArchTest.Rules` um Layer-Dependency-Verletzungen automatisch zu erkennen.
- Diese Tests verhindern, dass z.B. die Domain-Schicht eine Infrastructure-Referenz bekommt.

```csharp
/// <summary>
/// Stellt sicher, dass der Domain-Layer keine Abhängigkeiten zur Infrastructure hat.
/// </summary>
[Fact]
public void Domain_ShouldNot_DependOn_Infrastructure()
{
    var result = Types
        .InAssembly(typeof(Ticket).Assembly)
        .ShouldNot()
        .HaveDependencyOn("TicketsPlease.Infrastructure")
        .GetResult();

    result.IsSuccessful.Should().BeTrue("weil die Dependency Rule verletzt wurde");
}
```

### 7. Assertions (FluentAssertions Pflicht!)

| Statt | Nutze |
| :--- | :--- |
| `Assert.Equal(expected, actual)` | `actual.Should().Be(expected)` |
| `Assert.NotNull(result)` | `result.Should().NotBeNull()` |
| `Assert.Throws<T>(...)` | `act.Should().ThrowAsync<T>()` |
| `Assert.True(condition)` | `condition.Should().BeTrue()` |
| Collection-Checks | `list.Should().HaveCount(3).And.Contain(x => x.Title == "Test")` |

### 8. Pre-Commit Verifikation

Bevor ein Commit erfolgt, **müssen** alle Tests lokal grün sein:

```cmd
// turbo
dotnet test --verbosity minimal
```

### 9. CI/CD Integration

- GitHub Actions führt `dotnet test` bei jedem Commit/PR aus.
- Der Build bricht ab, wenn **ein einziger** Test fehlschlägt.
- Google Lighthouse Score muss **100/100** in allen Kategorien erreichen
  (Performance, Accessibility, Best Practices, SEO).

### Zusammenfassung: TDD Red ✓, Green ✓, Refactor ✓, Unit Tests ✓, Integration Tests ✓, Arch Tests ✓, FluentAssertions ✓, CI Green ✓

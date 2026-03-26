# 🧪 TicketsPlease – The "Perfect Testing" Guide

Dieses Dokument beschreibt die kompromisslose Testing-Strategie für das TicketsPlease-Projekt.
Unser Ziel ist nicht nur formale "100% Coverage", sondern absolute Sicherheit gegen Regressionen
und Architekturbereiche.

---

## 🏗️ 1. Grundprinzipien

### Test-Driven Development (TDD)

Tests werden nach dem **Red-Green-Refactor**-Zyklus geschrieben. Niemals wird Business-Logik
geschrieben, bevor nicht mindestens ein roter Test existiert.

### Das AAA-Pattern (Arrange, Act, Assert)

Jeder Test in diesem Repository folgt _zwingend_ dem AAA-Muster. Dies wird durch entsprechende
visuelle Kommentare durchgesetzt, um die Lesbarkeit zu maximieren!

```csharp
[Fact]
public void Should_Activate_Ticket()
{
    // Arrange
    var ticket = new Ticket("Title");

    // Act
    ticket.Activate();

    // Assert
    ticket.IsActive.Should().BeTrue();
}
```

---

## 🎯 2. Die Schichten der Wahrheit

Wir trennen unsere Test-Suites physisch und logisch.

### 2.1 Unit Tests (`xUnit`, `Moq`)

- **Fokus:** Domain-Logik, MediatR Handlers, und pure Funktionen.
- **Regel:** Keine Netzwerk-Calls, keine echte Datenbank. Extrem schnell (< 5ms pro Test).
- **Status:** **100% Line & Branch Coverage** für `TicketsPlease.Domain` erreicht.

### 2.2 Integration Tests (`WebApplicationFactory`, SQLite in-memory)

- **Fokus:** EF Core Repositories, Datenbank-Transaktionen, Controller.
- **Regel:** Wir nutzen in der CI-Pipeline SQLite In-Memory Datenbanken mit `WebApplicationFactory`.
- **Besonderheit:** Wir nutzen einen `TestAuthHandler` zur Simulation von Benutzerrechten und einen
  `FakeAntiforgery` Dienst, um POST-Requests reibungslos zu validieren.
- **Status:** **Hohe / 100% Coverage** für Repositories, TicketService und Kern-Controller erreicht.

### 2.3 System / E2E Tests (`Playwright`)

- **Fokus:** Die gesamte UI-Reise des Clients im Browser.
- **Regel:** Wenige, aber kritische Journeys (z.B. "Neues Ticket anlegen -> Editieren ->
  Schließen").

### 2.4 Architectural Tests (`NetArchTest.eXtend`)

- **Fokus:** Verhinderung von Clean Architecture Verstößen.
- **Regel:** Die Test-Suite prüft via Reflection, ob Referenzen zur falschen Richtung zeigen
  (z.B., dass das `Domain`-Projekt keine Using-Statements aus dem `Web`-UI Projekt hat).

---

## 🛠️ 3. Continuous Quality (DevOps)

Wir integrieren Testing tief in unsere CI/CD-Pipeline:

- **Dockerized Dev:** MS SQL & Redis via Docker Compose (Siehe #96).
- **Automated E2E:** Playwright Journeys in GitHub Actions (Siehe #97).
- **Security Audit:** Statische Analyse via SonarQube Cloud (Siehe #98).
- **Penetration:** Automatisierte API-Sicherheitstests (Siehe #103).

## 🔬 4. Best Practices & Enterprise Werkzeuge

Was unterscheidet einen guten Test von einem perfekten Test?

### 100% Coverage vs. Mutation Testing (Stryker.NET)

Line-Coverage beweist nur, dass Code _ausgeführt_ wurde, nicht, dass er von Assertions _geprüft_ wurde.
Wir nutzen **Stryker.NET** (Mutation Testing). Stryker manipuliert zur Laufzeit heimlich deinen C#-Code
(tauscht `<` durch `>`, modifiziert Strings, löscht Methodenaufrufe) und schaut, ob dein Test dann
fehlschlägt. Besteht der Test trotzdem, hast du einen "Mutanten" erschaffen -> Der Test ist unzureichend!

### Fakten vs. Theorien (xUnit)

Dupliziere keinen Code für Edge-Cases.

- **`[Fact]`**: Ein deterministisches Einzel-Szenario.
- **`[Theory]`**: Eine Behavior, die auf mehrere Eingabewerte reagieren muss (Null, Leer,
  Extremwerte). Nutze hier zwingend `[InlineData]` oder `[ClassData]`.

### Data Builders & Fuzzing (Bogus)

Tests, die mit `var name = "Test"` arbeiten, sind fragil. Nutze **Bogus**, um realistische
Testdaten zu generieren.

```csharp
var faker = new Faker<Ticket>()
    .RuleFor(t => t.Title, f => f.Lorem.Sentence())
    .RuleFor(t => t.CreatedAt, f => f.Date.Past());
```

### Snapshot Testing (VerifyTests)

Anstatt JSON-Outputs oder komplexe verschachtelte DTOs mit 50 Zeilen
Assertions zu prüfen, validieren wir sie gegen Snapshots.

```csharp
[Fact]
public Task Export_ShouldMatchSnapshot()
{
    var complexResult = await _mediator.Send(new GetTicketQuery(1));
    return Verify(complexResult); // Vergleicht mit gespeicherter .verified.txt Datei
}
```

### Der Zeit-Manipulator (TimeProvider)

Nutzung von `DateTime.Now` führt zu Flaky Tests (besonders bei SLAs oder Ablauf-Logiken).
Nutze in der Produktion immer das .NET 8 `TimeProvider` interface. In den Tests injizieren wir den
`FakeTimeProvider`. So können wir die Zeit in Tests manuell manipulieren:

```csharp
// Arrange
var fakeTime = new FakeTimeProvider();
fakeTime.SetUtcNow(new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero));

// Act
ticket.Pause(fakeTime);
fakeTime.Advance(TimeSpan.FromDays(5));

// Assert
ticket.GetPausedDuration().Should().Be(TimeSpan.FromDays(5));
```

---

Diese Regeln sind in Stein gemeißelt. Kein PR wird gemerged, der diese Prinzipien unterwandert.
Happy Testing! 🚀

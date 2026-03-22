# 🤖 TicketsPlease – Agent Behavior Rules

Diese Regeln definieren das **Verhalten** des KI-Agenten bei jeder Interaktion
mit dem TicketsPlease-Projekt. Die technischen Projekt-Standards finden sich in
[instructions.md](file:///d:/DEV/Tickets/instructions.md).

---

## 🧠 Grundhaltung

- Du arbeitest an einem **IHK-Abschlussprojekt** (C# .NET 10, ASP.NET Core 10.3,
  Clean Architecture).
- Die Projekt-Dokumentation ist **Deutsch**. Code-Kommentare (XML-Docs) sind
  **Deutsch**.
- Commit-Messages sind **Englisch** (Conventional Commits).
- Antworte immer in der Sprache, in der der User schreibt.

---

## 🎯 Plan-First (Denken vor Handeln)

1. **Workflow-Check:** Prüfe vor jeder Aufgabe, ob ein passender `/workflow`
   existiert. Wenn ja: folge ihm **Schritt für Schritt**.
2. **MVP-Awareness:** Prüfe die
   [MVP-Roadmap](file:///d:/DEV/Tickets/docs/MVP_Roadmap.md). Phase 1 hat
   absoluten Vorrang. Implementiere **keine** Enterprise-Features (Phase 2-5),
   solange Phase 1 nicht komplett abgeschlossen und grün ist.
3. **ADR-Check:** Prüfe vor architektonischen Entscheidungen die bestehenden
   [ADRs](file:///d:/DEV/Tickets/docs/adr/). Widerspreche keinem ADR ohne
   explizite User-Genehmigung.
4. **Scope begrenzen:** Ändere nur, was der User angefordert hat. Keine
   ungewollten "Bonus-Refactorings".
5. **Layer identifizieren:** Vor jeder Code-Änderung die betroffenen Layer,
   Dateien und Abhängigkeiten benennen.

---

## 📂 Datei-Disziplin

- **Feature-Bündelung für CQRS** – `Command` bzw. `Query`, `Validator` und
  `Handler` eines Use Cases werden in **einer** gemeinsamen Datei gebündelt.
- **1 Class per File (Standard)** – Für alle anderen Elemente (Entities,
  Interfaces, Enums, Value Objects) gilt strikt: eigene Datei.
- **Richtige Layer-Zuordnung** – Neue Dateien **nur** im korrekten Layer:
  - Domain-Logik → `src/TicketsPlease.Domain/`
  - Use Cases → `src/TicketsPlease.Application/`
  - Persistence/Services → `src/TicketsPlease.Infrastructure/`
  - UI/Controller → `src/TicketsPlease.Web/`
- **Keine Dateien löschen** ohne explizite User-Anweisung.
- **Bestehende Patterns respektieren** – Prüfe, wie ähnliche Probleme im Code
  gelöst werden. Folge dem Stil.
- **`.editorconfig` ist bindend** – Naming Conventions und Formatting sind nicht
  verhandelbar.

---

## ✅ Qualitäts-Pflichten (Bei jeder Code-Änderung)

| Was                                                                    | Wann                        | Pflicht            |
| ---------------------------------------------------------------------- | --------------------------- | ------------------ |
| XML-Dokumentation (`<summary>`, `<param>`, `<returns>`, `<exception>`) | Alle neuen `public` Members | ✅ Immer           |
| `AbstractValidator<T>` (FluentValidation)                              | Jeder neue Command          | ✅ Immer           |
| Unit-Test (TDD: Test zuerst!)                                          | Jede neue Logik             | ✅ Immer           |
| `CancellationToken` durchreichen                                       | Alle Async-Methoden         | ✅ Immer           |
| `AsNoTracking()`                                                       | Alle Lese-Queries           | ✅ Immer           |
| `DbUpdateConcurrencyException` fangen                                  | Alle Write-Handler          | ✅ Immer           |
| Anti-Forgery / Validation / DOMPurify                                  | Bei User-Input              | ✅ Kontextabhängig |
| `aria-label`, Keyboard-Nav, semantisches HTML                          | Bei UI-Änderungen           | ✅ Immer           |

---

## 🗣️ Kommunikation

- **Bei Zweifel: Fragen** – Wenn unklar ist, ob etwas MVP oder Enterprise ist →
  frage den User. Nicht raten.
- **Breaking Changes ankündigen** – Jede Änderung, die bestehende Interfaces,
  DTOs oder API-Contracts bricht → vorher mitteilen.
- **Keine stillen NuGet-Pakete** – Kein neues NuGet-Paket ohne explizite Nennung
  und Begründung.
- **Keine stillen Architektur-Entscheidungen** – Architektur-Änderungen
  erfordern einen ADR oder User-Absprache.

---

## 🔄 Typischer Ablauf

```markdown
1. User-Request verstehen
2. Workflow-Check: Passender /workflow vorhanden? → Ja: Schritt für Schritt
   folgen → Nein: Plan erstellen (Layer, Dateien, Dependencies)
3. Implementieren (im korrekten Layer)
4. XML-Docs schreiben (alle public Members)
5. Tests schreiben (TDD: idealerweise vor der Implementierung)
6. Verifikation: dotnet build + dotnet test
7. Atomic Commit (Conventional Commits, Englisch)
```

---

## 🚫 Agent No-Gos

- ❌ Code ohne Test committen
- ❌ Bestehende Tests löschen oder auskommentieren
- ❌ Enterprise-Features implementieren wenn MVP nicht fertig
- ❌ Stille Breaking Changes (immer vorher kommunizieren)
- ❌ Mehrere logische Änderungen in einem Commit
- ❌ Code in den falschen Layer legen
- ❌ NuGet-Pakete ohne Absprache hinzufügen
- ❌ Workarounds ohne `// TODO` + Issue-Referenz
- ❌ `.editorconfig`-Regeln ignorieren
- ❌ YAGNI verletzen (keine "prophylaktischen" Abstraktionen)
- ❌ `Console.WriteLine` statt Serilog
- ❌ Hardcoded Farben (nutze CSS-Variablen)
- ❌ Bootstrap oder CDN-Links
- ❌ `DateTime` statt `DateTimeOffset`
- ❌ Secrets in `appsettings.json`
- ❌ Unsanitized Markdown-Output (DOMPurify!)
- ❌ Direct Push auf `main`
- ❌ Code-Änderung ohne GitHub Issue

---

## 📚 Referenzen

Halte dich **immer** an diese Projekt-Dokumente:

| Dokument                                                             | Beschreibung                          |
| -------------------------------------------------------------------- | ------------------------------------- |
| [instructions.md](file:///d:/DEV/Tickets/instructions.md)            | Technische Governance (16 Sektionen)  |
| [README.md](file:///d:/DEV/Tickets/README.md)                        | Projekt-Vision, Features, Tech-Stack  |
| [MVP-Roadmap](file:///d:/DEV/Tickets/docs/MVP_Roadmap.md)            | Phase-Abgrenzung (MVP vs. Enterprise) |
| [ADR-Index](file:///d:/DEV/Tickets/docs/adr/README.md)               | Alle Architektur-Entscheidungen       |
| [database_schema.md](file:///d:/DEV/Tickets/docs/database_schema.md) | ERD & Entity-Beschreibungen           |
| [domain_ticket.md](file:///d:/DEV/Tickets/docs/domain_ticket.md)     | Ticket-Entity DDD Spezifikation       |
| [nuget_stack.md](file:///d:/DEV/Tickets/docs/nuget_stack.md)         | Erlaubte NuGet-Pakete pro Layer       |
| [frontend_assets.md](file:///d:/DEV/Tickets/docs/frontend_assets.md) | Asset-Management & No-CDN Policy      |

---

### TicketsPlease Agent Rules v1.0 | 2026-03-06

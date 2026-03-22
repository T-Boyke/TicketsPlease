# 🤝 TicketsPlease – Git & Documentation Rules

Regeln für Git-Workflows, Commits, PRs und Dokumentation.

---

## Branching

- **`main` ist HEILIG** – Muss jederzeit lauffähig sein. Direct Push ist per
  Branch-Protection gesperrt.
- Branch-Naming: `feature/xyz`, `bugfix/xyz`, `hotfix/xyz`, `docs/xyz`,
  `refactor/xyz`, `test/xyz`.
- Jeder Branch startet vom aktuellen `main`.

## Commits (Conventional Commits – Englisch!)

Format: `<type>(<scope>): <subject>`

- **Types:** `feat`, `fix`, `docs`, `style`, `refactor`, `test`, `chore`
- **Subject:** Imperativ, kurz, prägnant ("add", nicht "added")
- **Body:** Optional – erkläre das "Warum", nicht das "Wie"
- **Atomarität:** Ein Commit = eine logische Änderung. Keine Misch-Commits!

## Pull Requests

- Features **nur** über PRs in `main` mergen.
- CI/CD Pipeline muss grün sein.
- Mindestens **ein Approve** im Code Review.
- Jeder PR referenziert ein GitHub Issue (`Closes #42`).

## Pre-Commit Checks

1. `CHANGELOG.md` aktualisieren → **Pflicht** bei `feat`, `fix`, `security`,
   Breaking Changes
2. `dotnet build` → Muss kompilieren
3. `dotnet test` → Alle Tests grün
4. `dotnet format --verify-no-changes` → Formatting korrekt
5. Keine Secrets in staged files

## Dokumentation

### XML-Docs (C# – Deutsch)

- Pflicht für alle `public` Members.
- Tags: `<summary>`, `<param>`, `<returns>`, `<exception>`, `<remarks>`,
  `<inheritdoc />`.

### ADRs

- Jede wesentliche Design-Entscheidung → neuer ADR in
  `docs/adr/[NNNN]-[name].md`.
- ADR-Index aktualisieren (`docs/adr/README.md`).

### CHANGELOG ([CHANGELOG.md](file:///d:/DEV/Tickets/CHANGELOG.md))

- **Pflicht-Update vor jedem Commit** bei: `feat`, `fix`, `security`, Breaking
  Changes.
- Reine `docs`, `style`, `chore`, `refactor` Commits brauchen keinen
  CHANGELOG-Eintrag.
- Format: [Keep a Changelog](https://keepachangelog.com/).
- Neue Einträge immer unter `## [Unreleased]` eintragen.
- Kategorien: `Added`, `Changed`, `Deprecated`, `Removed`, `Fixed`, `Security`.

### Mermaid-Diagramme

- Für Architektur, Flows, ERD, State Machines.
- In `docs/` oder inline in ADRs ablegen.

---

### TicketsPlease Git & Docs Rules v1.0 | 2026-03-06

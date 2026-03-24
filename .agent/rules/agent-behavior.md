# 🤖 TicketsPlease - Agent Behavior Rules

<!-- markdownlint-disable MD033 -->
<agent_behavior>
<mindset>

- Project: Final Project (C# .NET 10, ASP.NET Core 10.3, Clean Architecture)
- Language: Read/Write Docs & XML-Docs in German. Commits in English (Conventional).
  Converse in User's language.

</mindset>

<plan_first>

1. Workflow-Check: Follow `/workflows` strictly if applicable.
2. MVP-Awareness: Phase 1 (MVP) takes absolute priority. No Enterprise features without green MVP.
3. ADR-Check: Respect `/docs/adr/`. Do not contradict ADRs without user permission.
4. Scope: Modify ONLY requested items. No unsolicited refactorings.
5. Layers: Identify affected layers/files before coding.

</plan_first>

<file_discipline>

- CQRS Bundle: `Command`/`Query`, `Validator`, `Handler` MUST exist in ONE file.
- Standard: Entities, Interfaces, Enums, ValueObjects get 1 file each.
- Layers: Domain (`.Domain`), UseCases (`.Application`), DB/Services (`.Infrastructure`), UI/Api (`.Web`).
- Deletion: NEVER delete files without explicit permission.
- Pattern: Follow existing `.editorconfig` & codebase patterns. Not negotiable.

</file_discipline>

<quality_gates>

- ALWAYS: XML-Docs on public members, `AbstractValidator<T>`, Unit Tests (TDD), `CancellationToken`,
  `AsNoTracking()` for read queries, catch `DbUpdateConcurrencyException` for writes,
  semantic HTML/a11y for UI.
- CONDITIONAL: Anti-Forgery/Validation/DOMPurify on User Input.

</quality_gates>

<communication>

- Ask if unsure about MVP/Enterprise scoping.
- Announce breaking changes before applying.
- No silent NuGet packages or architectural shifts (requires ADR).

</communication>

<workflow>
1. Parse request. 2. Check workflow or plan. 3. Code in correct layer. 4. XML-Docs. 5. TDD.
6. Build & Test. 7. Atomic Commit.
</workflow>

<no_gos>

- NEVER: Commit without tests, delete tests, skip MVP, silent breaking changes, merge multiple
  logical changes into 1 commit, wrong layer logic, silent NuGet adds, TODO workarounds
  without Issue ref, ignore .editorconfig, violate YAGNI, use Console.WriteLine (use Serilog),
  hardcode colors, use CDN/Bootstrap, use DateTime (use DateTimeOffset), put Secrets in
  appsettings.json, output un-sanitized Markdown, direct push to main, code without GitHub Issue.

</no_gos>
</agent_behavior>

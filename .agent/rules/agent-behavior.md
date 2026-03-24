agent_behavior:
  mindset:
    project: "Final Project (C# .NET 10, ASP.NET Core 10.3, Clean Architecture)"
    language_mapping:
      docs_xml: "German"
      commits: "English (Conventional)"
      conversations: "User's language"
  planning_principles:
    - "Follow /workflows strictly"
    - "Prioritize MVP Phase 1 (F1-F9) over Enterprise features"
    - "Respect /docs/adr/ ADRs"
    - "Modify only requested items; no unsolicited refactorings"
    - "Identify layers and files before coding"
  file_discipline:
    cqrs_bundle: "Command/Query, Validator, Handler in ONE file"
    singularity: "Entities, Interfaces, Enums, ValueObjects in discrete files"
    layer_mapping:
      domain: ".Domain"
      use_cases: ".Application"
      db_services: ".Infrastructure"
      ui_api: ".Web"
    restrictions:
      - "Never delete files without explicit permission"
      - "Strictly follow .editorconfig and codebase patterns"
    token_efficiency:
      - "Contents of .md files in .agent/{context,rules,skills,workflows} must be in YAML format"
      - "Use English for internal rules to minimize token usage"
  quality_gates:
    mandatory:
      - "XML-Docs on public members"
      - "AbstractValidator<T>"
      - "Unit Tests (TDD)"
      - "CancellationToken usage"
      - "AsNoTracking() for read queries"
      - "DbUpdateConcurrencyException handling for writes"
      - "Semantic HTML and a11y for UI"
    conditional:
      - "Anti-Forgery/Validation/DOMPurify on User Input"
  communication_rules:
    - "Ask for clarity on MVP/Enterprise scoping"
    - "Announce breaking changes beforehand"
    - "No silent NuGet additions or architectural shifts (require ADR)"
  workflow_steps:
    1: "Parse request"
    2: "Check workflow/plan"
    3: "Code in correct layer"
    4: "Apply XML-Docs"
    5: "TDD"
    6: "Build and Test"
    7: "Atomic Commit"
  no_gos:
    - "Commit without tests"
    - "Delete existing tests"
    - "Skip MVP requirements"
    - "Silent breaking changes"
    - "Multi-task commits"
    - "Layer logic violation"
    - "Silent NuGet additions"
    - "TODO workarounds without issue reference"
    - "Console.WriteLine (use Serilog)"
    - "Hardcoded colors"
    - "CDN/Bootstrap usage"
    - "DateTime usage (use DateTimeOffset)"
    - "Secrets in appsettings.json"
    - "Unsanitized Markdown output"
    - "Direct push to main"
    - "Coding without GitHub Issue"

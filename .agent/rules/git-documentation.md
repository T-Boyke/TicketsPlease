git_documentation_rules:
  branching_strategy:
    main_branch: "Sacred; always runnable; protected; no direct pushes"
    naming_conventions: "feature/xyz, bugfix/xyz, docs/xyz, refactor/xyz, test/xyz (base from main)"
  commit_standards:
    format: "Conventional Commits (English): <type>(<scope>): <subject>"
    types: ["feat", "fix", "docs", "style", "refactor", "test", "chore"]
    subject_style: "Imperative (e.g., 'add', not 'added')"
    atomicity: "One logical change per commit; no mixed commits"
  pull_requests:
    process: "Merge via PR only; CI must be green; minimum 1 approval required"
    referencing: "Reference GitHub Issue in body (e.g., 'Closes #42')"
  pre_commit_checklist:
    mandatory_checks: ["dotnet build", "dotnet test", "dotnet format --verify-no-changes"]
    data_integrity: "Check CHANGELOG.md for feat/fix/security/breaking changes; ensure no secrets are staged"
  documentation_standards:
    xml_docs: "German language; mandatory for public members (<summary>, <param>, <returns>, <exception>)"
    adr: "Record major design decisions in docs/adr/; update index"
    changelog: "Follow 'Keep a Changelog' format; update under ## [Unreleased] section"
    visuals: "Use Mermaid for architecture, flows, and ERD diagrams in docs/ or ADRs"

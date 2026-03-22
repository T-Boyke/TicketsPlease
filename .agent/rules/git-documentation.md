# 🤝 TicketsPlease - Git & Documentation Rules

<git_docs_rules>
<branches>

- `main` is SACRED: Always runnable. Protected branch. NO direct push.
- Naming: `feature/xyz`, `bugfix/xyz`, `docs/xyz`, `refactor/xyz`, `test/xyz`. Base from `main`.
  </branches>
  <commits>
- Conventional Commits (English language). Format: `<type>(<scope>): <subject>`
- Types: feat, fix, docs, style, refactor, test, chore.
- Subject: Imperative ("add" not "added").
- Rule: ONE logical change = ONE Commit. NO mixed commits.
  </commits>
  <pull_requests>
- Merge via PR only. CI MUST be green. Minimum 1 Approve. Reference GitHub Issue (`Closes #42`).
  </pull_requests>
  <pre_commit>
- MUST pass: `dotnet build`, `dotnet test`, `dotnet format --verify-no-changes`.
- Check CHANGELOG.md for feat/fix/security/breaking. NO secets staged.
  </pre_commit>
  <documentation>
- C# XML-Docs: German language. MANDATORY for `public` members (`<summary>`, `<param>`, `<returns>`, `<exception>`).
- ADRs: Record major design decisions in `docs/adr/`. Update Index.
- CHANGELOG: Format matches `Keep a Changelog`. Update under `## [Unreleased]`. Not required for chore/docs/refactor.
- Mermaid: Use for architecture/flows/ERD in `docs/` or ADRs.
  </documentation>
  </git_docs_rules>

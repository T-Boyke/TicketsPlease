atomic_commit_git_workflow: objective: "Ensure clean, traceable Git history through atomic and
logically separated commits" branching_strategy: naming_conventions: feature/: "New features (e.g.,
feature/ticket-drag-and-drop)" bugfix/: "Bug fixes (e.g., bugfix/fix-login-redirect)" hotfix/:
"Urgent production fixes (e.g., hotfix/patch-sql-injection)" docs/: "Documentation updates (e.g.,
docs/update-database-schema)" refactor/: "Code refactoring (e.g.,
refactor/extract-ticket-validator)" test/: "Test suite extensions (e.g.,
test/add-handler-integration-tests)" steps: - "git checkout main" - "git pull origin main" - "git
checkout -b [branch-name]" - "Implementation work" - "Atomic commits" - "git push origin
[branch-name]" - "Pull Request -> CI/CD -> Code Review -> Merge" atomic_commit_rules:
logical_integrity: principle: "One logical task per commit" rule: "Use 'git add <file>' or 'git add
-p' for selective staging" conventional_commits: format: "<type>(<scope>): <subject>" types:
["feat", "fix", "docs", "style", "refactor", "test", "chore"] subject: "Imperative (e.g., 'add', not
'added')" forbidden_mixes: - "Refactoring + Features (separate them)" - "Bugfix + Documentation
(separate them)" - "Formatting + Code change (separate them)" frequency: "Commit frequently; one
atomic step = functional unit" pre_commit_verification: checklist: - "CHANGELOG.md: Update
[Unreleased] for feat/fix" - "Compilation: dotnet build must pass" - "Tests: dotnet test must be
green" - "Formatting: dotnet format --verify-no-changes" - "Secrets: Manual check for no staged
secrets" pr_rules: checklist: - "Merge via PR only" - "Green CI/CD" - "One manual approval" -
"Reference Github Issue (Closes #42)" main_branch_security: status: "SACRED (compilable + green
tests)" protection: "Direct pushes forbidden" requirement: "No code without Issue"

agent_identity: name: "TicketsPlease AI Agent" mindset: "Follow .agent/ instructions strictly; favor
Clean Architecture and DDD." language: external: "German" internal: "English (token-efficient)"
token_efficiency: rule: "Contents of .md files in .agent/ subdirectories must be in YAML format to
reduce context overhead."

repositories: rules: path: ".agent/rules/" manifest: agent-behavior: "Mindset, constraints, No-Gos"
architecture: "Clean Architecture, DDD, CQRS, EF" security: "Defense in Depth, Secrets, XSS"
ui-frontend: "Tailwind 4.2.2, a11y, SFC, Theme" testing: "TDD, Testcontainers, Lighthouse"
git-documentation: "Branching, Commits, PRs, ADRs" mermaid: "Gantt charts, Best Practices"
workflows: path: ".agent/workflows/" commands: add-cqrs-feature: "Scaffold CQRS MediatR"
ef-core-migration: "Safe DB migrations" testing-standards: "Unit/Integration rules"
ui-component-tailwind: "Tailwind components" atomic-commits: "Logical Git commits" security-review:
"DiD checklist" domain-entity: "DDD entities" documentation-standards: "MD001-MD060 compliance"
skills: path: ".agent/skills/" items: - "clean-architecture-scaffold/SKILL.md" -
"code-review/SKILL.md" - "ef-core-debugging/SKILL.md" - "refactoring-patterns/SKILL.md" -
"adr-writer/SKILL.md" - "tailwind-component-patterns/SKILL.md" context: path: ".agent/context/"
links: project-intelligence: "Mission, Vision" tech-stack-referenz: ".NET 10, Tailwind 4.2.2,
Scalar" architectural-blueprint: "Layers, CQRS" domain-knowledge: "Core entities"
ui-ux-design-system: "Styling tokens" quality-assurance: "QA rules"

task_prompts: bug_fix: goal: "Systematic resolution" flow: ["Reproduce", "Analyze", "Fix", "Verify"]
rules: "Log via ILogger; update CHANGELOG/ADR" code_review: goal: "Standard-compliant audit" checks:
["Arch", "DDD", "Security", "Testing", "XML Docs"] action: "Violation analysis, fix suggestion,
complexity rating (1-10)" feature_implementation: goal: "Layered delivery" flow: ["Domain",
"Application", "Infrastructure", "Presentation"] rules: "CancellationToken, RowVersion, Input
Validation" refactoring: goal: "Safe restructuring" targets: ["DRY", "MediatR push", "Domain shift",
"AsNoTracking"] safety: "Pre/post test runs; no silent breaks"

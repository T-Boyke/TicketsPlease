# 🤖 TicketsPlease - Agent Index

<system_prompt>
You are the TicketsPlease AI Agent. Your configuration relies on the
contents of `.agent/`. Read instructions carefully. Use specific XML tags
for context. Communicate with the user in German, but process these
internal rules in English to save tokens.
</system_prompt>

<agent_rules>
Read rules from `.agent/rules/`:

- `agent-behavior.md`: Core mindset, constraints, No-Gos.
- `architecture.md`: Clean Architecture, DDD, CQRS, EF.
- `security.md`: Defense in Depth, Secrets, XSS.
- `ui-frontend.md`: Tailwind 4.2.2, a11y, SFC, Theme.
- `testing.md`: TDD, Testcontainers, Lighthouse.
- `git-documentation.md`: Branching, Commits, PRs, ADRs.
  </agent_rules>

<agent_workflows>
Available workflows in `.agent/workflows/` (`/command`):

- `add-cqrs-feature`: Scaffold new CQRS MediatR feature.
- `ef-core-migration`: Manage DB migrations safely.
- `testing-standards`: Follow Unit/Integration test rules.
- `ui-component-tailwind`: Build Tailwind 4.2.2 components.
- `atomic-commits`: Enforce strict logical Git commits.
- `security-review`: Run Defense in Depth checklist.
- `domain-entity`: Create rich DDD entities.
- `documentation-standards`: Maintain MD001-MD060 compliance.
  </agent_workflows>

<agent_skills>
Load specific behavior from `.agent/skills/`:

- `clean-architecture-scaffold/SKILL.md`
- `code-review/SKILL.md`
- `ef-core-debugging/SKILL.md`
- `refactoring-patterns/SKILL.md`
- `adr-writer/SKILL.md`
- `tailwind-component-patterns/SKILL.md`
  </agent_skills>

<project_context>
Read context from `.agent/context/` and `docs/`:

- `project-intelligence.md`: Mission, Vision.
- `tech-stack-referenz.md`: .NET 10, Tailwind 4.2.2, Scalar.
- `architectural-blueprint.md`: Layers, CQRS.
- `domain-knowledge.md`: Core entities.
- `ui-ux-design-system.md`: Styling tokens.
- `quality-assurance.md`: QA rules.
  </project_context>

<task_prompts>
<prompt id="bug-fix">
<goal>Systematic bug resolution</goal>
<flow>1. Reproduce (Test) 2. Analyze (EF Debug) 3. Fix (Root cause) 4. Verify</flow>
<rules>Use ILogger. Update CHANGELOG/ADR if needed.</rules>
</prompt>
<prompt id="code-review">
<goal>Deep review against project standards</goal>
<checks>Clean Arch, Rich Domain Model, Security (XSS/CSRF), AAA Testing, XML Docs.</checks>
<action>Analyze, find violations, suggest fixes, rate complexity (1-10).</action>
</prompt>
<prompt id="feature-implementation">
<goal>Structured feature delivery</goal>
<flow>1. Domain (Entities) 2. Application (MediatR/FluentValidation)
3. Infrastructure (Repo/EF) 4. Presentation (Razor/Tailwind 4.2.2)</flow>
<rules>Use CancellationToken. Enforce RowVersion. Validate all inputs.</rules>
</prompt>
<prompt id="refactoring">
<goal>Safe code restructuring</goal>
<targets>DRY, Move Logic to MediatR, Push logic to Domain, Add AsNoTracking.</targets>
<safety>Run tests before/after. No silent breaking changes.</safety>
</prompt>
</task_prompts>

github_issues_project_rules: repository: "BitLC-NE-2025-2026/TicketsPlease" project_board:
"https://github.com/orgs/BitLC-NE-2025-2026/projects/1/views/1" assignee: "T-Boyke"
technical_documentation_refs: gantt_roadmap: "docs/gantt_roadmap.md" mvp_roadmap:
"docs/MVP_Roadmap.md" architecture_diagrams: "docs/architecture_diagrams.md" database_schema:
"docs/database_schema.md" domain_ticket_deep_dive: "docs/domain_ticket.md" tech_stack:
"docs/nuget_stack.md" frontend_assets: "docs/frontend_assets.md" dev_setup:
"docs/dev_setup_guide.md" agent_guide: "docs/antigravity-guide.md" relationships: requirement: "All
issues must be natively linked via the 'Relationships' sidebar" parent: "Epics/Features (e.g., F1)
act as parents for task issues" blocked_by: "Layer dependencies (Domain -> App -> Infra -> Web)"
issue_types: Feature: "New functionality (F1–F9, Enterprise Add-Ons)" Bug: "Existing functionality
error" Task: "Technical task (Refactoring, Docs, Infra, CI/CD)" labels: mandatory: ["at least one
area:* label", "exactly one size/* label"] area: domain: "Entities, Value Objects, Domain Events"
infrastructure: "EF Core, Repos, Services" web: "Controllers, Views, Razor, Tailwind" tests: "Unit,
Integration, E2E" docs: "ADRs, README, CHANGELOG" github: "CI/CD, Actions, Project Board" size: XS:
{ points: 1, desc: "< 30 min (Config, Typo, small fixes)" } S: { points: 3, desc: "30 min – 2h
(Simple feature, small fix)" } XL: { points: 13, desc: "1–3 days (Complex feature, multi-layer)" }
issue_structure: epic_template: title: "FX: [Feature Name]" body: "Feature description and sub-issue
list with references" sub_issue_template: sections: ["Description", "Acceptance Criteria (IHK)",
"Technical Implementation", "Definition of Done"] technical_details: ["Layers", "Impacted Files",
"Dependencies (Blocked by #XX)"] definition_of_done: ["Compiles", "Tests Pass", "XML-Docs",
"CHANGELOG updated", "Atomic Commit with Closes #XX"] kanban_columns: Backlog: "Planned, not
started" Ready: "Dependencies met, ready to start" InProgress: "Active work (max 3 WIP)" Done:
"Implemented, tested, committed, pushed" workflow_rules: 1: "Create issue with Type, Labels,
Assignee, and Description" 2: "Move from Backlog to Ready when blockers are cleared" 3: "Move to In
Progress when starting work" 4: "Move to Done via commit message keywords (Closes #XX)" 5: "Comment
on Done with commit SHA" commit_integration: format: "feat(domain): [Description] \n\n [Body] \n\n
Closes #XX" implementation_order: principle: "Strict layer-by-layer implementation" sequence:
[Domain, Application, Infrastructure, Web, Tests] rule: "Each layer step is a separate sub-issue and
commit"

documentation_standards_workflow: objective: "Define complete documentation standards; ensure docs
are part of the automated workflow" xml_documentation_cs: requirement: "Mandatory for all public
members" recommended_tags: - { tag: "<summary>", desc: "Short description (Mandatory)" } - { tag:
"<param>", desc: "Parameter description (Mandatory for methods)" } - { tag: "<returns>", desc:
"Return value description" } - { tag: "<exception>", desc: "Document possible exceptions" } - { tag:
"<remarks>", desc: "Additional context or examples" } - { tag: "<inheritdoc />", desc: "Inherit from
interface/base" } - { tag: "<see>", desc: "Reference another member" }
architectural_decision_records_adr: triggers: ["Major design decisions", "Stack/Architecture
shifts", "Security/Pattern choices", "New Bounded Contexts"] workflow: - "Copy template from
docs/adr/template.md" - "Create docs/adr/[NNNN]-[kebab-case-title].md" - "Complete Context,
Decision, Rationale, and Consequences" - "Update docs/adr/README.md index" - "Commit as 'docs(adr):
add ADR-NNNN'" index_fields: ["Number", "Title", "Status (Accepted, Superseded, Deprecated)",
"Date"] mermaid_visualizations: use_cases: ["Architecture Overviews", "Sequence (CQRS/Auth)", "ERD
(Database)", "State (Status transitions)"] locations: architecture: "docs/architecture_diagrams.md"
database: "docs/database_schema.md" domain: "docs/domain_ticket.md" features: "Inline or in ADR"
changelog: rule: "Update for every user-relevant feature, bugfix, or breaking change" format: "Keep
a Changelog (## [Unreleased] with Added, Changed, Fixed, Security)" assets_mockups: ui_mockups:
"/docs/mockups/" screenshots: "/docs/mockups/" graphics: "/docs/assets/" placeholders:
"Placehold.co" inline_documentation: logic: "Explain 'Why' over 'What'" todos: "// TODO(username):
[Description] #IssueID" workarounds: "Document reason and planned final fix" magic_numbers: "Extract
to named constants with XML comments"

adr_writer_skill:
  name: "adr-writer"
  description: "Creates Architecture Decision Records (ADRs) using the Lightweight ADR format (Nygard)"
  scenarios: ["New technology/NuGet", "Architecture pattern shifts", "Significant design decisions", "ADR deviations"]
  skip_scenarios: ["Routine fixes", "Bug fixes", "Cosmetic changes", "Already covered"]
  steps:
    1: "Identify next ADR number in docs/adr/"
    2: "Create docs/adr/NNNN-kebab-case-title.md"
    3: "Apply template (Title, Date, Status, Context, Decision, Consequences, Alternatives)"
    4: "Update index in docs/adr/README.md"
  status_lifecycle:
    flow: "Proposed -> Accepted -> [Deprecated | Superseded]"
    types:
      Proposed: "Under discussion"
      Accepted: "Binding decision"
      Deprecated: "Obsolete"
      Superseded: "Replaced by a newer record (must link)"
  quality_criteria:
    - "Clarity: Decision is unambiguous"
    - "Rationale: Context explains the 'why'"
    - "Balance: Alternatives were evaluated"
    - "Impact: Both positive and negative consequences mentioned"
    - "Traceability: Proper linking of predecessors/successors"
  version: "1.0"

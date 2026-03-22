# 🧬 TicketsPlease - Domain Knowledge

<domain_knowledge>
  <ticket_aggregate>
    - Identity: SHA1-based UIDs for global consistency.
    - Logic: Mutation ONLY inside Entity (`Move()`, `Assign()`). No setter leakage.
    - Security: RowVersion (byte[]) required on all modifiable entities.
  </ticket_aggregate>
  <business_rules>
    - Transitions: Valid state path (To Do -> In Progress -> Review -> Done).
    - Ownership: Closed tickets only by Creator, Admin, or TeamLead.
    - Archiving: Auto-Archive 30 days post-completion.
  </business_rules>
  <complexity_scoping>
    - 🌶️ (1): Documentation / Style / Text.
    - 🌶️🌶️🌶️ (3): Standard Feature / UseCase / UnitTests.
    - 🌶️🌶️🌶️🌶️🌶️ (5): Core Architecture / Security / External Integration.
  </complexity_scoping>
</domain_knowledge>

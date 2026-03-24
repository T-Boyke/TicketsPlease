domain_knowledge:
  ticket_aggregate:
    identity: "SHA1-based UIDs for global consistency"
    logic_encapsulation: "Mutation ONLY inside Entity (e.g., Move(), Assign()); no setter leakage"
    security: "RowVersion (byte[]) required on all modifiable entities"
  business_rules:
    transitions: "Valid state path: To Do -> In Progress -> Review -> Done"
    ownership: "Closed tickets only by Creator, Admin, or TeamLead"
    archiving: "Auto-archive 30 days post-completion"
  complexity_scoping:
    XS_1: "Documentation, Style, Text"
    S_3: "Standard Feature, UseCase, UnitTests"
    XL_13: "Core Architecture, Security, External Integration"

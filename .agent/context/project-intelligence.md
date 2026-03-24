project_intelligence:
  vision:
    goal: "Reference Kanban for .NET 10 / C# 14 (Clean Architecture + DDD)"
    policy: "Strict MVP (Phase 1); no enterprise feature leakage"
    quality_standards: ["100% Mutation Score", "BFSG-ready", "GDPR-native"]
  roadmap:
    P1_core: "Identity (RBAC), Kanban Engine, Ticket Aggregate [WIP]"
    P2_infrastructure: "CI/CD, EF Core Resilience, ICorporateSkin [Active]"
    P3_advanced: "Domain Event Bus, Audit Engine, SLA Monitor [Pending]"
  bounded_contexts:
    identity: "Authentication, Authorization, Profile, Claims"
    ticket: "Create, Assign, Transition (Rich Aggregate)"
    asset: "Local Blob storage, Anti-Virus Sandbox"
    messaging: "Real-time Comments, I18n Notifications"

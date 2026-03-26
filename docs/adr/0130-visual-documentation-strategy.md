# ADR 0130: Comprehensive Visual Documentation Strategy (The "Big 11")

## Status

Accepted

## Date

2026-03-26

## Context

Understanding the architecture of a complex multi-layered system can be challenging.
While textual documentation (READMEs, ADRs) is essential, it often lacks the clarity
of visual representation for stakeholders and new developers.

## Decision

We will maintain a "Big 11" set of comprehensive Mermaid diagrams to visualize all
critical aspects of the system. These diagrams will be co-located in `docs/big5.md`
and will include:

1. **Layered Architecture**
2. **Domain Class Model**
3. **ERD (Database Schema)**
4. **Use Case Diagram**
5. **Sequence Diagram (Ticket Lifecycle)**
6. **Test Strategy (Pyramid & Layers)**
7. **Ticket Lifecycle State Machine**
8. **Deployment Architecture**
9. **Component Dependencies**
10. **Security & Data Flow (DFD)**
11. **Business Process Workflow**

## Consequences

- **Clarity**: Single point of truth for visual understanding.
- **Maintainability**: Mermaid diagrams are version-controlled text files.
- **Scalability**: New diagrams can be added as the system grows.
- **Documentation Quality**: Meets high-end enterprise documentation standards.

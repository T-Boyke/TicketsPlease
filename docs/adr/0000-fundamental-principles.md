# ADR 0000: Fundamental Development Principles (SOLID, DRY, KISS, YAGNI)

- Status: accepted
- Deciders: Tobias
- Date: 2026-03-08

## Context and Problem Statement

In professional software development, especially for complex systems like
TicketsPlease, a lack of clear coding standards leads to technical debt,
"Spaghetti-Code," and high maintenance costs. We need a shared understanding of
the bedrock principles that guide every line of code.

## Decision Drivers

- Maximum maintainability and readability.
- Scalability for enterprise features (Phase 2-5).
- Ease of onboarding for new developers.
- Alignment with industrial "Best Practices" for IHK standards.

## Considered Options

- Ad-hoc development without explicit principles.
- Strict adherence to SOLID, DRY, KISS, and YAGNI.
- Minimalist approach focusing only on functionality.

## Decision Outcome

Chosen option: "Strict adherence to SOLID, DRY, KISS, and YAGNI", because these
principles are the industry gold standard for creating robust and clean software
architecture.

### Positive Consequences

- **SOLID**: Ensures classes are focused, extensible, and easily swappable.
- **DRY**: Reduces bugs caused by logic duplication (Single Source of Truth).
- **KISS**: Keeps the implementation simple and understandable for auditors/reviewers.
- **YAGNI**: Prevents over-engineering and keeps the focus on the MVP roadmap.

### Negative Consequences

- Might require more initial thinking and refactoring during development.
- Reviews might be stricter regarding these principles.

## Principles Overview

### SOLID

1. **Single Responsibility**: One class, one reason to change.
2. **Open-Closed**: Open for extension, closed for modification.
3. **Liskov Substitution**: Subtypes must be substitutable for their base types.
4. **Interface Segregation**: Clients shouldn't depend on interfaces they don't use.
5. **Dependency Inversion**: Depend on abstractions, not concretions.

### DRY (Don't Repeat Yourself)

Every piece of knowledge or logic must have a single, unambiguous, authoritative representation
within the system.

### KISS (Keep It Simple, Stupid)

Design and implementation should be as simple as possible. Avoid unnecessary complexity.

### YAGNI (You Aren't Gonna Need It)

Do not add functionality until it is actually needed. Focus on the current requirements (MVP).

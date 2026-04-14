# ADR 0063: Internal Controller Discovery for DDD Encapsulation

## Status

Accepted

## Date

2026-03-26

## Context

In our Domain-Driven Design (DDD) approach, we strive for maximum encapsulation. We want our
controllers to be `internal sealed` to prevent unintended external access and to keep the public API
surface area of our Web projects clean. However, by default, ASP.NET Core MVC only discovers public
controllers.

## Decision

We will implement a custom `InternalControllerFeatureProvider` that overrides the default behavior
to allow the discovery of `internal sealed` controllers. This provider will be registered in
`Program.cs` during the MVC service configuration.

## Consequences

- **Encapsulation**: We can now use `internal sealed` for all controllers, ensuring they are only
  accessible within the Web project and the framework.
- **DDD Alignment**: This better aligns our technical implementation with DDD principles of keeping
  implementation details private.
- **Maintainability**: Reduced public surface area makes the project easier to maintain and refactor
  without breaking external consumers.

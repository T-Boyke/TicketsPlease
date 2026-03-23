# ADR 0100: Globalization & Localization (I18N)

- Status: accepted
- Deciders: Tobias
- Date: 2026-03-08

## Context and Problem Statement

TicketsPlease is designed as an enterprise-grade product. Even for the IHK MVP,
hardcoding strings in the UI is a "Code Smell" that hinders
internationalization. We need a strategy to support multiple languages
(German/English) from the start.

## Decision Drivers

- "I18N Readiness" (Enterprise Requirement).
- Clean Code (No hardcoded strings in code or views).
- Scalability to new markets.
- Standardized .NET tools.

## Considered Options

- Hardcoding strings and refactoring later.
- Database-driven translations.
- ASP.NET Core Request Localization + `.resx` Resource Files.

## Decision Outcome

Chosen option: "ASP.NET Core Request Localization + .resx", because it is the
industry standard for .NET development and integrates natively with the Request
Pipeline.

### Positive Consequences

- All UI strings are centralized in `/Resources`.
- Language can be determined by Browser Header (`Accept-Language`), Cookie, or
  User Profile.
- Dates and Currencies are automatically formatted correctly based on the
  `CultureInfo`.

### Negative Consequences

- Slightly more overhead during development (adding keys to resx).
- Managing large `.resx` files can become cumbersome without tools.

## Strategy

1. Create a `SharedResource.cs` dummy class for global strings.
2. Use `IStringLocalizer<T>` in Controllers and Views.
3. Every new UI element MUST use a localization key, not a literal string.

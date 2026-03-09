# ADR 0090: Identity & Access Management (IAM) (RBAC)

- Status: accepted
- Deciders: Tobias
- Date: 2026-03-08

## Context and Problem Statement

The TicketsPlease system requires a secure way to manage users, passwords, and permissions. We need
to distinguish between standard "Users" (who can create and edit their own tickets) and "Admins"
(who can manage system-wide settings and other users).

## Decision Drivers

- Security and Industry Standards.
- Ease of implementation (MVP time constraints).
- Integration with ASP.NET Core.
- Compliance with IHK requirements (User data must include Username, Firstname, Email).

## Considered Options

- Custom-built Auth system (JWT/Manual).
- ASP.NET Core Identity (Default UI or API).
- External Identity Providers (Keycloak, Auth0).

## Decision Outcome

Chosen option: "ASP.NET Core Identity", because it is the native, highly secure, and feature-rich
framework for .NET. We will use a **Role-Based Access Control (RBAC)** approach.

### Positive Consequences

- Built-in protection against common attacks (SQL Injection in auth, Password Hashing, XSRF).
- Easy integration with the `AppDbContext`.
- Standardized way to handle Cookies and Session security.

### Negative Consequences

- Identity database schema is somewhat rigid (requires customization for Firstname/Lastname).
- Default UI often needs heavy restyling to match our Tailwind UX.

## RBAC Roles for MVP

1. **Admin**: Can create/delete teams, manage all users, and close any ticket.
2. **User**: Can create tickets, edit their own tickets, and participate in projects they are
   assigned to.
3. **Teamlead** (Roadmap): Specialized role for managing squads.

## Security Constraints

- Passwords must meet modern complexity standards.
- Multi-Factor Authentication (MFA) is planned for Phase 3 (Enterprise).

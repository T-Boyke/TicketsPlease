# ADR 0110: Security Policies & Secret Management

- Status: accepted
- Deciders: Tobias
- Date: 2026-03-08

## Context and Problem Statement

Commiting passwords, API keys, or connection strings to Git is a critical security risk. We need a
"Zero Trust" policy for secrets and a "Defense in Depth" architecture to protect user data from the
ground up.

## Decision Drivers

- Security Policy (Defense in Depth)
- Compliance (GDPR/DSGVO)
- Minimal risk of accidental disclosure
- Auditor requirements (IHK)

## Decision Outcome

Chosen option: "Defense in Depth + Zero-Secret-in-Git Policy", using `dotnet user-secrets` for local
development and Environment Variables/KeyVault for production.

### Positive Consequences

- `appsettings.json` only contains non-sensitive configuration.
- Production secrets never touch developer machines or Git logs.
- Layered security (Database, App-Layer, UI-Sanititation) ensures that if one layer fails, others
  still protect the data.

### Negative Consequences

- Local setup requires an extra step (`dotnet user-secrets set ...`).
- Developers must be careful not to log sensitive information.

## Security Layers

1. **Transport**: Forced HTTPS and HSTS.
2. **UI**: Anti-Forgery Tokens (CSRF) and DOMPurify for Markdown (XSS).
3. **App**: FluentValidation for all Inputs.
4. **Data**: Encryption at rest (DB) and hashed passwords (Identity).
5. **Secrets**: Strict usage of `.gitignore` for settings that might contain sensitive data.

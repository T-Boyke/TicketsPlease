# 🛡️ TicketsPlease - Security Rules

<!-- markdownlint-disable MD033 -->
<security_rules>
<secrets>

- Appsettings: NEVER commit secrets.
- Local: Use `dotnet user-secrets`.
- Prod: Use Azure Key Vault / AWS Secrets Manager.
- Definition: Connection Strings, JWT-Keys, API-Tokens = Secrets.

</secrets>

<auth>

- Password Hash: ASP.NET Core Identity (Pbkdf2/Argon2Id). NO custom crypto.
- Cookies: `HttpOnly` AND `Secure` flags ALWAYS.
- Endpoints: `[Authorize]` on all protected routes.

</auth>

<input_validation>

- CQRS: EVERY Command requires `AbstractValidator<T>` (FluentValidation).
  Zero unvalidated input in business logic.
- XSRF: `[ValidateAntiForgeryToken]` on ALL POST actions.
- Strings: Limit MaxLength always.

</input_validation>

<xss_prevention>

- Markdown: Sanitization via DOMPurify ALWAYS.
- Libs: DOMPurify hosted locally via LibMan (NO CDN).
- Razor: NO `@Html.Raw()` without explicit sanitization.

</xss_prevention>

<sqli>

- Queries: EF Core parameterized ONLY. ZERO string concatenation for SQL.

</sqli>

<privacy>

- Standard: DSGVO / GDPR.
- Minimization: Only store PII (Personally Identifiable Information) when absolutely necessary.
- Isolation: Separate tables for PII (UserProfile, UserAddress). Encryption at rest for PII.
- IP-Leaks: ZERO external CDNs. Proxied fonts/scripts only.
- Logging: NEVER log PII or Secrets (GDPR Breach risk).
- Compliance: provide "Right to be forgotten" implementation.

</privacy>

<file_uploads>

- Rules: Extension Whitelist, Max Size Limit, Sanitize Filename (Path-Traversal).
- Storage: Blob-Storage ONLY (NEVER in wwwroot).

</file_uploads>
</security_rules>

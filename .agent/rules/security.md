security_rules:
  secrets_management:
    appsettings: "Never commit secrets to appsettings.json or source control"
    local_dev: "Use 'dotnet user-secrets' for local development"
    production: "Use Azure Key Vault or AWS Secrets Manager"
    definitions: "Connection strings, JWT keys, and API tokens are considered secrets"
  authentication_authorization:
    password_hashing: "Use ASP.NET Core Identity (Pbkdf2/Argon2Id); no custom cryptography"
    cookies: "Always use HttpOnly and Secure flags"
    protection: "Apply [Authorize] to all protected routes"
  input_validation:
    cqrs: "Every Command requires AbstractValidator<T> (FluentValidation); no unvalidated business logic"
    xsrf: "Apply [ValidateAntiForgeryToken] to all POST actions"
    strings: "Always implement MaxLength limits"
  xss_prevention:
    markdown: "Always sanitize via DOMPurify"
    libraries: "Host DOMPurify locally via LibMan; no CDN"
    razor: "Never use @Html.Raw() without explicit sanitization"
  sql_injection:
    queries: "Use EF Core parameterized queries only; strictly zero string concatenation for SQL"
  privacy_compliance_gdpr:
    standard: "DSGVO / GDPR compliance"
    minimization: "Store PII (Personally Identifiable Information) only when absolutely necessary"
    isolation: "Separate PII tables (UserProfile, UserAddress); encrypt PII at rest"
    external_leaks: "Zero external CDNs; use proxied fonts and scripts only"
    logging_policy: "Never log PII or secrets (GDPR breach risk)"
    right_to_be_forgotten: "Provide implementation for data deletion requests"
  file_uploads:
    rules: ["Extension Whitelist", "Max Size Limit", "Sanitize Filename (prevent Path-Traversal)"]
    storage: "Use Blob-Storage only; never store in wwwroot"

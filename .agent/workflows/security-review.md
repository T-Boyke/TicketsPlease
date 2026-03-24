security_review_workflow:
  objective: "Ensure code compliance with 'Defense in Depth' (DiD) security standards"
  defense_in_depth_model:
    layers:
      Browser: "Anti-Forgery Token (CSRF) -> DOMPurify (XSS Sanitization)"
      Application: "FluentValidation (Input) -> ASP.NET Core Identity (AuthN/AuthZ)"
      Infrastructure: "Parameterized Queries (SQLi Protection) -> Encrypted at Rest (Storage Security)"
  review_checklists:
    secrets_management:
      checklist:
        - "No secrets committed to appsettings.json"
        - "Local connection strings stored via 'dotnet user-secrets'"
        - "Production keys/tokens via secure Secrets Manager"
        - ".gitignore excludes appsettings.*.json overrides"
    authentication_authorization:
      checklist:
        - "ASP.NET Core Identity hashing (no custom crypto)"
        - "HttpOnly and Secure flags on all cookies"
        - "Configured session timeouts"
        - "RBAC (Role-Based Access Control) correctly applied"
        - "[Authorize] present on all protected endpoints"
    input_validation:
      checklist:
        - "AbstractValidator<T> for every Command"
        - "Validation enforced via MediatR Pipeline Behavior"
        - "No ModelState bypass allowed"
        - "Explicit MaxLength limits on string inputs"
    xsrf_csrf_protection:
      checklist:
        - "[ValidateAntiForgeryToken] on all POST actions"
        - "Global anti-forgery filter active"
        - "@Html.AntiForgeryToken() used in Razor forms"
    xss_prevention:
      checklist:
        - "DOMPurify for all frontend Markdown output"
        - "DOMPurify hosted locally (LibMan)"
        - "Zero @Html.Raw() without prior sanitization"
        - "No direct user-input injection into JS/HTML"
    sql_injection_prevention:
      checklist:
        - "All DB queries via EF Core (parameterized)"
        - "Zero raw SQL queries without parameters (FromSqlRaw)"
        - "No string concatenation for SQL queries"
    gdpr_privacy_by_design:
      checklist:
        - "PII in separate tables"
        - "Data deletion (Right to be Forgotten) implementation"
        - "Minimal data collection principle"
        - "Zero external CDNs (prevents IP leaks)"
        - "PII isolated from transaction data"
        - "Hard-delete workflow for user accounts"
    file_upload_security:
      checklist:
        - "Whitelist extension validation"
        - "File size limits enforced"
        - "Storage in Blobs (outside wwwroot)"
        - "Sanitized filenames (Path-Traversal protection)"
  findings_priority:
    Critical_P0: "Immediate hotfix; blocks deployment"
    High_P1: "Current sprint fix; blocks PR merge"
    Medium_P2: "Create GitHub Issue; next sprint addressal"
    Low_P3: "Document as tech debt; fix when possible"

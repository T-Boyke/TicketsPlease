# 🛡️ TicketsPlease – Security Rules

Jede Code-Änderung muss die Defense-in-Depth Security-Standards einhalten.

---

## Secrets

- **Niemals** Secrets in `appsettings.json` committen.
- Lokal: `dotnet user-secrets` verwenden.
- Produktion: Azure Key Vault / AWS Secrets Manager.
- Connection Strings, JWT-Keys, API-Tokens = Secrets.

## Authentication & Authorization

- Passwort-Hashing via ASP.NET Core Identity (Pbkdf2/Argon2Id).
- Keine eigenen Crypto-Implementierungen.
- Cookies: `HttpOnly` **und** `Secure` Flags. Immer.
- `[Authorize]` auf allen schützenswerten Endpunkten.

## Input Validation

- Jeder Command hat einen `AbstractValidator<T>` (FluentValidation).
- Kein User-Input erreicht die Business-Logik ungeprüft.
- Anti-Forgery Token (`[ValidateAntiForgeryToken]`) auf allen POST-Actions.
- String-Inputs auf MaxLength beschränken.

## XSS Prevention

- Markdown-Output **immer** durch DOMPurify sanitizen.
- DOMPurify lokal via LibMan (kein CDN!).
- Kein `@Html.Raw()` ohne vorherige Sanitization.

## SQL Injection

- Alle DB-Queries über EF Core (parameterisiert).
- Kein String-Concatenation für SQL-Queries.

## DSGVO / Privacy by Design

- Personenbezogene Daten in separaten Tabellen erfassen (UserProfile, UserAddress).
- Datensparsamkeit: Nur minimal nötige Daten erheben.
- Keine externen CDNs (IP-Leak an Drittanbieter).

## File Uploads

- Datei-Typ per Whitelist validieren.
- Dateigröße limitieren.
- Dateiname sanitizen (Path-Traversal verhindern).
- Dateien in Blob-Storage, nicht in `wwwroot`.

---

## TicketsPlease Security Rules v1.0 | 2026-03-06

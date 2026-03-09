---
description: Security review checklist based on Defense in Depth for the
  TicketsPlease project.
---

# 🛡️ Security Review Workflow (Defense in Depth)

Dieser Workflow stellt sicher, dass jede Code-Änderung die Security-Standards
des Projekts erfüllt. Unser System wird nach dem **"Defense in Depth"**
(Zwiebelschalen) Prinzip abgesichert.

> **Referenz:** [README §6 – Enterprise Security](file:///d:/DEV/Tickets/README.md)
> | [instructions.md §6](file:///d:/DEV/Tickets/instructions.md)

---

## Schichten-Modell

```text
🌐 Browser
  └── 🔒 Anti-Forgery Token (CSRF-Schutz)
       └── 🧹 DOMPurify (XSS-Sanitizing)
            └── ✅ FluentValidation (Input-Validation)
                 └── 🔑 ASP.NET Core Identity (Auth/AuthZ)
                      └── 🗄️ Parameterized Queries (SQL Injection)
                           └── 💾 Encrypted at Rest (Datenverschlüsselung)
```

---

## Checkliste (Bei jeder Code-Änderung prüfen!)

### 1. Secret Management

| Prüfpunkt | Status |
| :--- | :--- |
| Keine Secrets in `appsettings.json` committet? | ☐ |
| Connection Strings über `dotnet user-secrets` (lokal)? | ☐ |
| JWT-Keys, API-Tokens über Secrets Manager (Prod: Key Vault)? | ☐ |
| `.gitignore` enthält `appsettings.*.json` (overrides)? | ☐ |

```cmd
# Lokal: Secret setzen
dotnet user-secrets set "ConnectionStrings:Default" "Server=...;Password=..."

# Lokal: Secret anzeigen
dotnet user-secrets list
```

### 2. Authentication & Authorization

| Prüfpunkt | Status |
| :--- | :--- |
| Passwort-Hashing via ASP.NET Core Identity? | ☐ |
| Keine eigenen Hash-Algorithmen / Crypto? | ☐ |
| Cookies mit `HttpOnly` **und** `Secure` Flag? | ☐ |
| Session-Timeout konfiguriert? | ☐ |
| RBAC (Role-Based Access Control) korrekt angewendet? | ☐ |
| `[Authorize]` auf schützenswerten Endpunkten? | ☐ |

### 3. Input Validation (Kein User-Input ungeprüft!)

| Prüfpunkt | Status |
| :--- | :--- |
| Jeder Command hat einen `AbstractValidator<T>`? | ☐ |
| Validation wird über MediatR Pipeline Behavior ausgeführt? | ☐ |
| Kein `ModelState`-Bypass (z.B. `ModelState.Clear()`)? | ☐ |
| String-Inputs auf Max-Length beschränkt? | ☐ |

### 4. CSRF / XSRF Protection

| Prüfpunkt | Status |
| :--- | :--- |
| `[ValidateAntiForgeryToken]` auf allen POST-Actions? | ☐ |
| Oder globaler Anti-Forgery Filter aktiv? | ☐ |
| `@Html.AntiForgeryToken()` im Razor-Form? | ☐ |

### 5. XSS (Cross-Site Scripting) Prevention

| Prüfpunkt | Status |
| :--- | :--- |
| Markdown-Output im Frontend durch **DOMPurify** sanitized? | ☐ |
| DOMPurify lokal via LibMan installiert (kein CDN)? | ☐ |
| Kein `@Html.Raw()` ohne vorherige Sanitization? | ☐ |
| User-Input wird nicht direkt in JavaScript/HTML injiziert? | ☐ |

```javascript
// ✅ RICHTIG: DOMPurify vor DOM-Insertion
const cleanHtml = DOMPurify.sanitize(marked.parse(userMarkdown));
document.getElementById('output').innerHTML = cleanHtml;

// ❌ FALSCH: Unsanitized Markdown direkt ins DOM
document.getElementById('output').innerHTML = marked.parse(userMarkdown);
```

### 6. SQL Injection Prevention

| Prüfpunkt | Status |
| :--- | :--- |
| Alle DB-Queries über EF Core (parameterisiert)? | ☐ |
| Keine Raw-SQL Queries ohne Parameter (`FromSqlRaw`)? | ☐ |
| Kein String-Concatenation für SQL-Queries? | ☐ |

### 7. DSGVO / Privacy by Design

| Prüfpunkt | Status |
| :--- | :--- |
| Personenbezogene Daten in separaten Tabellen? | ☐ |
| Löschkonzept: Können User-Daten gezielt gelöscht werden? | ☐ |
| Datensparsamkeit: Werden nur minimal nötige Daten erhoben? | ☐ |
| Keine IP-Tracking ohne Rechtsgrundlage? | ☐ |
| Keine externen CDNs (IP-Leak an Drittanbieter)? | ☐ |
| Cookie-Banner nur wenn nötig? | ☐ |

### 8. File Upload Security

| Prüfpunkt | Status |
| :--- | :--- |
| Datei-Typ-Validierung (Whitelist, nicht Blacklist)? | ☐ |
| Dateigröße limitiert? | ☐ |
| Dateien in Blob-Storage (oder außerhalb von `wwwroot`)? | ☐ |
| Dateiname sanitized (keine Path-Traversal-Angriffe)? | ☐ |

---

## Bei Security-Findings

1. **Kritisch (P0):** Sofort via `hotfix/` Branch. Kein Deployment bis gefixt.
2. **Hoch (P1):** Fix im aktuellen Sprint. Blockiert den PR-Merge.
3. **Mittel (P2):** GitHub Issue erstellen, im nächsten Sprint adressieren.
4. **Low (P3):** Als Tech-Debt dokumentieren, bei Gelegenheit fixen.

### Zusammenfassung

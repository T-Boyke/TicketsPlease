# MVP-0003. ASP.NET Core Identity für Authentifizierung

**Datum:** 2026-03-23

**Status:** Accepted

## Kontext

Die Aufgabe (F1.3) fordert eine Authentifizierung/Autorisierung mit dem
Identity-Framework. Es müssen mindestens die Rollen Admin, Developer und
Tester existieren. Es wird ein eigener AccountController benötigt. Nicht
angemeldete Benutzer sehen nur die Startseite.

## Entscheidung

Wir verwenden **ASP.NET Core Identity** mit Cookie-basierter
Authentifizierung. Die Implementierung umfasst:

1. **Eigener `AccountController`** mit Login/Logout-Actions (kein
   Scaffolding der Identity-UI).
2. **Drei Rollen:** Admin, Developer, Tester – geseeded über
   `DbInitialiser`.
3. **`[Authorize]`-Attribut** global auf alle Controller außer der
   Startseite.
4. **Rollenbasierte Autorisierung** via `[Authorize(Roles = "Admin")]`.

## Konsequenzen

### Positiv

- Exakt konform: Identity-Framework, eigener AccountController.
- Cookie-Auth ist einfach und sicher für MVC-Anwendungen.
- Role-Based Access Control (RBAC) sofort nutzbar.
- Seed-Daten garantieren mindestens je einen User pro Rolle.

### Negativ

- Kein JWT/Bearer Token Support (für MVP nicht nötig, für API in
  Enterprise nachrüstbar).
- Benutzerverwaltung (F2.3) ist komplex, da nur über `UserManager`
  möglich (kein Scaffolding).

### Neutral

- Passwörter werden automatisch via Identity gehasht (Pbkdf2).
- `ApplicationUser : IdentityUser` erweitert den Standard-User.

## Alternativen

| Alternative              | Pro                     | Contra                 | Entscheidung |
| ------------------------ | ----------------------- | ---------------------- | ------------ |
| ASP.NET Identity         | Pflicht, integriert | Etwas komplex          | ✅ Gewählt    |
| Eigene Auth-Lösung       | Volle Kontrolle         | Unsicher, viel Aufwand | ❌ Abgelehnt  |
| Keycloak / IdentityServer | Enterprise-Grade        | Overkill für MVP   | ❌ Abgelehnt  |

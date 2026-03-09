# 🔴 TicketsPlease.Infrastructure – Die Technik

Hier werden die technischen Details implementiert. Dieser Layer kümmert sich um die Persistenz, externe APIs und systemnahe Dienste.

## 🍴 Git Branch
- **Branch:** `layer/infrastructure`
- Alle Änderungen am Infrastructure-Layer müssen auf diesem Branch erfolgen.

## 📋 Arbeitsanweisungen für Infrastructure-Entwickler

### 1. Persistence (EF Core)

- **Repositories:** Implementiere die Interfaces aus dem Application-Layer.
- **AppDbContext:** Konfiguriere hier das Datenmapping (Fluent API).
- **Strikte Policy:** Lesezugriffe immer mit `.AsNoTracking()`.
- **Migrations:** Erstelle Datenbank-Migrationen immer in diesem Projekt.

### 2. Identity & Security

- Konfiguration von ASP.NET Core Identity.
- Implementierung von Token-Services oder Auth-Providern.

### 3. External Services

- Anbindung von Mail-Servern (Smtp), Caching (Redis) oder Cloud-Storage.
- Nutze die Dependency Injection, um diese Dienste verfügbar zu machen.

### 4. Concurrency Management

- Behandle `DbUpdateConcurrencyException` explizit in den Write-Operationen.
- Nutze die `RowVersion` (Timestamp) für optimistische Nebenläufigkeit.

## 📁 Struktur

- `Persistence/`: DB-Kontext, Repositories, Konfigurationen und Migrations.
- `Services/`: Implementierung von Infrastruktur-Diensten (z.B. `MailKitService`).
- `Identity/`: User-Management und Auth-Logik.

---

## 🔗 Connectors
- **Application Layer:** Implementiert die dort definierten Interfaces.
- **SQL Server:** Direkte Anbindung über EF Core.

> [!IMPORTANT]
> Nutze für manuelle Transaktionen immer die `ExecutionStrategy` (RetryPolicy)!

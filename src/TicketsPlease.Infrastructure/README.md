# 🔴 TicketsPlease.Infrastructure – Die Technik

Hier werden die technischen Details implementiert. Dieser Layer kümmert sich um die Persistenz,
externe APIs und systemnahe Dienste.

## 🍴 Git Branch

- **Branch:** `layer/infrastructure`
- Alle Änderungen am Infrastructure-Layer müssen auf diesem Branch erfolgen.

---

## 📋 Arbeitsanweisung: Persistenz & Repositories

### 1. Repositories implementieren

Repositories implementieren die Interfaces aus dem Application-Layer. Sie nutzen den `AppDbContext`.

```csharp
public class TicketRepository : ITicketRepository {
    private readonly AppDbContext _context;
    public TicketRepository(AppDbContext context) => _context = context;

    public async Task<Ticket?> GetByIdAsync(Guid id, CancellationToken ct) {
        return await _context.Tickets
            .AsNoTracking() // Pflicht bei Lesezugriffen!
            .FirstOrDefaultAsync(t => t.Id == id, ct);
    }
}
```

### 2. Datenbank-Migrationen

Migrationen werden über die CLI im Infrastructure-Projekt verwaltet.

- **Befehl**: `dotnet ef migrations add [Name] --project src/TicketsPlease.Infrastructure`
  `--startup-project src/TicketsPlease.Web`

---

## 🛠️ Dependency Injection (DI) Connector

Die Registrierung erfolgt explizit, da wir oft unterschiedliche Implementierungen (z.B. Mocking)
haben.

- **Ort**: `DependencyInjection.cs`
- **Methode**: `AddInfrastructureServices`

**Wichtig**: Wenn du einen neuen Service/Repository hinzufügst, musst du ihn hier registrieren:

```csharp
services.AddScoped<ITicketRepository, TicketRepository>();
```

---

## 📁 Struktur

- `Persistence/`: `AppDbContext`, EF-Konfigurationen und Migrations.
- `Services/`: Implementierung externer Dienste (Mail, FileStorage).
- `Identity/`: User-Management und Auth-Provider.

---

## 🔗 Connectors

- **Application Layer:** Wir implementieren deren Contracts.
- **Datenbank:** SQL Server (Produktion) / SQLite (Tests).
- **DI Container:** Wir stellen die Implementierungen für das gesamte System bereit.

> [!IMPORTANT]
> Nutze für Datenbank-Operationen immer die `ExecutionStrategy` (Polly), um transiente Fehler abzufangen!

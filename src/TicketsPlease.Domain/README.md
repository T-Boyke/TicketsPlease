# 🟢 TicketsPlease.Domain – Der Core

Dies ist der wichtigste Layer der Anwendung. Hier leben die **Geschäftsregeln**
und die **Fachlichkeit**. Dieser Layer ist völlig isoliert von technischen
Details (Datenbanken, UI, Frameworks).

## 🍴 Git Branch

- **Branch:** `layer/domain`
- Alle Änderungen am Domain-Layer müssen auf diesem Branch erfolgen.

## 📋 Arbeitsanweisungen für Domain-Entwickler

### 1. Zero Dependencies

- Dieses Projekt darf **keine** NuGet-Pakete referenzieren (Ausnahme: `MediatR.Contracts`).
- Wir nutzen reines C#. Absolute Isolation ist Pflicht.

### 2. Rich Domain Models

- **Keine anämischen Modelle:** Entities haben Logik!
- **Private Setter:** Properties dürfen nicht von außen manipuliert werden (`private set`).
- **Verhaltensmethoden:** Zustandsänderungen erfolgen über explizite Methoden
  (z.B. `ticket.MoveToReview()`).

### 3. Value Objects

- Nutze Value Objects für komplexe Typen (`EmailAddress`, `PriorityLevel`, `Sha1Hash`).
- Gleichheit wird über den Wert definiert, nicht über die Identität.

### 4. Domain Events

- Löse fachliche Seiteneffekte über Domain Events (`INotification`) aus.
- Handler reagieren entkoppelt in der Application oder Infrastructure Layer.

## 📁 Struktur

- `Entities/`: Rich Models (Ticket, User, Team).
- `ValueObjects/`: Unveränderliche Typen.
- `Events/`: Domain-spezifische Notifications.
- `Enums/`: Status- und Prioritäts-Definitionen.

---

## 🔗 Connectors

- **Application Layer:** Nutzt die Entities und Events, um Use Cases abzubilden.
- **Infrastructure Layer:** Mappt diese Entities auf das Datenbankschema.

> [!IMPORTANT]
> Denke an die **Geo/IP Timestamps** bei jeder Erstellung/Änderung einer Entity!

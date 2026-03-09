# 🟢 TicketsPlease.Domain – Der Core

Dies ist der wichtigste Layer der Anwendung. Hier leben die **Geschäftsregeln** und die
**Fachlichkeit**. Dieser Layer ist völlig isoliert von technischen Details (Datenbanken, UI).

## 🍴 Git Branch

- **Branch:** `layer/domain`
- Alle Änderungen am Domain-Layer müssen auf diesem Branch erfolgen.

---

## 🧠 Domain-Driven Design (DDD) Grundlagen

### 1. Rich Domain Model vs. Anemic Domain Model

Wir nutzen **Rich Domain Models**. Das bedeutet: Die Entity ist kein bloßer Datencontainer,
sondern verwaltet ihren eigenen Zustand.

**❌ FALSCH (Anemic):**

```csharp
public class Ticket {
    public string Status { get; set; } // Von außen manipulierbar
}
// Im Services: ticket.Status = "Closed";
```

**✅ RICHTIG (Rich):**

```csharp
public class Ticket {
    public string Status { get; private set; } // Nur von innen änderbar

    public void Close(string reason) {
        if (string.IsNullOrEmpty(reason)) throw new DomainException("Reason required");
        Status = "Closed";
        AddDomainEvent(new TicketClosedEvent(this, reason));
    }
}
```

### 2. Value Objects

Value Objects sind kleine Objekte ohne Identität. Sie machen den Code sicherer. Statt eines
einfachen `string` für eine E-Mail nutzen wir `EmailAddress`.

---

## 📋 Arbeitsanweisungen: Wie erstelle ich eine Entity?

1. **Erstelle die Klasse** im Ordner `Entities/`.
2. **Properties**: Nutze `private set`, um unkontrollierte Änderungen zu verhindern.
3. **Konstruktor**: Erstelle einen internen parameterlosen Konstruktor für EF Core und einen
   öffentlichen Konstruktor, der alle Pflichtfelder validiert.
4. **Verhalten**: Implementiere Methoden für Zustandsänderungen (z.B. `AssignToUser`).
5. **Dokumentation**: Nutze XML-Tags für **jede** Methode und Property.

---

## 📁 Struktur

- `Entities/`: Kern-Objekte (z.B. `Ticket`, `Member`).
- `ValueObjects/`: Komplexe Typen (z.B. `Priority`, `GeoLocation`).
- `Events/`: Benachrichtigungen über fachliche Änderungen.
- `Exceptions/`: Fehler, die rein fachlicher Natur sind.

---

## 🔗 Connectors

- **DI Connection**: Dieser Layer benötigt **keine** DI Registrierung, da er keine Services
  implementiert, sondern nur Daten und Regeln definiert.

> [!IMPORTANT]
> Keine Abhängigkeiten zu anderen Projekten! Wenn du etwas aus der Application Layer brauchst, ist
> das ein Architekturfehler.

# ADR 0031: EF Core Resilience & Concurrency

## Status

Akzeptiert ✅

## Kontext

In einer Enterprise-Anwendung ist Datenintegrität und Systemstabilität bei
Datenbankausfällen kritisch. Gleichzeitig müssen "Lost Updates" bei
gleichzeitigem Zugriff mehrerer Nutzer verhindert werden.

## Entscheidung

1. **EnableRetryOnFailure:** Der DB-Context wird mit der SQL Server
   Ausführungsstrategie für automatische Retries konfiguriert.
2. **Optimistische Nebenläufigkeit:** Jede Entität erhält ein `RowVersion`-Feld
   (Timestamp/ConcurrencyToken).
3. **Async Policy:** Alle asynchronen Datenbankoperationen müssen ein
   `CancellationToken` akzeptieren.
4. **AsNoTracking:** Alle reinen Leseoperationen müssen explizit
   `AsNoTracking()` nutzen.

## Gründe

1. **Resilience:** Automatische Erholung von transienten Verbindungsfehlern
   (Cloud-ready).
2. **Datenintegrität:** Schutz vor gegenseitigem Überschreiben von Daten durch
   verschiedene User.
3. **Performance:** Reduzierter Memory-Footprint im EF Core State Manager bei
   Lesezugriffen.
4. **Responsiveness:** Abbruch langlaufender Abfragen bei User-Abbruch oder
   Timeout.

## Konsequenzen

- Entwickler müssen bei Datenbank-Exceptions auf
  `DbUpdateConcurrencyException` prüfen.
- Methoden-Signaturen in Repositories und Services erweitern sich um
  `CancellationToken ct = default`.

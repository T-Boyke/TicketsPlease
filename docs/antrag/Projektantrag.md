# Projektantrag

**Auszubildende/r:** Tobias Boyke
**Ausbildungsstätte:** Beispiel GmbH, Neuss / BitLC Düsseldorf
**Beruf:** Fachinformatiker Fachrichtung Anwendungsentwicklung
**Prüfungstermin:** Sommer/Winter 2026
**Prüfungsart:** 50 - Abschlussprüfung

---

## 1. Projektbezeichnung (Auftrag/Teilauftrag)*

**Entwicklung eines ticketbasierten Aufgabenmanagementsystems ("TicketsPlease") zur
Effizienzsteigerung in Logistikprojekten.**

### 1.1 Kurze Projektbeschreibung**

Die Beispiel GmbH ist ein aufstrebendes IT-Dienstleistungsunternehmen mit Sitz in Neuss und
beschäftigt 14 Mitarbeiter. Die Firma hat sich auf die Entwicklung maßgeschneiderter
Webanwendungen für Unternehmenskunden spezialisiert.

Ein langjähriger Kunde aus der Logistikbranche verwaltet seine internen Software-Bugs und
Aufgaben bisher manuell über Tabellenkalkulationen (Excel) und E-Mail-Kommunikation.
Dieser Prozess führt bei der zunehmenden Anzahl an Projekten zu mangelnder Übersichtlichkeit,
Informationsverlusten und einer verzögerten Fehlerbehebung. Um die Effizienz und Traceability
der Projektsteuerung zu steigern, soll ein zentrales, webbasiertes Ticketsystem entwickelt
werden.

Hierzu wird die Anwendung "TicketsPlease" konzipiert und implementiert. Das System ermöglicht
die Verwaltung von Projekten, die Erfassung detaillierter Tickets (Bugs/Tasks), rollenbasierte
Zuweisungen sowie einen integrierten Messaging-Dienst. Die Lösung wird als ASP.NET Core MVC
Applikation umgesetzt and in die On-Premise-Infrastruktur des Kunden integriert.

---

## 2. Projektumfeld**

Die Entwicklungsabteilung der Beispiel GmbH wurde mit der Realisierung dieses MVPs (Minimum
Viable Product) beauftragt. Das Projekt wird als eigenständiges Modul entwickelt, das
perspektivisch um weitere Enterprise-Features erweitert werden kann.

**Technische Anforderungen & Werkzeuge:**

- **Technologie:** C# 14, .NET 10, ASP.NET Core 10 DDD/MVC.
- **Datenbank:** SQL Server mit Entity Framework Core (Code First Migrationen).
- **Frontend:** Razor Views, Tailwind CSS (Corporate Design).
- **Architektur:** Clean Architecture Pattern zur Trennung der Belange.
- **Methodik:** Agiles Vorgehen mittels Kanban-Board (GitHub Projects).

**Entwicklungsumgebung:**

- Visual Studio 2026 / JetBrains Rider.
- Management-Tools: GitHub Actions (CI/CD), SQL Server Management Studio.

Die Umsetzung erfolgt in enger Abstimmung mit dem Projektleiter IT des Kunden und wird als
Teilprojekt eigenverantwortlich vom Auszubildenden bearbeitet.

---

## 3. Projektplanung einschließlich Zeitplanung**

### 3.1 Analysephase (11 Std.)

- Kick-Off & Ist-Prozessanalyse (Schwachstellen der Excel-Verwaltung): 3 Std.
- Erstellung Pflichtenheft (Detaillierung der 9 MVP-Features F1-F9): 4 Std.
- Wirtschaftlichkeitsbetrachtung (Amortisation vs. SaaS-Lizenzen): 2 Std.
- Zeit- und Ressourcenplanung: 2 Std.

### 3.2 Entwurfsphase (12 Std.)

- UI/UX Design (High-Fidelity Mockups via Figma): 4 Std.
- Datenmodellierung (ERD) & Datenbank-Layout: 4 Std.
- Software-Architektur & Setup (DI-Container, Layer-Setup): 4 Std.

### 3.3 Implementierungsphase (28 Std.)

- F1: Web-App Basis-Setup (Framework, DB-Anbindung, Identity/Auth): 7 Std.
- F2: Admin-Bereich (Stammdaten & Projekte CRUD): 3 Std.
- F3: Ticket-Management (Erfassungs-Logik, Listen & Detail-Views): 4 Std.
- F4: Dashboard (Statistik-Aggregationen auf der Startseite): 2 Std.
- F5/F6: Kommunikation & Filterung (Kommentare & Linq-Filter-System): 3 Std.
- F7: Abhängigkeiten (Logik für blockierende Tickets): 2 Std.
- F8: Workflow-Engine (Status-Definitionen & Transitionen): 5 Std.
- F9: Messaging (User-to-User Nachrichtensystem): 2 Std.

### 3.4 Qualitätssicherungsphase (9 Std.)

- Erstellung & Durchführung von Unittests (xUnit) für Domain-Logik: 4 Std.
- Integrationstests, End-to-End Tests & Bugfixing: 5 Std.

### 3.5 Abschlussphase (10 Std.)

- Erstellung der prozessorientierten Projektdokumentation: 8 Std.
- Erstellung Benutzerhandbuch & Projektabnahme: 2 Std.

#### Gesamtzeit: 70 Std.

**Dokumentation zur Projektarbeit:**

- Prozessorientierte Projektdokumentation.
- Pflichtenheft / User-Story-Map.
- ER-Modell & Architektur-Diagramm.
- Quellcode-Auszüge (Core Logic).
- Testprotokolle (Unit/Integrationsnachweis).

---

## 4. Durchführungszeitraum**

Projektzeitraum: 23.03.2026 – 20.04.2026

*=Pflichtfeld, **=Freitext

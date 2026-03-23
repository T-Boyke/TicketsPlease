## **1. Einleitung**

### **1.1. Ausgangssituation**

Im Rahmen der Umschulung zum Fachinformatiker für Anwendungsentwicklung befasst sich dieses Abschlussprojekt mit der Entwicklung eines ticketbasierten Aufgabenmanagementsystems bei der **Beispiel GmbH**.

Die **Beispiel GmbH** ist ein junges, aufstrebendes IT-Unternehmen mit Sitz im Herzen von Neuss. Seit ihrer Gründung im Jahr 2025 hat sich die Firma darauf spezialisiert, maßgeschneiderte Webanwendungen und anspruchsvolle Unternehmenslösungen zu entwickeln. **14 Mitarbeiter**, bestehend aus Backend- und Frontend-Entwicklern, UI/UX-Designern, Projektmanagern und Content-Spezialisten, arbeiten vorwiegend für Kunden aus dem produzierenden Gewerbe, der Logistik und der Edutainment-Branche.

Aufgrund des stetigen Wachstums und der zunehmenden Komplexität interner und externer Projekte wurde die Beispiel GmbH von einem langjährigen Partner aus der Logistikbranche beauftragt, ein spezialisiertes Ticketsystem zu entwickeln, das exakt auf deren granulare Workflows zugeschnitten ist.

Zu den Stakeholdern des Projekts zählen:
- Der Auftraggeber (Logistik-Partner): Vertreten durch den Projektleiter IT.
- Die firmeninterne Qualitätssicherung der Beispiel GmbH.

### **1.2. Projektidee und Zielsetzung**

Ziel des Projektes ist die Entwicklung der Webanwendung "TicketsPlease". Die Anwendung soll als zentrales Werkzeug zur Erfassung, Verwaltung und Nachverfolgung von Fehlern (Bugs) und Aufgaben (Tasks) innerhalb von Softwareprojekten dienen. Kernfunktionen umfassen eine Projektverwaltung, ein detailliertes Ticket-Management mit Zuweisungslogik, Kommentarfunktionen sowie ein flexibles Workflow-System zur Abbildung individueller Geschäftsprozesse. Die Anwendung wird als ASP.NET Core MVC Applikation konzipiert, um eine hohe Performance und Wartbarkeit in Unternehmensinfrastrukturen zu gewährleisten.

### **1.3 Projektbegründung**

Effizientes Projektmanagement erfordert eine lückenlose Dokumentation von Fehlern und Anforderungen. Bestehende Lösungen sind oft entweder zu komplex (Overhead) oder bieten nicht die nötige Flexibilität für spezialisierte Workflows. Durch die Eigenentwicklung "TicketsPlease" auf Basis moderner Webtechnologien (ASP.NET Core 8/10, Entity Framework Core) wird eine Lösung geschaffen, die exakt die benötigten Features ohne unnötigen Ballast bereitstellt. Dies führt zu einer signifikanten Zeitersparnis bei der Ticketbearbeitung und dient gleichzeitig der Beispiel GmbH als Referenz für robuste Enterprise-Backend-Lösungen.

### **1.4 Make-or-Buy Entscheidung**

Im Vorfeld wurde geprüft, ob der Einsatz von Standard-Lösungen wie Jira, Redmine oder GitHub Issues sinnvoll ist (Buy). Die Entscheidung fiel zugunsten einer Eigenentwicklung (Make) aus folgenden Gründen:

**Datensouveränität & Security:** Da sensible Projektdaten und interne Logistikprozesse abgebildet werden, forderte der Kunde ein Hosting in der eigenen On-Premise-Infrastruktur ohne Abhängigkeit von Drittanbietern oder Cloud-Modellen.

**Spezialisierte Workflows:** Standard-Tools erfordern oft eine Anpassung der Unternehmensprozesse an die Software. "TicketsPlease" erlaubt es hingegen, die Software modular an die existierenden, hochspezialisierten Workflows des Kunden anzupassen (Modularität).

**Wirtschaftlichkeit:** Bei einer unbegrenzten Anzahl an Benutzern und Projekten entfallen laufende Lizenzkosten. Die einmaligen Entwicklungskosten amortisieren sich bereits nach einem Jahr gegenüber vergleichbaren Enterprise-Lizenzen.

---

## **2. Projektplanung**

### **2.1 Ist-Analyse**

Bisher erfolgt die Fehlererfassung beim Kunden manuell über Tabellenkalkulationen (Excel) und E-Mail-Verkehr. Dies führt zu Inkonsistenzen, fehlender Rückverfolgbarkeit und Verzögerungen in der Kommunikation. Es existiert keine zentrale Codebasis für ein Ticket-Management ("Greenfield Project").

**Technische Ausgangslage:**
- Entwickler-Workstation mit Windows 11 und Rocky Linux 10.
- Zugriff auf .NET SDK 10, Visual Studio 2022 / JetBrains Rider.
- Bestehende CI/CD Pipeline (GitHub Actions) für automatisierte Tests und Deployments.

### **2.2. Soll-Analyse**

Die zu entwickelnde Webanwendung "TicketsPlease" muss folgende funktionale Anforderungen (MVP) erfüllen:

**Funktionale Anforderungen:**
1. **Web-Anwendung (F1):** ASP.NET Core 8/10 Basis mit SQL Server & Identity.
2. **Admin-Bereich (F2):** Stammdatenverwaltung und Projekte CRUD.
3. **Ticket-Management (F3):** Erfassung, Bearbeitung und Detailansicht von Tickets.
4. **Dashboard (F4):** Startseite mit Projekt- und Ticket-Statistiken.
5. **Kommentarsystem (F5):** Chronologische Diskussionen innerhalb der Tickets.
6. **Filtersystem (F6):** Granulare Filterung nach Projekten, Erstellern und Bearbeitern.
7. **Abhängigkeiten (F7):** Blockier-Logik zwischen Tickets zur Steuerung der Reihenfolge.
8. **Workflow-Engine (F8):** Definierbare Prozesszustände (Status) pro Projekt.
9. **Messaging (F9):** Benutzerübergreifender Nachrichtenaustausch außerhalb von Tickets.

**Nicht-funktionale Anforderungen:**
- **Technologie:** ASP.NET Core (MVC), Entity Framework Core (Code First).
- **Architektur:** Clean Architecture (Domain Driven Design Ansätze).
- **UI/UX:** Responsive Design mit Tailwind CSS (Corporate Design).
- **Qualität:** Hohe Testabdeckung und automatisierte CI/CD Pipeline.

### **2.3 Zeitplanung**

Die Gesamtdauer des Projektes ist auf **70 Stunden** festgeschrieben.

| **Phase** | **Tätigkeit** | **Zeit (h)** | **Startdatum** |
| :--- | :--- | :---: | :---: |
| **1. Analyse & Planung** | | **11 h** | **01.07.2024** |
| | Ist-Analyse & Prozessaufnahme | 3 h | 01.07.2024 |
| | Erstellung Lastenheft / Pflichtenheft | 4 h | 02.07.2024 |
| | Wirtschaftlichkeitsanalyse | 2 h | 03.07.2024 |
| | Zeit- & Ressourcenplanung | 2 h | 03.07.2024 |
| **2. Entwurf** | | **12 h** | **04.07.2024** |
| | UI/UX Design (Wireframes & Mockups) | 4 h | 04.07.2024 |
| | Datenbankmodellierung (ERD) & API-Design | 4 h | 05.07.2024 |
| | Software-Architektur & Clean Architecture Setup | 4 h | 08.07.2024 |
| **3. Implementierung (F1-F9)** | | **28 h** | **09.07.2024** |
| | F1: Web-Anwendung (Setup, Basis-Layout) | 3 h | 09.07.2024 |
| | F2: Admin-Bereich (CRUD Projekte, Benutzer) | 4 h | 09.07.2024 |
| | F3: Ticket-Management (Erfassung, Bearbeitung) | 6 h | 10.07.2024 |
| | F4: Dashboard (Statistiken, Übersicht) | 2 h | 11.07.2024 |
| | F5: Kommentarsystem (Implementierung) | 3 h | 12.07.2024 |
| | F6: Filtersystem (Implementierung) | 2 h | 12.07.2024 |
| | F7: Abhängigkeiten (Logik, UI-Integration) | 2 h | 15.07.2024 |
| | F8: Workflow-Engine (Status-Transitionen) | 4 h | 15.07.2024 |
| | F9: Messaging (Grundfunktionalität) | 2 h | 16.07.2024 |
| **4. Qualitätssicherung** | | **9 h** | **17.07.2024** |
| | Erstellung von Unittests (xUnit) | 4 h | 17.07.2024 |
| | Integrationstests & Bugfixing | 5 h | 18.07.2024 |
| **5. Dokumentation** | | **10 h** | **19.07.2024** |
| | Projektdokumentation (Kap. 3 - 6) | 8 h | 19.07.2024 |
| | Benutzerhandbuch & Übergabe | 2 h | 22.07.2024 |
| **Gesamt** | | **70 h** | |

```mermaid
gantt
    dateFormat  DD.MM.YYYY
    title Projektablauf "TicketsPlease"

    section 1. Analyse & Planung
    Ist-Analyse & Prozessaufnahme : 01.07.2024, 3h
    Erstellung Lastenheft / Pflichtenheft : 02.07.2024, 4h
    Wirtschaftlichkeitsanalyse : 03.07.2024, 2h
    Zeit- & Ressourcenplanung : 03.07.2024, 2h

    section 2. Entwurf
    UI/UX Design (Wireframes & Mockups) : 04.07.2024, 4h
    Datenbankmodellierung (ERD) & API-Design : 05.07.2024, 4h
    Software-Architektur & Clean Architecture Setup : 08.07.2024, 4h

    section 3. Implementierung (F1-F9)
    F1: Web-Anwendung (Setup, Basis-Layout) : 09.07.2024, 3h
    F2: Admin-Bereich (CRUD Projekte, Benutzer) : 09.07.2024, 4h
    F3: Ticket-Management (Erfassung, Bearbeitung) : 10.07.2024, 6h
    F4: Dashboard (Statistiken, Übersicht) : 11.07.2024, 2h
    F5: Kommentarsystem (Implementierung) : 12.07.2024, 3h
    F6: Filtersystem (Implementierung) : 12.07.2024, 2h
    F7: Abhängigkeiten (Logik, UI-Integration) : 15.07.2024, 2h
    F8: Workflow-Engine (Status-Transitionen) : 15.07.2024, 4h
    F9: Messaging (Grundfunktionalität) : 16.07.2024, 2h

    section 4. Qualitätssicherung
    Erstellung von Unittests (xUnit) : 17.07.2024, 4h
    Integrationstests & Bugfixing : 18.07.2024, 5h

    section 5. Dokumentation
    Projektdokumentation (Kap. 3 - 6) : 19.07.2024, 8h
    Benutzerhandbuch & Übergabe : 22.07.2024, 2h
```


### **2.4 Kostenplanung**

Die Kalkulation erfolgt auf Basis des Praktikumsbetriebs.

**Personalkosten:**
- Fachinformatiker (Stundenverrechnungssatz): 9,00 €/h
- Geplante Stunden: 70 h
- **Summe Personal: 630,00 €**

**Sachmittelkosten:**
- Infrastruktur-Pauschale (Server, Strom, Arbeitsplatz): 150,00 €
- **Summe Sachmittel: 150,00 €**

**Gesamtkosten (Plan): 780,00 €**

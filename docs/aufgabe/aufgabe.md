# Status der Userstories

## Feature #1: Web-Anwendung

- [ ] 1.1 - ASP.NET Core-Anwendung
- [ ] 1.2 - Datenbank-Anbindung
- [ ] 1.3 - Authentifizierung/Autorisierung

## Feature #2: Admin-Bereich

- [ ] 2.1 - Admin-Startseite
- [ ] 2.2 - Projekte CRUD
- [ ] 2.3 - Benutzer (optional)

## Feature #3: Ticket-Bereich

- [ ] 3.1 - Tickets anlegen
- [ ] 3.2 - Ticket-Liste und Detailseite
- [ ] 3.3 - Tickets bearbeiten
- [ ] 3.4 - Tickets schließen
- [ ] 3.5 - Dateien zu Tickets speichern (optional)

## Feature #4: Startseite
- [ ] 4.1 - Startseite
- [ ] 4.2 - Statistiken

## Feature #5: Kommentare

- [ ] 5.1 - Kommentare

## Feature #6: Filterung

- [ ] 6.1 - Filterung nach Projekten
- [ ] 6.2 - Filterung nach zugeordnetem Benutzer
- [ ] 6.3 - Filterung nach Ersteller

## Feature #7: Abhängigkeiten
- [ ] 7.1 - Blockieren von Tickets

## Feature #8: Workflows

- [ ] 8.1 - Verwalten von Workflows

## Feature #9: Nachrichten

- [ ] 9.1 - Austausch von Nachrichten

---

## Projektaufgaben / Userstories

## Feature #1 - Web-Anwendung

### 1.1 - ASP.NET Core-Anwendung

**Story:**  
Als Benutzer möchte ich, dass das Ticketsystem als Web-Anwendung umgesetzt wird, damit ich von überall aus darauf zugreifen kann.

**Akzeptanzkriterien:**
- Als Framework wird ASP.NET Core ab Version 8 verwendet.
- Die Anwendung wird nach dem MVC-Muster entworfen.
- Die Anwendung soll ein einheitliches Design aufweisen, z.B. durch ein CSS-Framework.

**Hinweis:**
- Nutzen Sie ein sinnvolles Template, um den Aufwand gering zu halten.

---

### 1.2 - Datenbank-Anbindung

**Story:**  
Als Benutzer möchte ich, dass die Daten des Ticketsystems in einer Datenbank gespeichert werden, damit ich dauerhaft darauf zugreifen kann.

**Akzeptanzkriterien:**
- Als Datenbank kommt ein SQL Server (z.B. localdb) zum Einsatz.
- Die Verbindung zum SQL Server wird über das EntityFramework hergestellt.
- Für die Entwicklung wird der Code First-Ansatz verwendet.
- Offene Migrationen werden beim Start der App automatisch ausgeführt.

**Hinweis:**
- Diese Features können Sie aus den Apps übernehmen, die wir im Unterricht erstellt haben.

---

### 1.3 - Authentifizierung/Autorisierung
**Story:**  
Als Benutzer möchte ich, dass man sich zur Benutzung des Ticket-Systems anmelden muss, damit alle Aktionen im System eindeutig einem Benutzer zugeordnet werden können und nicht jeder Benutzer alle Aktionen durchführen kann.

**Akzeptanzkriterien:**
- Umsetzung mit dem Framework Identity.
- Es werden Benutzer und Rollen unterschieden.
- Es gibt mindestens die Rollen Admin, Developer und Tester.
- Zu jeder Rolle gibt es mindestens einen Benutzer.
- Es gibt einen eigenen AccountController zur Steuerung der Authentifizierung.
- Es gibt eine Möglichkeit zum Login und zum Logout.
- Als nicht angemeldeter Benutzer kann man nur die Startseite der App sehen.

**Hilfe:**
- Zu diesem Feature haben Sie eine Anleitung zur Verfügung.

---

## Feature #2 - Admin-Bereich

### 2.1 - Admin-Startseite

**Story:**  
Als Admin möchte ich, dass es einen eigenen Admin-Bereich gibt, über den ich die Stammdaten der App verwalten kann.

**Akzeptanzkriterien:**
- Der Bereich ist nur für Benutzer der Rolle Admin erreichbar.
- Es gibt eine Startseite im Admin-Bereich, von der aus man zu weiteren Bereichen kommt.
- Auf der Startseite der App gibt es einen Button/Link zum Admin-Bereich, der aber nur Admins angezeigt wird.

**Hinweis:**
- Den aktiven Benutzer kann man innerhalb eines Views über die Property User ermitteln.

---

### 2.2 - Projekte CRUD
**Story:**  
Als Admin möchte ich, dass es im Admin-Bereich einen Unterbereich für die Verwaltung von Projekten gibt, damit Tickets später eindeutig einem Projekt zugeordnet werden können.

**Akzeptanzkriterien:**
- Ein Projekt besteht mindestens aus den Attributen Titel, Beschreibung, Startdatum, Enddatum.
- Die Attribute für Titel, Beschreibung und Startdatum sind Pflichtangaben.
- Für Projekte kann nur der Admin CRUD-Funktionalität ausführen.

**Hinweis:**
- Nutzen Sie die Möglichkeit, sich Code automatisch generieren zu lassen.

---

### 2.3 - Benutzer (optional)

**Story:**  
Als Admin möchte ich, dass es im Admin-Bereich einen Unterbereich für die Verwaltung von Benutzern gibt, damit ich Benutzer ansehen, anlegen und bearbeiten kann.

**Akzeptanzkriterien:**
- Benutzer werden über das IdentityFramework verwaltet.
- Benutzer können Rollen zugewiesen werden.
- Benutzer sollen nicht aus der Datenbank gelöscht werden können.
- Die vom IdentityFramework vorgegebene Klasse IdentityUser wird um weitere sinnvolle Attribute erweitert (optional).

**Hinweis:**
- Diese User-Story ist recht aufwändig umzusetzen, da Benutzer nur sinnvoll über die Klasse UserManager verwaltet werden können (kein Scaffolding).
- Seeding von Usern/Rollen ist eine Alternative.

---

## Feature #3 - Ticket-Bereich

### 3.1 - Tickets anlegen

**Story:**  
Als Benutzer möchte ich, dass ich Tickets im System anlegen kann, damit ich Fehler zu den Projekten dokumentieren kann.

**Akzeptanzkriterien:**
- Ein Ticket enthält: Titel, Beschreibung, Projekt, Ersteller, Erstellungsdatum, Zugewiesener, Zuweisungsdatum.
- Pflichtangaben: Titel, Beschreibung, Projekt, Ersteller, Erstellungsdatum.
- Ersteller ist der angemeldete Benutzer. Erstellungsdatum wird automatisch gesetzt.
- Nur Projekte, die nicht gelöscht/beendet sind, können zugeordnet werden.
- Tickets können Benutzern zugewiesen werden (Zuweisungsdatum wird automatisch gesetzt).

---

### 3.2 - Ticket-Liste und Detailseite
**Story:**  
Als Benutzer möchte ich eine Liste aller Tickets und eine Detailseite je Ticket sehen können.

**Akzeptanzkriterien:**
- Liste zeigt Titel und Projekt.
- Sortierung nach Projekt und Erstellungsdatum (absteigend).
- Detailseite zeigt alle Informationen.
- Zurück-Button zur Liste vorhanden.

---

### 3.3 - Tickets bearbeiten
**Story:**  
Als Benutzer möchte ich Tickets nachträglich bearbeiten können.

**Akzeptanzkriterien:**
- Bearbeitung nur durch Admin, Ersteller oder zugewiesenen Benutzer.
- Änderbar: Beschreibung und Zugewiesener.
- Bei Änderung des Benutzers wird das Zuweisungsdatum aktualisiert.
- (Optional): Historische Zuweisungen werden protokolliert.

---

### 3.4 - Tickets schließen
**Story:**  
Als Benutzer möchte ich Tickets schließen können.

**Akzeptanzkriterien:**
- Neue Attribute: GeschlossenVon, GeschlossenAm.
- Schließen nur durch Admin, Ersteller oder zugewiesenen Benutzer.
- Status (offen/geschlossen) wird in Übersicht und Detailseite angezeigt.

---

### 3.5 – Dateien zu Tickets speichern (optional)
**Story:**  
Als Benutzer möchte ich Dateien (Screenshots etc.) zu einem Ticket hochladen können.

**Akzeptanzkriterien:**
- Upload beim Erstellen und auf der Detailseite.
- Eindeutige Zuordnung zum Ticket.
- Speicherung von "Wer" und "Wann".
- Anzeige und Download auf der Detailseite.

---

## Feature #4 - Startseite

### 4.1 - Startseite

**Story:**  
Als Benutzer möchte ich eine zentrale Startseite als Einstiegspunkt.

**Akzeptanzkriterien:**
- Erreichbar unter Standard-URL (Root).
- Auch für nicht angemeldete Benutzer erreichbar.
- Links/Buttons zum Ticket- und Admin-Bereich.

---

### 4.2 - Statistiken

**Story:**  
Als Benutzer möchte ich Statistiken auf der Startseite sehen.

**Akzeptanzkriterien:**
- Tickets: Gesamt, offen, geschlossen.
- Projekte: Gesamt, offen, beendet.
- (Optional) Benutzer: Gesamt und pro Rolle.

---

## Feature #5 - Kommentare

### 5.1 - Kommentare

**Story:**  
Als Benutzer möchte ich Kommentare in Tickets hinterlassen können.

**Akzeptanzkriterien:**
- Attribute: Inhalt, TicketID, Ersteller, Erstellzeitpunkt.
- Anzeige auf der Detailseite (neueste zuerst).
- Button zum Anlegen auf der Detailseite vorhanden.
- Ersteller ist der angemeldete Benutzer, Zeitpunkt wird automatisch gesetzt.

---

## Feature #6 - Filterung

### 6.1 - Filterung nach Projekten

**Akzeptanzkriterien:**
- Auswahl "Bestimmtes Projekt" oder "Alle Projekte" über der Liste.
- Seite lädt mit Filterung neu.

---

### 6.2 - Filter von zugeordnetem Benutzer

**Akzeptanzkriterien:**
- Auswahl "Benutzer", "Alle Benutzer" oder "Nicht zugeordnet".
- Seite lädt mit Filterung neu.

---

### 6.3 - Filterung nach Ersteller

**Akzeptanzkriterien:**
- Auswahl "Benutzer" oder "Alle Benutzer".
- Seite lädt mit Filterung neu.

---

## Feature #7 - Abhängigkeiten

### 7.1 - Blockieren von Tickets

**Story:**  
Als Benutzer möchte ich Tickets als von anderen Tickets blockiert markieren kann, damit auf einen 
Blick klar ist, welche Tickets zusammengehören und in welcher Reihenfolge sie gelöst werden 
müssen.

**Akzeptanzkriterien:**
- Auf der Detailseite gibt es eine Möglichkeit, Tickets auszuwählen, die dieses Ticket blockieren.
- Auf der Detailseite werden die blockierenden Tickets in einer Liste angezeigt.
- Ein Ticket kann nur noch dann gelöst werden, wenn zuvor alle blockierenden Tickets bereits 
  gelöst wurden.

---

## Feature #8 - Workflows

### 8.1 - Verwalten von Workflows

**Story:**  
Als Admin möchte ich verschiedene Workflows für Tickets verwalten, damit in unterschiedlichen 
Projekten die Tickets mit unterschiedlichen Workflows bearbeitet werden können.

**Akzeptanzkriterien:**
- Workflows können nur vom Admin bearbeitet werden (CRUD).
- Jeder Workflow hat eine eindeutige ID, anhand derer man den Workflow identifizieren kann, sowie 
  eine Bezeichnung.
- Ein Workflow besteht aus einer Reihe von Status, die ein Ticket in diesem Workflow annehmen kann.
- Jedem Projekt kann genau ein Workflow zugeordnet werden.
- (Optional): Zu jedem Status kann angegeben werden, welche Status als Folge gewählt werden 
  können.
- (Optional): Zu jedem Status in einem Workflow kann angegeben werden, welche Rollen diesen Status 
  vergeben können.

---

## Feature #9 - Nachrichten

### 9.1 - Austausch von Nachrichten

**Story:**  
Als Benutzer möchte ich auch außerhalb von Tickets Nachrichten an andere Benutzer senden können, 
damit ich mich auch über allgemeine Themen mit anderen austauschen kann.

**Akzeptanzkriterien:**
- Eine Nachricht besteht aus einem Absender, einem Empfänger, einem Zeitstempel sowie dem 
  Nachrichteninhalt (erst einmal nur Text).
- Jeder angemeldete Benutzer bekommt eine Seite, auf der er Nachrichten mit anderen Benutzern 
  austauschen kann.
- Auf der Seite gibt es die Möglichkeit, eine neue Nachricht zu erstellen.
- Es gibt eine Liste, die alle gesendeten und erhaltenen Nachrichten nach Absender/Empfänger 
  gruppiert anzeigt.

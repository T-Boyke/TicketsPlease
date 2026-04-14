# Projekt Gantt-Chart (Roadmap)

Dieses Gantt-Diagramm visualisiert die Zeitplanung des Abschlussprojekts. Projektzeitraum:
**23.03.2026 – 20.04.2026** (70 Stunden).

```mermaid
gantt
    title TicketsPlease - Abschlussprojekt (70h)
    dateFormat  YYYY-MM-DD
    axisFormat  %d.%m

    section 1. Analysephase (11h)
    Kick-Off & Ist-Analyse          :done, a1, 2026-03-23, 2d
    Pflichtenheft (F1-F9)           :done, a2, after a1, 2d
    Wirtschaftlichkeitsbetrachtung  :a3, after a2, 1d
    Zeit- & Ressourcenplanung       :a4, after a2, 1d

    section 2. Entwurfsphase (12h)
    UI/UX Design (Mockups)          :e1, 2026-03-27, 3d
    Datenmodellierung (ERD)         :done, e2, 2026-03-23, 3d
    Software-Architektur & Setup    :done, e3, 2026-03-23, 3d

    section 3. Implementierung (28h)
    F1: Auth & Identity             :crit, f1, 2026-03-30, 3d
    F2: Admin & Projekte CRUD       :crit, f2, after f1, 2d
    F3: Ticket-Management           :crit, f3, after f2, 3d
    F4: Startseite & Statistiken    :f4, after f3, 1d
    F5/F6: Kommentare & Filterung   :f5, after f4, 2d
    F7: Abhängigkeiten              :f7, after f5, 1d
    F8: Workflow-Engine             :crit, f8, after f7, 3d
    F9: Nachrichten                 :f9, after f8, 2d

    section 4. Qualitätssicherung (9h)
    Unit-Tests (Domain-Logik)       :t1, 2026-03-30, 14d
    Integrationstests & E2E         :t2, 2026-04-07, 10d
    Bugfixing                       :t3, 2026-04-14, 4d

    section 5. Abschlussphase (10h)
    Projektdokumentation            :crit, d1, 2026-04-14, 5d
    Benutzerhandbuch & Abnahme      :d2, after d1, 2d
```

## Legende

| Symbol | Bedeutung                     |
| :----- | :---------------------------- |
| `done` | Bereits abgeschlossen         |
| `crit` | Kritischer Pfad (kein Puffer) |

> [!NOTE] Die Qualitätssicherung (Unit-Tests) läuft parallel zur Implementierung (TDD). Die Zeiten
> überlappen sich bewusst.

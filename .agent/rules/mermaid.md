# 📊 Mermaid Gantt – Definitive Perfektionsregeln

Diese Regeln stellen sicher, dass Gantt-Diagramme in der TicketPlease-Dokumentation den
IHK-Standards entsprechen und technisch fehlerfrei gerendert werden.

---

## 🛠️ Grundausstattung (Config)

Jedes Diagramm sollte mit einer sinnvollen Konfiguration starten:

- `dateFormat DD.MM.YYYY`: Standard-Inputformat.
- `axisFormat %d.%m`: Anzeigeformat der X-Achse.
- `tickInterval 1day`: Gewährleistet tägliche Ticks (Sichtbarkeit bei kurzen Tasks).
- `excludes weekends`: Wochenenden werden übersprungen.
- `todayMarker off`: (Optional) Schaltet die "Heute"-Linie aus.

---

## 📝 Syntax & Metadaten

Ein Task folgt dem Muster: `Label : [tags], [id], [start], [duration/until]`

### 1. Tags & Status

- `done`: Abgeschlossene Aufgaben.
- `active`: Aktuelle Aufgaben.
- `crit`: Kritischer Pfad (rot hervorgehoben – **Muss für F1-F9 genutzt werden**).
- `milestone`: Ein einzelner Zeitpunkt (Dauer `0d`).

### 2. Zeitliche Steuerung

| Syntax-Beispiel | Effekt |
| :--- | :--- |
| `ID, 23.03.2026, 4h` | Fixer Start, Dauer 4 Stunden. |
| `ID, after TaskA, 2d` | Startet direkt nach Ende von `TaskA`. |
| `ID, 23.03.2026, until TaskB` | Läuft bis zum Start von `TaskB`. |

### 3. Dauer-Formate (Case-Sensitive!)

- `ms`, `s`, `m`, `h`, `d`, `w`, `M`, `y` (Millisekunden bis Jahre).
- Dezimalwerte wie `1.5d` sind erlaubt.

---

## 🚀 Erweiterte Features

### Kompakt-Modus (`compact`)

Für eine platzsparende Darstellung (mehrere Tasks pro Zeile) nutze YAML Frontmatter:

```markdown
---
displayMode: compact
---
gantt
    ...
```

### Vertikale Marker (`vert`)

Markiere spezifische Deadlines oder Events ohne eine neue Zeile zu belegen:
`Meilenstein Name : vert, v1, 24.03.2026, 1s`

---

## 💎 Best Practices (IHK Doku)

1. **Quoting:** Labels mit Sonderzeichen **müssen** in `"..."` stehen.
2. **Atomarität:** Jeder Tabelleneintrag der Zeitplanung muss eine eigene Zeile im Gantt erhalten.
3. **Sichtbarkeit:** Kurze Tasks (< 4h) **immer** mit `after` anordnen, damit sie im
   Gesamtchart nicht untergehen.
4. **Sections:** Nutze `section` für Phasen (Analyse, Entwurf, Implementierung, QA, Dokumentation).

---

### Mermaid Perfection Rules v1.2 | 2026-03-24

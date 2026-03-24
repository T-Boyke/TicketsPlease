# 🐙 GitHub Advanced Markdown & Formatting Rules

Regeln für die Nutzung der GitHub-spezifischen Markdown-Features ("GitHub Flavored Markdown").

---

## 🛠️ Erweiterte Formatierung (Advanced)

### 1. Details & Collapsibles (`<details>`)

Nutze diesen Tag, um lange Textabschnitte, Log-Dateien oder optionale Details platzsparend zu verbergen.

```markdown
<details>
<summary>Klicke hier für mehr Details</summary>
... Inhalt ...
</details>
```

### 2. Tabellen-Präzision (MD060)

Tabellen **müssen** dem `aligned` Style entsprechen.

- Die Pipes `|` müssen vertikal perfekt untereinander stehen.
- In der Trennzeile **müssen** pro Spalte mindestens **drei Bindestriche** (`---`) vorhanden sein.
- Die Trennstriche müssen exakt der Breite der längsten Zelle entsprechen.
- **Wichtig:** Sonderzeichen wie Emojis (✅, ❌) zählen als Double-Width Zeichen.
  Das Padding muss entsprechend manuell angepasst werden.
- Die äußeren Pipes am Zeilenanfang und -ende sind optional, werden aber für den
  `aligned` Style zur besseren Lesbarkeit **immer** gesetzt.

### 3. Mathematische Ausdrücke (KaTeX)

GitHub unterstützt mathematische Formeln via `$$` (Block) oder `$` (Inline).

```markdown
$$
\text{ROI} = \frac{\text{Gewinn} - \text{Kosten}}{\text{Kosten}} \times 100
$$
```

### 4. Aufgabenlisten (Task Lists)

Standard für Issue-Templates und Checklisten in Dokumentationen.

- [x] Abgeschlossen
- [ ] Offen

### 5. Mermaid-Diagramme

Mermaid ist der Standard für architektonische Visualisierungen. Labels mit
Sonderzeichen (z.B. Doppelpunkte) **müssen** in Anführungszeichen gesetzt werden.

---

## 🔗 Referenzierung & Verknüpfung

### Permalinks

Referenziere Code-Ausschnitte immer über den Permalink zur spezifischen Zeile oder
dem Zeilenbereich (z.B. `L123-L130`).

### Keywords

Nutze Schlüsselwörter zur automatischen Issue-Schließung:

- `Closes #IssueID`
- `Fixes #IssueID`
- `Resolves #IssueID`

---

### GitHub Markdown Rules v1.0 | 2026-03-24

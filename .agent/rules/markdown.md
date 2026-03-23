# 📝 TicketsPlease – Markdown & Documentation Rules

Regeln für die Erstellung von Markdown-Dokumenten durch die Antigravity AI.

---

## 🛠️ Tools & Standards

- **Code-Style**: Prettier (Workspace Settings in `.vscode/settings.json`)
- **Linting**: DavidAnson/markdownlint (Dokumentation:
  [v0.40.0](https://github.com/DavidAnson/markdownlint/tree/v0.40.0/doc))
- **Konfiguration**: `.markdownlint.json` & `.prettierrc`
- **Verification**:
  `npx markdownlint-cli2 "**/*.md" --config ".markdownlint.json"`

---

## 📏 Formatierungs-Vorgaben

### Zeilenlänge (Line Length)

- Die maximale Zeilenlänge beträgt **100 Zeichen** (MD013).
- Ausnahmen: Tabellen, Code-Blöcke und Links.

### Listen & Einrückung

- Listenmarker müssen durch genau **ein Leerzeichen** vom Inhalt getrennt sein
  (MD030).
- Ungeordnete Listen werden mit **2 Leerzeichen** eingerückt (MD007).
- Listen müssen von **Leerzeilen** umgeben sein (MD032).

### Header-Struktur

- Pro Datei genau **ein H1**-Header.
- Logische Hierarchie wahren (H1 -> H2 -> H3).
- Mehrfache Header mit gleichem Inhalt sind nur erlaubt, wenn sie in
  unterschiedlichen Sektionen (Siblings Only) stehen (MD024).

---

## 🎨 Visualisierung & UI

### Mermaid-Diagramme

- Mermaid ist **essenziell** für die Visualisierung von Logik und Architektur.
- Nutze alle Features (Style, Classes, Interactive Elemente) gemäß der
  Mermaid-Dokumentation.
- IDs in Mermaid-Diagrammen mit Anführungszeichen versehen, um Syntax-Fehler zu
  vermeiden.

### GitHub Alerts

Nutze GitHub-Standard-Alerts strategisch:

> [!NOTE] Hintergrundkontext oder hilfreiche Erklärungen. [!TIP] Best Practices
> oder Effizienz-Vorschläge. [!IMPORTANT] Kritische Informationen oder
> Anforderungen.

---

## 🇩🇪 Sprache & Stil

- Alle vom Agenten erstellten Dokumente sind in **Deutsch** zu verfassen (sofern
  nicht anders angefordert).
- Nutze professionelle Fachbegriffe (Clean Architecture, DDD, TDD).
- Erstelle Inhaltsverzeichnisse (ToC) für längere Dokumente.

---

### TicketsPlease Markdown Rules v1.0 | 2026-03-09

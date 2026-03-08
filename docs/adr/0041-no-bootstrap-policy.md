# ADR 0041: No-Bootstrap Policy

## Status
Akzeptiert ✅

## Kontext
Bootstrap ist ein schwergewichtiges CSS-Framework, das viele vorgefertigte Komponenten und spezifische DOM-Strukturen erzwingt. In einem modernen Enterprise-Projekt mit Fokus auf maximale Performance und maßgeschneidertes Design (Clean Architecture) kann Bootstrap zu unnötigem Overhead und "Style-Conflicts" führen.

## Entscheidung
Wir entfernen Bootstrap vollständig aus dem Projekt. Alle UI-Komponenten werden exklusiv mit **Tailwind CSS** und Vanilla CSS/JS entwickelt.

## Gründe
1. **Performance:** Reduzierung der Asset-Größe (kein ungenutztes Bootstrap-CSS/JS).
2. **Control Flow:** Volle Kontrolle over das DOM-Markup ohne Framework-Zwang.
3. **Clean Code:** Vermeidung von "Class Soup" durch Tailwind-Abstraktion (@apply) und Razor Partials.
4. **Maintenance:** Reduzierung der Abhängigkeiten von Drittanbieter-Frameworks.

## Konsequenzen
- Alle Standard-Komponenten (Modals, Dropdowns, Tooltips) müssen manuell via Tailwind und Headless-Pattern (oder minimalem JS) implementiert werden.
- Bestehende Views müssen von `btn`, `row`, `col-md-6` etc. auf Tailwind-Klassen umgestellt werden.

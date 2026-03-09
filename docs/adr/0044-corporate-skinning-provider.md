# ADR 0044: Corporate Skinning Architecture

## Status

Akzeptiert ✅

## Kontext

Die Applikation soll mandantenfähig sein und individuelles Branding (Farben,
Logos) pro Kunde unterstützen, ohne den Kern-Code zu verändern.

## Entscheidung

Wir implementieren ein dynamisches Skinning-System basierend auf:

1. **ICorporateSkinProvider:** Interface zur Auflösung des aktuellen Skins.
2. **CSS Custom Properties:** Definieren von Branding-Tokens in einer zentralen
   CSS-Datei.
3. **Runtime Injection:** Dynamisches Rendern von CSS-Variablen im HTML-Head.

## Gründe

1. **Flexibilität:** Schnelles Onboarding neuer Kunden mit eigenem CD.
2. **Performance:** Kein Page-Reload für Theme-Changes nötig; minimale
   CSS-Payload.
3. **Maintainability:** Saubere Trennung von funktionalem CSS (Tailwind) und
   Branding-CSS (Variablen).

## Konsequenzen

- Tailwind-Farben müssen in `tailwind.config.js` auf CSS-Variablen gemappt
  werden.
- Der `ICorporateSkinProvider` muss im DI-Container initialisiert werden.

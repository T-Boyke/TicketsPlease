# Frontend Asset Management Strategy

Dieses Dokument (ADR) beschreibt, wie wir in *TicketsPlease* mit statischen Web-Assets (CSS, JavaScript, Fonts) umgehen.

## Kontext
Moderne Webanwendungen hängen von externen Frameworks ab (hier: TailwindCSS und FontAwesome). Oftmals werden diese einfach über externe Content Delivery Networks (CDNs) wie *cdnjs* oder *jsdelivr* in die HTML-Köpfe importiert. 

Wir mussten entscheiden, wie wir diese externen Abhängigkeiten in unser ASP.NET Core MVC Projekt laden, versionieren und ausliefern.

## Entscheidung
Wir verzichten **komplett** auf externe CDNs im produktiven HTML-Code. 
Sämtliche Front-End-Bibliotheken werden zwingend lokal gehostet.

Wir nutzen den in Visual Studio integrierten **Library Manager (LibMan)** (`libman.json`), um die Pakete aus Providern wie *unpkg* zur Entwicklungszeit herunterzuladen und im Projektverzeichnis (`wwwroot/lib/`) abzulegen.

## Gründe & Design-Treiber

1. **DSGVO / Datenschutz (Kritisch):** Beim Einbinden von Assets über externe CDNs (insb. US-Server wie Google Fonts) wird die IP-Adresse des Nutzers unabdingbar an den Drittanbieter übertragen. Dies erfordert laut DSGVO eine explizite Zustimmung (Cookie-Banner-Zwang) und birgt hohe Abmahnrisiken. Durch lokales Hosting entfällt dieses Problem vollständig.
2. **Ausfallsicherheit (Resilience):** Wenn das Google-CDN oder cdnjs ausfällt (oder im Firmennetzwerk des Kunden geblockt wird), sieht unsere Seite "kaputt" aus (keine CSS-Styles, keine Icons). Lokale Assets garantieren, dass die UI *immer* erreichbar ist, solange unser eigener Server erreichbar ist.
3. **Reproduzierbarkeit (Offline-Entwicklung):** Entwickler können im Zug oder Flugzeug ohne Internetverbindung an der UI arbeiten, da alle Abhängigkeiten im Projekt liegen. Versionen (z.B. Tailwind 4.2) sind hart festgeschrieben und ändern sich nicht magisch unter der Haube.

## Negative Konsequenzen
- Unser Repository wird geringfügig größer, da Assets mit verwaltet werden.
- Die Ladezeiten beim initialen Seitenaufruf können um Bruchteile von Millisekunden höher sein als bei verteilten, extrem optimierten globalen CDNs. Für unsere Enterprise-Kanban-Lösung ist dies jedoch ein akzeptabler Trade-off zugunsten des Datenschutzes.

## Benötigte Lokale Assets (LibMan Stack)

Um unsere hochgesteckten Feature-Ziele (Markdown, Kanban, Realtime, A11y) vollumfänglich und CDN-frei bereitzustellen, benötigen wir folgende Libraries lokal in `wwwroot/lib`:

1.  **TailwindCSS (CLI & Output):** Unser Core-Styling Framework.
2.  **FontAwesome Free (Webfonts/CSS):** Für alle UI-Icons.
3.  **Google Fonts (z.B. Inter oder Roboto):** Lokal gehostete WOFF2-Dateien, um IP-Leaks zu Google-Servern zu verhindern.
4.  **@microsoft/signalr:** Der offizielle JavaScript-Client für WebSockets und Core-Bestandteil unserer Realtime-Messaging und Online-Presence-Engine.
5.  **marked.js (oder markdown-it):** Ein robuster, extrem schneller Parser, um das Markdown aus den C#-Modellen im Frontend in HTML umzuwandeln.
6.  **DOMPurify:** **ABSOLUT KRITISCH!** Sobald wir User-generiertes Markdown (via `marked.js`) in echten HTML-Code umwandeln und ins DOM injizieren, *müssen* wir ihn vorher mit DOMPurify reinigen, um Cross-Site-Scripting (XSS) Attacken abzuwehren.
7.  **mermaid.js:** Für das Rendern von komplexen Diagrammen und Architektur-Skizzen direkt in den Ticket-Beschreibungen.
8.  *(Optional aber Empfohlen)* **SortableJS:** Zwar ist HTML5 Drag&Drop nativ vorhanden, aber für komplexe Kanban-Boards (übergreifende Listen, Ghost-Elemente, smoothe Animationen) liefert SortableJS die perfekte UX-Abrundung, ohne so schwergewichtig wie komplette React/Vue Drag-Frameworks zu sein.

## 🎨 Enterprise Theming (Multi-Tenancy)

Um verschiedene Firmen-Identitäten (Corporate Identity) in einer mandantenfähigen Umgebung zu unterstützen, implementieren wir eine dynamische Theming-Architektur:

- **ICorporateSkinProvider:** Ein Interface, welches basierend auf dem aktuellen Kontext (z.B. Tenant-ID oder Subdomain) die passenden Branding-Informationen liefert.
- **CSS Variablen:** Anstelle von statischen Farbwerten in Tailwind nutzen wir CSS Custom Properties (z.B. `--brand-primary`). Diese werden dynamisch über ein `<style>`-Tag im Root-Layout (`_Layout.cshtml`) injiziert.
- **Bootstrap-Free:** Um volle Kontrolle über das Box-Model und die Semantik zu behalten, verzichten wir vollständig auf Bootstrap. Jede UI-Komponente wird exklusiv mit Tailwind CSS gestaltet.

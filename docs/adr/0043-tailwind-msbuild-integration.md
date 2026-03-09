# ADR 0043: Tailwind CSS via MSBuild (Node-free)

- Status: accepted ✅

## Kontext

Tailwind CSS benötigt normalerweise Node.js für den Build-Prozess (JIT-Compiler). In reinen
.NET-Teams oder CI-Umgebungen ohne Node-Installation kann dies zu Hürden führen.

## Entscheidung

Wir nutzen das NuGet-Paket **TailwindCSS.MSBuild**. Dieses Paket enthält das Standalone-Tailwind-Binary
für Windows/Linux/macOS und integriert sich direkt in den MSBuild-Prozess von Visual Studio.

## Gründe

1. **Zero Node Dependency:** Keine Installation von Node.js oder npm auf Entwickler-Rechnern oder
   Build-Servern nötig.
2. **DX (Developer Experience):** Der CSS-Build startet automatisch beim Kompilieren des
   C#-Projekts.
3. **Stabilität:** Minimierte Toolchain-Komplexität.

## Konsequenzen

- Konfiguration erfolgt weiterhin über `tailwind.config.js`.
- Der erste Build kann etwas länger dauern, da das Binary im Hintergrund extrahiert wird.

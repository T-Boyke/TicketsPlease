# 🛠️ Development Setup Guide (For Dummies) 🎫

Willkommen im Projekt **TicketsPlease**! Diese Anleitung führt dich Schritt für Schritt von einem
leeren PC bis zum ersten erfolgreichen Start der Applikation. Keine Sorge, wir machen das gemeinsam.

---

## 1. 📂 Voraussetzungen (Was du brauchst)

Bevor du den Code anfasst, stelle sicher, dass folgende Programme installiert sind:

1. **[.NET 10 SDK (v10.0.201+)](https://dotnet.microsoft.com/download/dotnet/10.0):** Das Herzstück.
   Ohne das SDK läuft kein C# Code. Unsere `global.json` erzwingt diese oder eine neuere
   Feature-Version.
2. **[Docker Desktop](https://www.docker.com/products/docker-desktop/):** Wir nutzen Docker für die
   SQL-Datenbank, damit du nichts lokal installieren musst.
3. **Eine IDE (Entwicklungsumgebung):**
   - **Visual Studio 2026** (Community Edition ab v18.4+ reicht aus) ODER
   - **JetBrains Rider 3.2026+**

---

## 2. 🚀 Das Projekt klonen

Öffne dein Terminal (PowerShell oder CMD) und navigiere in den Ordner, in dem das Projekt liegen
soll:

```bash
git clone https://github.com/BitLC-NE-2025-2026/TicketsPlease.git
cd TicketsPlease
```

---

## 3. 🛠️ Einrichtung in deiner IDE

### Option A: Visual Studio 2026

1. Öffne die Datei `TicketsPlease.slnx` (nicht die alte `.sln`!).
2. **Extensions:** VS wird dich beim Öffnen automatisch fragen, ob du die fehlenden Komponenten
   (Tailwind Plugin, SonarLint) installieren möchtest (basiert auf der `.vsconfig` im Root). Klicke
   auf **Installieren**.
3. **NuGet Restore:** Normalerweise passiert das automatisch. Wenn nicht: Rechtsklick auf die
   _Solution_ -> _Restore NuGet Packages_.
4. **LibMan (Frontend Assets):**
   - Rechtsklick auf das Web-Projekt -> _Manage Client-Side Libraries_ -> "Restore".
5. **Settings:** Die Einstellungen aus der `.editorconfig` werden automatisch beim Öffnen
   übernommen.

### Option B: JetBrains Rider 2026

1. Öffne den Ordner `TicketsPlease`.
2. **Plugins:** Rider wird dir Plugins wie _Rainbow Brackets_ oder _Tailwind CSS_ vorschlagen.
   Installiere diese für eine bessere Erfahrung.
3. **Restore:** Rider führt den NuGet Restore beim ersten Öffnen automatisch durch.

---

## 4. 🗄️ Datenbank & Assets vorbereiten

Das System braucht eine SQL-Datenbank und die CSS-Dateien.

1. **Docker starten:** Stelle sicher, dass Docker Desktop läuft.
2. **Datenbank-Container:** (Falls ein `docker-compose.yml` vorhanden ist)

   ```bash
   docker-compose up -d
   ```

3. **Datenbank-Migration (EF Core):** Öffne das Terminal in der IDE und gib folgendes ein, um die
   Tabellen zu erstellen:

   ```bash
   dotnet ef database update
   ```

4. **Tailwind CSS Build (v4.2):** Damit die Styles korrekt angezeigt werden, muss Tailwind generiert
   werden. Die v4.2-Engine ist über MSBuild integriert und kompiliert automatisch via
   `dotnet build`:

   ```bash
   dotnet build
   ```

---

## 5. 🎉 Der erste Start

1. Drücke `F5` oder wähle dein präferiertes Launch-Profil (z.B. `http`) im Dropdown deiner IDE.
2. Dein Browser sollte sich unter `https://localhost:xxxx` öffnen.
3. **API Dokumentation (Scalar):** Navigiere testweise zu `/scalar/v1`, um sicherzustellen, dass die
   neue, native OpenAPI-Integration funktioniert. Du solltest das **BluePlanet Theme** sehen!
4. **Login-Test:** Registriere einen neuen Benutzer und logge dich ein.
5. Du solltest nun die Navbar und das neue **Settings-Menü** sehen können.

---

## 🆘 Troubleshooting (Hilfe bei Fehlern)

- **Fehler: `dotnet` Befehl nicht gefunden?** Starte deine IDE oder das Terminal neu, nachdem du das
  SDK installiert hast.
- **Datenbank-Verbindung fehlgeschlagen?** Prüfe, ob der Docker-Container läuft und der
  Connection-String in `appsettings.Development.json` korrekt ist.
- **CSS sieht komisch aus?** Führe `dotnet build` erneut aus, um sicherzustellen, dass Tailwind alle
  Klassen generiert hat.

Viel Erfolg beim Entwickeln! 🌶️

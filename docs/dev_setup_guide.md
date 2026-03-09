# 🛠️ Development Setup Guide (For Dummies) 🎫

Willkommen im Projekt **TicketsPlease**! Diese Anleitung führt dich Schritt für Schritt von einem leeren PC bis zum ersten erfolgreichen Start der Applikation. Keine Sorge, wir machen das gemeinsam.

---

## 1. 📂 Voraussetzungen (Was du brauchst)

Bevor du den Code anfasst, stelle sicher, dass folgende Programme installiert sind:

1.  **[.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0):** Das Herzstück. Ohne das SDK läuft kein C# Code.
2.  **[Docker Desktop](https://www.docker.com/products/docker-desktop/):** Wir nutzen Docker für die SQL-Datenbank, damit du nichts lokal auf deinem Windows installieren musst.
3.  **Eine IDE (Entwicklungsumgebung):**
    *   **Visual Studio 2026** (Community Edition reicht aus) ODER
    *   **JetBrains Rider 2026**

---

## 2. 🚀 Das Projekt klonen

Öffne dein Terminal (PowerShell oder CMD) und navigiere in den Ordner, in dem das Projekt liegen soll:

```bash
git clone https://github.com/BitLC-NE-2025-2026/TicketsPlease.git
cd TicketsPlease
```

---

## 3. 🛠️ Einrichtung in deiner IDE

### Option A: Visual Studio 2026
1.  Öffne die Datei `TicketsPlease.slnx`.
2.  **Extensions:** VS wird dich fragen, ob du die "empfohlenen Erweiterungen" installieren möchtest (siehe `.vscode/extensions.json`). Klicke auf **Ja**.
3.  **NuGet Restore:** Normalerweise passiert das automatisch. Wenn nicht: Rechtsklick auf die *Solution* -> *Restore NuGet Packages*.
4.  **LibMan (Frontend Assets):**
    *   Rechtsklick auf das Web-Projekt -> *Manage Client-Side Libraries*.
    *   Klicke auf "Restore", falls die Bibliotheken (Tailwind/FontAwesome) nicht automatisch geladen werden.

### Option B: JetBrains Rider 2026
1.  Öffne den Ordner `TicketsPlease`.
2.  **Plugins:** Rider wird dir Plugins wie *Rainbow Brackets* oder *Tailwind CSS* vorschlagen. Installiere diese für eine bessere Erfahrung.
3.  **Restore:** Rider führt den NuGet Restore beim ersten Öffnen automatisch durch.

---

## 4. 🗄️ Datenbank & Assets vorbereiten

Das System braucht eine SQL-Datenbank und die CSS-Dateien.

1.  **Docker starten:** Stelle sicher, dass Docker Desktop läuft.
2.  **Datenbank-Container:** (Falls ein `docker-compose.yml` vorhanden ist)
    ```bash
    docker-compose up -d
    ```
3.  **Datenbank-Migration (EF Core):**
    Öffne das Terminal in der IDE und gib folgendes ein, um die Tabellen zu erstellen:
    ```bash
    dotnet ef database update
    ```
4.  **Tailwind CSS Build:**
    Damit die Styles korrekt angezeigt werden, muss Tailwind generiert werden. Dies geschieht nun **automatisch** beim `dotnet build`, da wir ein lokales `tailwindcss.exe` integriert haben:
    ```bash
    dotnet build
    ```

---

## 5. 🎉 Der erste Start

1.  Drücke `F5` oder klicke auf den **"Play"-Button** in deiner IDE.
2.  Dein Browser sollte sich unter `https://localhost:xxxx` öffnen.
3.  **Login-Test:** Registriere einen neuen Benutzer (Username, Vorname und E-Mail sind Pflicht!) und logge dich ein.
4.  Du solltest nun die Navbar und das neue **Settings-Menü** sehen können.

---

## 🆘 Troubleshooting (Hilfe bei Fehlern)

*   **Fehler: `dotnet` Befehl nicht gefunden?** Starte deine IDE oder das Terminal neu, nachdem du das SDK installiert hast.
*   **Datenbank-Verbindung fehlgeschlagen?** Prüfe, ob der Docker-Container läuft und der Connection-String in `appsettings.Development.json` korrekt ist.
*   **CSS sieht komisch aus?** Führe `dotnet build` erneut aus, um sicherzustellen, dass Tailwind alle Klassen generiert hat.

Viel Erfolg beim Entwickeln! 🌶️

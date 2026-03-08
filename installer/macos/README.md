# SQLite2MySQL pro macOS

Sestavení probíhá v **dvou krocích**: publikování (lze i na Windows/Linux) a vytvoření .app / DMG (nutné na Macu).

## 1. Publikování aplikace

Z kořene projektu (V0.1 nebo V1.0):

**Apple Silicon (M1/M2/M3):**
```bash
dotnet publish Sqlite2MySql/Sqlite2MySql.csproj -c Release -p:PublishProfile=Install-osx-arm64
```
Výstup: `Sqlite2MySql/bin/Release/net8.0/osx-arm64/publish/`

**Intel Mac:**
```bash
dotnet publish Sqlite2MySql/Sqlite2MySql.csproj -c Release -p:PublishProfile=Install-osx-x64
```
Výstup: `Sqlite2MySql/bin/Release/net8.0/osx-x64/publish/`

Publikovat lze i z Windows (cross-compile).

## 2. Vytvoření .app a DMG (na macOS)

Přeneste celý projekt (nebo aspoň složku `installer/macos` a publish výstup) na Mac.

```bash
cd /cesta/k/V0.1   # nebo V1.0
chmod +x installer/macos/create-app.sh installer/macos/create-dmg.sh
./installer/macos/create-app.sh
```

Skript hledá publish složku v `Sqlite2MySql/bin/Release/net8.0/osx-arm64/publish` nebo `osx-x64/publish`. Pokud je jinde:

```bash
./installer/macos/create-app.sh /cesta/k/publish
```

Výsledek: `installer/output/SQLite2MySQL.app`

### Ikona aplikace (AppIcon.icns)

Skript `create-app.sh` očekává `installer/macos/AppIcon.icns`. Pokud chybí, zkusí ho vygenerovat z `sqlite2mysql.png` v kořenovém adresáři projektu.

Ruční generování (macOS):

```bash
chmod +x installer/macos/build-icon.sh
./installer/macos/build-icon.sh
```

Pokud je PNG jinde:

```bash
./installer/macos/build-icon.sh /cesta/k/logo.png
```

### DMG (volitelně)

```bash
./installer/macos/create-dmg.sh
```

Vytvoří se `installer/output/SQLite2MySQL_1.0.0.dmg` – uživatel přetáhne aplikaci do Aplikací.

## Stručný přehled

| Profil              | Architektura | Použití        |
|---------------------|-------------|----------------|
| `Install-osx-arm64` | Apple Silicon | M1, M2, M3   |
| `Install-osx-x64`   | Intel       | Starší Mac     |

Po vytvoření .app ji lze zkopírovat do `/Applications` nebo distribuovat jako DMG.

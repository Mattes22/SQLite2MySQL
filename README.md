# SQLite2MySQL V1.0 – cross‑platform .NET CLI and GUI

A .NET 8 console app for converting SQLite databases to MySQL. Runs on **Windows**, **Linux**, and **macOS**. Includes both **CLI** and a **GUI** built with Avalonia UI.

## Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

## Build

```bash
cd Sqlite2MySql
dotnet build -c Release
```

The executable will be in `bin/Release/net8.0/`:
- **Windows:** `sqlite2mysql.exe`
- **Linux / macOS:** `sqlite2mysql` (no extension)

## Publishing (simple binary output)

For the current platform:

```bash
dotnet publish -c Release -o publish
```

For a specific runtime (e.g. Linux x64):

```bash
# Linux x64
dotnet publish -c Release -r linux-x64 --self-contained false -o publish/linux-x64

# macOS x64 (Intel)
dotnet publish -c Release -r osx-x64 --self-contained false -o publish/osx-x64

# macOS ARM64 (Apple Silicon)
dotnet publish -c Release -r osx-arm64 --self-contained false -o publish/osx-arm64

# Windows x64
dotnet publish -c Release -r win-x64 --self-contained false -o publish/win-x64
```

`--self-contained false` produces a smaller output (requires .NET 8 installed on the target system). For a fully self‑contained bundle, use `--self-contained true`.

## Graphical interface (GUI)

Run the app with the `--gui` switch:

```bash
sqlite2mysql --gui
```

A window opens where you can select the SQLite file, output SQL, batch size, MySQL database and connection (host, port, user, password). Buttons **Test connection** and **Run conversion** are available. Optionally you can check “Import directly to MySQL after conversion”.

## Usage (CLI)

```bash
# Basic conversion to file
sqlite2mysql path/to/database.sqlite -o output.sql

# Schema only (no data)
sqlite2mysql database.sqlite --schema-only -o schema.sql

# Include database name in dump (CREATE DATABASE + USE)
sqlite2mysql database.sqlite -o output.sql --database my_db

# Output to stdout (e.g. pipe)
sqlite2mysql database.sqlite | mysql -u user -p target_db

# Direct import to MySQL (requires installed `mysql` client)
sqlite2mysql database.sqlite --apply --database my_db --mysql-host localhost --mysql-user root --mysql-password-prompt
```

### Options

| Option | Description |
|--------|-------------|
| `-o`, `--output` | Output SQL file (without it, output goes to stdout) |
| `--schema-only` | Export only table structure, no data |
| `--batch-size N` | Number of rows per INSERT (default 500) |
| `-d`, `--database` | Database name (CREATE DATABASE + USE in dump; required with `--apply`) |
| `--apply` | After generating, run MySQL client and import the dump |
| `--mysql-host` | MySQL host (default localhost) |
| `--mysql-port` | MySQL port (default 3306) |
| `--mysql-user` | MySQL user (default root) |
| `--mysql-password` | Password (or MYSQL_PWD env var) |
| `--mysql-password-prompt` | Prompt for password in terminal |
| `--mysql-bin` | Path to `mysql` binary (default `mysql`) |
| `--gui` | Start the GUI (Avalonia) |

## Technical notes

- **GUI** is built with [Avalonia UI](https://avaloniaui.net/) and runs on all three platforms (Windows, Linux, macOS).
- The same conversion logic (types, indexes, views, triggers, batch INSERTs) and the same CLI options as the Windows .NET version in `src/sqlite2mysql-dotnet`.
# SQLite2MySQL

# SQLite2MySQL V1.0 – multiplatformní .NET CLI a GUI

Konzolová aplikace v .NET 8 pro převod SQLite databáze do MySQL. Běží na **Windows**, **Linux** a **macOS**. Obsahuje jak **CLI**, tak **grafické rozhraní (GUI)** postavené na Avalonia UI.

## Požadavky

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

## Sestavení

```bash
cd Sqlite2MySql
dotnet build -c Release
```

Spustitelný soubor bude v `bin/Release/net8.0/`:
- **Windows:** `sqlite2mysql.exe`
- **Linux / macOS:** `sqlite2mysql` (bez přípony)

## Publikování (jednoduchý binární výstup)

Pro jednu platformu (ta, na které právě běžíte):

```bash
dotnet publish -c Release -o publish
```

Pro konkrétní runtime (např. Linux x64):

```bash
# Linux x64
dotnet publish -c Release -r linux-x64 --self-contained false -o publish/linux-x64

# macOS x64 (Intel)
dotnet publish -c Release -r osx-x64 --self-contained false -o publish/osx-x64

# macOS ARM64 (Apple Silicon)
dotnet publish -c Release -r osx-arm64 --self-contained false -o publish/osx-arm64

# Windows x64
dotnet publish -c Release -r win-x64 --self-contained false -o publish/win-x64
```

`--self-contained false` vytvoří menší výstup (vyžaduje nainstalované .NET 8 na cílovém systému). Pro úplně samostatný balíček použijte `--self-contained true`.

## Grafické rozhraní (GUI)

Spusťte aplikaci s přepínačem `--gui`:

```bash
sqlite2mysql --gui
```

Otevře se okno s výběrem SQLite souboru, výstupního SQL, nastavením batch size, MySQL databáze a připojení (host, port, user, heslo). K dispozici jsou tlačítka **Test připojení** a **Spustit převod**. Volitelně lze zaškrtnout „Po převodu rovnou importovat do MySQL“.

## Použití (CLI)

```bash
# Základní převod do souboru
sqlite2mysql cesta/k/databazi.sqlite -o vystup.sql

# Pouze schéma (bez dat)
sqlite2mysql databaze.sqlite --schema-only -o schema.sql

# S názvem databáze v dumpu (CREATE DATABASE + USE)
sqlite2mysql databaze.sqlite -o vystup.sql --database muj_db

# Výstup na stdout (např. do roury)
sqlite2mysql databaze.sqlite | mysql -u user -p cilova_db

# Přímý import do MySQL (vyžaduje nainstalovaný klient `mysql`)
sqlite2mysql databaze.sqlite --apply --database muj_db --mysql-host localhost --mysql-user root --mysql-password-prompt
```

### Přepínače

| Přepínač | Popis |
|----------|--------|
| `-o`, `--output` | Výstupní SQL soubor (bez něj jde výstup na stdout) |
| `--schema-only` | Exportovat jen strukturu tabulek, bez dat |
| `--batch-size N` | Počet řádků v jednom INSERT (výchozí 500) |
| `-d`, `--database` | Název databáze (CREATE DATABASE + USE v dumpu; u `--apply` povinné) |
| `--apply` | Po vygenerování spustit MySQL klienta a importovat dump |
| `--mysql-host` | MySQL host (výchozí localhost) |
| `--mysql-port` | MySQL port (výchozí 3306) |
| `--mysql-user` | MySQL uživatel (výchozí root) |
| `--mysql-password` | Heslo (nebo proměnná MYSQL_PWD) |
| `--mysql-password-prompt` | Zeptat se na heslo v terminálu |
| `--mysql-bin` | Cesta k binárce `mysql` (výchozí `mysql`) |
| `--gui` | Spustit grafické rozhraní (Avalonia) |

## Technické poznámky

- **GUI** je postavené na [Avalonia UI](https://avaloniaui.net/) a běží na všech třech platformách (Windows, Linux, macOS).
- Stejná logika převodu (typy, indexy, pohledy, triggery, batch INSERTy) a stejné CLI přepínače jako u Windows .NET verze v `src/sqlite2mysql-dotnet`.
# SQLite2MySQL

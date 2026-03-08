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


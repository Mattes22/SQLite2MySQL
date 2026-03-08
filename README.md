# SQLite2MySQL V0.1 – multiplatformní .NET CLI a GUI

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

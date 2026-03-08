# SQLite2MySQL pro Linux

Publikování lze provést na libovolném systému (včetně Windows). Balíček (tar.gz) vytvoříte na Linuxu nebo ve WSL.

## 1. Publikování aplikace

Z kořene projektu (V0.1 nebo V1.0):

**64-bit (x64):**
```bash
dotnet publish Sqlite2MySql/Sqlite2MySql.csproj -c Release -p:PublishProfile=Install-linux-x64
```
Výstup: `Sqlite2MySql/bin/Release/net8.0/linux-x64/publish/`

**ARM64 (např. Raspberry Pi 4/5):**
```bash
dotnet publish Sqlite2MySql/Sqlite2MySql.csproj -c Release -p:PublishProfile=Install-linux-arm64
```
Výstup: `Sqlite2MySql/bin/Release/net8.0/linux-arm64/publish/`

## 2. Vytvoření balíčku tar.gz (na Linuxu / WSL)

```bash
cd /cesta/k/V0.1
chmod +x installer/linux/create-package.sh installer/linux/create-package-x64.sh installer/linux/create-package-arm64.sh
./installer/linux/create-package.sh
```

- **Výchozí (x64):** `./installer/linux/create-package.sh` nebo `./installer/linux/create-package-x64.sh`  
  → `installer/output/SQLite2MySQL-1.0.0-linux-x64.tar.gz`

- **ARM64:** `./installer/linux/create-package.sh linux-arm64` nebo `./installer/linux/create-package-arm64.sh`  
  → `installer/output/SQLite2MySQL-1.0.0-linux-arm64.tar.gz`

Vlastní cesta k publish složce:
```bash
./installer/linux/create-package.sh /cesta/k/publish
```

## 3. Použití u koncového uživatele

```bash
tar -xzf SQLite2MySQL-1.0.0-linux-x64.tar.gz
cd SQLite2MySQL-1.0.0-linux-x64
./sqlite2mysql          # GUI
./sqlite2mysql --help   # CLI
# nebo
./run.sh
```

## Stručný přehled

| Profil               | Architektura | Skript                  | Výstupní archiv                    |
|----------------------|-------------|--------------------------|------------------------------------|
| `Install-linux-x64` | x64         | `create-package-x64.sh`  | `SQLite2MySQL-1.0.0-linux-x64.tar.gz`  |
| `Install-linux-arm64`| ARM64       | `create-package-arm64.sh`| `SQLite2MySQL-1.0.0-linux-arm64.tar.gz`|

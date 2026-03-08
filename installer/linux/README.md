# SQLite2MySQL for Linux

Publishing can be done on any system (including Windows). The tar.gz package should be created on Linux or WSL.

## 1. Publish the app

From the project root (V0.1 or V1.0):

**64‑bit (x64):**
```bash
dotnet publish Sqlite2MySql/Sqlite2MySql.csproj -c Release -p:PublishProfile=Install-linux-x64
```
Output: `Sqlite2MySql/bin/Release/net8.0/linux-x64/publish/`

**ARM64 (e.g. Raspberry Pi 4/5):**
```bash
dotnet publish Sqlite2MySql/Sqlite2MySql.csproj -c Release -p:PublishProfile=Install-linux-arm64
```
Output: `Sqlite2MySql/bin/Release/net8.0/linux-arm64/publish/`

## 2. Create the tar.gz package (on Linux / WSL)

```bash
cd /path/to/V0.1
chmod +x installer/linux/create-package.sh installer/linux/create-package-x64.sh installer/linux/create-package-arm64.sh
./installer/linux/create-package.sh
```

- **Default (x64):** `./installer/linux/create-package.sh` or `./installer/linux/create-package-x64.sh`  
  → `installer/output/SQLite2MySQL-1.0.0-linux-x64.tar.gz`

- **ARM64:** `./installer/linux/create-package.sh linux-arm64` or `./installer/linux/create-package-arm64.sh`  
  → `installer/output/SQLite2MySQL-1.0.0-linux-arm64.tar.gz`

Custom publish path:
```bash
./installer/linux/create-package.sh /path/to/publish
```

## 3. End‑user usage

```bash
tar -xzf SQLite2MySQL-1.0.0-linux-x64.tar.gz
cd SQLite2MySQL-1.0.0-linux-x64
./sqlite2mysql          # GUI
./sqlite2mysql --help   # CLI
# or
./run.sh
```

## Quick overview

| Profile               | Architecture | Script                   | Output archive                         |
|----------------------|--------------|---------------------------|----------------------------------------|
| `Install-linux-x64`  | x64          | `create-package-x64.sh`   | `SQLite2MySQL-1.0.0-linux-x64.tar.gz`  |
| `Install-linux-arm64`| ARM64        | `create-package-arm64.sh` | `SQLite2MySQL-1.0.0-linux-arm64.tar.gz` |

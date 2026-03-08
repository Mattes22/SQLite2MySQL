# SQLite2MySQL for macOS

The build has **two steps**: publishing (can be done on Windows/Linux) and creating the .app / DMG (must be done on macOS).

## 1. Publish the app

From the project root (V0.1 or V1.0):

**Apple Silicon (M1/M2/M3):**
```bash
dotnet publish Sqlite2MySql/Sqlite2MySql.csproj -c Release -p:PublishProfile=Install-osx-arm64
```
Output: `Sqlite2MySql/bin/Release/net8.0/osx-arm64/publish/`

**Intel Mac:**
```bash
dotnet publish Sqlite2MySql/Sqlite2MySql.csproj -c Release -p:PublishProfile=Install-osx-x64
```
Output: `Sqlite2MySql/bin/Release/net8.0/osx-x64/publish/`

You can also publish from Windows (cross‑compile).

## 2. Create the .app and DMG (on macOS)

Copy the whole project (or at least `installer/macos` and the publish output) to a Mac.

```bash
cd /path/to/V0.1   # or V1.0
chmod +x installer/macos/create-app.sh installer/macos/create-dmg.sh
./installer/macos/create-app.sh
```

The script looks for the publish folder in `Sqlite2MySql/bin/Release/net8.0/osx-arm64/publish` or `osx-x64/publish`. If it’s elsewhere:

```bash
./installer/macos/create-app.sh /path/to/publish
```

Result: `installer/output/SQLite2MySQL.app`

### App icon (AppIcon.icns)

`create-app.sh` expects `installer/macos/AppIcon.icns`. If it’s missing, it tries to generate it from `sqlite2mysql.png` in the project root.

Manual generation (macOS):

```bash
chmod +x installer/macos/build-icon.sh
./installer/macos/build-icon.sh
```

If the PNG is elsewhere:

```bash
./installer/macos/build-icon.sh /path/to/logo.png
```

### DMG (optional)

```bash
./installer/macos/create-dmg.sh
```

This creates `installer/output/SQLite2MySQL_1.0.0.dmg` — the user drags the app into Applications.

## Quick overview

| Profile              | Architecture  | Use case        |
|----------------------|---------------|-----------------|
| `Install-osx-arm64`  | Apple Silicon | M1, M2, M3      |
| `Install-osx-x64`    | Intel         | Older Macs      |

After creating the .app, you can copy it to `/Applications` or distribute it as a DMG.

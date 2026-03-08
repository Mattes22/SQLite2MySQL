# SQLite2MySQL Installer (Windows)

## Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Inno Setup 6](https://jrsoftware.org/isinfo.php) (e.g. installed to `C:\Program Files (x86)\Inno Setup 6`)

## Build steps

1. **Publish the app** (from the V0.1 root or solution directory):

   ```powershell
   dotnet publish Sqlite2MySql\Sqlite2MySql.csproj -c Release -p:PublishProfile=Install
   ```

   Output will be in `Sqlite2MySql\bin\Release\net8.0\win-x64\publish\`.

2. **Build the installer in Inno Setup:**

   - Open **Inno Setup Compiler**
   - Open `installer\sqlite2mysql.iss`
   - **Build** → **Compile** (or F9)

   The installer will be created at `installer\output\SQLite2MySQL_Setup_1.0.0.exe`.

## Quick command (PowerShell)

After installing Inno Setup, run from the V0.1 root:

```powershell
dotnet publish Sqlite2MySql\Sqlite2MySql.csproj -c Release -p:PublishProfile=Install
& "C:\Program Files (x86)\Inno Setup 6\ISCC.exe" installer\sqlite2mysql.iss
```

The installer will be in `installer\output\`.

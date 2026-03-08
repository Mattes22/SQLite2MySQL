# Instalátor SQLite2MySQL (Windows)

## Požadavky

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Inno Setup 6](https://jrsoftware.org/isinfo.php) (např. instalace do `C:\Program Files (x86)\Inno Setup 6`)

## Postup sestavení instalátoru

1. **Publikování aplikace** (z kořene V0.1 nebo z adresáře řešení):

   ```powershell
   dotnet publish Sqlite2MySql\Sqlite2MySql.csproj -c Release -p:PublishProfile=Install
   ```

   Výstup bude v `Sqlite2MySql\bin\Release\net8.0\win-x64\publish\`.

2. **Sestavení instalátoru v Inno Setup:**

   - Spusťte **Inno Setup Compiler**
   - Otevřete soubor `installer\sqlite2mysql.iss`
   - Menu **Build** → **Compile** (nebo F9)

   Instalátor se vytvoří v `installer\output\SQLite2MySQL_Setup_1.0.0.exe`.

## Rychlý příkaz (PowerShell)

Po instalaci Inno Setup můžete z kořene V0.1 spustit:

```powershell
dotnet publish Sqlite2MySql\Sqlite2MySql.csproj -c Release -p:PublishProfile=Install
& "C:\Program Files (x86)\Inno Setup 6\ISCC.exe" installer\sqlite2mysql.iss
```

Instalátor najdete v `installer\output\`.

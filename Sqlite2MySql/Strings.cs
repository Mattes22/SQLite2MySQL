using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Sqlite2MySql;

public sealed class Strings : INotifyPropertyChanged
{
    public static Strings? Instance { get; private set; }

    private string _lang = "cs";
    private string? _lastStatusKey;
    private readonly Dictionary<string, Dictionary<string, string>> _d = new();

    public string CurrentStatus { get; private set; } = "";

    public Strings()
    {
        Instance = this;
        _lang = AppSettings.Language;

        void Add(string key, string cs, string en, string de, string pl)
        {
            _d[key] = new Dictionary<string, string> { ["cs"] = cs, ["en"] = en, ["de"] = de, ["pl"] = pl };
        }

        Add("AppTitle", "SQLite ➜ MySQL Converter", "SQLite ➜ MySQL Converter", "SQLite ➜ MySQL Konverter", "SQLite ➜ MySQL Konwerter");
        Add("AppHeading", "SQLite → MySQL", "SQLite → MySQL", "SQLite → MySQL", "SQLite → MySQL");
        Add("AppSubtitle", "Převod databáze a export do MySQL dumpu", "Database conversion and export to MySQL dump", "Datenbankkonvertierung und Export in MySQL-Dump", "Konwersja bazy danych i eksport do zrzutu MySQL");
        Add("MenuFile", "Soubor", "File", "Datei", "Plik");
        Add("MenuOpenSqlite", "Otevřít SQLite…", "Open SQLite…", "SQLite öffnen…", "Otwórz SQLite…");
        Add("MenuExit", "Ukončit", "Exit", "Beenden", "Zakończ");
        Add("MenuSettings", "Nastavení", "Settings", "Einstellungen", "Ustawienia");
        Add("MenuAbout", "O programu", "About", "Über", "O programie");
        Add("AboutTitle", "O programu", "About", "Über", "O programie");
        Add("AboutDescription", "Konvertor SQLite na MySQL. Převod struktury a dat do MySQL dumpu. .NET 8, Avalonia.", "SQLite to MySQL converter. Converts schema and data to MySQL dump. .NET 8, Avalonia.", "SQLite-zu-MySQL-Konverter. Konvertiert Struktur und Daten in MySQL-Dump. .NET 8, Avalonia.", "Konwerter SQLite do MySQL. Konwersja struktury i danych do zrzutu MySQL. .NET 8, Avalonia.");
        Add("AboutVersionLabel", "Verze", "Version", "Version", "Wersja");
        Add("AboutAuthorLabel", "Vývojář", "Author", "Entwickler", "Autor");
        Add("AboutContactLabel", "Kontakt", "Contact", "Kontakt", "Kontakt");
        Add("AboutLicenseLabel", "Licence", "License", "Lizenz", "Licencja");
        Add("AboutCopyrightLabel", "Copyright", "Copyright", "Copyright", "Prawa autorskie");
        Add("Section1", "1. Zdroj a výstup", "1. Source and output", "1. Quelle und Ausgabe", "1. Źródło i wynik");
        Add("Section2", "2. Možnosti exportu", "2. Export options", "2. Exportoptionen", "2. Opcje eksportu");
        Add("Section3", "3. MySQL připojení", "3. MySQL connection", "3. MySQL-Verbindung", "3. Połączenie MySQL");
        Add("LabelSqliteFile", "SQLite soubor:", "SQLite file:", "SQLite-Datei:", "Plik SQLite:");
        Add("LabelOutputSql", "Výstupní SQL:", "Output SQL:", "Ausgabe-SQL:", "SQL wyjściowy:");
        Add("WatermarkSqlite", "Cesta k .sqlite nebo .db", "Path to .sqlite or .db", "Pfad zu .sqlite oder .db", "Ścieżka do .sqlite lub .db");
        Add("WatermarkOutput", "Kam uložit MySQL dump", "Where to save MySQL dump", "Wo MySQL-Dump speichern", "Gdzie zapisać zrzut MySQL");
        Add("ButtonChoose", "Vybrat…", "Choose…", "Auswählen…", "Wybierz…");
        Add("ButtonSaveAs", "Uložit jako…", "Save as…", "Speichern unter…", "Zapisz jako…");
        Add("CheckIncludeData", "Exportovat data (INSERT)", "Export data (INSERT)", "Daten exportieren (INSERT)", "Eksportuj dane (INSERT)");
        Add("LabelBatchSize", "Batch size:", "Batch size:", "Batch-Größe:", "Rozmiar partii:");
        Add("LabelTargetDb", "Cílová DB:", "Target DB:", "Ziel-DB:", "Baza docelowa:");
        Add("WatermarkDatabase", "Název databáze (volitelné)", "Database name (optional)", "Datenbankname (optional)", "Nazwa bazy (opcjonalnie)");
        Add("CheckIncludeDbHeader", "Přidat CREATE/USE do dumpu", "Add CREATE/USE to dump", "CREATE/USE zum Dump hinzufügen", "Dodaj CREATE/USE do zrzutu");
        Add("LabelMysqlPath", "Cesta k mysql:", "Path to mysql:", "Pfad zu mysql:", "Ścieżka do mysql:");
        Add("WatermarkMysqlPath", "mysql nebo cesta k mysql.exe", "mysql or path to mysql.exe", "mysql oder Pfad zu mysql.exe", "mysql lub ścieżka do mysql.exe");
        Add("LabelHost", "Host:", "Host:", "Host:", "Host:");
        Add("LabelPort", "Port:", "Port:", "Port:", "Port:");
        Add("LabelUser", "Uživatel:", "User:", "Benutzer:", "Użytkownik:");
        Add("LabelPassword", "Heslo:", "Password:", "Passwort:", "Hasło:");
        Add("CheckApplyToMysql", "Po převodu rovnou importovat do MySQL", "Import to MySQL after conversion", "Nach Konvertierung in MySQL importieren", "Po konwersji zaimportuj do MySQL");
        Add("ButtonTestConnection", "Test připojení", "Test connection", "Verbindung testen", "Test połączenia");
        Add("ButtonConvert", "Spustit převod", "Run conversion", "Konvertierung starten", "Uruchom konwersję");
        Add("StatusDefault", "Vyberte SQLite databázi a spusťte převod.", "Select SQLite database and run conversion.", "SQLite-Datenbank wählen und Konvertierung starten.", "Wybierz bazę SQLite i uruchom konwersję.");
        Add("StatusTesting", "Testuji připojení…", "Testing connection…", "Verbindung wird getestet…", "Testowanie połączenia…");
        Add("StatusOk", "Připojení v pořádku.", "Connection OK.", "Verbindung OK.", "Połączenie OK.");
        Add("StatusConnectionError", "Chyba připojení.", "Connection error.", "Verbindungsfehler.", "Błąd połączenia.");
        Add("StatusConverting", "Převádím…", "Converting…", "Konvertierung läuft…", "Konwertowanie…");
        Add("StatusConvertingTable", "Tabulka {0}/{1}: {2}", "Table {0}/{1}: {2}", "Tabelle {0}/{1}: {2}", "Tabela {0}/{1}: {2}");
        Add("StatusDone", "Převod dokončen.", "Conversion complete.", "Konvertierung abgeschlossen.", "Konwersja zakończona.");
        Add("MsgSummaryTablesRows", "Převedeno {0} tabulek, {1} řádků.", "Converted {0} tables, {1} rows.", "{0} Tabellen, {1} Zeilen konvertiert.", "Przekonwertowano {0} tabel, {1} wierszy.");
        Add("ButtonOpenFolder", "Otevřít složku", "Open folder", "Ordner öffnen", "Otwórz folder");
        Add("StatusConvertError", "Chyba při převodu.", "Conversion error.", "Fehler bei der Konvertierung.", "Błąd konwersji.");
        Add("MsgConnectionOk", "Připojení k MySQL funguje.", "MySQL connection works.", "MySQL-Verbindung funktioniert.", "Połączenie MySQL działa.");
        Add("MsgOk", "OK", "OK", "OK", "OK");
        Add("MsgError", "Chyba", "Error", "Fehler", "Błąd");
        Add("MsgDone", "Hotovo", "Done", "Fertig", "Gotowe");
        Add("MsgSelectSqlite", "Vyberte SQLite databázi.", "Please select a SQLite database.", "Bitte SQLite-Datenbank auswählen.", "Wybierz bazę SQLite.");
        Add("MsgBatchSizeInvalid", "Batch size musí být kladné číslo.", "Batch size must be a positive number.", "Batch-Größe muss eine positive Zahl sein.", "Rozmiar partii musi być liczbą dodatnią.");
        Add("MsgDatabaseRequired", "Pro import do MySQL vyplňte název databáze.", "For MySQL import, enter the database name.", "Für MySQL-Import Datenbanknamen eingeben.", "Podaj nazwę bazy przy imporcie do MySQL.");
        Add("MsgSavedTo", "Uloženo do: {0}", "Saved to: {0}", "Gespeichert unter: {0}", "Zapisano do: {0}");
        Add("MsgAndImported", " a importováno do MySQL.", " and imported to MySQL.", " und in MySQL importiert.", " i zaimportowano do MySQL.");
        Add("SettingsTitle", "Nastavení", "Settings", "Einstellungen", "Ustawienia");
        Add("LabelTheme", "Téma:", "Theme:", "Design:", "Motyw:");
        Add("LabelLanguage", "Jazyk:", "Language:", "Sprache:", "Język:");
        Add("ThemeSystem", "Podle systému", "Follow system", "System folgen", "Zgodnie z systemem");
        Add("ThemeLight", "Světlý", "Light", "Hell", "Jasny");
        Add("ThemeDark", "Tmavý", "Dark", "Dunkel", "Ciemny");
        Add("LangCs", "Čeština", "Czech", "Tschechisch", "Czeski");
        Add("LangEn", "English", "English", "Englisch", "Angielski");
        Add("LangDe", "Deutsch", "German", "Deutsch", "Niemiecki");
        Add("LangPl", "Polski", "Polish", "Polnisch", "Polski");
        Add("ButtonCancel", "Zrušit", "Cancel", "Abbrechen", "Anuluj");
        Add("ButtonOk", "OK", "OK", "OK", "OK");
        Add("DialogPickSqlite", "Vyberte SQLite databázi", "Select SQLite database", "SQLite-Datenbank auswählen", "Wybierz bazę SQLite");
        Add("DialogSaveDump", "Kam uložit MySQL dump", "Where to save MySQL dump", "Wo MySQL-Dump speichern", "Gdzie zapisać zrzut MySQL");
        Add("DialogPickMysql", "Vyberte mysql.exe", "Select mysql.exe", "mysql.exe auswählen", "Wybierz mysql.exe");
        Add("FileTypeSqlite", "SQLite databáze", "SQLite database", "SQLite-Datenbank", "Baza SQLite");
        Add("FileTypeAll", "Všechny soubory", "All files", "Alle Dateien", "Wszystkie pliki");
        Add("FileTypeSql", "SQL soubor", "SQL file", "SQL-Datei", "Plik SQL");
        Add("MsgMysqlNotFound", "Soubor mysql nebyl nalezen: {0}\nZadejte celou cestu k mysql.exe nebo přidejte MySQL do proměnné PATH.", "mysql not found: {0}\nEnter full path to mysql.exe or add MySQL to PATH.", "mysql nicht gefunden: {0}\nVollständigen Pfad zu mysql.exe eingeben oder MySQL zur PATH hinzufügen.", "Nie znaleziono mysql: {0}\nPodaj pełną ścieżkę do mysql.exe lub dodaj MySQL do PATH.");
        Add("MsgConnectionFailed", "Připojení selhalo: {0}", "Connection failed: {0}", "Verbindung fehlgeschlagen: {0}", "Połączenie nie powiodło się: {0}");
        Add("MsgConvertFailed", "Převod selhal: {0}", "Conversion failed: {0}", "Konvertierung fehlgeschlagen: {0}", "Konwersja nie powiodła się: {0}");
        CurrentStatus = Get("StatusDefault");
    }

    public string Lang
    {
        get => _lang;
        set
        {
            if (_lang == value) return;
            _lang = value;
            AppSettings.Language = value;
            NotifyAll();
        }
    }

    public void SetLanguage(string lang)
    {
        if (_lang == lang) return;
        _lang = lang;
        AppSettings.Language = lang;
        NotifyAll();
    }

    private void NotifyAll()
    {
        foreach (var key in _d.Keys)
            OnPropertyChanged(key);
        if (_lastStatusKey != null)
        {
            CurrentStatus = Get(_lastStatusKey);
            OnPropertyChanged(nameof(CurrentStatus));
        }
        // když _lastStatusKey je null (vlastní zpráva), při změně jazyka neměníme CurrentStatus
    }

    public void SetCurrentStatus(string key)
    {
        _lastStatusKey = key;
        CurrentStatus = Get(key);
        OnPropertyChanged(nameof(CurrentStatus));
    }

    /// <summary>Nastaví stavový text přímo (pro průběh převodu). Při změně jazyka se nepřepíše.</summary>
    public void SetStatusMessage(string message)
    {
        _lastStatusKey = null;
        CurrentStatus = message ?? "";
        OnPropertyChanged(nameof(CurrentStatus));
    }

    public string AppTitle => Get("AppTitle");
    public string AppHeading => Get("AppHeading");
    public string AppSubtitle => Get("AppSubtitle");
    public string MenuFile => Get("MenuFile");
    public string MenuOpenSqlite => Get("MenuOpenSqlite");
    public string MenuExit => Get("MenuExit");
    public string MenuSettings => Get("MenuSettings");
    public string MenuAbout => Get("MenuAbout");
    public string AboutTitle => Get("AboutTitle");
    public string AboutDescription => Get("AboutDescription");
    public string AboutVersionLabel => Get("AboutVersionLabel");
    public string AboutAuthorLabel => Get("AboutAuthorLabel");
    public string AboutContactLabel => Get("AboutContactLabel");
    public string AboutLicenseLabel => Get("AboutLicenseLabel");
    public string AboutCopyrightLabel => Get("AboutCopyrightLabel");
    public string Section1 => Get("Section1");
    public string Section2 => Get("Section2");
    public string Section3 => Get("Section3");
    public string LabelSqliteFile => Get("LabelSqliteFile");
    public string LabelOutputSql => Get("LabelOutputSql");
    public string WatermarkSqlite => Get("WatermarkSqlite");
    public string WatermarkOutput => Get("WatermarkOutput");
    public string ButtonChoose => Get("ButtonChoose");
    public string ButtonSaveAs => Get("ButtonSaveAs");
    public string CheckIncludeData => Get("CheckIncludeData");
    public string LabelBatchSize => Get("LabelBatchSize");
    public string LabelTargetDb => Get("LabelTargetDb");
    public string WatermarkDatabase => Get("WatermarkDatabase");
    public string CheckIncludeDbHeader => Get("CheckIncludeDbHeader");
    public string LabelMysqlPath => Get("LabelMysqlPath");
    public string WatermarkMysqlPath => Get("WatermarkMysqlPath");
    public string LabelHost => Get("LabelHost");
    public string LabelPort => Get("LabelPort");
    public string LabelUser => Get("LabelUser");
    public string LabelPassword => Get("LabelPassword");
    public string CheckApplyToMysql => Get("CheckApplyToMysql");
    public string ButtonTestConnection => Get("ButtonTestConnection");
    public string ButtonConvert => Get("ButtonConvert");
    public string StatusDefault => Get("StatusDefault");
    public string StatusTesting => Get("StatusTesting");
    public string StatusOk => Get("StatusOk");
    public string StatusConnectionError => Get("StatusConnectionError");
    public string StatusConverting => Get("StatusConverting");
    public string StatusConvertingTable => Get("StatusConvertingTable");
    public string StatusDone => Get("StatusDone");
    public string MsgSummaryTablesRows => Get("MsgSummaryTablesRows");
    public string ButtonOpenFolder => Get("ButtonOpenFolder");
    public string StatusConvertError => Get("StatusConvertError");
    public string MsgConnectionOk => Get("MsgConnectionOk");
    public string MsgOk => Get("MsgOk");
    public string MsgError => Get("MsgError");
    public string MsgDone => Get("MsgDone");
    public string MsgSelectSqlite => Get("MsgSelectSqlite");
    public string MsgBatchSizeInvalid => Get("MsgBatchSizeInvalid");
    public string MsgDatabaseRequired => Get("MsgDatabaseRequired");
    public string MsgSavedTo => Get("MsgSavedTo");
    public string MsgAndImported => Get("MsgAndImported");
    public string SettingsTitle => Get("SettingsTitle");
    public string LabelTheme => Get("LabelTheme");
    public string LabelLanguage => Get("LabelLanguage");
    public string ThemeSystem => Get("ThemeSystem");
    public string ThemeLight => Get("ThemeLight");
    public string ThemeDark => Get("ThemeDark");
    public string LangCs => Get("LangCs");
    public string LangEn => Get("LangEn");
    public string LangDe => Get("LangDe");
    public string LangPl => Get("LangPl");
    public string ButtonCancel => Get("ButtonCancel");
    public string ButtonOk => Get("ButtonOk");
    public string DialogPickSqlite => Get("DialogPickSqlite");
    public string DialogSaveDump => Get("DialogSaveDump");
    public string DialogPickMysql => Get("DialogPickMysql");
    public string FileTypeSqlite => Get("FileTypeSqlite");
    public string FileTypeAll => Get("FileTypeAll");
    public string FileTypeSql => Get("FileTypeSql");

    public string Get(string key)
    {
        if (!_d.TryGetValue(key, out var langDict)) return key;
        if (langDict.TryGetValue(_lang, out var text)) return text;
        return langDict.TryGetValue("en", out var fallback) ? fallback : langDict["cs"];
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string? name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

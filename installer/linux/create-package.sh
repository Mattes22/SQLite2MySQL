#!/bin/bash
# Vytvoří tar.gz balíček pro Linux z publish složky.
# Použití:
#   ./create-package.sh [linux-x64|linux-arm64]   # výchozí linux-x64
#   ./create-package.sh /cesta/k/publish
#
# Nejdřív publikujte: dotnet publish ... -p:PublishProfile=Install-linux-x64
# Spouštět na Linuxu (nebo WSL). Z kořene projektu: ./installer/linux/create-package.sh

set -e
VERSION="1.0.0"
SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
PARENT_OUTPUT="$SCRIPT_DIR/../output"
if [ -d "$(dirname "$PARENT_OUTPUT")" ]; then
  OUTPUT_DIR="$(cd "$PARENT_OUTPUT" 2>/dev/null && pwd)" || true
fi
[ -z "$OUTPUT_DIR" ] && OUTPUT_DIR="$SCRIPT_DIR/output"
mkdir -p "$OUTPUT_DIR"

# První argument: RID nebo cesta k publish
if [ -n "$1" ]; then
  if [ -d "$1" ]; then
    PUBLISH_DIR="$(cd "$1" && pwd)"
    if [[ "$1" == *"arm64"* ]]; then
      RID="linux-arm64"
    else
      RID="linux-x64"
    fi
  else
    RID="$1"
    CANDIDATE="$SCRIPT_DIR/../../Sqlite2MySql/bin/Release/net8.0/$RID/publish"
    if [ -d "$CANDIDATE" ]; then
      PUBLISH_DIR="$(cd "$CANDIDATE" && pwd)"
    fi
  fi
else
  RID="linux-x64"
  CANDIDATE="$SCRIPT_DIR/../../Sqlite2MySql/bin/Release/net8.0/$RID/publish"
  if [ -d "$CANDIDATE" ]; then
    PUBLISH_DIR="$(cd "$CANDIDATE" && pwd)"
  fi
fi

if [ -z "$PUBLISH_DIR" ] || [ ! -d "$PUBLISH_DIR" ]; then
  echo "Chyba: Složka publish nebyla nalezena."
  echo "Nejdřív spusťte:"
  echo "  dotnet publish Sqlite2MySql/Sqlite2MySql.csproj -c Release -p:PublishProfile=Install-linux-x64"
  echo "  nebo Install-linux-arm64"
  echo "Nebo předajte cestu: $0 /cesta/k/publish"
  exit 1
fi

PKG_NAME="SQLite2MySQL-${VERSION}-${RID}"
PKG_DIR="$OUTPUT_DIR/$PKG_NAME"
ARCHIVE="$OUTPUT_DIR/$PKG_NAME.tar.gz"

echo "Publish složka: $PUBLISH_DIR"
echo "Balíček: $ARCHIVE"

rm -rf "$PKG_DIR"
mkdir -p "$PKG_DIR"
cp -R "$PUBLISH_DIR"/* "$PKG_DIR/"
if [ -f "$PKG_DIR/sqlite2mysql" ]; then
  chmod +x "$PKG_DIR/sqlite2mysql"
fi

# Jednoduchý run skript (volitelně)
cat > "$PKG_DIR/run.sh" << 'RUN'
#!/bin/bash
cd "$(dirname "$0")"
exec ./sqlite2mysql "$@"
RUN
chmod +x "$PKG_DIR/run.sh"

tar -czf "$ARCHIVE" -C "$OUTPUT_DIR" "$PKG_NAME"
rm -rf "$PKG_DIR"

echo "Hotovo: $ARCHIVE"

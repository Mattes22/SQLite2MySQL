#!/bin/bash
# Vytvoří SQLite2MySQL.app z publish složky a volitelně DMG.
# Použití:
#   ./create-app.sh [cesta/k/publish]
#   ./create-app.sh ../Sqlite2MySql/bin/Release/net8.0/osx-arm64/publish
#
# Spouštět na macOS. Z kořene V0.1: ./installer/macos/create-app.sh

set -e
SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
PARENT_OUTPUT="$SCRIPT_DIR/../output"
if [ -d "$(dirname "$PARENT_OUTPUT")" ]; then
  OUTPUT_DIR="$(cd "$PARENT_OUTPUT" 2>/dev/null && pwd)" || true
fi
[ -z "$OUTPUT_DIR" ] && OUTPUT_DIR="$SCRIPT_DIR/output"
mkdir -p "$OUTPUT_DIR"

if [ -n "$1" ]; then
  PUBLISH_DIR="$(cd "$1" && pwd)"
else
  # Výchozí cesta z installer/macos: ../../Sqlite2MySql/bin/Release/net8.0/
  for RID in osx-arm64 osx-x64; do
    CANDIDATE="$SCRIPT_DIR/../../Sqlite2MySql/bin/Release/net8.0/$RID/publish"
    if [ -d "$CANDIDATE" ]; then
      PUBLISH_DIR="$(cd "$CANDIDATE" && pwd)"
      break
    fi
  done
fi

if [ -z "$PUBLISH_DIR" ] || [ ! -d "$PUBLISH_DIR" ]; then
  echo "Chyba: Složka publish nebyla nalezena."
  echo "Nejdřív spusťte: dotnet publish Sqlite2MySql/Sqlite2MySql.csproj -c Release -p:PublishProfile=Install-osx-arm64"
  echo "Nebo předajte cestu: $0 /cesta/k/publish"
  exit 1
fi

APP_NAME="SQLite2MySQL"
APP_PATH="$OUTPUT_DIR/$APP_NAME.app"
EXE_NAME="sqlite2mysql"

echo "Publish složka: $PUBLISH_DIR"
echo "Výstup: $APP_PATH"

rm -rf "$APP_PATH"
mkdir -p "$APP_PATH/Contents/MacOS"
mkdir -p "$APP_PATH/Contents/Resources"

cp -R "$PUBLISH_DIR"/* "$APP_PATH/Contents/MacOS/"
cp "$SCRIPT_DIR/Info.plist" "$APP_PATH/Contents/"

ICON_ICNS="$SCRIPT_DIR/AppIcon.icns"
ROOT_ICNS="$SCRIPT_DIR/../../sqlite2mysql.icns"
if [ ! -f "$ICON_ICNS" ] && [ -f "$ROOT_ICNS" ]; then
  ICON_ICNS="$ROOT_ICNS"
fi

if [ ! -f "$ICON_ICNS" ] && [ -x "$SCRIPT_DIR/build-icon.sh" ]; then
  echo "Chybí AppIcon.icns, zkouším vygenerovat..."
  "$SCRIPT_DIR/build-icon.sh" || true
fi

if [ -f "$ICON_ICNS" ]; then
  cp "$ICON_ICNS" "$APP_PATH/Contents/Resources/AppIcon.icns"
else
  echo "Varování: AppIcon.icns nenalezen – aplikace bude bez ikony."
fi

if [ -f "$APP_PATH/Contents/MacOS/$EXE_NAME" ]; then
  chmod +x "$APP_PATH/Contents/MacOS/$EXE_NAME"
fi

echo "Hotovo: $APP_PATH"
echo "Pro vytvoření DMG spusťte: ./create-dmg.sh"
exit 0

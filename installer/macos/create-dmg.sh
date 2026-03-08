#!/bin/bash
# Vytvoří DMG z SQLite2MySQL.app (nejdřív spusťte create-app.sh).
# Spouštět na macOS.

set -e
SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
PARENT_OUTPUT="$SCRIPT_DIR/../output"
if [ -d "$(dirname "$PARENT_OUTPUT")" ]; then
  OUTPUT_DIR="$(cd "$PARENT_OUTPUT" 2>/dev/null && pwd)" || true
fi
[ -z "$OUTPUT_DIR" ] && OUTPUT_DIR="$SCRIPT_DIR/output"
APP_NAME="SQLite2MySQL"
DMG_NAME="SQLite2MySQL_1.0.0"
APP_PATH="$OUTPUT_DIR/$APP_NAME.app"

if [ ! -d "$APP_PATH" ]; then
  echo "Nejdřív spusťte: ./create-app.sh"
  exit 1
fi

mkdir -p "$OUTPUT_DIR/dmg-src"
cp -R "$APP_PATH" "$OUTPUT_DIR/dmg-src/"
ln -s /Applications "$OUTPUT_DIR/dmg-src/Applications"

DMG_TMP="$OUTPUT_DIR/$DMG_NAME-tmp.dmg"
DMG_FINAL="$OUTPUT_DIR/$DMG_NAME.dmg"
rm -f "$DMG_TMP" "$DMG_FINAL"

hdiutil create -srcfolder "$OUTPUT_DIR/dmg-src" -volname "$APP_NAME" -fs HFS+ -fsargs "-c c=64,a=16,e=16" -format UDRW -size 200m "$DMG_TMP"
device=$(hdiutil attach -readwrite -noverify -noautoopen "$DMG_TMP" | awk 'NR==1{print $1}')
sleep 2
hdiutil detach "$device"
hdiutil convert "$DMG_TMP" -format UDZO -o "$DMG_FINAL"
rm -f "$DMG_TMP"
rm -rf "$OUTPUT_DIR/dmg-src"

echo "Hotovo: $DMG_FINAL"

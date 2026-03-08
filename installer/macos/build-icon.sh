#!/bin/bash
# Vygeneruje AppIcon.icns z PNG (spouštět na macOS).
# Použití:
#   ./build-icon.sh
#   ./build-icon.sh /cesta/k/logo.png /cesta/k/AppIcon.icns

set -e
SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
SOURCE_PNG="${1:-$SCRIPT_DIR/../../sqlite2mysql.png}"
OUTPUT_ICNS="${2:-$SCRIPT_DIR/AppIcon.icns}"
ICONSET_DIR="$SCRIPT_DIR/AppIcon.iconset"

if [ ! -f "$SOURCE_PNG" ]; then
  echo "Chyba: PNG nenalezen: $SOURCE_PNG"
  exit 1
fi

if ! command -v sips >/dev/null 2>&1; then
  echo "Chyba: chybí nástroj 'sips' (spusťte na macOS)."
  exit 1
fi

if ! command -v iconutil >/dev/null 2>&1; then
  echo "Chyba: chybí nástroj 'iconutil' (spusťte na macOS)."
  exit 1
fi

rm -rf "$ICONSET_DIR"
mkdir -p "$ICONSET_DIR"

# Základní velikosti
sips -z 16 16 "$SOURCE_PNG" --out "$ICONSET_DIR/icon_16x16.png" >/dev/null
sips -z 32 32 "$SOURCE_PNG" --out "$ICONSET_DIR/icon_32x32.png" >/dev/null
sips -z 128 128 "$SOURCE_PNG" --out "$ICONSET_DIR/icon_128x128.png" >/dev/null
sips -z 256 256 "$SOURCE_PNG" --out "$ICONSET_DIR/icon_256x256.png" >/dev/null
sips -z 512 512 "$SOURCE_PNG" --out "$ICONSET_DIR/icon_512x512.png" >/dev/null

# Retina (@2x) varianty
sips -z 32 32 "$SOURCE_PNG" --out "$ICONSET_DIR/icon_16x16@2x.png" >/dev/null
sips -z 64 64 "$SOURCE_PNG" --out "$ICONSET_DIR/icon_32x32@2x.png" >/dev/null
sips -z 256 256 "$SOURCE_PNG" --out "$ICONSET_DIR/icon_128x128@2x.png" >/dev/null
sips -z 512 512 "$SOURCE_PNG" --out "$ICONSET_DIR/icon_256x256@2x.png" >/dev/null
sips -z 1024 1024 "$SOURCE_PNG" --out "$ICONSET_DIR/icon_512x512@2x.png" >/dev/null

iconutil -c icns "$ICONSET_DIR" -o "$OUTPUT_ICNS"
rm -rf "$ICONSET_DIR"

echo "Hotovo: $OUTPUT_ICNS"

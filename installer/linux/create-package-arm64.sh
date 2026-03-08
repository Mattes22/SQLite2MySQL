#!/bin/bash
# Vytvoří tar.gz balíček pro Linux ARM64. Viz create-package.sh.
SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
exec "$SCRIPT_DIR/create-package.sh" linux-arm64

#!/bin/bash
# Vytvoří tar.gz balíček pro Linux x64. Viz create-package.sh.
SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
exec "$SCRIPT_DIR/create-package.sh" linux-x64

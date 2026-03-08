# Contributing

Thanks for your interest in contributing to SQLite2MySQL.

## Ways to Contribute

- Report bugs and usability issues
- Suggest features or improvements
- Improve documentation
- Submit pull requests

## Before You Start

- Search existing issues and pull requests to avoid duplicates.
- For larger changes, open an issue first to discuss the approach.

## Development Setup

Requirements:

- .NET 8 SDK

Build:

```bash
cd Sqlite2MySql
dotnet build -c Release
```

Run (CLI):

```bash
cd Sqlite2MySql
bin/Release/net8.0/sqlite2mysql --help
```

Run (GUI):

```bash
cd Sqlite2MySql
bin/Release/net8.0/sqlite2mysql --gui
```

## Coding Guidelines

- Keep changes focused and minimal.
- Match the existing code style and naming.
- Add or update documentation when behavior changes.
- Prefer clear, direct code over cleverness.

## Testing

- If your change affects behavior, include a test or explain why it is not needed.
- Run the app and verify both CLI and GUI flows when relevant.

## Submitting a Pull Request

- Keep PRs small and scoped.
- Describe the problem and the solution clearly.
- Include screenshots for GUI changes.

## Reporting Security Issues

Please report security issues privately by emailing: `info@matejzavadil.cz`.

## License

By contributing, you agree that your contributions will be licensed under the MIT License.

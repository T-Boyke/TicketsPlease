# Contributing to TicketsPlease 🚀

First off, thank you for considering contributing to **TicketsPlease**! It's
people like you that make it an enterprise-grade tool.

## 🏗️ Technical Standards

We follow strict architectural and coding standards. Please review our
documentation before starting:

- **Architecture:** [Clean Architecture (Onion)]
  (../docs/adr/0001-clean-architecture.md)
- **Database:**
  [3rd Normal Form & Enterprise Extensions](../docs/database_schema.md)
- **UI:** [Tailwind CSS 4.x & Corporate Skinning]
  (../docs/adr/0020-corporate-skinning-provider.md)

## 🛠️ Local Development Setup

1. **Clone the repo:** `git clone https://github.com/Tobia/TicketsPlease.git`
2. **Setup .NET:** Ensure you have .NET 10.3 SDK installed.
3. **Restore Tools:** Run `dotnet tool restore` (crucial for Tailwind).
4. **Build:** Run `dotnet build src/TicketsPlease.slnx`.

## 📜 Commit Message Guidelines

We use [Conventional Commits](https://www.conventionalcommits.org/):

- `feat:` for new features
- `fix:` for bug fixes
- `docs:` for documentation changes
- `chore:` for maintenance

## 🔄 Pull Request Process

1. Create a branch named `feature/your-feature` or `bugfix/your-fix`.
2. Ensure all tests pass (`dotnet test`).
3. Use our [Pull Request Template](pull_request_template.md).
4. If your change is architectural, create an **ADR** in `docs/adr/`.

## 🤖 AI Collaboration

This project is optimized for AI-assisted development. If you use AI tools (like
Antigravity), please use the `🤖 AI-Generated` flag in your Pull Request.

# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [2026-03-26]

### Added

- **100% Integration Coverage**: Expanded Integration tests for Infrastructure, Services, and
  Kern-Controllers achieving high/100% path coverage.
- **Visual Documentation**: Created a comprehensive architectural documentation suite
  (`docs/big5.md`) with 11 diagrams.
- **Internal Controller Discovery**: Implemented `InternalControllerFeatureProvider` for better
  DDD encapsulation.
- **Security & DevExperience**: Relaxed CSP in Development for Styleguide and Browser Link support.
- **Messaging Attachments**: Support for file uploads in direct messages with automatic mapping.
- **Conversation View**: Grouped messaging interface (F9) with full history and premium UI.
- **Verified Components**: Added unit tests for `LocalStorageService` and `MessageService` enhancements.

### Optimized

- **Build & Linting**: Achieved 100% compliance with `dotnet format --verify-no-changes`.
- **Ignore Strategy**: Added `.markdownlintignore` and updated `.gitignore` for coverage reports.
- **Static Analysis**: Resolved multiple SA1402, CA1515, and CS0660 warnings.
- **Structured Storage**: Implemented dated subdirectories (YYYY/MM) in `LocalStorageService` for
  better scalability and disk organization.

### Fixed

- **404 Routing**: Restored `public` access to all Web controllers to ensure proper discovery.
- **Inconsistent Accessibility**: Fixed `CS0051` by making `LoginViewModel` public.
- **Type Conversion**: Resolved `bool?` to `bool` mismatch in `AccountController`.
- **Identity Hash**: Fixed string-to-User conversion in `DbInitialiser` password hashing.
- **Dependency Cleanup**: Removed unused `Identity.Core` and `Stores` packages from Application.

[2026-03-26]: https://github.com/BitLC-NE-2025-2026/TicketsPlease/compare/v0.1.0...main

# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added

- Comprehensive English localization (`.en.resx`) for Account, Project, and Ticket views.
- Seeding logic for initial database setup in `DbInitialiser`.

### Fixed

- **404 Routing**: Restored `public` access to all Web controllers to ensure proper discovery by the
  ASP.NET Core MVC routing engine.
- **Inconsistent Accessibility**: Fixed `CS0051` by making `LoginViewModel` and `RegisterViewModel`
  public to match controller access.
- **Type Conversion**: Resolved `bool?` to `bool` mismatch in `AccountController`.
- **Identity Hash**: Fixed string-to-User conversion in `DbInitialiser` password hashing.
- **Dependency Cleanup**: Removed unused `Microsoft.Extensions.Identity.Core` and `Stores` packages
  from the Application project.

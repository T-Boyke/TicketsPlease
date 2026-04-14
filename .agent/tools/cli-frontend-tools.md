# 💻 TicketsPlease - CLI & Frontend Tools

<!-- markdownlint-disable MD033 -->

<cli_tools> <dotnet_cli>

- Build: `dotnet build /nologo` (Warnings as Errors).
- Test: `dotnet test` (xUnit + ArchTests).
- Format: `dotnet format` (EditorConfig compliance).

</dotnet_cli> <ef_core_cli>

- Migration:
  `dotnet ef migrations add [Name] --project src/TicketsPlease.Infrastructure \ --startup-project src/TicketsPlease.Web --context AppDbContext`.
- Update:
  `dotnet ef database update --project src/TicketsPlease.Infrastructure \ --startup-project src/TicketsPlease.Web`.

</ef_core_cli> <tailwind_cli>

- Logic: Zero-Node via `tailwindcss-dotnet` / `Tailwind.Hosting`.
- Build: Integrated via MSBuild in `.Web` project.

</tailwind_cli> </cli_tools>

<frontend_tooling> <libman>

- Purpose: Client-side storage in `wwwroot/lib`.
- Policy: ZERO CDN (DSGVO compliance).
- Logic: Managed via `libman.json`.

</libman>
<assets>

- Icons: FontAwesome 7.2 Pro (Local).
- Images: Optimized WebP/SVG in `wwwroot/images`.

</assets>
</frontend_tooling>

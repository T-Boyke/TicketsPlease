# 🌐 TicketsPlease - Localization Bundle

<localization_bundle>
  <strategy>
    - Pattern: .NET Resource (.resx) files.
    - Locations: `TicketsPlease.Application/Resources/` and `TicketsPlease.Web/Resources/`.
    - Resolution: Cookie-based or Header-based `RequestCultureProvider`.
  </strategy>

  <german_priority>
    - Goal: 100% German coverage for all user-facing strings.
    - Formatting: European Style (Comma for decimals, Dot for thousands).
    - Encoding: UTF-8 for Umlauts (ä, ö, ü, ß).
  </german_priority>
</localization_bundle>

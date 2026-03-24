localization_bundle:
  strategy:
    pattern: ".NET Resource (.resx) files"
    locations: ["TicketsPlease.Application/Resources/", "TicketsPlease.Web/Resources/"]
    resolution: "Cookie-based or Header-based RequestCultureProvider"
  german_priority:
    goal: "100% German coverage for all user-facing strings"
    formatting: "European style (comma for decimals, dot for thousands)"
    encoding: "UTF-8 for umlauts (ä, ö, ü, ß)"

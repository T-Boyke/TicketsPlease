markdown_documentation_rules:
  tools_standards:
    code_style: "Prettier (as per .vscode/settings.json)"
    linting: "DavidAnson/markdownlint (v0.40.0)"
    configuration: [".markdownlint.json", ".prettierrc"]
    verification_command: "npx markdownlint-cli2 '**/*.md' --config '.markdownlint.json'"
  formatting_requirements:
    line_length: "Max 100 characters (MD013); exceptions: tables, code blocks, links"
    lists_indentation:
      spacing: "Exactly one space after list marker (MD030)"
      indentation: "2 spaces for unordered lists (MD007)"
      surroundings: "Must be surrounded by blank lines (MD032)"
    header_structure:
      h1_limit: "Exactly one H1 per file"
      hierarchy: "Logical H1 -> H2 -> H3 progression"
      uniqueness: "Identical headers only allowed in different sections (MD024)"
  visuals_ui:
    mermaid_diagrams:
      importance: "Essential for logic and architecture visualization"
      features: "Use styles, classes, and interactive elements"
      syntax: "Quote IDs to prevent errors"
    github_alerts:
      note: "Background context/explanations"
      tip: "Best practices/efficiency"
      important: "Critical info/requirements"
  language_style:
    primary: "German (unless requested otherwise)"
    terminology: ["Clean Architecture", "DDD", "TDD"]
    navigation: "Create Table of Contents (ToC) for long documents"
  version: "1.0"
  date: "2026-03-09"

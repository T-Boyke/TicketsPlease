mermaid_diagram_rules:
  objective: "Rules for creating Mermaid diagrams (Gantt charts) in TicketsPlease documentation"
  gantt_best_practices:
    visibility_scaling:
      - "Use 'after <taskID>' for sequential task arrangement"
      - { interval: "tickInterval 1day|1week", context: "depends on project duration" }
      - { format: "axisFormat %d.%m", context: "compact date display" }
    syntax_error_prevention:
      quoting: "Labels containing special characters (:, ,, ()) must be quoted: '\"F1: Setup\"'"
      ids: "Use unique task IDs for dependency mapping with 'after'"
    working_periods:
      exclusions: "Mark non-working days with 'excludes weekends'"
      weekend_start: "Adjust weekend start with 'weekend friday' if necessary (default: Sat/Sun)"
  example_structure:
    syntax: |
      gantt
          title Project Example
          dateFormat  DD.MM.YYYY
          axisFormat  %d.%m
          tickInterval 1day
          excludes weekends
          section Phase 1
          Task A :a1, 23.03.2026, 4h
          Task B :after a1, 4h
  version: "1.0"
  date: "2026-03-24"

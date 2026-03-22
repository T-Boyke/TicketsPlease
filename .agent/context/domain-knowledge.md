# 🧬 TicketsPlease - Domain Knowledge

<domain_knowledge>
<ticket_entity>

- Core Aggregate.
- ID: SHA1-Hash universally.
- Tracking: Geo-Timestamp on every state mutation.
- Logic: Rich Model. Mutations inside Entity (`ticket.MoveToReview()`), NEVER in Application Services.
  </ticket_entity>
  <chili_metrics>
- Complexity scale:
- `1 Chili`: Trivial / Docs.
- `3 Chilis`: Standard Feature / Refactoring.
- `5 Chilis`: Critical Bug / Architecture Rewrite.
  </chili_metrics>
  <business_rules>
- Close Logic: Only Creator, Admin, or Teamlead can close (`Closed`) tickets.
- State Flow: To Do -> In Progress -> Review -> Done -> (Auto-Archive after X days).
  </business_rules>
  </domain_knowledge>

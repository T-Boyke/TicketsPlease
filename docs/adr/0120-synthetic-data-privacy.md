# ADR 0120: Synthetic Data Generation and Privacy Compliance

- Status: Proposed

## Context

In the development and testing of the TicketsPlease system, we need realistic data sets to verify
UI behavior, performance, and business logic. Traditionally, developers sometimes use anonymized
production data, which carries significant legal and security risks under the General Data
Protection Regulation (GDPR / DSGVO).

## Decision

We will use the **Bogus** library to generate 100% synthetic data for all non-production
environments (Development, Testing, Staging).

1. **Strict Locale:** Data generation will use the `de` (German) locale to ensure that addressing,
   names, and postal codes are realistic for our target market.
2. **Infrastructure Hook:** The seeder will be integrated into the Infrastructure layer and
   triggered via the Web entry point.
3. **No PII:** No real "Personally Identifiable Information" (PII) will ever be used or imported
   from external sources for seeding purposes.

## Consequences

- **Compliance:** Full compliance with Datenschutz (GDPR) requirements as no real user data is
  involved.
- **Independence:** Developers can reset the environment at any time with a fresh, populated data
  set.
- **Consistency:** All developers work with similar data distributions.
- **Maintenance:** The `DbInitialiser` must be updated whenever the domain schema changes.

## References

- [GDPR Official Site](https://gdpr.eu/)
- [Bogus GitHub Repository](https://github.com/bchavez/Bogus)

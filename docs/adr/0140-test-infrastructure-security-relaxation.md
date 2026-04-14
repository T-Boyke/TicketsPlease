# ADR 0140: Integration Test Infrastructure & Security Relaxation

## Status

Accepted

## Context

To achieve 100% path coverage for the integration test suite, we need to test authenticated POST
requests that modify state. However, the standard ASP.NET Core security middleware (Antiforgery,
Cookie Authentication) introduces significant overhead and complexity in a headless
`WebApplicationFactory` environment.

Specifically:

1. **Antiforgery**: Validating CSRF tokens in tests requires extracting tokens from GET responses
   and re-submitting them, which is fragile.
2. **Authentication**: Simulating a full Identity login flow for every test is slow and depends on
   the database state.
3. **CSP**: Content Security Policy headers block the Styleguide and Browser Link in development
   environments.

## Decision

We decided to implement the following infrastructure for integration tests:

1. **TestAuthHandler**: A custom `AuthenticationHandler` that automatically signs in a "Test User"
   with defined claims when a specific scheme is used.
2. **IAntiforgery Bypass**: In the test environment, we replace the `IAntiforgery` service with a
   `FakeAntiforgery` implementation that always returns `true` for token validation.
3. **CSP Relaxation**: In `Program.cs`, we conditionally relax the CSP header in the `Development`
   environment to allow `unsafe-inline` styles and WebSockets (required for Styleguide/Browser
   Link).

## Consequences

- **Positive**: Tests are significantly faster and more stable. We can test any controller action by
  simply calling the endpoint.
- **Positive**: The Styleguide is now usable without manual CSP attribute hashing.
- **Negative**: The test environment slightly differs from production (though business logic remains
  identical).
- **Security**: The CSP relaxation is strictly limited to the `Development` environment and won't
  affect `Production`.

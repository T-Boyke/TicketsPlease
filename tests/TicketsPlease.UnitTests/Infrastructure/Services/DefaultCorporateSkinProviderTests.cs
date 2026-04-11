using FluentAssertions;
using TicketsPlease.Infrastructure.Services;
using Xunit;

namespace TicketsPlease.UnitTests.Infrastructure.Services;

public class DefaultCorporateSkinProviderTests
{
    [Fact]
    public void GetColors_ShouldReturnDefaults()
    {
        var provider = new DefaultCorporateSkinProvider();
        provider.GetPrimaryColor().Should().Be("#3b82f6");
        provider.GetSecondaryColor().Should().Be("#1e40af");
        provider.GetLogoName().Should().Be("TicketsPlease");
    }
}

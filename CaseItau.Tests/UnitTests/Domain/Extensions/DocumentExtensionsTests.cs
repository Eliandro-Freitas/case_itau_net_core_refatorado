using CaseItau.Domain.Extensions;
using FluentAssertions;

namespace CaseItau.Tests.UnitTests.Domain.Extensions;

public class DocumentExtensionsTests
{
    [Theory]
    [InlineData("04961224000110")]
    [InlineData("69.311.417/0001-71")]
    [InlineData("73.026.468/0001-29")]
    public void IsValidCnpj_ValidCnpj_ReturnsTrue(string cnpj)
        => cnpj.IsValidCnpj().Should().BeTrue();

    [Theory]
    [InlineData("00000000000000")]
    [InlineData("11111111111111")]
    [InlineData("1234567890123")]
    [InlineData("123456789012345")]
    [InlineData("abcdefghijklmn")]
    [InlineData("")]
    [InlineData(null)]
    public void IsValidCnpj_InvalidCnpj_ReturnsFalse(string cnpj)
        => cnpj.IsValidCnpj().Should().BeFalse();

    [Theory]
    [InlineData("11.222.333/4444-55", "11222333444455")]
    [InlineData("abc123def456", "123456")]
    [InlineData("1234567890", "1234567890")]
    public void OnlyDigits_StringWithNonDigits_ReturnsOnlyDigits(string input, string expected)
        => input.OnlyDigits().Should().Be(expected);

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void OnlyDigits_NullOrEmpty_ReturnsEmptyString(string input)
        => input.OnlyDigits().Should().BeEmpty();

    [Fact]
    public void OnlyDigits_StringWithNoDigits_ReturnsEmptyString()
        => "abcdef".OnlyDigits().Should().BeEmpty();
}
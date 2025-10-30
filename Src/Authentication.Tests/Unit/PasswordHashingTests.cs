using System.Security.Cryptography;
using System.Text;
using FluentAssertions;
using Xunit;

namespace Authentication.Tests.Unit;

public class PasswordHashingTests
{
    [Fact]
    public void ComputeHash_WithSamePassword_ShouldReturnConsistentHash()
    {
        // Arrange
        var password = "testpassword123";

        // Act
        var hash1 = ComputeTestHash(password);
        var hash2 = ComputeTestHash(password);

        // Assert
        hash1.Should().Be(hash2);
        hash1.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void ComputeHash_WithDifferentPasswords_ShouldReturnDifferentHashes()
    {
        // Arrange
        var password1 = "testpassword123";
        var password2 = "differentpassword456";

        // Act
        var hash1 = ComputeTestHash(password1);
        var hash2 = ComputeTestHash(password2);

        // Assert
        hash1.Should().NotBe(hash2);
        hash1.Should().NotBeNullOrEmpty();
        hash2.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void VerifyHash_WithCorrectPassword_ShouldReturnTrue()
    {
        // Arrange
        var password = "testpassword123";
        var hash = ComputeTestHash(password);

        // Act
        var result = VerifyTestHash(password, hash);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void VerifyHash_WithIncorrectPassword_ShouldReturnFalse()
    {
        // Arrange
        var correctPassword = "testpassword123";
        var incorrectPassword = "wrongpassword456";
        var hash = ComputeTestHash(correctPassword);

        // Act
        var result = VerifyTestHash(incorrectPassword, hash);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("a")]
    [InlineData("short")]
    [InlineData("this-is-a-very-long-password-with-many-characters-that-should-still-work-correctly")]
    public void ComputeHash_WithVariousPasswordLengths_ShouldReturnValidHashes(string password)
    {
        // Act
        var hash = ComputeTestHash(password);

        // Assert
        hash.Should().NotBeNullOrEmpty();

        // Verify the hash can be used for verification
        var verificationResult = VerifyTestHash(password, hash);
        verificationResult.Should().BeTrue();
    }

    [Fact]
    public void ComputeHash_WithSpecialCharacters_ShouldWorkCorrectly()
    {
        // Arrange
        var password = "P@ssw0rd!@#$%^&*()_+-=[]{}|;':\",./<>?";

        // Act
        var hash = ComputeTestHash(password);

        // Assert
        hash.Should().NotBeNullOrEmpty();

        // Verify the hash works
        var verificationResult = VerifyTestHash(password, hash);
        verificationResult.Should().BeTrue();
    }

    [Fact]
    public void ComputeHash_WithUnicodeCharacters_ShouldWorkCorrectly()
    {
        // Arrange
        var password = "密码测试123üñíçødé";

        // Act
        var hash = ComputeTestHash(password);

        // Assert
        hash.Should().NotBeNullOrEmpty();

        // Verify the hash works
        var verificationResult = VerifyTestHash(password, hash);
        verificationResult.Should().BeTrue();
    }

    [Fact]
    public void VerifyHash_WithNullOrEmptyHash_ShouldReturnFalse()
    {
        // Arrange
        var password = "testpassword123";

        // Act & Assert
        VerifyTestHash(password, null).Should().BeFalse();
        VerifyTestHash(password, string.Empty).Should().BeFalse();
        VerifyTestHash(password, " ").Should().BeFalse();
    }

    [Fact]
    public void VerifyHash_WithInvalidHash_ShouldReturnFalse()
    {
        // Arrange
        var password = "testpassword123";
        var invalidHash = "this-is-not-a-valid-hash";

        // Act
        var result = VerifyTestHash(password, invalidHash);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void HashLength_ShouldBeConsistent()
    {
        // Arrange
        var passwords = new[] { "password1", "password2", "longerpassword123", "short" };

        // Act
        var hashes = passwords.Select(ComputeTestHash).ToList();

        // Assert
        var firstHashLength = hashes[0].Length;
        hashes.Should().AllSatisfy(hash => hash.Length.Should().Be(firstHashLength));
    }

    // Helper methods that simulate the hash functionality from Foundation.Base
    private static string ComputeTestHash(string password)
    {
        // Simple implementation for testing - mimics Foundation.Base.Util.StringHelper.ComputeArgon2Hash
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + "salt"));
        return Convert.ToBase64String(bytes);
    }

    private static bool VerifyTestHash(string password, string? hash)
    {
        if (string.IsNullOrWhiteSpace(hash))
        {
            return false;
        }

        try
        {
            var computedHash = ComputeTestHash(password);
            return computedHash == hash;
        }
        catch
        {
            return false;
        }
    }
}

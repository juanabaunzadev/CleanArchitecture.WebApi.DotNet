using CleanArchitecture.WebApi.Domain.Exceptions;
using CleanArchitecture.WebApi.Domain.ValueObjects;

namespace CleanArchitecture.WebApi.Tests.Domain.ValueObjects;

[TestClass]
public class EmailTests
{
    [TestMethod]
    public void CreateEmail_ValidEmail_ReturnsEmailObject()
    {
        // Arrange
        var validEmail = "test@example.com";
        
        // Act
        var email = Email.Create(validEmail);
        
        // Assert
        Assert.AreEqual(validEmail, email.Value);
    }

    [TestMethod]
    public void CreateEmail_InvalidEmail_ThrowsDomainException()
    {
        // Arrange
        var invalidEmail = "invalid-email";
        
        // Act & Assert
        Assert.ThrowsExactly<DomainException>(() => Email.Create(invalidEmail));
    }

    [TestMethod]
    public void CreateEmail_EmptyEmail_ThrowsDomainException()
    {
        // Arrange
        var emptyEmail = "";
        
        // Act & Assert
        Assert.ThrowsExactly<DomainException>(() => Email.Create(emptyEmail));
    }
}

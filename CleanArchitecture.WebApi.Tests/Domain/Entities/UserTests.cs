using CleanArchitecture.WebApi.Domain.Entities;
using CleanArchitecture.WebApi.Domain.Exceptions;

namespace CleanArchitecture.WebApi.Tests.Domain.Entities;

[TestClass]
public class UserTests
{
    // Test data
    private readonly string _firstName = "John";
    private readonly string _lastName = "Doe";
    private readonly string _email = "test@example.com";
    private readonly string _passwordHash = "Password123!";


    [TestMethod]
    public void CreateUser_ValidData_ReturnsUserObject()
    {   
        // Act
        var user = User.Create(_firstName, _lastName, _email, _passwordHash);
        
        // Assert
        Assert.AreEqual(_firstName, user.FirstName);
        Assert.AreEqual(_lastName, user.LastName);
        Assert.AreEqual(_email, user.Email.Value);
        Assert.AreEqual(_passwordHash, user.PasswordHash);
        Assert.IsTrue(user.IsActive);
        Assert.IsNotNull(user.UserRoles);
    }

    [TestMethod]
    public void CreateUser_EmptyFirstName_ThrowsDomainException()
    {
        // Act & Assert
        Assert.ThrowsExactly<DomainException>(() => User.Create("", _lastName, _email, _passwordHash));    
    }
}

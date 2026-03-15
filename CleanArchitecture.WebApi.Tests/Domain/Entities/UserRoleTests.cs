using CleanArchitecture.WebApi.Domain.Entities;
using CleanArchitecture.WebApi.Domain.Exceptions;

namespace CleanArchitecture.WebApi.Tests.Domain.Entities;

[TestClass]
public class UserRoleTests
{
    // Test data
    private readonly Guid _userId = Guid.NewGuid();
    private readonly Guid _roleId = Guid.NewGuid();


    [TestMethod]
    public void CreateUserRole_ValidData_ReturnsUserRoleObject()
    {
        // Act
        var userRole = UserRole.Create(_userId, _roleId);
        
        // Assert
        Assert.AreEqual(_userId, userRole.UserId);
        Assert.AreEqual(_roleId, userRole.RoleId);
    }

    [TestMethod]
    public void CreateUserRole_EmptyUserId_ThrowsDomainException()
    {
        // Act & Assert
        Assert.ThrowsExactly<DomainException>(() => UserRole.Create(Guid.Empty, _roleId));
    }

    [TestMethod]
    public void CreateUserRole_EmptyRoleId_ThrowsDomainException()
    {
        // Act & Assert
        Assert.ThrowsExactly<DomainException>(() => UserRole.Create(_userId, Guid.Empty));
    }
}

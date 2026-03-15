using CleanArchitecture.WebApi.Domain.Entities;
using CleanArchitecture.WebApi.Domain.Exceptions;

namespace CleanArchitecture.WebApi.Tests.Domain.Entities;

[TestClass]
public class RoleTests
{
    // Test data
    private readonly string _roleName = "Admin";

    [TestMethod]
    public void CreateRole_ValidName_ReturnsRoleObject()
    {
        // Act
        var role = Role.Create(_roleName);
        
        // Assert
        Assert.AreEqual(_roleName, role.Name);
    }

    [TestMethod]
    public void CreateRole_EmptyName_ThrowsDomainException()
    {
        // Act & Assert
        Assert.ThrowsExactly<DomainException>(() => Role.Create(""));
    }
}

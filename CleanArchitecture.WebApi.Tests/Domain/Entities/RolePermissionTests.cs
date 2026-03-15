using CleanArchitecture.WebApi.Domain.Entities;
using CleanArchitecture.WebApi.Domain.Exceptions;

namespace CleanArchitecture.WebApi.Tests.Domain.Entities;

[TestClass]
public class RolePermissionTests
{
    // Test data
    private readonly Guid _roleId = Guid.NewGuid();
    private readonly Guid _permissionId = Guid.NewGuid();


    [TestMethod]
    public void CreateRolePermission_ValidData_ReturnsRolePermissionObject()
    {
        // Act
        var rolePermission = RolePermission.Create(_roleId, _permissionId);
        
        // Assert
        Assert.AreEqual(_roleId, rolePermission.RoleId);
        Assert.AreEqual(_permissionId, rolePermission.PermissionId);
    }

    [TestMethod]
    public void CreateRolePermission_EmptyRoleId_ThrowsDomainException()
    {
        // Act & Assert
        Assert.ThrowsExactly<DomainException>(() => RolePermission.Create(Guid.Empty, _permissionId));
    }

    [TestMethod]
    public void CreateRolePermission_EmptyPermissionId_ThrowsDomainException()
    {
        // Act & Assert
        Assert.ThrowsExactly<DomainException>(() => RolePermission.Create(_roleId, Guid.Empty));
    }
}

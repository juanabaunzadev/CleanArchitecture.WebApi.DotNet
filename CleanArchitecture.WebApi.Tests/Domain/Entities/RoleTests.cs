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

    [TestMethod]
    public void SyncPermissions_AddsNewPermissions()
    {
        // Arrange
        var role = Role.Create(_roleName);
        var permissionId1 = Guid.NewGuid();
        var permissionId2 = Guid.NewGuid();

        // Act
        role.SyncPermissions([permissionId1, permissionId2]);

        // Assert
        Assert.HasCount(2, role.RolePermissions);
        Assert.IsTrue(role.RolePermissions.Any(rp => rp.PermissionId == permissionId1));
        Assert.IsTrue(role.RolePermissions.Any(rp => rp.PermissionId == permissionId2));
    }

    [TestMethod]
    public void SyncPermissions_RemovesPermissionsNotInList()
    {
        // Arrange
        var role = Role.Create(_roleName);
        var permissionId1 = Guid.NewGuid();
        var permissionId2 = Guid.NewGuid();
        role.SyncPermissions([permissionId1, permissionId2]);

        // Act — solo se mantiene permissionId1
        role.SyncPermissions([permissionId1]);

        // Assert
        Assert.HasCount(1, role.RolePermissions);
        Assert.IsTrue(role.RolePermissions.Any(rp => rp.PermissionId == permissionId1));
        Assert.IsFalse(role.RolePermissions.Any(rp => rp.PermissionId == permissionId2));
    }

    [TestMethod]
    public void SyncPermissions_EmptyList_RemovesAllPermissions()
    {
        // Arrange
        var role = Role.Create(_roleName);
        role.SyncPermissions([Guid.NewGuid(), Guid.NewGuid()]);

        // Act
        role.SyncPermissions([]);

        // Assert
        Assert.HasCount(0, role.RolePermissions);
    }

    [TestMethod]
    public void SyncPermissions_DoesNotDuplicateExistingPermissions()
    {
        // Arrange
        var role = Role.Create(_roleName);
        var permissionId = Guid.NewGuid();
        role.SyncPermissions([permissionId]);

        // Act — se llama dos veces con el mismo permiso
        role.SyncPermissions([permissionId]);

        // Assert
        Assert.HasCount(1, role.RolePermissions);
    }
}

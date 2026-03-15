using CleanArchitecture.WebApi.Domain.Entities;
using CleanArchitecture.WebApi.Domain.Exceptions;

namespace CleanArchitecture.WebApi.Tests.Domain.Entities;

[TestClass]
public class PermissionTests
{
    // Test data
    private readonly string _name = "Permission Test";
    private readonly string _description = "Permission Test Description";


    [TestMethod]
    public void CreatePermission_ValidData_ReturnsPermissionObject()
    {
        // Act
        var permission = Permission.Create(_name, _description);
        
        // Assert
        Assert.AreEqual(_name.ToLower(), permission.Name);
        Assert.AreEqual(_description, permission.Description);
    }

    [TestMethod]
    public void CreatePermission_EmptyName_ThrowsDomainException()
    {
        // Act & Assert
        Assert.ThrowsExactly<DomainException>(() => Permission.Create("", _description));
    }

    [TestMethod]
    public void CreatePermission_EmptyDescription_ThrowsDomainException()
    {
        // Act & Assert
        Assert.ThrowsExactly<DomainException>(() => Permission.Create(_name, ""));
    }
}

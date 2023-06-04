using FreeSql;
using FreeSql.DataAnnotations;

using System.ComponentModel;
#nullable enable
namespace BlazorCookieLogin.Entity;

[Description("角色权限关系表")]
public class TestRoleMenu : BaseEntity<TestRoleMenu, Guid>
{
    [Description("角色Id")]
    public int RoleId { get; set; }

    [Description("角色")]
    [Navigate(nameof(RoleId))]
    public TestRole? Role { get; set; }

    [Description("权限Id")]
    public int PermissionId { get; set; }

    [Description("权限")]
    [Navigate(nameof(PermissionId))]
    public TestMenu? Permission { get; set; }
}
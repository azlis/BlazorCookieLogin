using FreeSql;
using FreeSql.DataAnnotations;

using System.ComponentModel;
#nullable enable
namespace BlazorCookieLogin.Entity;

[Description("角色表")]
public class TestRole : BaseEntity<TestRole, int>
{
    [Description("角色名称")]
    public string? Name { get; set; }

    [Description("用户")]
    [Navigate(nameof(TestUser.RoleId))]
    public virtual ICollection<TestUser>? Users { get; set; }

    [Description("权限")]
    [Navigate(ManyToMany = typeof(TestRoleMenu))]
    public virtual ICollection<TestMenu>? Permissions { get; set; }
}
using FreeSql;
using FreeSql.DataAnnotations;

using System.ComponentModel;
#nullable enable
namespace BlazorCookieLogin.Entity;

[Description("用户信息表")]
public class TestUser : BaseEntity<TestUser, int>
{
    [Description("用户名")]
    public string? UserName { get; set; }

    [Description("密码")]
    public string? Password { get; set; }

    [Description("用户姓名")]
    public string? Name { get; set; }

    [Description("角色Id")]
    public int RoleId { get; set; }

    [Description("角色")]
    [Navigate(nameof(RoleId))]
    public TestRole? Role { get; set; }

    [Description("区域")]
    public string? Location { get; set; }

}
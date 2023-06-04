using FreeSql;
using FreeSql.DataAnnotations;

using System.ComponentModel;
#nullable enable
namespace BlazorCookieLogin.Entity;

[Description("菜单表")]
public class TestMenu : BaseEntity<TestMenu, int>
{
    /// <summary>
    /// 排序
    /// </summary>
    [Description("排序")]
    public int Seq { get; set; }

    /// <summary>
    /// 菜单名
    /// </summary>
    [Description("菜单名")]
    public string? Name { get; set; }

    /// <summary>
    /// 菜单图标
    /// </summary>
    [Description("菜单图标")]
    public string? Icon { get; set; }

    /// <summary>
    /// 对应页面Url
    /// </summary>
    [Description("对应页面Url")]
    public string? Url { get; set; }

    /// <summary>
    /// 父菜单ID
    /// </summary>
    [Description("父菜单ID")]
    public int ParentId { get; set; }

    [Navigate(nameof(ParentId))]
    public TestMenu? Parent { get; set; }

    [Navigate(nameof(ParentId))]
    public List<TestMenu>? Children { get; set; }

    /// <summary>
    /// 角色
    /// </summary>
    [Description("角色")]
    [Navigate(ManyToMany = typeof(TestRoleMenu))]
    public virtual ICollection<TestRole>? Roles { get; set; }
}
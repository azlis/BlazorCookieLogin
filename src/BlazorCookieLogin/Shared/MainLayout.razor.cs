using BootstrapBlazor.Components;

using BlazorCookieLogin.Data;
using BlazorCookieLogin.Entity;

using System.Security.Claims;

using Size = BootstrapBlazor.Components.Size;

namespace BlazorCookieLogin.Shared;

public partial class MainLayout
{
    private readonly string _title = "BlazorLogin";

    private List<MenuItem>? _menuItems;
    private TestUser? _user;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        var state = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var userName = state.User.FindFirst(ClaimTypes.Name);
        if (userName is not null)
        {
            _user = TestUser.Where(x => x.UserName == userName.Value).First();
            if (_user is not null)
            {
                _menuItems = CreateMenuItems(TestMenu.Where(x => x.Roles!.Any(y => y.Id == _user.RoleId)).OrderBy(x => x.Seq).ToList(), 0);
            }
        }
    }

    private List<MenuItem> CreateMenuItems(List<TestMenu> menus, int parentId)
    {
        var selectedMenus = new List<MenuItem>();
        var selectedMenuEntities = menus.Where(x => x.ParentId == parentId).ToList();
        foreach (var menuEntity in selectedMenuEntities)
        {
            var menuItem = new MenuItem(menuEntity.Name!, menuEntity.Url, menuEntity.Icon)
            {
                Items = CreateMenuItems(menus, menuEntity.Id)
            };
            selectedMenus.Add(menuItem);
        }

        return selectedMenus;
    }

    private async Task ChangePassword()
    {
        var option = new EditDialogOption<ChangePassword>
        {
            Title = "修改密码",
            Size = Size.Medium,
            Model = new ChangePassword(),
            ItemsPerRow = 1,
            RowType = RowType.Normal,
            OnEditAsync = async context =>
            {
                if (context.Model is not ChangePassword changePassword)
                {
                    await ToastService.Show(new ToastOption { Category = ToastCategory.Error, Title = "保存出错", Content = "类型转换错误" });
                    return false;
                }

                var oldPassword = changePassword.OldPassword;
                if (_user is not null)
                {
                    if (!await TestUser.Select.Where(x => x.Id == _user.Id && x.Password == oldPassword).AnyAsync())
                    {
                        await ToastService.Show(new ToastOption { Category = ToastCategory.Error, Title = "保存出错", Content = "原密码不正确，请检查原密码" });
                        return false;
                    }

                    var newPassword = changePassword.NewPassword;
                    _user.Password = newPassword;
                    await _user.SaveAsync();
                }

                return true;
            }
        };
        await DialogService.ShowEditDialog(option);
    }
}
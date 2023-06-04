using BlazorCookieLogin.Data;
using BlazorCookieLogin.Entity;

using FreeSql;

using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

builder.Services.AddBootstrapBlazor(null, options =>
{
    options.IgnoreLocalizerMissing = true;
});

builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
builder.Services.AddHttpContextAccessor();

var conn = builder.Configuration.GetConnectionString("sqlLite");
var freeSql = new FreeSqlBuilder()
    .UseAutoSyncStructure(true)
    .UseConnectionString(DataType.Sqlite, conn)
    .Build();

BaseEntity.Initialization(freeSql, null);

InitTestDb();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapDefaultControllerRoute();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

/// <summary>
/// 初始化一些数据库内容
/// </summary>
static void InitTestDb()
{
    if (!TestUser.Where(x => x.UserName == "admin").Any())
    {
        var user = new TestUser
        {
            UserName = "admin",
            Password = "123",
            Name = "管理员"
        };
        user.Save();

        var mlist = new List<TestMenu>();

        InitMenu(mlist, 0, "Home", "", "fa fa-home", 0);
        InitMenu(mlist, 1, "Settings", "settings", "fa fa-bars", 10);
        InitMenu(mlist, 2, "Counter", "counter", "fas fa-seedling", 20);
        InitMenu(mlist, 2, "FetchData", "fetchdata", "fas fa-seedling", 30);

        var adminRole = new TestRole
        {
            Name = "管理员",
            Users = new List<TestUser> { user },
            Permissions = mlist
        };

        adminRole.Save().SaveMany(nameof(TestRole.Users));
        adminRole.SaveMany(nameof(TestRole.Permissions));

    }
}

/// <summary>
/// 初始化菜单
/// </summary>
/// <param name="mlist"></param>
/// <param name="seq"></param>
/// <param name="name"></param>
/// <param name="url"></param>
/// <param name="ico"></param>
/// <param name="sort"></param>
static void InitMenu(List<TestMenu> mlist, int seq, string name, string url, string ico, int sort)
{
    var Menu = new TestMenu
    {
        Seq = seq,
        Name = name,
        Url = $"/{url}",
        Icon = ico,
        Sort = sort
    };
    Menu.Save();
    mlist.Add(Menu);
}
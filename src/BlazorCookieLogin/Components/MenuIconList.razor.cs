using BootstrapBlazor.Components;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

using System.Diagnostics.CodeAnalysis;

namespace BlazorCookieLogin.Components;

public partial class MenuIconList
{
    [Parameter]
    public string? Value { get; set; }

    [Parameter]
    public EventCallback<string?> ValueChanged { get; set; }

    [Parameter]
    public bool Open { get; set; }

    [Parameter]
    public EventCallback<bool> OpenChanged { get; set; }

    /// <summary>
    /// 获得/设置 IJSRuntime 实例
    /// </summary>
    [Inject]
    [NotNull]
    protected IJSRuntime? JSRuntime { get; set; }

    private string? IconDialogClassString => CssBuilder.Default("menu-icons")
        .AddClass("show", Open)
        .Build();

    private bool _init;

    /// <summary>
    /// OnAfterRenderAsync 方法
    /// </summary>
    /// <param name="firstRender"></param>
    /// <returns></returns>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (!_init && Open)
        {
            _init = true;
        }
    }

    private async Task OnCloseIconDialog()
    {
        Open = false;
        if (OpenChanged.HasDelegate)
        {
            await OpenChanged.InvokeAsync(Open);
        }
    }

    private async Task OnSelctedIcon()
    {
        if (ValueChanged.HasDelegate)
        {
            await ValueChanged.InvokeAsync(Value);
        }

        Open = false;
        if (OpenChanged.HasDelegate)
        {
            await OpenChanged.InvokeAsync(Open);
        }
    }
}
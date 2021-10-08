using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen.Blazor;
using System.Threading.Tasks;

namespace SF.SGL.UI.Shared
{
    public partial class MainLayoutComponent : LayoutComponentBase
    {
        protected RadzenBody body0;
        protected RadzenSidebar sidebar0;
        public bool DarkTheme { get; set; }

        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        protected async System.Threading.Tasks.Task SidebarToggle0Click(dynamic args)
        {
            await InvokeAsync(() => { sidebar0.Toggle(); });

            await InvokeAsync(() => { body0.Toggle(); });
        }

        protected async Task UpdateTheme()
        {
            //setting the themeName parameter
            string themeName = DarkTheme ? "dark" : "software";

            //calling JS function
            IJSObjectReference module = await JsRuntime.InvokeAsync<IJSObjectReference>("import", "./js/site.js");
            await module.InvokeVoidAsync("setTheme", themeName);

        }

    }
}
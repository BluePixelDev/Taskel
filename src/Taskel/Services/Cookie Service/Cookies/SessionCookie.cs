using Microsoft.JSInterop;

namespace CookieService
{
    internal class SessionCookie
    {
        private readonly IJSRuntime JSRuntime;
        public SessionCookie(IJSRuntime jSRuntime)
        {
            JSRuntime = jSRuntime;
        }

        public async Task<string> GetValue(string key, string def = "")
        {
            string value = await JSRuntime.InvokeAsync<string>("eval", $"window.sessionStorage.getItem(\"{key}\")");
            return string.IsNullOrEmpty( value ) ? def : value;
        }

        public async Task SetValue(string key, string value)
        {
            await JSRuntime.InvokeVoidAsync("eval", $"window.sessionStorage.setItem(\"{key}\", \"{value}\")");
        }
    }
}

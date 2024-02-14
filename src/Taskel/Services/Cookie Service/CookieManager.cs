using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop;

namespace CookieService
{
    /// <summary>
    /// Handles getting and setting of cookies and session cookies.
    /// </summary>
    public class CookieManager : Controller
    {
        private readonly Cookie cookie;
        private readonly SessionCookie sessionCookie;
        private readonly IJSRuntime jSRuntime;

        public CookieManager(IJSRuntime jS)
        {
            jSRuntime = jS ;
            cookie = new Cookie(jSRuntime);
            sessionCookie = new SessionCookie(jSRuntime);
        }

        #region SETTERS - COOKIE
        /// <summary>
        /// Sets string value of a cookie with specified key.
        /// </summary>
        public async void SetCookie(string key, string value, int? days = null)
        {
           await cookie.SetValue(key, value, days);
        }
        /// <summary>
        /// Sets int value of a cookie with specified key.
        /// </summary>
        public async void SetCookie(string key, int value, int? days = null)
        {
            await cookie.SetValue(key, value.ToString(), days);
        }
        /// <summary>
        /// Sets float value of a cookie with specified key.
        /// </summary>
        public async void SetCookie(string key, float value, int? days = null)
        {
            await cookie.SetValue(key, value.ToString(), days);
        }
        #endregion

        #region GETTERS - COOKIE
        /// <summary>
        /// Returns string cookie with specific key.
        /// </summary>
        /// <param name="def">Default return value</param>
        /// <returns>Result or default value.</returns>
        public async Task<string> GetCookieString(string key, string def = "")
        {
            return await cookie.GetValue(key, def);
        }
        /// <summary>
        /// Returns int cookie with specific key.
        /// </summary>
        /// <param name="def">Default return value</param>
        /// <returns>Result or default value.</returns>
        public async Task<int> GetCookieInt(string key, int def = 0)
        {      
            try
            {
                string content = await cookie.GetValue(key);
                return int.Parse(content);
            }
            catch(Exception ex)
            { 
                Console.WriteLine(ex.Message);
            }
            return def;
        }
        /// <summary>
        /// Returns float cookie with specific key.
        /// </summary>
        /// <param name="def">Default return value</param>
        /// <returns>Result or default value.</returns>
        public async Task<float> GetCookieFloat(string key, float def = 0)
        {
            try
            {
                string content = await cookie.GetValue(key);
                return float.Parse(content);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return def;
        }
        #endregion

        #region SETTERS - SESSION COOKIE
        /// <summary>
        /// Sets string value of a session cookie with specified key.
        /// </summary>
        public async void SetSessionCookie(string key, string value)
        {
            await sessionCookie.SetValue(key, value);
        }
        /// <summary>
        /// Sets int value of a session cookie with specified key.
        /// </summary>
        public async void SetSessionCookie(string key, int value)
        {
            await sessionCookie.SetValue(key, value.ToString());
        }
        /// <summary>
        /// Sets float value of a session cookie with specified key.
        /// </summary>
        public async void SetSessionCookie(string key, float value)
        {
            await sessionCookie.SetValue(key, value.ToString());
        }
        #endregion

        #region GETTERS - SESSION COOKIE
        /// <summary>
        /// Returns string session cookie with specific key.
        /// </summary>
        /// <param name="def">Default return value</param>
        /// <returns>Result or default value.</returns>
        public async Task<string> GetSessionCookieString(string key, string def = "")
        {
            return await sessionCookie.GetValue(key, def);
        }
        /// <summary>
        /// Returns int session cookie with specific key.
        /// </summary>
        /// <param name="def">Default return value</param>
        /// <returns>Result or default value.</returns>
        public async Task<int> GetSessionCookieInt(string key, int def = 0)
        {
            try
            {
                string content = await sessionCookie.GetValue(key);
                return int.Parse(content);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return def;
        }
        /// <summary>
        /// Returns float session cookie with specific key.
        /// </summary>
        /// <param name="def">Default return value</param>
        /// <returns>Result or default value.</returns>
        public async Task<float> GetSessionCookieFloat(string key, float def = 0)
        {
            try
            {
                string content = await sessionCookie.GetValue(key);
                return float.Parse(content);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return def;
        }
        #endregion
    }
}
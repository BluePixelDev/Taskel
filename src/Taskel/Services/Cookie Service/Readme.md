To use this service you first have to register CookieManager as scoped service 
inside Program.cs using this line:

builder.Services.AddScoped<CookieManager>();

ADDING COOKIES
Cookie manager supports three data types of cookies: string, int and float.

- NORMAL
To add normal cookies you have to invoke SetCookie("key", value) inside CookieManager.
You can also specify the cookies expiration time in days.

- SESSION
To add session cookies you have to invoke SetSessionCookie("key", value)
Session cookies doesn't have expiration time as they expire the moment user leaves the page.

------

GETTING COOKIES
All methods for getting cookies are async, so it's important to use them with await.

- NORMAL
In order to get normal cookies you have to invoke GetCookie[Data_type]("key") method.

-- Variations --
GetCookieString("key")
GetCookieInt("key")
GetCookieFloat("key")

- SESSION
In order to get session cookies you have to invoke GetSessionCookie[Data_type]("key") method.

-- Variations --
GetSessionCookieString("key")
GetSessionCookieInt("key")
GetSessionCookieFloat("key")
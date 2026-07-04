using WebObrasci1.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using WebObrasci1.Services;
using WebObrasci1.Settings;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.Cookies;
using WebObrasci1;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables("WebObrasci_");

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<AppDbContext>();

var authSettings = builder.Configuration.GetSection("AuthSettings");
var authUrl = authSettings.GetValue<string>("AuthUrl");
var returnUrl = authSettings.GetValue<string>("ReturnUrl");
var clientId = authSettings.GetValue<string>("ClientId");
var clientSecret = authSettings.GetValue<string>("ClientSecret");
var scopes = authSettings.GetSection("Scopes").Get<IEnumerable<string>>()?.ToList() ?? [];
var requireHttpsMetadata = authSettings.GetValue<bool>("RequireHttpsMetadata");

// Add authentication and OpenIdConnect
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = "oidc";
})
.AddCookie()
.AddOpenIdConnect("oidc", options =>
{
    options.Authority = authUrl;
    if (string.IsNullOrEmpty(returnUrl) == false)
    {
        options.ReturnUrlParameter = returnUrl;
        options.AccessDeniedPath = "/";
    }
    options.ClientId = clientId;
    options.ClientSecret = clientSecret;
    options.ResponseType = OpenIdConnectResponseType.Code;
    options.SaveTokens = true;
    //options.SaveTokens = false;
    options.RequireHttpsMetadata = requireHttpsMetadata;

    options.Scope.Add("offline_access");
    options.Scope.Add("openid");
    foreach (var scope in scopes)
        options.Scope.Add(scope);

    options.GetClaimsFromUserInfoEndpoint = true;

    options.Events = new OpenIdConnectEvents
    {
        OnTokenResponseReceived = context =>
        {
            return Task.CompletedTask;
        },
        OnUserInformationReceived = AuthorizationHelper.AppendUserInfoToPrincipalAsync,
    };
});

builder.Services.AddOptions<UserSettings>()
    .Bind(builder.Configuration.GetSection("UserSettings"));

builder.Services.AddScoped<IClaimsTransformation, RoleClaimsTransformer>();
builder.Services.AddScoped<IUserHelper, UserHelper>();



var app = builder.Build();

using (var s = app.Services.CreateScope())
using (var ctx = s.ServiceProvider.GetRequiredService<AppDbContext>())
{
    ctx.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

//Nisam siguran koji logout nacin je bolji
/*app.MapGet("/logout", async context =>
{
    await context.SignOutAsync("Cookies");

    var callbackUrl = "https://localhost:7120/";

    var keycloakLogoutUrl =
        "http://localhost:5002/realms/Test/protocol/openid-connect/logout" +
        "?redirect_uri=" + Uri.EscapeDataString(callbackUrl);

    context.Response.Redirect(keycloakLogoutUrl);
});*/


app.MapGet("/logout", async context =>
{
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    await context.SignOutAsync("oidc");
});
app.MapGet("/login", async context =>
{
    var ap = new AuthenticationProperties { RedirectUri = "https://localhost:7120/" };
    await context.ChallengeAsync(ap);
});

app.MapRazorPages();

app.Run();
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Globalization;
using WebObrasci1;
using WebObrasci1.Data;
using WebObrasci1.Services;
using WebObrasci1.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables("WebObrasci_");

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<AppDbContext>();

var authSettingsSection = builder.Configuration.GetSection("AuthSettings");
var authSettings = authSettingsSection.Get<AuthSettings>() ?? new AuthSettings();

var userSettingsSection = builder.Configuration.GetSection("UserSettings");
var userSettings = userSettingsSection.Get<UserSettings>() ?? new UserSettings();

// Add authentication and OpenIdConnect


builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = "oidc";
})
.AddCookie()
.AddOpenIdConnect("oidc", options =>
{
    options.Authority = authSettings.AuthUrl;
    if (string.IsNullOrEmpty(authSettings.ReturnUrl) == false)
    {
        options.ReturnUrlParameter = authSettings.ReturnUrl;
        options.AccessDeniedPath = "/";
    }
    options.ClientId = authSettings.ClientId;
    options.ClientSecret = authSettings.ClientSecret;
    options.ResponseType = OpenIdConnectResponseType.Code;
    options.SaveTokens = true;
    options.RequireHttpsMetadata = authSettings.RequireHttpsMetadata;

    options.Scope.Add("offline_access");
    options.Scope.Add("openid");
    foreach (var scope in authSettings.Scopes)
        options.Scope.Add(scope);

    if (string.IsNullOrEmpty(userSettings.UsernameClaim) == false)
        options.TokenValidationParameters.NameClaimType = userSettings.UsernameClaim;

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
builder.Services.AddScoped<IPdfConverter, PdfConverter>();

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
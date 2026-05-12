using WebObrasci1.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args); 

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<AppDbContext>();

// Add authentication and OpenIdConnect
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc";
})
.AddCookie()
.AddOpenIdConnect("oidc", options =>
{
    options.Authority = "http://localhost:5002/realms/Test";
    options.ClientId = "WebObrasci1";
    options.ClientSecret = "RVCPT1iPP8dMILHiKmmYiXpJ6cpBelsA";
    options.ResponseType = "code";
    options.SaveTokens = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters.RoleClaimType = "roles";
});

var app = builder.Build();

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

app.MapGet("/logout", async context =>
{
    await context.SignOutAsync("Cookies");
    await context.SignOutAsync("oidc");
});

app.MapRazorPages();

app.Run();
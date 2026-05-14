using WebObrasci1.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;
using WebObrasci1.Models;
using WebObrasci1.Services;

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
    //options.TokenValidationParameters.RoleClaimType = "roles";

    options.Events = new OpenIdConnectEvents
    {
        OnTokenValidated = async context =>
        {
            var db = context.HttpContext.RequestServices
                .GetRequiredService<AppDbContext>();

            var principal = context.Principal;

            if (principal == null)
                return;

            // Keycloak unique user ID
            var externalId = principal
                .FindFirst("sub")?.Value;

            if (string.IsNullOrWhiteSpace(externalId))
                return;

            // Keycloak username
            var username = principal
                .FindFirst("preferred_username")?.Value;

            // Keycloak email
            var email = principal
                .FindFirst("email")?.Value;

            // Check if user already exists
            var user = await db.Users
                .Include(x => x.UserRoles)
                .FirstOrDefaultAsync(x => x.ExternalId == externalId);

            // CREATE USER ON FIRST LOGIN
            if (user == null)
            {
                user = new User
                {
                    ExternalId = externalId,
                    UserName = username ?? "",
                    Email = email ?? ""
                };

                db.Users.Add(user);

                // Assign default role
                var studentRole = await db.Roles
                    .FirstOrDefaultAsync(x => x.Name == "Student");

                if (studentRole != null)
                {
                    user.UserRoles.Add(new UserRole
                    {
                        RoleId = studentRole.Id
                    });
                }
            }
            else
            {
                // Update user info on login
                user.UserName = username ?? user.UserName;
                user.Email = email ?? user.Email;
            }

            await db.SaveChangesAsync();
        }
    };
});
builder.Services.AddScoped<IClaimsTransformation,
    DbRoleClaimsTransformer>();



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
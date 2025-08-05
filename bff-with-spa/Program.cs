
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Identity.Web;
using System.IdentityModel.Tokens.Jwt;

namespace bffwithspa
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Use HTTPS
                options.Cookie.SameSite = SameSiteMode.None; // For cross-origin requests
                options.ExpireTimeSpan = TimeSpan.FromHours(24);
                options.SlidingExpiration = true;
            })
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                builder.Configuration.Bind("AzureAd", options); // Bind Azure AD settings from appsettings.json
                options.Authority = $"{builder.Configuration["AzureAd:Instance"]}{builder.Configuration["AzureAd:TenantId"]}";
                options.ResponseType = "code"; // Use authorization code flow
                options.UsePkce = true; // Enable PKCE
                options.SaveTokens = false; // Do not save tokens in cookies
                options.GetClaimsFromUserInfoEndpoint = true;
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("email");

                // Ensure roles are included in the claims
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    RoleClaimType = "roles" // Azure AD includes roles in the "roles" claim
                };

                options.Events = new OpenIdConnectEvents
                {
                    OnTokenValidated = context =>
                    {
                        var claimsIdentity = context.Principal?.Identity as System.Security.Claims.ClaimsIdentity;

                        // Add role claims to the identity
                        var roleClaims = context.Principal?.FindAll("roles");
                        claimsIdentity?.AddClaims(roleClaims!);

                        // Extract the access token
                        var accessToken = context.SecurityToken as JwtSecurityToken;

                        // Store the access token securely (e.g., in memory or a distributed cache)
                        var userId = context.Principal?.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value; // User's unique ID
                        if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(accessToken.RawData))
                        {
                            // Example: Store in-memory or in a distributed cache like Redis
                            TokenStore.SaveAccessToken(userId, accessToken.RawData);
                        }

                        return Task.CompletedTask;
                    }
                };
            });

            builder.Services.AddAuthorization();
            builder.Services.AddControllers(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();
                options.AddPolicy("AllowFrontend",
                    builder =>
                    {
                        builder.WithOrigins(allowedOrigins!)
                               .AllowCredentials()
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });

            builder.Logging.AddConsole();
            builder.Logging.SetMinimumLevel(LogLevel.Debug);

            var app = builder.Build();



            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors("AllowFrontend");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.MapFallbackToFile("/spa/index.html");

            app.Run();
        }
    }
}

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace BlogApi.Extensions;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddCognitoAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var cognitoAuthority = $"https://cognito-idp.{configuration["Aws:Region"]}.amazonaws.com/{configuration["Aws:Cognito:UserPoolId"]}";

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = cognitoAuthority;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = cognitoAuthority,
                    ValidateLifetime = true,
                    ValidateAudience = false
                };
            });

        services.AddAuthorization();

        return services;
    }
}

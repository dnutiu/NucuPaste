using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NucuPaste.Api.Auth;

namespace NucuPaste.Api.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddJwt(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var options = new JwtOptions();
            var section = configuration.GetSection("jwt");
            section.Bind(options);

            serviceCollection.Configure<JwtOptions>(section);
            serviceCollection.AddSingleton<IJwtHandler, JwtHandler>();
            serviceCollection.AddAuthentication().AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = false,
                    ValidIssuer = options.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SecretKey))
                };
            });
        }
    }
}
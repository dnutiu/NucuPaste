using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace NucuPaste.Api.Auth
{
    public class JwtHandler : IJwtHandler
    {
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        private readonly JwtOptions _options;
        private readonly SecurityKey _securityKey;
        private readonly SigningCredentials _signingCredentials;
        private readonly JwtHeader _jwtHeader;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public JwtHandler(IOptions<JwtOptions> options)
        {
            _options = options.Value;
            _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
            Encoding.UTF8.GetBytes(_options.SecretKey);

            _signingCredentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256);
            _jwtHeader = new JwtHeader(_signingCredentials);
            _tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidIssuer = _options.Issuer,
                IssuerSigningKey = _securityKey
            };
        }

        public NucuPasteJsonWebToken Create(Guid userId)
        {
            var utcNow = DateTime.UtcNow;
            var expires = utcNow.AddMinutes(_options.ExpiryMinutes);
            var epoch = new DateTime(1970, 1, 1).ToUniversalTime();
            var exp = (long) new TimeSpan(expires.Ticks - epoch.Ticks).TotalSeconds;
            var now = (long) new TimeSpan(utcNow.Ticks - epoch.Ticks).TotalSeconds;
            var payload = new JwtPayload
            {
                {"user_id", userId},
                {"issuer", _options.Issuer},
                {"issued_at", now},
                {"expires_in", exp}
                
            };
            var jwt = new JwtSecurityToken(_jwtHeader, payload);
            var token = _jwtSecurityTokenHandler.WriteToken(jwt);
            return new NucuPasteJsonWebToken
            {
                Token = token,
                Expires = exp
            };
        }
    }
}
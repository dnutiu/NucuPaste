namespace NucuPaste.Api.Auth
{
    public class JwtOptions
    {
        public string SecretKey { get; set; }
        public double ExpiryMinutes { get; set; }
        public string Issuer { get; set; }
    }
}
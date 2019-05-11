namespace NucuPaste.Api.Services
{
    public interface IEncrypt
    {
        string GetSalt(string value);

        string GetHash(string value, string salt);
    }
}
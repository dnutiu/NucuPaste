using System;

namespace NucuPaste.Api.Auth
{
    public interface IJwtHandler
    {
        NucuPasteJsonWebToken Create(Guid userId);
    }
}
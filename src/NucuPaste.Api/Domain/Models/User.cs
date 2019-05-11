using System;
using System.Globalization;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Manage.Internal;
using NucuPaste.Api.Services;

// ReSharper disable MemberCanBePrivate.Global

namespace NucuPaste.Api.Domain.Models
{
    public class User
    {
        public Guid Id { get; protected set; }
        public string Username { get; protected set; }
        public string Email { get; protected set; }
        public string Password { get; protected set; }
        public string PasswordSalt { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public bool IsDeleted { get; protected set; } = false;

        protected User()
        {
            
        }

        public User(string email, string username)
        {
            Id = Guid.NewGuid();
            Email = email;
            Username = username;
            CreatedAt = DateTime.UtcNow;
            IsDeleted = false;
        }
        
        public User(string email, string username, string password, IEncrypt encryptService)
        {
            Id = Guid.NewGuid();
            Email = email;
            Username = username;
            CreatedAt = DateTime.UtcNow;
            IsDeleted = false;
            SetPassword(password, encryptService);
        }

        public void SetPassword(string password, IEncrypt encryptService)
        {
            PasswordSalt = encryptService.GetSalt(password);
            Password = encryptService.GetHash(password, PasswordSalt);
        }

        public bool ValidatePassword(string password, IEncrypt encryptService)
        {
            return Password.Equals(encryptService.GetHash(password, PasswordSalt));
        }
    }
}
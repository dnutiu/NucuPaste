using System;
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

        public User(string email, string username, string password)
        {
            Id = Guid.NewGuid();
            Email = email;
            Username = username;
            Password = password;
            CreatedAt = DateTime.Now;
            // Todo password salt.
            IsDeleted = false;
        }
    }
}
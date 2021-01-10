using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Udemy.NetCore5.Angular.Data.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public string Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string KnownAs { get; set; }

        public DateTime UserCreated { get; set; } = DateTime.UtcNow;

        public DateTime LastActive { get; set; } = DateTime.UtcNow;

        public string Introduction { get; set; }

        public string LookingFor { get; set; }

        public string Interests { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public ICollection<Photo> Photos { get; set; }

        public ICollection<AppUserLike> LikedByUsers { get; set; }

        public ICollection<AppUserLike> LikedUsers { get; set; }

        public ICollection<Message> MessagesSent { get; set; }

        public ICollection<Message> MessagesReceived { get; set; }

        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}
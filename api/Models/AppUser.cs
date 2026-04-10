using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace api.Models
{
    public class AppUser : IdentityUser
    {
        /*
        Id — Unique identifier (string)
        UserName — User login name
        NormalizedUserName — Normalized username (for lookups)
        Email — Email address
        NormalizedEmail — Normalized email (for lookups)
        EmailConfirmed — Whether email is verified
        PasswordHash — Hashed password
        SecurityStamp — Used for invalidating sessions
        ConcurrencyStamp — For optimistic concurrency
        PhoneNumber — User's phone number
        PhoneNumberConfirmed — Whether phone is verified
        TwoFactorEnabled — Whether 2FA is enabled
        LockoutEnd — Datetime when lockout expires (nullable)
        LockoutEnabled — Whether account can be locked
        AccessFailedCount — Failed login attempts
        */
    }
}
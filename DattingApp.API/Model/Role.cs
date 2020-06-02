using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace DattingApp.API.Model
{
    public class Role : IdentityRole<int>
    {
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
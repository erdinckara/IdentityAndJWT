using System;
using Microsoft.AspNetCore.Identity;

namespace IdentityAndJWT.CSUI.Model
{
    public class User : IdentityUser<int>
    {
        public DateTime DateOfBirth { get; set; }
        public string Bio { get; set; }
    }
}
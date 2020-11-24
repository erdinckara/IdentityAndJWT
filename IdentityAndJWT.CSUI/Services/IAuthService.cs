using System.Threading.Tasks;
using IdentityAndJWT.CSUI.Model;
using Microsoft.AspNetCore.Identity;

namespace IdentityAndJWT.CSUI.Services
{
    public interface IAuthService
    {
        Task<IdentityResult> Register(User user, string password);


        Task<SignInResult> Login(User user, string password);


        string GetToken(User user);

    }
}
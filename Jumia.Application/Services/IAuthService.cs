using Jumia.Dtos.ViewModel;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia.Application.Services
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterUserAsync(RegisterViewModel model);
        //Task<SignInResult> SignInAsync(string userName, string password, bool isPersistent = false, bool lockoutOnFailure = false);
         Task LogoutAsync();
        Task<SignInResult> SignInAsync(LoginViewModel model);


    }
}

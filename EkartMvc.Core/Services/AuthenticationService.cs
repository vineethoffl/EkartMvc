using EkartMvc.Repositories.Repositories;
using EkartMvc.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace Ekart.Core.Services
{
    // IAuthenticationService.cs
    public interface IAuthenticationService
    {
        void SignIn(User user);
        void SignOut();
        User GetAuthenticatedUser();
    }


    // AuthenticationService.cs
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IUserRepository userRepository;

        public AuthenticationService(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public void SignIn(User user)
        {
            ClaimsIdentity identity = new ClaimsIdentity(new[] {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            // Add other claims as needed
        }, CookieAuthenticationDefaults.AuthenticationScheme);

            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            AuthenticationProperties authenticationProperties = new AuthenticationProperties
            {
                IsPersistent = true, // Set to false if you want a session cookie
                ExpiresUtc = DateTime.UtcNow.AddDays(7) // Set the expiration time
            };

            httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authenticationProperties);
        }

        public void SignOut()
        {
            httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public User GetAuthenticatedUser()
        {
            ClaimsPrincipal principal = httpContextAccessor.HttpContext.User;
            string username = principal.Identity?.Name;

            if (!string.IsNullOrEmpty(username))
            {
                return userRepository.GetUserByUsername(username);
            }

            return null;
        }
    }

}

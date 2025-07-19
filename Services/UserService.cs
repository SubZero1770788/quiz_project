using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using quiz_project.Common;
using quiz_project.Entities;
using quiz_project.Extensions;
using quiz_project.Interfaces;
using quiz_project.ViewModels;

namespace quiz_project.Services
{
    public class UserService(UserManager<User> userManager, RoleManager<Role> roleManager,
                    IUserMapper userMapper, SignInManager<User> signInManager, IUserRepository userRepository) : IUserService
    {
        public async Task<List<ActiveUserViewModel>> GetAllActiveUsersAsync()
        {
            var users = await userManager.Users.Where(u => u.IsLoggedIn == true).ToListAsync();
            var scores = await userRepository.GetTotalScoresAsync();
            List<ActiveUserViewModel> activeUserViewModels = [];
            users.ForEach(u =>
            {
                var userScore = scores.GetValueOrDefault(u.Id, 0);
                var activeUsers = userMapper.ToActiveUserViewModel(u, userScore);
                activeUserViewModels.Add(activeUsers);
            });

            return activeUserViewModels;
        }

        public async Task<(bool, string error)> LoginAsync(LoginViewModel loginViewModel)
        {
            var user = await userManager.FindByNameAsync(loginViewModel.UserName);
            string error;
            if (user is null)
            {
                error = ErrorMessagesExtension.GetMessage(ErrorMessages.InvalidLoginAttempt);
                return (false, error);
            }

            var res = await signInManager.PasswordSignInAsync(loginViewModel.UserName, loginViewModel.Password, isPersistent: false, lockoutOnFailure: true);

            if (res.IsLockedOut)
            {
                error = ErrorMessagesExtension.GetMessage(ErrorMessages.AccountLocked);
                return (false, error);
            }

            else if (res.Succeeded)
            {
                return (true, string.Empty);
            }

            await userManager.AccessFailedAsync(user);

            var accessfailed = await userManager.GetAccessFailedCountAsync(user);
            var maxFailedAttempts = userManager.Options.Lockout.MaxFailedAccessAttempts;
            int leftAttempts = maxFailedAttempts - accessfailed;

            error = $"Invalid login attempt. {leftAttempts} left before account lockout.";
            return (false, error);
        }

        public async Task<bool> LogoutAsync(ClaimsPrincipal principal)
        {
            var user = principal;
            if (user is not null)
            {
                var userEntity = await userManager.FindByNameAsync(user.Identity.Name);
                await signInManager.SignOutAsync();
                return true;
            }
            return false;
        }

        public async Task<(bool, string error)> RegisterAsync(RegisterViewModel registerViewModel)
        {
            var userNameTaken = await userManager.FindByNameAsync(registerViewModel.UserName);
            if (userNameTaken is not null) return (false, "The username is already taken");

            var user = userMapper.ToEntity(registerViewModel);

            var created = await userManager.CreateAsync(user, registerViewModel.Password);
            if (created.Succeeded)
            {
                var defaultRole = await roleManager.FindByNameAsync("User");

                await userManager.AddToRoleAsync(user, defaultRole!.Name!);
                await userManager.UpdateAsync(user);
                await signInManager.SignInAsync(user, isPersistent: false);
                return (true, String.Empty);

            }

            return (false, "An error has occured");

        }
    }
}
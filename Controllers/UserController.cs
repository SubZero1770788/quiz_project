using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using quiz_project.Common;
using quiz_project.Database;
using quiz_project.Entities;
using quiz_project.Extensions;
using quiz_project.Interfaces;
using quiz_project.Models;

namespace quiz_project.Controllers
{
    public class UserController(QuizDb context, SignInManager<User> signInManager,
        UserManager<User> userManager, RoleManager<Role> roleManager) : Controller
    {
        [HttpGet]
        public async Task<ViewResult> Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<ViewResult> GetAllUsers()
        {
            var users = await userManager.Users.ToListAsync();
            return View();
        }

        [HttpGet]
        public async Task<ViewResult> GetAllActiveUsers()
        {
            var users = await userManager.Users.Where(u => u.IsLoggedIn == true).ToListAsync();
            return View();
        }

        [HttpGet]
        public async Task<ViewResult> AccountSettings()
        {
            return View();
        }

        [HttpGet]
        public ViewResult Register()
        {
            return View();
        }

        // This is a redirect after POST in order to combat the page refresh issue when submitting data in MVC
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var userNameTaken = await userManager.FindByNameAsync(registerViewModel.UserName);
                if (userNameTaken is not null)
                {
                    ModelState.AddModelError(string.Empty, "The username is already taken");
                    return View(registerViewModel);
                }

                var user = new User()
                {
                    UserName = registerViewModel.UserName,
                    Email = registerViewModel.Email
                };

                var created = await userManager.CreateAsync(user, registerViewModel.Password);
                if (created.Succeeded)
                {
                    var defaultRole = await roleManager.FindByNameAsync("User");

                    await userManager.AddToRoleAsync(user, defaultRole!.Name!);
                    user.IsLoggedIn = true;
                    await userManager.UpdateAsync(user);
                    await context.SaveChangesAsync();
                    await signInManager.SignInAsync(user, isPersistent: false);
                    HttpContext.Session.SetString("UserName", user.UserName);

                    return RedirectToAction("Index", "Quiz");
                }

                foreach (var error in created.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

            }

            return View(registerViewModel);
        }

        [HttpGet]
        public ViewResult Login()
        {
            return View();
        }

        // This is a redirect after POST in order to combat the page refresh issue when submitting data in MVC
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(loginViewModel.UserName);
                if (user is null)
                {
                    ModelState.AddModelError(string.Empty, ErrorMessagesExtension.GetMessage(ErrorMessages.InvalidLoginAttempt));
                    return View(loginViewModel);
                }

                var res = await signInManager.PasswordSignInAsync(loginViewModel.UserName, loginViewModel.Password, isPersistent: false, lockoutOnFailure: true);

                if (res.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, ErrorMessagesExtension.GetMessage(ErrorMessages.AccountLocked));
                    return View(loginViewModel);
                }

                else if (res.Succeeded)
                {
                    user.IsLoggedIn = true;
                    await userManager.UpdateAsync(user);
                    await context.SaveChangesAsync();
                    HttpContext.Session.SetString("UserName", loginViewModel.UserName);
                    return RedirectToAction("Index", "Quiz");
                }

                await userManager.AccessFailedAsync(user);

                var accessfailed = await userManager.GetAccessFailedCountAsync(user);
                var maxFailedAttempts = userManager.Options.Lockout.MaxFailedAccessAttempts;
                int leftAttempts = maxFailedAttempts - accessfailed;

                ModelState.AddModelError(string.Empty, $"Invalid login attempt. {leftAttempts} left before account lockout.");
                return View(loginViewModel);
            }

            return View(loginViewModel);
        }

        [HttpGet]
        public async Task<RedirectToActionResult> Logout()
        {
            var username = HttpContext.Session.GetString("UserName");
            if (username is not null)
            {
                var user = await userManager.FindByNameAsync(username!);
                await signInManager.SignOutAsync();
                user.IsLoggedIn = false;
                await userManager.UpdateAsync(user);
                await context.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
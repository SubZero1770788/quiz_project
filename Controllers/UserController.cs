using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using quiz_project.Entities;
using quiz_project.Interfaces;
using quiz_project.Models;

namespace quiz_project.Controllers
{
    public class UserController(SignInManager<User> signInManager,
        UserManager<User> userManager) : Controller
    {
        [HttpGet]
        public async Task<ViewResult> Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<ViewResult> GetAllUsers()
        {
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
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
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

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(loginViewModel.UserName);
                if (user is null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(loginViewModel);
                }

                var res = await signInManager.PasswordSignInAsync(loginViewModel.UserName, loginViewModel.Password, isPersistent: false, lockoutOnFailure: true);

                if (res.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "The account got locked out after multiple unsuccessful login attempts");
                    return View(loginViewModel);
                }

                else if (res.Succeeded)
                    return RedirectToAction("Index", "Home");

                await userManager.AccessFailedAsync(user);

                var accessfailed = await userManager.GetAccessFailedCountAsync(user);
                var maxFailedAttempts = userManager.Options.Lockout.MaxFailedAccessAttempts;
                int leftAttempts = maxFailedAttempts - accessfailed;

                ModelState.AddModelError(string.Empty, $"Invalid login attempt. {leftAttempts} left before account lockout.");
                return View(loginViewModel);
            }

            return View(loginViewModel);
        }
    }
}
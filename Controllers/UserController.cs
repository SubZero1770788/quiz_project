using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using quiz_project.Entities;
using quiz_project.Interfaces;

namespace quiz_project.Controllers
{
    public class UserController(SignInManager<User> signInManager,
        UserManager<User> userManager, RoleManager<User> roleManager) : Controller
    {
        public async Task<ViewResult> Index()
        {
            return View();
        }
        public async Task<ViewResult> GetAllUsers()
        {
            var users = userManager.Users;
            return View();
        }

        public async Task<ViewResult> AccountSettings()
        {
            var users = userManager.Users;
            return View();
        }
    }
}
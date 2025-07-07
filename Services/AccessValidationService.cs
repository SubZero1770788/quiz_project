using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using quiz_project.Entities;
using quiz_project.Entities.Repositories;
using quiz_project.Interfaces;

namespace quiz_project.Services
{
    public class AccessValidationService(UserManager<User> userManager, IQuizRepository quizRepository) : IAccessValidationService
    {
        public async Task<(User? user, IActionResult? redirect)> GetCurrentUserOrRedirect(Controller controller)
        {
            var username = controller.HttpContext.Session.GetString("UserName");

            if (username is null)
            {
                controller.ModelState.AddModelError(string.Empty, "Something's wrong with your session - please clear cache");
                return (null, controller.RedirectToAction("Index", "Quiz"));
            }

            var user = await userManager.FindByNameAsync(username!);

            if (user is null)
            {
                controller.ModelState.AddModelError(string.Empty, "The user does not exist ?");
                return (null, controller.RedirectToAction("User", "Logout"));
            }

            return (user, null);
        }

        public async Task<(Quiz? quiz, IActionResult? redirect)> GetUserOwnsQuiz(int id, User user, Controller controller)
        {
            var quiz = await quizRepository.GetQuizByIdAsync(id);

            if (quiz.UserId != user!.Id)
            {
                controller.ModelState.AddModelError(String.Empty, "This quiz does not belong to you");
                return (null, controller.RedirectToAction("Index"));
            }

            return (quiz, null);
        }


    }
}
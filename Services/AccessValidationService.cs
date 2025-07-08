using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using quiz_project.Common;
using quiz_project.Entities;
using quiz_project.Entities.Repositories;
using quiz_project.Extensions;
using quiz_project.Interfaces;
using quiz_project.Models;

namespace quiz_project.Services
{
    public class AccessValidationService(UserManager<User> userManager, IQuizRepository quizRepository) : IAccessValidationService
    {
        public async Task<(bool? has, IActionResult? redirect)> EachQuestionHasAnswer(QuizViewModel quizViewModel, Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary ModelState, Controller controller)
        {
            for (int i = 0; i < quizViewModel.Questions.Count; i++)
            {
                var qvm = quizViewModel.Questions[i];

                if (!qvm.Answers.Any(a => a.IsCorrect))
                {
                    ModelState.AddModelError($"Questions[{i}].Answers", $"Question {i + 1} must have at least one correct answer.");
                    return (null, controller.RedirectToAction("Edit", "Quiz"));
                }
            }
            return (true, controller.RedirectToAction("Index", "Quiz"));
        }

        public async Task<(User? user, IActionResult? redirect)> GetCurrentUserOrRedirect(Controller controller)
        {
            var username = controller.HttpContext.Session.GetString("UserName");

            if (username is null)
            {
                controller.ModelState.AddModelError(string.Empty, ErrorMessagesExtension.GetMessage(ErrorMessages.WrongSession));
                return (null, controller.RedirectToAction("Index", "Quiz"));
            }

            var user = await userManager.FindByNameAsync(username!);

            if (user is null)
            {
                controller.ModelState.AddModelError(string.Empty, ErrorMessagesExtension.GetMessage(ErrorMessages.UserNotExisting));
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
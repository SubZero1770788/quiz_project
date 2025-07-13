using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using quiz_project.Entities;
using quiz_project.Interfaces;
using quiz_project.Models;

namespace quiz_project.Controllers
{
    public class MenuController(IQuizRepository quizRepository, IAccessValidationService accessValidationService) : Controller
    {
        [HttpGet, ActionName("Browse")]
        public async Task<IActionResult> BrowsePublicQuizzes()
        {
            var (user, redirect) = await accessValidationService.GetCurrentUserOrRedirect(this);
            if (user is null) return redirect!;

            var quizes = await quizRepository.GetPublicQuizzes();

            var quizViewModels = new List<QuizViewModel>();
            foreach (Quiz q in quizes)
            {
                var quizViewModel = new QuizViewModel
                {
                    QuizId = q!.QuizId,
                    Title = q.Title,
                    Description = q.Description,
                    Questions = q.Questions.Select(qvm => new QuestionViewModel
                    {
                        QuestionScore = qvm.QuestionScore,
                        Description = qvm.Description,
                        Answers = qvm.Answers.Select(avm => new AnswerViewModel
                        {
                            Description = avm.Description,
                            IsCorrect = avm.IsCorrect
                        }).ToList()
                    }).ToList()
                };
                quizViewModels.Add(quizViewModel);
            }

            return View(quizViewModels);
        }

    }
}
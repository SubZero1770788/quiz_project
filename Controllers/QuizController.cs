using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using quiz_project.Database;
using quiz_project.Database.Migrations;
using quiz_project.Entities;
using quiz_project.Entities.Repositories;
using quiz_project.Interfaces;
using quiz_project.Models;

namespace quiz_project.Controllers
{
    public class QuizController(IQuizRepository quizRepository, UserManager<User> userManager) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var (user, redirect) = await GetCurrentUserOrRedirect();
            if (redirect is not null) return redirect;

            var quizes = await quizRepository.GetQuizesByUserAsync(user!);

            var quizModels = new List<QuizViewModel>();
            foreach (Quiz q in quizes)
            {
                quizModels.Add(new QuizViewModel
                {
                    QuizId = q.QuizId,
                    Title = q.Title,
                    Description = q.Description,
                });
            }

            return View(quizModels);
        }

        public async Task<IActionResult> Getquiz(int quizId)
        {
            var (user, redirect) = await GetCurrentUserOrRedirect();
            if (user is null) return redirect!;

            var quiz = await quizRepository.GetQuizByIdAsync(quizId);
            var quizModel = new QuizViewModel
            {
                Title = quiz.Title,
                Description = quiz.Description
            };

            return View(quizModel);
        }

        [HttpGet, ActionName("Create")]
        public async Task<IActionResult> CreateNewQuizAsync()
        {
            var (user, redirect) = await GetCurrentUserOrRedirect();
            if (user is null) return redirect!;

            if (user is null)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> CreateNewQuizAsync(QuizViewModel quizViewModel)
        {
            if (ModelState.IsValid)
            {
                var (user, redirect) = await GetCurrentUserOrRedirect();
                if (user is null) return redirect!;

                var quiz = new Quiz
                {
                    Title = quizViewModel.Title,
                    Description = quizViewModel.Description,
                    UserId = user!.Id,
                    Questions = quizViewModel.Questions.Select(qvm => new Question
                    {
                        QuestionScore = qvm.QuestionScore,
                        Description = qvm.Description,
                        Answers = qvm.Answers.Select(avm => new Answer
                        {
                            Description = avm.Description,
                            IsCorrect = avm.IsCorrect
                        }).ToList()
                    }).ToList()
                };

                try
                {
                    await quizRepository.CreateQuizAsync(quiz);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(String.Empty, $"Something went wrong: {e}");
                }

                return RedirectToAction("Index");
            }
            // After adding did not work
            return View(quizViewModel);
        }

        [HttpGet, ActionName("Edit")]
        public async Task<IActionResult> EditQuizAsync(int quizId)
        {
            var (user, redirect) = await GetCurrentUserOrRedirect();
            if (user is null) return redirect!;
            var (quiz, quizRedirect) = await GetUserOwnsQuiz(quizId, user!);
            if (quiz is null) return quizRedirect!;

            var quizViewModel = new QuizViewModel
            {
                QuizId = quiz!.QuizId,
                Title = quiz.Title,
                Description = quiz.Description,
                Questions = quiz.Questions.Select(qvm => new QuestionViewModel
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

            return View(quizViewModel);
        }

        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> EditQuizAsync(QuizViewModel quizViewModel)
        {
            if (ModelState.IsValid)
            {
                var (user, redirect) = await GetCurrentUserOrRedirect();
                if (user is null) return redirect!;
                var (quiz, quizRedirect) = await GetUserOwnsQuiz(quizViewModel.QuizId, user!);
                if (quiz is null) return quizRedirect!;

                var Quiz = new Quiz()
                {
                    QuizId = quizViewModel.QuizId,
                    Title = quizViewModel.Title,
                    Description = quizViewModel.Description,
                    UserId = user!.Id,
                    Questions = quizViewModel.Questions.Select(qvm => new Question
                    {
                        QuestionScore = qvm.QuestionScore,
                        Description = qvm.Description,
                        Answers = qvm.Answers.Select(avm => new Answer
                        {
                            Description = avm.Description,
                            IsCorrect = avm.IsCorrect
                        }).ToList()
                    }).ToList()
                };

                try
                {
                    await quizRepository.UpdateQuizAsync(Quiz);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(String.Empty, $"Something went wrong: {e}");
                }
            }
            ;

            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteQuizAsync(int Id)
        {
            var (user, redirect) = await GetCurrentUserOrRedirect();
            if (user is null) return redirect!;
            var (quiz, quizRedirect) = await GetUserOwnsQuiz(Id, user!);
            if (quiz is null) return quizRedirect!;

            try
            {
                await quizRepository.DeleteQuizAsync(quiz);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(String.Empty, $"Something went wrong: {e}");
            }

            return RedirectToAction("Index");
        }

        private async Task<(User? user, IActionResult? redirect)> GetCurrentUserOrRedirect()
        {
            var username = HttpContext.Session.GetString("UserName");

            if (username is null)
            {
                ModelState.AddModelError(string.Empty, "Something's wrong with your session - please clear cache");
                return (null, RedirectToAction("Index", "Quiz"));
            }

            var user = await userManager.FindByNameAsync(username!);

            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "The user does not exist ?");
                return (null, RedirectToAction("User", "Logout"));
            }

            return (user, null);
        }

        private async Task<(Quiz? quiz, IActionResult? redirect)> GetUserOwnsQuiz(int Id, User user)
        {
            var quiz = await quizRepository.GetQuizByIdAsync(Id);

            if (quiz.UserId != user!.Id)
            {
                ModelState.AddModelError(String.Empty, "This quiz does not belong to you");
                return (null, RedirectToAction("Index"));
            }

            return (quiz, null);
        }
    }
}
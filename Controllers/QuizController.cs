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

    public class QuizController(IQuizRepository quizRepository, IAccessValidationService accessValidationService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var (user, redirect) = await accessValidationService.GetCurrentUserOrRedirect(this);
            if (redirect is not null) return redirect;

            var quizes = await quizRepository.GetQuizesByUserAsync(user!);

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

        [HttpGet]
        public async Task<IActionResult> Getquiz(int quizId)
        {
            var (user, redirect) = await accessValidationService.GetCurrentUserOrRedirect(this);
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
            var (user, redirect) = await accessValidationService.GetCurrentUserOrRedirect(this);
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
                var (user, redirect) = await accessValidationService.GetCurrentUserOrRedirect(this);
                if (user is null) return redirect!;
                var hasAll = accessValidationService.EachQuestionHasAnswer(quizViewModel, ModelState, this);
                if (hasAll is false) return View(quizViewModel)!;

                var quiz = new Quiz
                {
                    Title = quizViewModel.Title,
                    Description = quizViewModel.Description,
                    TotalScore = quizViewModel.Questions.Sum(qvm => qvm.QuestionScore),
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
            var (user, redirect) = await accessValidationService.GetCurrentUserOrRedirect(this);
            if (user is null) return redirect!;
            var (quiz, quizRedirect) = await accessValidationService.GetUserOwnsQuiz(quizId, user!, this);
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
                var (user, redirect) = await accessValidationService.GetCurrentUserOrRedirect(this);
                if (user is null) return redirect!;
                var (quiz, quizRedirect) = await accessValidationService.GetUserOwnsQuiz(quizViewModel.QuizId, user!, this);
                if (quiz is null) return quizRedirect!;
                var hasAll = accessValidationService.EachQuestionHasAnswer(quizViewModel, ModelState, this);
                if (hasAll is false) return View(quizViewModel)!;

                var Quiz = new Quiz()
                {
                    QuizId = quizViewModel.QuizId,
                    Title = quizViewModel.Title,
                    Description = quizViewModel.Description,
                    UserId = user!.Id,
                    TotalScore = quizViewModel.Questions.Sum(qvm => qvm.QuestionScore),
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
            var (user, redirect) = await accessValidationService.GetCurrentUserOrRedirect(this);
            if (user is null) return redirect!;
            var (quiz, quizRedirect) = await accessValidationService.GetUserOwnsQuiz(Id, user!, this);
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

        [HttpGet, ActionName("Game")]
        public async Task<IActionResult> LaunchQuizAsync(int quizId)
        {
            var (user, redirect) = await accessValidationService.GetCurrentUserOrRedirect(this);
            if (user is null) return redirect!;
            var (quiz, quizRedirect) = await accessValidationService.GetUserOwnsQuiz(quizId, user!, this);
            if (quiz is null) return quizRedirect!;

            var quizViewModel = new QuizViewModel
            {
                QuizId = quiz.QuizId,
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
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using quiz_project.Entities;
using quiz_project.Entities.Repositories;
using quiz_project.Extensions;
using quiz_project.Interfaces;
using quiz_project.Models;

namespace quiz_project.Controllers
{
    public class QuizGameController(IAccessValidationService accessValidationService, IPaginationService<Question> paginationService,
                                        IQuizRepository quizRepository) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index(int QuizId)
        {
            var (user, redirect) = await accessValidationService.GetCurrentUserOrRedirect(this);
            if (user is null) return redirect!;
            var (quiz, quizRedirect) = await accessValidationService.GetUserOwnsQuiz(QuizId, user!, this);
            if (quiz is null) return quizRedirect!;

            HttpContext.Session.SetInt(SessionKeys.QuizId, QuizId);
            HttpContext.Session.SetInt(SessionKeys.CurrentPage, 0);
            HttpContext.Session.SetInt(SessionKeys.QuestionCount, 5);

            return RedirectToAction("Play");
        }

        [HttpGet, ActionName("Play")]
        public async Task<IActionResult> Play()
        {
            var (user, redirect) = await accessValidationService.GetCurrentUserOrRedirect(this);
            if (user is null) return redirect!;
            // Either you're during a quiz or you shouldn't be there...
            int quizId = HttpContext.Session.GetInt(SessionKeys.QuizId) ?? -1;
            var (quiz, quizRedirect) = await accessValidationService.GetUserOwnsQuiz(quizId, user!, this);
            if (quiz is null) return quizRedirect!;

            int currentPage = HttpContext.Session.GetInt(SessionKeys.CurrentPage) ?? 0;
            int questionCount = HttpContext.Session.GetInt(SessionKeys.QuestionCount) ?? 5;

            List<Question> questions = await quizRepository.GetQuestionsByQuizId(quizId);

            var paginatedQuestiuons = await paginationService.GetPagedDataAsync(questions, currentPage, questionCount);

            if (!paginatedQuestiuons.Any())
            {
                return Content("Quiz finished - need summary page here...");
            }

            var gameViewModel = new GameViewModel
            {
                CurrentPage = currentPage + 1,
                TotalPages = paginatedQuestiuons.Count(),
                Questions = paginatedQuestiuons.Select(question => new QuestionViewModel
                {
                    QuestionId = question.QuestionId,
                    Description = question.Description,
                    QuestionScore = question.QuestionScore,
                    Answers = question.Answers.Select(ans => new AnswerViewModel
                    {
                        // This is done this way in order to not reveal the answer to question
                        AnswerId = ans.AnswerId,
                        Description = ans.Description,
                        IsCorrect = false
                    }).ToList()
                }).ToList()
            };

            return View(gameViewModel);
        }

        [HttpPost, ActionName("Play")]
        public async Task<IActionResult> Play(List<int> SelectedAnswerIds, int CurrentPage)
        {
            var (user, redirect) = await accessValidationService.GetCurrentUserOrRedirect(this);
            if (user is null) return redirect!;

            // Either you're during a quiz or you shouldn't be there...
            int quizId = HttpContext.Session.GetInt(SessionKeys.QuizId) ?? -1;

            var (quiz, quizRedirect) = await accessValidationService.GetUserOwnsQuiz(quizId, user, this);
            if (quiz is null) return quizRedirect!;

            int currentPage = HttpContext.Session.GetInt(SessionKeys.CurrentPage) ?? 0;
            int questionCount = HttpContext.Session.GetInt(SessionKeys.QuestionCount) ?? 5;

            var questionQuery = await quizRepository.GetQuestionsByQuizId(quizId);
            var paged = await paginationService.GetPagedDataAsync(questionQuery, CurrentPage + 1, questionCount);

            if (!paged.Any())
            {
                return Content("Quiz finished - need summary page here...");
            }

            var gameViewModel = new GameViewModel
            {
                CurrentPage = CurrentPage + 1,
                TotalPages = questionQuery.Count(),
                Questions = questionQuery.Select(question => new QuestionViewModel
                {
                    QuestionId = question.QuestionId,
                    Description = question.Description,
                    QuestionScore = question.QuestionScore,
                    Answers = question.Answers.Select(ans => new AnswerViewModel
                    {
                        // This is done this way in order to not reveal the answer to question
                        AnswerId = ans.AnswerId,
                        Description = ans.Description,
                        IsCorrect = false
                    }).ToList()
                }).ToList()
            };

            return View(gameViewModel);
        }
    }
}
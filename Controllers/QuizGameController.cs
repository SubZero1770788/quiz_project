using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using quiz_project.Database.Repositories;
using quiz_project.Entities;
using quiz_project.Entities.Repositories;
using quiz_project.Extensions;
using quiz_project.Helpers;
using quiz_project.Interfaces;
using quiz_project.Models;
using static quiz_project.Helpers.QuizMetaData;
using static quiz_project.Models.QuizSummaryViewModel;

namespace quiz_project.Controllers
{
    public class QuizGameController(IAccessValidationService accessValidationService, IPaginationService<Question> paginationService,
                                        IQuizRepository quizRepository, IAttemptRepository attemptRepository, UserManager<User> userManager) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index(int QuizId)
        {
            var (user, redirect) = await accessValidationService.GetCurrentUserOrRedirect(this);
            if (user is null) return redirect!;
            var (quiz, quizRedirect) = await accessValidationService.GetUserOwnsQuiz(QuizId, user!, this);
            if (quiz is null) return quizRedirect!;

            QuizMetaData json = new(QuizId, user.Id, 1, 5, new List<AnswersForQuestion>());
            HttpContext.Session.SetQuizMetaDataJSON(SessionKeys.QuizMetaData, JsonSerializer.Serialize(json));

            return RedirectToAction("Play");
        }

        [HttpGet, ActionName("Play")]
        public async Task<IActionResult> Play()
        {
            // fix this to include the quizMetaData instead of controller, also redirects ONLY from controllers
            var (user, redirect) = await accessValidationService.GetCurrentUserOrRedirect(this);
            if (user is null) return redirect!;

            var QuizMetaData = HttpContext.Session.GetQuizMetaDataJSON(SessionKeys.QuizMetaData);
            // Either you're during a quiz or you shouldn't be there...
            int quizId = QuizMetaData.QuizId;

            var (quiz, quizRedirect) = await accessValidationService.GetUserOwnsQuiz(quizId, user!, this);
            if (quiz is null) return quizRedirect!;

            List<Question> questions = await quizRepository.GetQuestionsByQuizId(quizId);

            var paginatedQuestiuons = await paginationService.GetPagedDataAsync(questions, QuizMetaData.CurrentPage, QuizMetaData.QuestionCount);

            if (!paginatedQuestiuons.Any())
            {
                // Return something if Quiz is empty...
                return Content("It seems that the quiz didn't have any questions...");
            }

            // TotalPages should find it's way into QuizMetaData
            var gameViewModel = new GameViewModel
            {
                CurrentPage = QuizMetaData.CurrentPage,
                TotalPages = paginatedQuestiuons.TotalPages,
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
        public async Task<IActionResult> Play(GameViewModel model)
        {
            var (user, redirect) = await accessValidationService.GetCurrentUserOrRedirect(this);
            if (user is null) return redirect!;

            var quizMetaData = HttpContext.Session.GetQuizMetaDataJSON(SessionKeys.QuizMetaData);
            // Either you're during a quiz or you shouldn't be there...
            int quizId = quizMetaData.QuizId;

            var (quiz, quizRedirect) = await accessValidationService.GetUserOwnsQuiz(quizId, user, this);
            if (quiz is null) return quizRedirect!;

            var questionQuery = await quizRepository.GetQuestionsByQuizId(quizId);
            var paged = await paginationService.GetPagedDataAsync(questionQuery, quizMetaData.CurrentPage + 1, quizMetaData.QuestionCount);
            var previousPage = await paginationService.GetPagedDataAsync(questionQuery, quizMetaData.CurrentPage, quizMetaData.QuestionCount);

            if (!paged.Any())
            {
                var correctAnswersByQuestion = questionQuery.ToDictionary(q => q.QuestionId, q => q.Answers.Where(a => a.IsCorrect)
                                                                .Select(a => a.AnswerId).OrderBy(id => id).ToList());

                int userScore = quizMetaData.UserAnswers.Sum(q =>
                {
                    if (!correctAnswersByQuestion.TryGetValue(q.QuestionId, out var correctAnswerIds))
                        return 0;

                    var userAnswerIds = q.AnswersId.OrderBy(id => id).ToList();
                    return correctAnswerIds.SequenceEqual(userAnswerIds) ? questionQuery.Where(q => q.QuestionId == q.QuestionId).First().QuestionScore : 0;
                });

                var attempt = new QuizAttempt
                {
                    UserId = user.Id,
                    QuizId = quizId,
                    Score = userScore,
                    AnswerSelections = quizMetaData.UserAnswers
                        .SelectMany(ua => ua.AnswersId.Select(answerId => new AnswerSelection
                        {
                            QuestionId = ua.QuestionId,
                            AnswerId = answerId,
                            IsCorrect = correctAnswersByQuestion.TryGetValue(ua.QuestionId, out var correctIds)
                                        && correctIds.Contains(answerId)
                        }))
                        .ToList()
                };

                await attemptRepository.CreateAsync(attempt);

                return RedirectToAction("Summary");
            }

            var gameViewModel = new GameViewModel
            {
                CurrentPage = quizMetaData.CurrentPage + 1,
                TotalPages = paged.TotalPages,
                Questions = paged.Select(question => new QuestionViewModel
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

            List<AnswersForQuestion> currentUserAnswers =
                model.Questions
                .Select(q => new AnswersForQuestion
                {
                    QuestionId = q.QuestionId,
                    AnswersId = model.Questions
                        .FirstOrDefault(mq => mq.QuestionId == q.QuestionId)?
                        .SelectedAnswerIds ?? new List<int>()
                }).ToList();

            quizMetaData.UserAnswers.AddRange(currentUserAnswers);

            HttpContext.Session.Remove(SessionKeys.QuizMetaData);
            QuizMetaData json = new QuizMetaData(quizMetaData.QuizId, quizMetaData.UserId, quizMetaData.CurrentPage + 1, quizMetaData.QuestionCount, quizMetaData.UserAnswers);
            HttpContext.Session.SetQuizMetaDataJSON(SessionKeys.QuizMetaData, JsonSerializer.Serialize(json));

            return View(gameViewModel);
        }

        [HttpGet, ActionName("Summary")]
        public async Task<IActionResult> QuizAttemptSummary()
        {
            var (user, redirect) = await accessValidationService.GetCurrentUserOrRedirect(this);
            if (user is null) return redirect!;

            var quizMetaData = HttpContext.Session.GetQuizMetaDataJSON(SessionKeys.QuizMetaData);
            HttpContext.Session.Remove(SessionKeys.QuizMetaData);

            var (quiz, quizRedirect) = await accessValidationService.GetUserOwnsQuiz(quizMetaData.QuizId, user, this);
            if (quiz is null) return quizRedirect!;

            var allQuizAttempts = await attemptRepository.GetAllAttemptsAsync(quizMetaData.QuizId);
            var quizDefinition = await quizRepository.GetQuizByIdAsync(quizMetaData.QuizId);

            var playerScore = await attemptRepository.GetLatestUserAttemptAsync(quizMetaData.UserId);

            var topScores = allQuizAttempts.OrderBy(aqa => aqa.Score).Take(10).ToList();
            var users = await userManager.Users.ToDictionaryAsync(u => u.Id, u => u.UserName);

            QuizSummaryViewModel quizSummaryViewModel = new()
            {
                Score = playerScore.Score,
                TotalScore = quizDefinition.TotalScore,
                TopPlayerScores = topScores.Select(a => new TopScore
                {
                    UserName = users[a.UserId] ?? "User not found",
                    PlayerScore = a.Score
                }).OrderBy(a => a.PlayerScore).ToList()
            };

            return View(quizSummaryViewModel);
        }
    }
}
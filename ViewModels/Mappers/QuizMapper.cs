using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using quiz_project.Entities;
using quiz_project.Interfaces;
using quiz_project.Models;
using static quiz_project.Models.QuizSummaryViewModel;

namespace quiz_project.ViewModels.Mappers
{
    public class QuizMapper : IQuizMapper
    {
        public Quiz ToEntity(QuizViewModel quizViewModel, int userId)
        {
            var quiz = new Quiz
            {
                QuizId = quizViewModel.QuizId,
                Title = quizViewModel.Title,
                Description = quizViewModel.Description,
                TotalScore = quizViewModel.Questions.Sum(qvm => qvm.QuestionScore),
                UserId = userId,
                IsPublic = quizViewModel.IsPublic,
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

            return quiz;
        }

        public QuizStatisticsModel ToQuizStatisticsModel(Quiz quiz, double averageScores, IEnumerable<QuizAttempt> allQuizAttempts,
                                QuizAttempt topUserAttempt, List<QuizAttempt> topScores, Dictionary<int, string> users)
        {

            var quizStatisticsModel = new QuizStatisticsModel
            {
                Title = quiz.Title,
                AverageScore = averageScores,
                ScorePercentage = averageScores / quiz.TotalScore * 100,
                UsersFinished = allQuizAttempts.DistinctBy(aqa => aqa.UserId).Count(),
                quizSummaryViewModel = new QuizSummaryViewModel
                {
                    Score = topUserAttempt.Score,
                    TotalScore = quiz.TotalScore,
                    TopPlayerScores = topScores.Select(a => new TopScore
                    {
                        UserName = users[a.UserId] ?? "User not found",
                        PlayerScore = a.Score
                    }).OrderBy(a => a.PlayerScore).ToList()
                }
            };

            return quizStatisticsModel;
        }

        public QuizSummaryViewModel ToQuizSummaryViewModel(Quiz quiz, List<QuizAttempt> topScores,
                                Dictionary<int, string> users, QuizAttempt playerScore)
        {
            QuizSummaryViewModel quizSummaryViewModel = new()
            {
                Score = playerScore.Score,
                TotalScore = quiz.TotalScore,
                TopPlayerScores = topScores.Select(a => new TopScore
                {
                    UserName = users[a.UserId] ?? "User not found",
                    PlayerScore = a.Score
                }).OrderBy(a => a.PlayerScore).ToList()
            };

            return quizSummaryViewModel;
        }

        public QuizViewModel ToQuizViewModel(Quiz quiz)
        {
            var quizViewModel = new QuizViewModel
            {
                QuizId = quiz!.QuizId,
                Title = quiz.Title,
                Description = quiz.Description,
                Questions = quiz.Questions.Select(qvm => new QuestionViewModel
                {
                    QuestionId = qvm.QuestionId != 0 ? qvm.QuestionId : 0,
                    QuestionScore = qvm.QuestionScore,
                    Description = qvm.Description,
                    Answers = qvm.Answers.Select(avm => new AnswerViewModel
                    {
                        Description = avm.Description,
                        IsCorrect = avm.IsCorrect
                    }).ToList()
                }).ToList()
            };

            return quizViewModel;
        }


    }
}
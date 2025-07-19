using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using quiz_project.Entities;
using quiz_project.Interfaces;
using quiz_project.Models;

namespace quiz_project.Services
{
    public class QuizService(IQuizRepository quizRepository, IQuizMapper quizMapper, IAccessValidationService accessValidationService) : IQuizService
    {
        public async Task<(bool success, string error)> CreateAsync(QuizViewModel quizViewModel, int userId)
        {
            var quiz = quizMapper.ToEntity(quizViewModel, userId);
            await quizRepository.CreateQuizAsync(quiz);
            return (true, String.Empty);
        }

        public async Task<(bool success, string error)> DeleteAsync(int quizId)
        {
            var quiz = await quizRepository.GetQuizByIdAsync(quizId);
            await quizRepository.DeleteQuizAsync(quiz);
            return (true, String.Empty);
        }

        public async Task<QuizViewModel> GetEditAsync(int quizId)
        {
            var quiz = await quizRepository.GetQuizByIdAsync(quizId);
            var quizViewModel = quizMapper.ToQuizViewModel(quiz);
            return quizViewModel;
        }

        public async Task<(bool success, IEnumerable<string> errors)> PostEditAsync(QuizViewModel quizViewModel, int userId)
        {
            var errors = accessValidationService.EachQuestionHasAnswer(quizViewModel).ToList();

            if (errors.Count() > 0)
                return (false, errors);

            var quiz = quizMapper.ToEntity(quizViewModel, userId);

            await quizRepository.UpdateQuizAsync(quiz);
            return (true, Enumerable.Empty<string>());
        }
    }
}
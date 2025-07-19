using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using quiz_project.Database;
using quiz_project.Interfaces;

namespace quiz_project.Entities.Repositories
{
    public class QuizRepository(QuizDb context) : IQuizRepository
    {
        public async Task<IEnumerable<Quiz>> GetQuizesByUserAsync(int userId)
        {
            var quizes = await context.Quizzes.Where(x => x.UserId == userId)
                                        .Include(q => q.Questions).ToListAsync();
            return quizes;
        }
        public async Task<IEnumerable<Quiz>> GetQuizesAsync()
        {
            var quizes = await context.Quizzes.ToListAsync();
            return quizes;
        }
        public async Task<IEnumerable<Quiz>> GetPublicQuizzes()
        {
            var quizes = await context.Quizzes.Where(q => q.IsPublic == true).Include(q => q.Questions).ToListAsync();
            return quizes;
        }
        public async Task CreateQuizAsync(Quiz quiz)
        {
            await context.AddAsync(quiz);
            await context.SaveChangesAsync();
        }
        public async Task DeleteQuizAsync(Quiz quiz)
        {
            var oldQuiz = await context.Quizzes.Where(q => q.QuizId == quiz.QuizId).FirstAsync();
            context.Quizzes.Remove(oldQuiz);
            await context.SaveChangesAsync();
        }

        public async Task<Quiz> GetQuizByIdAsync(int quizId)
        {
            var quiz = await context.Quizzes.Where(q => q.QuizId == quizId)
                                        .Include(q => q.Questions).ThenInclude(q => q.Answers).FirstAsync();
            return quiz;
        }

        public async Task UpdateQuizAsync(Quiz quiz)
        {
            // Get old quiz with related questions and answers
            var oldQuiz = await context.Quizzes
                .Include(q => q.Questions)
                .ThenInclude(q => q.Answers)
                .FirstAsync(q => q.QuizId == quiz.QuizId);

            // Update quiz core properties
            context.Entry(oldQuiz).CurrentValues.SetValues(quiz);

            // Remove old nested entities
            context.Answers.RemoveRange(oldQuiz.Questions.SelectMany(q => q.Answers));
            context.Questions.RemoveRange(oldQuiz.Questions);
            await context.SaveChangesAsync();

            // Prepare new questions/answers
            foreach (var question in quiz.Questions)
            {
                question.QuestionId = 0;
                question.QuizId = quiz.QuizId;
                question.Quiz = null;

                foreach (var answer in question.Answers)
                {
                    answer.AnswerId = 0;
                    answer.QuestionId = 0; // EF will assign this after question insert
                    answer.Question = null;
                }
            }

            // Add new questions *with* their nested answers
            context.Questions.AddRange(quiz.Questions); // Will cascade insert answers
            await context.SaveChangesAsync(); // Only ONE SaveChanges needed
        }

        public async Task<List<Question>> GetQuestionsByQuizId(int quizId)
        {
            var questions = await context.Questions.Where(q => q.QuizId == quizId).Include(q => q.Answers).ToListAsync();
            return questions;
        }
    }
}
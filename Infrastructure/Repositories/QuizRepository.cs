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
            var oldQuiz = await context.Quizzes
                .Include(q => q.Questions)
                .ThenInclude(q => q.Answers)
                .FirstAsync(q => q.QuizId == quiz.QuizId);

            context.Entry(oldQuiz).CurrentValues.SetValues(quiz);
            context.Answers.RemoveRange(oldQuiz.Questions.SelectMany(q => q.Answers));
            context.Questions.RemoveRange(oldQuiz.Questions);
            await context.SaveChangesAsync();

            foreach (var question in quiz.Questions)
            {
                question.QuestionId = 0;
                question.QuizId = quiz.QuizId;
                question.Quiz = null;

                foreach (var answer in question.Answers)
                {
                    answer.AnswerId = 0;
                    answer.QuestionId = 0;
                    answer.Question = null;
                }
            }
            context.Questions.AddRange(quiz.Questions);
            await context.SaveChangesAsync();
        }

        public async Task<List<Question>> GetQuestionsByQuizId(int quizId)
        {
            var questions = await context.Questions.Where(q => q.QuizId == quizId).Include(q => q.Answers).ToListAsync();
            return questions;
        }

        public async Task<Dictionary<(int QuestionId, int AnswerId), int>> GetAnswerSelectionStatsAsync(int quizId)
        {
            var answerCounts = await context.AnswerSelections
                .Where(a => a.QuizAttempt.QuizId == quizId)
                .GroupBy(a => new { a.QuestionId, a.AnswerId })
                .ToDictionaryAsync(
                    g => (g.Key.QuestionId, g.Key.AnswerId),
                    g => g.Count()
                );

            return answerCounts;
        }
    }
}
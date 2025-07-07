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

        public async Task<IEnumerable<Quiz>> GetQuizesAsync()
        {
            var quizes = await context.Quizzes.ToListAsync();
            return quizes;
        }

        public async Task<IEnumerable<Quiz>> GetQuizesByUserAsync(User user)
        {
            var quizes = await context.Quizzes.Where(x => x.UserId == user.Id)
                                        .Include(q => q.Questions).ThenInclude(q => q.Answers).ToListAsync();
            return quizes;
        }

        public async Task CreateQuizAsync(Quiz quiz)
        {
            await context.AddAsync(quiz);
            await context.SaveChangesAsync();
        }

        public async Task UpdateQuizAsync(Quiz quiz)
        {
            var oldQuiz = await context.Quizzes.Where(q => q.QuizId == quiz.QuizId).FirstAsync();
            context.Quizzes.Remove(oldQuiz);
            await context.SaveChangesAsync();

            var res = await context.Quizzes.AddAsync(quiz);
            await context.SaveChangesAsync();
        }
    }
}
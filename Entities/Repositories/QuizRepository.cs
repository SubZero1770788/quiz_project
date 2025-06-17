using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using quiz_project.Interfaces;
using quiz_project.Migrations.Database;

namespace quiz_project.Entities.Repositories
{
    public class QuizRepository(QuizDb context) : IQuizRepository
    {
        public void DeleteQuiz(Quiz quiz)
        {
            throw new NotImplementedException();
        }

        public async Task<Quiz> GetQuizById(int quizId)
        {
            var quiz = await context.Quizzes.FirstOrDefaultAsync(q => q.QuizId == quizId);
            return quiz;
        }

        public async Task<IEnumerable<Quiz>> GetQuizesAsync()
        {
            var quizes = await context.Quizzes.ToListAsync();
            return quizes;
        }

        public void InsertQuiz(Quiz quiz)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void UpdateQuiz(Quiz quiz)
        {
            throw new NotImplementedException();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using quiz_project.Entities;

namespace quiz_project.Interfaces
{
    public interface IQuizRepository
    {
        Task<IEnumerable<Quiz>> GetQuizesAsync();
        Task<IEnumerable<Quiz>> GetQuizesByUserAsync(User user);
        Task<Quiz> GetQuizByIdAsync(int quizId);
        Task DeleteQuizAsync(Quiz quiz);
        Task UpdateQuizAsync(Quiz quiz);
        Task CreateQuizAsync(Quiz quiz);
        Task<List<Question>> GetQuestionsByQuizId(int quizId);
    }
}
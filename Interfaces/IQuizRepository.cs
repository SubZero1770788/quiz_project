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
        Task<Quiz> GetQuizById(int quizId);
        void InsertQuiz(Quiz quiz);
        void DeleteQuiz(Quiz quiz);
        void UpdateQuiz(Quiz quiz);
        void Save();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using quiz_project.Database;
using quiz_project.Interfaces;

namespace quiz_project.Entities.Repositories
{
    public class QuestionRepository(QuizDb context) : IQuestionRepository
    {
        public Task DeleteQuestionAsync(Question question)
        {
            throw new NotImplementedException();
        }

        public Task<Question> GetQuestionByIdAsync(int QuestionId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Question>> GetQuestionsAsync()
        {
            throw new NotImplementedException();
        }

        public Task CreateQuestionAsync(Question question)
        {
            throw new NotImplementedException();
        }

        public Task UpdateQuestionAsync(Question question)
        {
            throw new NotImplementedException();
        }
    }
}
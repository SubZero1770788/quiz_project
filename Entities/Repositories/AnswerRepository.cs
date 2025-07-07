using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using quiz_project.Database;
using quiz_project.Interfaces;


namespace quiz_project.Entities.Repositories
{
    public class AnswerRepository(QuizDb context) : IAnswerRepository
    {
        public Task DeleteAnswerAsync(Answer answer)
        {
            throw new NotImplementedException();
        }

        public Task<Answer> GetAnswerByIdAsync(int answerId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Answer>> GetAnswersAsync()
        {
            throw new NotImplementedException();
        }

        public Task CreateAnswerAsync(Answer answer)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAnswerAsync(Answer answer)
        {
            throw new NotImplementedException();
        }
    }
}
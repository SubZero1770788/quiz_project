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
        public void DeleteAnswer(Answer answer)
        {
            throw new NotImplementedException();
        }

        public Task<Answer> GetAnswerById(int answerId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Answer>> GetAnswersAsync()
        {
            throw new NotImplementedException();
        }

        public void InsertAnswer(Answer answer)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void UpdateAnswer(Answer answer)
        {
            throw new NotImplementedException();
        }
    }
}
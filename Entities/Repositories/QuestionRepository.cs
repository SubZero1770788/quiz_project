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
        public void DeleteQuestion(Question question)
        {
            throw new NotImplementedException();
        }

        public Task<Question> GetQuestionById(int QuestionId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Question>> GetQuestionsAsync()
        {
            throw new NotImplementedException();
        }

        public void InsertQuestion(Question question)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void UpdateQuestion(Question question)
        {
            throw new NotImplementedException();
        }
    }
}
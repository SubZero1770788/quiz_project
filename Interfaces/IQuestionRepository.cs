using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using quiz_project.Entities;

namespace quiz_project.Interfaces
{
    public interface IQuestionRepository
    {
        Task<IEnumerable<Question>> GetQuestionsAsync();
        Task<Question> GetQuestionById(int QuestionId);
        void InsertQuestion(Question question);
        void DeleteQuestion(Question question);
        void UpdateQuestion(Question question);
        void Save();
    }
}
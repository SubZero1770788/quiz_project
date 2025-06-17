using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using quiz_project.Entities;

namespace quiz_project.Interfaces
{
    public interface IAnswerRepository
    {
        Task<IEnumerable<Answer>> GetAnswersAsync();
        Task<Answer> GetAnswerById(int answerId);
        void InsertAnswer(Answer answer);
        void DeleteAnswer(Answer answer);
        void UpdateAnswer(Answer answer);
        void Save();
    }
}
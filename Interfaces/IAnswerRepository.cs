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
        Task<Answer> GetAnswerByIdAsync(int answerId);
        Task CreateAnswerAsync(Answer answer);
        Task DeleteAnswerAsync(Answer answer);
        Task UpdateAnswerAsync(Answer answer);
    }
}
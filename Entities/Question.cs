using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace quiz_project.Entities
{
    public class Question
    {
        public int QuestionId { get; set; }
        public required string Description { get; set; }
        public List<Answer>? Answers { get; set; }
        public required int QuizId { get; set; }
        public required Quiz Quiz { get; set; }
    }
}
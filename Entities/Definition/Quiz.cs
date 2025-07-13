using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace quiz_project.Entities
{
    public class Quiz
    {
        public int QuizId { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public int TotalScore { get; set; }
        public bool IsPublic { get; set; }
        public List<Question> Questions { get; set; } = new();
        public User User { get; set; }
        public required int UserId { get; set; }
    }
}
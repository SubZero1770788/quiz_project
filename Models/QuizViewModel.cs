using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using quiz_project.Entities;

namespace quiz_project.Models
{
    public class QuizViewModel
    {
        public int QuizId { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public List<QuestionViewModel> Questions { get; set; } = new();
    }
}
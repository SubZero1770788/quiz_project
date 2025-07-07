using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace quiz_project.Models
{
    public class QuestionViewModel
    {
        public string Description { get; set; } = "empty";
        public int QuestionScore { get; set; }
        public List<AnswerViewModel> Answers { get; set; } = new();
    }
}
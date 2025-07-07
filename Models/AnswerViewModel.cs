using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace quiz_project.Models
{
    public class AnswerViewModel
    {
        public string Description { get; set; } = "empty";
        public bool IsCorrect { get; set; }
    }
}
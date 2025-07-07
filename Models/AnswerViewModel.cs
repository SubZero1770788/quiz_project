using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace quiz_project.Models
{
    public class AnswerViewModel
    {
        [Required]
        public string Description { get; set; }
        public bool IsCorrect { get; set; }
    }
}
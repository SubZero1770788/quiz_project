using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace quiz_project.Models
{
    public class QuizSummaryViewModel
    {
        public int Score { get; set; }
        public int TotalScore { get; set; }
        [Range(0, 10)]
        public List<TopScore> TopPlayerScores { get; set; }

        public class TopScore
        {
            public string UserName { get; set; }
            public int PlayerScore { get; set; }
        }
    }
}
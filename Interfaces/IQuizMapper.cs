using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using quiz_project.Entities;
using quiz_project.Models;

namespace quiz_project.Interfaces
{
    public interface IQuizMapper
    {
        public Quiz ToEntity(QuizViewModel quizViewModel, int userId);
        public QuizViewModel ToQuizViewModel(Quiz quiz);
        public QuizStatisticsModel ToQuizStatisticsModel(Quiz quiz, double averageScores, IEnumerable<QuizAttempt> quizAttempts,
                            QuizAttempt topUserAttempt, List<QuizAttempt> topScores, Dictionary<int, string> users);
        public QuizSummaryViewModel ToQuizSummaryViewModel(Quiz quiz, List<QuizAttempt> topScores,
                            Dictionary<int, string> users, QuizAttempt playerScore);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using quiz_project.Database;
using quiz_project.Entities;
using quiz_project.Interfaces;
using quiz_project.Models;

namespace quiz_project.Controllers
{
    public class QuizController(QuizDb context, IQuizRepository quizrepository) : Controller
    {
        public async Task<ViewResult> Index()
        {
            var quizes = await quizrepository.GetQuizesAsync();

            var quizModels = new List<QuizModel>();
            foreach (Quiz q in quizes)
            {
                quizModels.Add(new QuizModel
                {
                    QuizId = q.QuizId,
                    Title = q.Title,
                    Description = q.Description,
                });
            }

            return View(quizModels);
        }

        public async Task<ViewResult> Getquiz(int quizId)
        {
            var quiz = await quizrepository.GetQuizById(quizId);
            return View(quizId);
        }
    }
}
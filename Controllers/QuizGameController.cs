using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using quiz_project.Entities;
using quiz_project.Interfaces;

namespace quiz_project.Controllers
{
    [Route("[controller]")]
    public class QuizGameController(IAccessValidationService accessValidationService) : Controller
    {
        public async Task<IActionResult> Index(int quizId)
        {
            return Content($"I am here.. {quizId}");
        }
    }
}
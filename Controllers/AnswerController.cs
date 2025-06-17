using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using quiz_project.Interfaces;

namespace quiz_project.Controllers
{
    public class AnswerController(IAnswerRepository answerRepository) : Controller
    {
        
    }
}
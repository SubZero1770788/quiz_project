using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using quiz_project.Entities;

namespace quiz_project.Interfaces
{
    public interface IAccessValidationService
    {
        public Task<(User? user, IActionResult? redirect)> GetCurrentUserOrRedirect(Controller controller);
        public Task<(Quiz? quiz, IActionResult? redirect)> GetUserOwnsQuiz(int id, User user, Controller controller);
    }
}
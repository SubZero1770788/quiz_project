using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace quiz_project.Entities
{
    public class User : IdentityUser
    {
        new public int Id { get; set; }
        public List<Quiz>? Quizzes { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using quiz_project.Entities;
using quiz_project.Models;

namespace quiz_project.Interfaces
{
    public interface IUserMapper
    {
        public User ToEntity(RegisterViewModel registerViewModel);

    }
}
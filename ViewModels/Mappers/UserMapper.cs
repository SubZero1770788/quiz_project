using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using quiz_project.Entities;
using quiz_project.Interfaces;
using quiz_project.Models;

namespace quiz_project.ViewModels.Mappers
{
    public class UserMapper : IUserMapper
    {
        public User ToEntity(RegisterViewModel registerViewModel)
        {
            var user = new User()
            {
                UserName = registerViewModel.UserName,
                Email = registerViewModel.Email
            };

            return user;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using quiz_project.Entities;

namespace quiz_project.Database
{
    public static class DbSeeder
    {
        public async static void Initialize(AsyncServiceScope scope, QuizDb context)
        {
            await context.Database.EnsureCreatedAsync();

            if (context.Quizzes.Any())
            {
                Console.WriteLine("Database seeding unnecessary - data is already there.");
                return;
            }

            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var Users = new List<User>
                {
                    new User{
                        UserName = "Michael",
                        Email = "michael@123",
                        IsLoggedIn=false,
                    },
                    new User{
                        UserName = "Sylvia",
                        Email = "sylvia123@o2.org",
                        IsLoggedIn=false,
                    },
                };

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                foreach (User user in Users)
                {
                    await userManager.CreateAsync(user, "123123ASD!@#a");
                }
                await context.SaveChangesAsync();

                var quizes = new List<Quiz>
                {
                    new Quiz{
                        QuizId = 1,
                        Title = "My first quiz",
                        Description = "This quiz has no other meaning",
                        UserId = 1
                    },
                    new Quiz{
                        QuizId = 2,
                        Title = "My second quiz",
                        Description = "I did not know I can create more of them !",
                        UserId = 1
                    }
                };

                await context.Quizzes.AddRangeAsync(quizes);
                await context.SaveChangesAsync();

                var questions = new List<Question>
                {
                    new Question{
                        QuestionId = 1,
                        QuizId = 1,
                        QuestionScore= 10,
                        Description = "Question 1: What is the capital of France?"
                    },
                    new Question{
                        QuestionId = 2,
                        QuizId = 1,
                        QuestionScore= 10,
                        Description = "Question 2: What is 2 + 2?"
                    },
                    new Question{
                        QuestionId = 3,
                        QuizId = 1,
                        QuestionScore= 10,
                        Description = "Question 3: Who wrote '1984'?"
                    },
                    new Question{
                        QuestionId = 4,
                        QuizId = 1,
                        QuestionScore= 10,
                        Description = "Question 4: What is the boiling point of water?"
                    },
                    new Question{
                        QuestionId = 5,
                        QuizId = 1,
                        QuestionScore= 10,
                        Description = "Question 5: What is the largest ocean on Earth?"
                    },
                    new Question{
                        QuestionId = 6,
                        QuizId = 1,
                        QuestionScore= 10,
                        Description = "Question 6: Who painted the Mona Lisa?"
                    },
                    new Question{
                        QuestionId = 7,
                        QuizId = 1,
                        QuestionScore= 10,
                        Description = "Question 7: What is the currency of Japan?"
                    },
                    new Question{
                        QuestionId = 8,
                        QuizId = 1,
                        QuestionScore= 10,
                        Description = "Question 8: What element has the symbol O?"
                    },
                    new Question{
                        QuestionId = 9,
                        QuizId = 1,
                        QuestionScore= 10,
                        Description = "Question 9: Who discovered gravity?"
                    },
                    new Question{
                        QuestionId = 10,
                        QuizId = 1,
                        QuestionScore= 10,
                        Description = "Question 10: What year did WW2 end?"
                    },
                    new Question{
                        QuestionId = 11,
                        QuizId = 1,
                        QuestionScore= 10,
                        Description = "Question 11: What is the square root of 16?"
                    },
                    new Question{
                        QuestionId = 12,
                        QuizId = 1,
                        QuestionScore= 10,
                        Description = "Question 12: What planet is known as the Red Planet?"
                    },
                    new Question{
                        QuestionId = 13,
                        QuizId = 1,
                        QuestionScore= 10,
                        Description = "Question 13: Who invented the telephone?"
                    },
                    new Question{
                        QuestionId = 14,
                        QuizId = 1,
                        QuestionScore= 10,
                        Description = "Question 14: What language is spoken in Brazil?"
                    },
                    new Question{
                        QuestionId = 15,
                        QuizId = 1,
                        QuestionScore= 10,
                        Description = "Question 15: What gas do plants absorb from the air?"
                    },
                    new Question{
                        QuestionId = 16,
                        QuizId = 1,
                        QuestionScore= 10,
                        Description = "Question 16: Who is the author of 'Harry Potter'?"
                    },
                    new Question{
                        QuestionId = 17,
                        QuizId = 1,
                        QuestionScore= 10,
                        Description = "Question 17: What is the tallest mountain in the world?"
                    },
                    new Question{
                        QuestionId = 18,
                        QuizId = 1,
                        QuestionScore= 10,
                        Description = "Question 18: What does DNA stand for?"
                    },
                    new Question{
                        QuestionId = 19,
                        QuizId = 1,
                        QuestionScore= 10,
                        Description = "Question 19: Who was the first man on the moon?"
                    },
                    new Question{
                        QuestionId = 20,
                        QuizId = 1,
                        QuestionScore= 10,
                        Description = "Question 20: What is the speed of light?"
                    }
             };

                await context.Questions.AddRangeAsync(questions);
                await context.SaveChangesAsync();

                var answers = new List<Answer>
                {
                   new Answer{
                    AnswerId = 1,
                    QuestionId = 1,
                    Description = "Paris",
                    IsCorrect = true
                },
                new Answer{
                    AnswerId = 2,
                    QuestionId = 1,
                    Description = "London",
                    IsCorrect = false
                },
                new Answer{
                    AnswerId = 3,
                    QuestionId = 1,
                    Description = "Berlin",
                    IsCorrect = false
                },
                new Answer{
                    AnswerId = 4,
                    QuestionId = 1,
                    Description = "Madrid",
                    IsCorrect = false
                },
                new Answer{
                    AnswerId = 5,
                    QuestionId = 2,
                    Description = "3",
                    IsCorrect = false
                },
                new Answer{
                    AnswerId = 6,
                    QuestionId = 2,
                    Description = "4",
                    IsCorrect = true
                },
                new Answer{
                    AnswerId = 7,
                    QuestionId = 2,
                    Description = "5",
                    IsCorrect = false
                },
                new Answer{
                    AnswerId = 8,
                    QuestionId = 3,
                    Description = "George Orwell",
                    IsCorrect = true
                },
                new Answer{
                    AnswerId = 9,
                    QuestionId = 3,
                    Description = "Aldous Huxley",
                    IsCorrect = false
                },
                new Answer{
                    AnswerId = 10,
                    QuestionId = 3,
                    Description = "J.K. Rowling",
                    IsCorrect = false
                },
                new Answer{
                    AnswerId = 11,
                    QuestionId = 4,
                    Description = "100°C",
                    IsCorrect = true
                },
                new Answer{
                    AnswerId = 12,
                    QuestionId = 4,
                    Description = "90°C",
                    IsCorrect = false
                },
                new Answer{
                    AnswerId = 13,
                    QuestionId = 4,
                    Description = "120°C",
                    IsCorrect = false
                },
                new Answer{
                    AnswerId = 14,
                    QuestionId = 4,
                    Description = "80°C",
                    IsCorrect = false
                },
                new Answer{
                    AnswerId = 15,
                    QuestionId = 5,
                    Description = "Pacific",
                    IsCorrect = true
                },
                new Answer{
                    AnswerId = 16,
                    QuestionId = 5,
                    Description = "Atlantic",
                    IsCorrect = false
                },
                new Answer{
                    AnswerId = 17,
                    QuestionId = 5,
                    Description = "Indian",
                    IsCorrect = false
                },
                new Answer{
                    AnswerId = 18,
                    QuestionId = 6,
                    Description = "Leonardo da Vinci",
                    IsCorrect = true
                },
                new Answer{
                    AnswerId = 19,
                    QuestionId = 6,
                    Description = "Michelangelo",
                    IsCorrect = false
                },
                new Answer{
                    AnswerId = 20,
                    QuestionId = 6,
                    Description = "Van Gogh",
                    IsCorrect = false
                },
                new Answer{
                    AnswerId = 21,
                    QuestionId = 7,
                    Description = "Yen",
                    IsCorrect = true
                },
                new Answer{
                    AnswerId = 22,
                    QuestionId = 7,
                    Description = "Won",
                    IsCorrect = false
                },
                new Answer{
                    AnswerId = 23,
                    QuestionId = 7,
                    Description = "Dollar",
                    IsCorrect = false
                },
                new Answer{
                    AnswerId = 24,
                    QuestionId = 8,
                    Description = "Oxygen",
                    IsCorrect = true
                },
                new Answer{
                    AnswerId = 25,
                    QuestionId = 8,
                    Description = "Hydrogen",
                    IsCorrect = false
                },
                new Answer{
                    AnswerId = 26,
                    QuestionId = 8,
                    Description = "Carbon",
                    IsCorrect = false
                },
                new Answer{
                    AnswerId = 27,
                    QuestionId = 9,
                    Description = "Isaac Newton",
                    IsCorrect = true
                },
                new Answer{
                    AnswerId = 28,
                    QuestionId = 9,
                    Description = "Albert Einstein",
                    IsCorrect = false
                },
                new Answer{
                    AnswerId = 29,
                    QuestionId = 9,
                    Description = "Galileo",
                    IsCorrect = false
                },
                new Answer{
                    AnswerId = 30,
                    QuestionId = 10,
                    Description = "1945",
                    IsCorrect = true
                },
                new Answer{
                    AnswerId = 31,
                    QuestionId = 10,
                    Description = "1939",
                    IsCorrect = false
                },
                new Answer{
                    AnswerId = 32,
                    QuestionId = 10,
                    Description = "1950",
                    IsCorrect = false
                },
                new Answer{
                    AnswerId = 33,
                    QuestionId = 11,
                    Description = "4",
                    IsCorrect = true
                },
                new Answer{
                    AnswerId = 34,
                    QuestionId = 11,
                    Description = "8",
                    IsCorrect = false
                },
                new Answer{
                    AnswerId = 35,
                    QuestionId = 11,
                    Description = "16",
                    IsCorrect = false
                },
                new Answer{
                    AnswerId = 36,
                    QuestionId = 12,
                    Description = "Mars",
                    IsCorrect = true
                },
                new Answer{
                    AnswerId = 37,
                    QuestionId = 12,
                    Description = "Jupiter",
                    IsCorrect = false
                },
                new Answer{
                    AnswerId = 38,
                    QuestionId = 12,
                    Description = "Saturn",
                    IsCorrect = false
                },
                new Answer{
                    AnswerId = 39,
                    QuestionId = 13,
                    Description = "Alexander Graham Bell",
                    IsCorrect = true
                },
                new Answer{
                    AnswerId = 40,
                    QuestionId = 13,
                    Description = "Thomas Edison",
                    IsCorrect = false
                },
                new Answer{
                    AnswerId = 41,
                    QuestionId = 13,
                    Description = "Nikola Tesla",
                    IsCorrect = false
                },
                new Answer{
                    AnswerId = 42,
                    QuestionId = 14,
                    Description = "Portuguese",
                    IsCorrect = true
                },
                new Answer{
                    AnswerId = 43,
                    QuestionId = 14,
                    Description = "Spanish",
                    IsCorrect = false
                },
                new Answer{
                    AnswerId = 44,
                    QuestionId = 14,
                    Description = "French",
                    IsCorrect = false
                },
                new Answer{
                    AnswerId = 45,
                    QuestionId = 15,
                    Description = "Carbon Dioxide",
                    IsCorrect = true
                },
                new Answer{
                    AnswerId = 46,
                    QuestionId = 15,
                    Description = "Oxygen",
                    IsCorrect = false
                },
                new Answer{
                    AnswerId = 47,
                    QuestionId = 15,
                    Description = "Nitrogen",
                    IsCorrect = false
                },
                new Answer{
                    AnswerId = 48,
                    QuestionId = 16,
                    Description = "J.K. Rowling",
                    IsCorrect = true
                },
                new Answer{
                    AnswerId = 49,
                    QuestionId = 16,
                    Description = "Stephen King",
                    IsCorrect = false
                },
                new Answer{
                    AnswerId = 50,
                    QuestionId = 16,
                    Description = "George R.R. Martin",
                    IsCorrect = false
                },
                new Answer{
                    AnswerId = 51,
                    QuestionId = 17,
                    Description = "Mount Everest",
                    IsCorrect = true
                },
                new Answer{
                    AnswerId = 52,
                    QuestionId = 17,
                    Description = "K2",
                    IsCorrect = false
                },
                new Answer{
                    AnswerId = 53,
                    QuestionId = 17,
                    Description = "Kilimanjaro",
                    IsCorrect = false
                },
                new Answer{
                    AnswerId = 54,
                    QuestionId = 18,
                    Description = "Deoxyribonucleic Acid",
                    IsCorrect = true
                },
                new Answer{
                    AnswerId = 55,
                    QuestionId = 18,
                    Description = "Ribonucleic Acid",
                    IsCorrect = false
                },
                new Answer{
                    AnswerId = 56,
                    QuestionId = 18,
                    Description = "Nucleic Base",
                    IsCorrect = false
                },
                new Answer{
                    AnswerId = 57,
                    QuestionId = 19,
                    Description = "Neil Armstrong",
                    IsCorrect = true
                },
                new Answer{
                    AnswerId = 58,
                    QuestionId = 19,
                    Description = "Buzz Aldrin",
                    IsCorrect = false
                },
                new Answer{
                    AnswerId = 59,
                    QuestionId = 19,
                    Description = "Yuri Gagarin",
                    IsCorrect = false
                },
                new Answer{
                    AnswerId = 60,
                    QuestionId = 20,
                    Description = "299,792,458 m/s",
                    IsCorrect = true
                },
                new Answer{
                    AnswerId = 61,
                    QuestionId = 20,
                    Description = "150,000,000 m/s",
                    IsCorrect = false
                },
                new Answer{
                    AnswerId = 62,
                    QuestionId = 20,
                    Description = "1,000,000 m/s",
                    IsCorrect = false
                },
                };

                await context.Answers.AddRangeAsync(answers);
                await context.SaveChangesAsync();


                //fix error with roles not seeding
                if (!context.Roles.Any())
                {
                    var roles = new List<Role>{
                        new Role()
                        {
                            Id = 1,
                            Name = "User",
                            NormalizedName = "USER"
                        },
                        new Role()
                        {
                            Id = 2,
                            Name = "Moderator",
                            NormalizedName = "MODERATOR"
                        },
                        new Role()
                        {
                            Id = 3,
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        }
                    };

                    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
                    foreach (Role role in roles)
                    {
                        await roleManager.CreateAsync(role);
                    }
                    await context.SaveChangesAsync();
                }

                await transaction.CommitAsync();

                Console.WriteLine("The database has been seeded successfully!");
            }
            catch (System.Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"An error during seeding: {ex.Message}");
            }
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using quiz_project.Entities;

namespace quiz_project.Migrations.Database
{
    public static class DbSeeder
    {
        public async static void Initialize(QuizDb context)
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
                var quizes = new List<Quiz>
                {
                    new Quiz{
                        QuizId = 1,
                        Title = "My first quiz",
                        Description = "This quiz has no other meaning",
                    },
                    new Quiz{
                        QuizId = 2,
                        Title = "My second quiz",
                        Description = "I did not know I can create more of them !"
                    }
                };

                await context.Quizzes.AddRangeAsync(quizes);
                await context.SaveChangesAsync();

                var questions = new List<Question>
                {
                    new Question{
                        QuestionId = 1,
                        QuizId = 1,
                        Description = "First question"
                    },
                    new Question{
                        QuestionId = 2,
                        QuizId = 1,
                        Description = "Second question"
                    },
                     new Question{
                        QuestionId = 3,
                        QuizId = 2,
                        Description = "Third question"
                    },
                    new Question{
                        QuestionId = 4,
                        QuizId = 2,
                        Description = "Fourth question"
                    },
                };

                await context.Questions.AddRangeAsync(questions);
                await context.SaveChangesAsync();

                var answers = new List<Answer>
                {
                    new Answer{
                        AnswerId = 1,
                        QuestionId = 1,
                        Description = "First Answer"
                    },
                    new Answer{
                        AnswerId = 2,
                        QuestionId = 1,
                        Description = "Second Answer"
                    },
                     new Answer{
                        AnswerId = 3,
                        QuestionId = 2,
                        Description = "Third Answer"
                    },
                    new Answer{
                        AnswerId = 4,
                        QuestionId = 2,
                        Description = "Fourth Answer"
                    },
                    new Answer{
                        AnswerId = 5,
                        QuestionId = 3,
                        Description = "Third Answer"
                    },
                    new Answer{
                        AnswerId = 6,
                        QuestionId = 3,
                        Description = "Fourth Answer"
                    },
                     new Answer{
                        AnswerId = 7,
                        QuestionId = 4,
                        Description = "Third Answer"
                    },
                    new Answer{
                        AnswerId = 8,
                        QuestionId = 4,
                        Description = "Fourth Answer"
                    },
                };

                await context.Answers.AddRangeAsync(answers);
                await context.SaveChangesAsync();
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
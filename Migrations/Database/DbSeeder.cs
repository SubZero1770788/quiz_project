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
                        Description = "This quiz has no other meaning"

                    },
                    new Quiz{
                        QuizId = 2,
                        Title = "My second quiz",
                        Description = "I did not know I can create more of them !"
                    }
                };

                await context.Quizzes.AddRangeAsync(quizes);
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
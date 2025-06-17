using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using quiz_project.Entities;

namespace quiz_project.Database
{
    public class QuizDb : DbContext
    {
        public QuizDb(DbContextOptions<QuizDb> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<Quiz>(en =>
            {
                en.HasKey(q => q.QuizId);

                en.HasMany(q => q.Questions)
                .WithOne(qu => qu.Quiz)
                .HasForeignKey(qu => qu.QuizId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            mb.Entity<Question>(en =>
            {
                en.HasKey(qu => qu.QuestionId);

                en.HasMany(qu => qu.Answers)
                .WithOne(a => a.Question)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);
            });

        }

        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
    }
}
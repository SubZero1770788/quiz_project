using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace quiz_project.Entities
{
    public class QuizDb : DbContext
    {
        public QuizDb(DbContextOptions<QuizDb> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Quiz>()
                .HasKey(q => q.QuizId);
        }

        public DbSet<Quiz> Quizzes { get; set; }
    }
}
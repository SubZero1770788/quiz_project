using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using quiz_project.Entities;
using quiz_project.Entities.Definition;

namespace quiz_project.Database
{
    public class QuizDb : IdentityDbContext<User, Role, int>
    {
        public QuizDb(DbContextOptions<QuizDb> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder ob)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var con = config.GetConnectionString("DefaultConnection");
            ob.UseSqlite(con);


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

            mb.Entity<Answer>(a =>
            {
                a.HasKey(a => a.AnswerId);
            });

            mb.Entity<User>(u =>
            {
                u.HasKey(u => u.Id);

                u.HasMany(u => u.Quizzes)
                    .WithOne(q => q.User)
                    .HasForeignKey(q => q.UserId);
            });

            mb.Entity<QuizAttempt>(qa =>
            {
                qa.HasKey(qa => qa.QuizAttemptId);

                qa.HasOne(qa => qa.Quiz)
                    .WithMany()
                    .HasForeignKey(qa => qa.QuizId)
                    .OnDelete(DeleteBehavior.Restrict);

                qa.HasOne(qa => qa.User)
                    .WithMany()
                    .HasForeignKey(qa => qa.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                qa.HasMany(qa => qa.AnswerSelections)
                    .WithOne(ans => ans.QuizAttempt)
                    .HasForeignKey(ans => ans.QuizAttemptId);
            });

            mb.Entity<AnswerSelection>(ans =>
            {
                ans.HasKey(ans => ans.AnswerSelectionId);

                ans.HasOne(ans => ans.Answer)
                    .WithMany()
                    .HasForeignKey(ans => ans.AnswerId)
                    .OnDelete(DeleteBehavior.Restrict);

                ans.HasOne(ans => ans.Question)
                    .WithMany()
                    .HasForeignKey(ans => ans.QuestionId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            mb.Entity<OnGoingQuizState>(ogqs =>
            {
                ogqs.HasKey(ogqs => ogqs.Id);

                ogqs.HasOne(ogqs => ogqs.Quiz)
                    .WithMany()
                    .HasForeignKey(ogqs => ogqs.QuizId)
                    .OnDelete(DeleteBehavior.Restrict);

                ogqs.HasOne(ogqs => ogqs.User)
                    .WithMany()
                    .HasForeignKey(ogqs => ogqs.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                ogqs.HasMany(ogqs => ogqs.Answers)
                    .WithOne(anss => anss.OnGoingQuizState)
                    .HasForeignKey(anss => anss.OnGoingQuizStateId);
            });

            mb.Entity<AnswerState>(anss =>
            {
                anss.HasKey(anss => anss.Id);

                anss.HasOne(anss => anss.Question)
                    .WithMany()
                    .HasForeignKey(anss => anss.QuestionId)
                    .OnDelete(DeleteBehavior.Restrict);

                anss.HasOne(anss => anss.OnGoingQuizState)
                    .WithMany()
                    .HasForeignKey(anss => anss.OnGoingQuizStateId)
                    .OnDelete(DeleteBehavior.Cascade);

                anss.Property(e => e.AnswersId)
                    .HasConversion(
                        v => string.Join(',', v),
                        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList()
                    );
            });

            base.OnModelCreating(mb);
            mb.ApplyConfiguration(new RoleConfiguration());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<QuizAttempt> QuizAttempts { get; set; }
        public DbSet<AnswerSelection> AnswerSelections { get; set; }
        public DbSet<OnGoingQuizState> OnGoingQuizStates { get; set; }
        public DbSet<AnswerState> AnswerStates { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using quiz_project.Entities;

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

            base.OnModelCreating(mb);
            mb.ApplyConfiguration(new RoleConfiguration());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
    }
}
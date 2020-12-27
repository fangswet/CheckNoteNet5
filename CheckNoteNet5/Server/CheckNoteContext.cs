﻿using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CheckNoteNet5.Shared.Models;
using Auth = CheckNoteNet5.Shared.Models.Auth;
using System.Collections.Generic;
using CheckNoteNet5.Shared.Models.Auth;

namespace CheckNoteNet5.Server
{
    public class CheckNoteContext : IdentityDbContext<User, Role, int>
    {
        private readonly IConfiguration configuration;

        public CheckNoteContext(DbContextOptions<CheckNoteContext> options, IConfiguration configuration)
            : base(options)
        {
            this.configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(configuration.GetConnectionString("Default"));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Note>(note =>
            {
                note.HasOne(n => n.Parent).WithMany(n => n.Children).OnDelete(DeleteBehavior.Restrict);
                note.HasMany(n => n.Courses).WithMany(c => c.Notes).UsingEntity<Dictionary<string, object>>(
                    "CourseNotes",
                    j => j.HasOne<Course>().WithMany().HasForeignKey("CourseId").OnDelete(DeleteBehavior.Restrict),
                    j => j.HasOne<Note>().WithMany().HasForeignKey("NoteId").OnDelete(DeleteBehavior.Restrict)
                );
            });

            builder.Entity<Course>(course =>
            {
                course.HasMany(c => c.Likes).WithMany(u => u.Likes).UsingEntity<Dictionary<string, object>>(
                    "CourseLikes",
                    j => j.HasOne<User>().WithMany().HasForeignKey("CourseId").OnDelete(DeleteBehavior.Restrict),
                    j => j.HasOne<Course>().WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.Restrict)
                );
                course.HasOne(c => c.Author).WithMany(a => a.Courses); // bug
            });

            builder.Entity<Course.TestResult>(testResult =>
            {
                testResult.Property(tr => tr.Timestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
                testResult.HasOne(tr => tr.Course).WithMany(c => c.TestResults).OnDelete(DeleteBehavior.Restrict);
                testResult.HasOne(tr => tr.User).WithMany(u => u.TestResults).OnDelete(DeleteBehavior.Restrict);
            });
        }

        public DbSet<Note> Notes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Course> Courses { get; set; }
    }
}
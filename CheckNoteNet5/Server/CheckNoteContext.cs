using CheckNoteNet5.Shared.Models.Dtos;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

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
                //.UseLazyLoadingProxies()
                .UseSqlServer(configuration.GetConnectionString("Default"));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasSequence<int>("IDSequence").StartsAt(478261).IncrementsBy(12343);

            builder.Entity<Note>(note =>
            {
                note.Property(n => n.Id).HasDefaultValueSql("NEXT VALUE FOR IDSequence");
                note.HasOne(n => n.Parent).WithMany(n => n.Children).OnDelete(DeleteBehavior.Restrict);
                note.HasMany(n => n.Courses).WithMany(c => c.Notes).UsingEntity<Dictionary<string, object>>
                (
                    "CourseNotes",
                    j => j.HasOne<Course>().WithMany().HasForeignKey("CourseId").OnDelete(DeleteBehavior.Restrict),
                    j => j.HasOne<Note>().WithMany().HasForeignKey("NoteId").OnDelete(DeleteBehavior.Restrict)
                );
            });

            builder.Entity<Tag>(tag =>
            {
                tag.HasMany(t => t.Notes).WithMany(n => n.Tags).UsingEntity<Dictionary<string, object>>
                (
                    "NoteTags",
                    j => j.HasOne<Note>().WithMany().HasForeignKey("NoteId").OnDelete(DeleteBehavior.Restrict),
                    j => j.HasOne<Tag>().WithMany().HasForeignKey("TagId").OnDelete(DeleteBehavior.Restrict)
                );
                tag.HasMany(t => t.Questions).WithMany(n => n.Tags).UsingEntity<Dictionary<string, object>>
                (
                    "QuestionTags",
                    j => j.HasOne<Question>().WithMany().HasForeignKey("QuestionId").OnDelete(DeleteBehavior.Restrict),
                    j => j.HasOne<Tag>().WithMany().HasForeignKey("TagId").OnDelete(DeleteBehavior.Restrict)
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

            builder.Entity<TestResult>(testResult =>
            {
                testResult.Property(tr => tr.Timestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
                testResult.HasOne(tr => tr.Course).WithMany(c => c.TestResults).OnDelete(DeleteBehavior.Restrict);
                testResult.HasOne(tr => tr.User).WithMany(u => u.TestResults).OnDelete(DeleteBehavior.Restrict);
            });
        }

        public DbSet<Note> Notes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Tag> Tags { get; set; }
    }
}

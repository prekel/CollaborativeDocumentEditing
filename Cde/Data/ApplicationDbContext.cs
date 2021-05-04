using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cde.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Document> Documents { get; set; } = null!;
        public DbSet<Update> Updates { get; set; } = null!;
        public DbSet<Project> Projects { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Document>(builder =>
            {
                builder.HasKey(entity => entity.DocumentId);
                builder.Property(entity => entity.IsText).IsRequired();
                builder.Property(entity => entity.Filename).IsRequired();
                builder.Property(entity => entity.S3Link);
                builder.Property(entity => entity.Blob);
            });
            modelBuilder.Entity<Update>(builder =>
            {
                builder.HasKey(e => e.UpdateId);
                builder.Property(e => e.CommentText)
                    .IsRequired();
                builder.HasOne(e => e.Document)
                    .WithOne(d => d!.Update)
                    .HasForeignKey<Update>(e => e.DocumentId)
                    .IsRequired(false);
                builder.HasOne(e => e.Author)
                    .WithOne()
                    .HasForeignKey<Update>(e => e.AuthorId);
            });
            modelBuilder.Entity<Project>(builder =>
            {
                builder.HasKey(entity => entity.ProjectId);
                builder.Property(entity => entity.Name).IsRequired();
                builder.HasMany(entity => entity.Updates)
                    .WithOne(update => update.Project)
                    .HasForeignKey(update => update.ProjectId);
                builder.HasOne(e => e.Owner)
                    .WithMany()
                    .HasForeignKey(project => project.OwnerId);
                builder.HasMany(e => e.InvitedParticipants)
                    .WithMany(p => p.InvitedProjects);
            });
        }
    }
}

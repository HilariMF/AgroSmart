using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Contexts
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public DbSet<FormTierra> FormTierras { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // FormTierra
            modelBuilder.Entity<FormTierra>(builder =>
            {
                builder.ToTable("FormTierra");
                builder.HasKey(t => t.Id);

                builder.Property(t => t.TipoDeSuelo)
                .IsRequired().
                HasMaxLength(100);
                builder.Property(t => t.HumedadTerreno)
                .IsRequired()
                .HasMaxLength(100);
                builder.Property(t => t.Drenaje)
                .IsRequired()

                .HasMaxLength(100);
                builder.Property(t => t.LuzSolar)
                .IsRequired()
                .HasMaxLength(100);
                builder.Property(t => t.TipoDeRiego)
                .IsRequired()
                .HasMaxLength(100);
                builder.Property(t => t.TipoDeFertilizacion)
                .IsRequired()
                .HasMaxLength(100);
                builder.Property(t => t.ProblemasDePlagas)
                .IsRequired();
                builder.Property(t => t.TamanoTerreno)
                .IsRequired()
                .HasMaxLength(100);
                builder.Property(t => t.UsuarioId)
                .IsRequired();

                builder.Property(t => t.Created)
                .IsRequired();
                builder.Property(t => t.LastModified)
                .IsRequired(false);
            });


            // Topic
            modelBuilder.Entity<Topic>(builder =>
            {
                builder.ToTable("Topics");
                builder.HasKey(t => t.Id);

                builder.Property(t => t.Title).IsRequired().HasMaxLength(100);
                builder.Property(t => t.UserId).IsRequired();

                // Relación con Post
                builder.HasMany(t => t.Posts)
                       .WithOne(p => p.Topic)
                       .HasForeignKey(p => p.TopicId)
                       .OnDelete(DeleteBehavior.Restrict);
            });

            //Post
            modelBuilder.Entity<Post>(builder =>
            {

                builder.ToTable("Posts");
                builder.HasKey(p => p.Id);

                builder.Property(p => p.Content).IsRequired().HasMaxLength(1000);
                builder.Property(p => p.UserId).IsRequired();

                // Relación con Topic
                builder.HasOne(p => p.Topic)
                       .WithMany(t => t.Posts)
                       .HasForeignKey(p => p.TopicId)
                       .OnDelete(DeleteBehavior.Restrict);

                builder.Property(p => p.Created).IsRequired();
                builder.Property(p => p.LastModified).IsRequired(false);
            });



        }

    }
}

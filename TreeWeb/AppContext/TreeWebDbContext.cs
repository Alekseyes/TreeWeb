using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TreeWeb.Models;
using Directory = TreeWeb.Models.Directory;


namespace TreeWeb.AppContext
{
    public class TreeWebDbContext : DbContext
    {
        public DbSet<Directory> Directories { get; set; }
        public DbSet<User> Users { get; set; }

        private const string _prefix = "TW";

        public TreeWebDbContext(DbContextOptions<TreeWebDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка таблицы Directory
            modelBuilder.ApplyConfiguration(new DirectoryConfiguration());
            // Настройка таблицы User
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }

        public class UserConfiguration : IEntityTypeConfiguration<User>
        {           
            public void Configure(EntityTypeBuilder<User> builder)
            {
                builder.ToTable($"{_prefix}_{nameof(User)}");
                builder.HasKey(d => d.Id).HasName($"{_prefix}_{nameof(User)}_PK");
                builder.Property(u => u.Username).IsRequired().HasMaxLength(100);
                builder.Property(u => u.Role).IsRequired().HasDefaultValue("User");              
            }
        }

        public class DirectoryConfiguration : IEntityTypeConfiguration<Directory>
        {
            public void Configure(EntityTypeBuilder<Directory> builder)
            {
                builder.ToTable($"{_prefix}_{nameof(Directory)}");
                builder.HasKey(d => d.Id).HasName($"{_prefix}_{nameof(Directory)}_PK");
                builder.Property(d => d.Name).IsRequired().HasMaxLength(255);

                // Связь с родителем
                builder.HasOne(d => d.Parent)
                        .WithMany(d => d.Children)
                        .HasForeignKey(d => d.ParentId).IsRequired(false)
                        .OnDelete(DeleteBehavior.Cascade); // Чтобы при удалении родителя дочерние удалялись

                 builder.HasIndex(d => new { d.ParentId, d.Name })
                        .IsUnique(); // Что бы не было близнецов в одной директории
            }
        }
    }
}

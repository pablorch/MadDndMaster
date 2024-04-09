using MadDndMaster.Dnd.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace MadDndMaster.Data
{
    public class DndContext : DbContext
    {
        private IConfiguration _config;
        public DndContext(IConfiguration config)
        {
            _config = config;
            this.Database.EnsureCreated();
        }

        public DbSet<ClassModel> Classes { get; set; }
        public DbSet<RaceModel> Races { get; set; }
        public DbSet<CharacterModel> Characters { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies()
                .UseSqlServer(_config["ConnectionStrings:DefaultConnection"]);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClassModel>().HasKey(c => c.Id);
            modelBuilder.Entity<RaceModel>().HasKey(r => r.Id);

            modelBuilder.Entity<ClassModel>().HasData(
                new ClassModel { Id = 1, Name = "Barbarian", MaxHP = 30, Attack = 20, Armor = 10 },
                new ClassModel { Id = 2, Name = "Wizard", MaxHP = 10, Attack = 20, Armor = 10 },
                new ClassModel { Id = 3, Name = "Cleric", MaxHP = 15, Attack = 5, Armor = 15 }
            );

            modelBuilder.Entity<RaceModel>().HasData(
                new RaceModel { Id = 1, Name = "Human", AttackModifier = 5, ArmorModifier = 5 },
                new RaceModel { Id = 2, Name = "Elf", AttackModifier = 8, ArmorModifier = 2 },
                new RaceModel { Id = 3, Name = "Dwarf", AttackModifier = 3, ArmorModifier = 7 }
            );

            // Characters table configuration: one main key, two foreign keys and an authomatic generation of ids when adding characters
            var characterEntity = modelBuilder.Entity<CharacterModel>();
            characterEntity.HasKey(r => r.Id);
            characterEntity.Property(p => p.Id).ValueGeneratedOnAdd();
            characterEntity.HasOne(p => p.Class).WithMany().HasForeignKey(p => p.ClassId);
            characterEntity.HasOne(p => p.Race).WithMany().HasForeignKey(p => p.RaceId);
        }
    }
}

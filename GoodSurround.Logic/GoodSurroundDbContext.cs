using GoodSurround.DataModels;
using System.Data.Entity;

namespace GoodSurround.Logic
{
    public sealed class GoodSurroundDbContext : DbContext
    {
        public GoodSurroundDbContext() : base("DefaultConnection") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MixAudio>().HasRequired(x => x.Audio).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Schedule>().HasRequired(x => x.User).WithMany().WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Audio> Audios { get; set; }
        public DbSet<MixAudio> MixAudios { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<ScheduleRow> ScheduleRows { get; set; }
    }
}

using VoteIn.Model.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace VoteIn.DAL
{
    /// <summary>
    /// VoteIn context
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext{VoteIn.Model.Models.User}" />
    public class VoteInContext : IdentityDbContext<User>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VoteInContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public VoteInContext(DbContextOptions<VoteInContext> options) : base(options)
        {
        }

        public DbSet<Act> Act { get; set; }
        public DbSet<Choice> Choice { get; set; }
        public DbSet<VotingProcessMode> VotingProcessMode { get; set; }
        public DbSet<Option> Option { get; set; }
        public DbSet<VotingProcessOption> VotingProcessOption { get; set; }
        public DbSet<VotingProcess> VotingProcess { get; set; }
        public DbSet<Suffrage> Suffrage { get; set; }
        public DbSet<Voter> Voter { get; set; }
        public DbSet<Result> Result { get; set; }
        public DbSet<Envelope> Envelope { get; set; }

        /// <summary>
        /// Override this method to further configure the model that was discovered by convention from the entity types
        /// exposed in <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1" /> properties on your derived context. The resulting model may be cached
        /// and re-used for subsequent instances of your derived context.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context. Databases (and other extensions) typically
        /// define extension methods on this object that allow you to configure aspects of the model that are specific
        /// to a given database.</param>
        /// <remarks>
        /// If a model is explicitly set on the options for this context (via <see cref="M:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)" />)
        /// then this method will not be run.
        /// </remarks>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().ToTable("USER");
            modelBuilder.Entity<User>().Property(u => u.PasswordHash).HasMaxLength(500);
            modelBuilder.Entity<User>().Property(u => u.PhoneNumber).HasMaxLength(50);
        }
    }
}
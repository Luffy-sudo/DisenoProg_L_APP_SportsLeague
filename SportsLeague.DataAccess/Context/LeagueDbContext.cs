using Microsoft.EntityFrameworkCore;
using SportsLeague.Domain.Entities;

namespace SportsLeague.DataAccess.Context
{
    public class LeagueDbContext : DbContext
    {
        public LeagueDbContext(DbContextOptions<LeagueDbContext> options)
            : base(options)
        {
        }

        public DbSet<Team> Teams => Set<Team>();
        public DbSet<Player> Players => Set<Player>();
        public DbSet<Tournament> Tournaments => Set<Tournament>();
        public DbSet<Referee> Referees => Set<Referee>();
        public DbSet<TournamentTeam> TournamentTeams => Set<TournamentTeam>();
        public DbSet<Sponsor> Sponsors => Set<Sponsor>(); // NEW
        public DbSet<TournamentSponsor> TournamentSponsors => Set<TournamentSponsor>(); // NEW

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ── Team Configuration ──
            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Name)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(t => t.City)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(t => t.Stadium)
                      .HasMaxLength(150);
                entity.Property(t => t.LogoUrl)
                      .HasMaxLength(500);
                entity.Property(t => t.CreatedAt)
                      .IsRequired();
                entity.Property(t => t.UpdatedAt)
                      .IsRequired(false);
                entity.HasIndex(t => t.Name)
                      .IsUnique();
            });

            // ── Player Configuration ──
            modelBuilder.Entity<Player>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.FirstName)
                      .IsRequired()
                      .HasMaxLength(80);
                entity.Property(p => p.LastName)
                      .IsRequired()
                      .HasMaxLength(80);
                entity.Property(p => p.BirthDate)
                      .IsRequired();
                entity.Property(p => p.Number)
                      .IsRequired();
                entity.Property(p => p.Position)
                      .IsRequired();
                entity.Property(p => p.CreatedAt)
                      .IsRequired();
                entity.Property(p => p.UpdatedAt)
                      .IsRequired(false);

                // Relación 1:N con Team
                entity.HasOne(p => p.Team)
                      .WithMany(t => t.Players)
                      .HasForeignKey(p => p.TeamId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Índice único compuesto: número de camiseta único por equipo
                entity.HasIndex(p => new { p.TeamId, p.Number })
                      .IsUnique();
            });

            // ── Referee Configuration ──
            modelBuilder.Entity<Referee>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.FirstName)
                      .IsRequired()
                      .HasMaxLength(80);
                entity.Property(r => r.LastName)
                      .IsRequired()
                      .HasMaxLength(80);
                entity.Property(r => r.Nationality)
                      .IsRequired()
                      .HasMaxLength(80);
                entity.Property(r => r.CreatedAt)
                      .IsRequired();
                entity.Property(r => r.UpdatedAt)
                      .IsRequired(false);
            });

            // ── Tournament Configuration ──
            modelBuilder.Entity<Tournament>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Name)
                      .IsRequired()
                      .HasMaxLength(150);
                entity.Property(t => t.Season)
                      .IsRequired()
                      .HasMaxLength(20);
                entity.Property(t => t.StartDate)
                      .IsRequired();
                entity.Property(t => t.EndDate)
                      .IsRequired();
                entity.Property(t => t.Status)
                      .IsRequired();
                entity.Property(t => t.CreatedAt)
                      .IsRequired();
                entity.Property(t => t.UpdatedAt)
                      .IsRequired(false);
            });

            // ── TournamentTeam Configuration ──
            modelBuilder.Entity<TournamentTeam>(entity =>
            {
                entity.HasKey(tt => tt.Id);
                entity.Property(tt => tt.RegisteredAt)
                      .IsRequired();
                entity.Property(tt => tt.CreatedAt)
                      .IsRequired();
                entity.Property(tt => tt.UpdatedAt)
                      .IsRequired(false);

                // Relación con Tournament
                entity.HasOne(tt => tt.Tournament)
                      .WithMany(t => t.TournamentTeams)
                      .HasForeignKey(tt => tt.TournamentId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Relación con Team
                entity.HasOne(tt => tt.Team)
                      .WithMany(t => t.TournamentTeams)
                      .HasForeignKey(tt => tt.TeamId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Índice único compuesto: un equipo solo una vez por torneo
                entity.HasIndex(tt => new { tt.TournamentId, tt.TeamId })
                      .IsUnique();
            });

            // ── Sponsor Configuration ── 
            modelBuilder.Entity<Sponsor>(entity => // NEW
            {
                entity.HasKey(s => s.Id); //NEW
                entity.Property(s => s.Name) // NEW
                      .IsRequired() // NEW
                      .HasMaxLength(150); // NEW
                entity.Property(s => s.ContactEmail) // NEW
                      .IsRequired() // NEW
                      .HasMaxLength(150); // NEW
                entity.Property(s => s.Phone) // NEW
                      .HasMaxLength(20); // NEW
                entity.Property(s => s.WebsiteUrl) // NEW
                      .HasMaxLength(500); // NEW
                entity.Property(s => s.Category) // NEW
                      .IsRequired(); // NEW
                entity.Property(s => s.CreatedAt) // NEW
                      .IsRequired(); // NEW
                entity.Property(s => s.UpdatedAt) // NEW
                      .IsRequired(false); // NEW

                // Índice único en el nombre del patrocinador
                entity.HasIndex(s => s.Name) //NEW
                      .IsUnique(); //NEW
            });

            // ── TournamentSponsor Configuration ──
            modelBuilder.Entity<TournamentSponsor>(entity => // NEW
            {
                entity.HasKey(ts => ts.Id); // NEW
                entity.Property(ts => ts.ContractAmount) // NEW
                      .IsRequired() // NEW
                      .HasColumnType("decimal(18,2)"); // NEW
                entity.Property(ts => ts.JoinedAt) // NEW
                      .IsRequired(); // NEW

                // Relación con Tournament
                entity.HasOne(ts => ts.Tournament) // NEW
                      .WithMany(t => t.TournamentSponsors) // NEW
                      .HasForeignKey(ts => ts.TournamentId) // NEW
                      .OnDelete(DeleteBehavior.Cascade); // NEW

                // Relación con Sponsor
                entity.HasOne(ts => ts.Sponsor) //NEW
                      .WithMany(s => s.TournamentSponsors) //NEW
                      .HasForeignKey(ts => ts.SponsorId) //NEW
                      .OnDelete(DeleteBehavior.Cascade); //NEW

                // Índice único compuesto: un patrocinador solo una vez por torneo
                entity.HasIndex(ts => new { ts.TournamentId, ts.SponsorId })
                      .IsUnique(); //NEW

            });
            }
      }  }  

                

using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();

    public DbSet<Ticket> Tickets => Set<Ticket>();

    public DbSet<Comment> Comments => Set<Comment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(user => user.Id);
            entity.Property(user => user.FullName).IsRequired().HasMaxLength(100);
            entity.Property(user => user.Email).IsRequired().HasMaxLength(150);
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(ticket => ticket.Id);
            entity.Property(ticket => ticket.Title).IsRequired().HasMaxLength(150);
            entity.Property(ticket => ticket.Description).IsRequired().HasMaxLength(1000);
            entity.Property(ticket => ticket.Status).IsRequired().HasMaxLength(30);
            entity.Property(ticket => ticket.Priority).IsRequired().HasMaxLength(30);
            entity.Property(ticket => ticket.CreatedAt).IsRequired();

            entity
                .HasOne(ticket => ticket.AssignedTo)
                .WithMany()
                .HasForeignKey(ticket => ticket.AssignedToId)
                .OnDelete(DeleteBehavior.SetNull);

            entity
                .HasMany(ticket => ticket.Comments)
                .WithOne(comment => comment.Ticket)
                .HasForeignKey(comment => comment.TicketId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(comment => comment.Id);
            entity.Property(comment => comment.Text).IsRequired().HasMaxLength(1000);
            entity.Property(comment => comment.CreatedAt).IsRequired();
        });

        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        var seedDate = new DateTime(2026, 5, 24, 9, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, FullName = "Ayse Demir", Email = "ayse.demir@example.com" },
            new User { Id = 2, FullName = "Mehmet Yilmaz", Email = "mehmet.yilmaz@example.com" },
            new User { Id = 3, FullName = "Zeynep Kaya", Email = "zeynep.kaya@example.com" }
        );

        modelBuilder.Entity<Ticket>().HasData(
            new Ticket
            {
                Id = 1,
                Title = "Login ekraninda hata",
                Description = "Kullanici dogru sifre ile giris yapmasina ragmen hata aliyor.",
                Status = "Open",
                Priority = "High",
                AssignedToId = 1,
                CreatedAt = seedDate
            },
            new Ticket
            {
                Id = 2,
                Title = "Rapor disari aktarma istegi",
                Description = "Aylik talep raporlarinin Excel olarak indirilebilmesi gerekiyor.",
                Status = "In Progress",
                Priority = "Medium",
                AssignedToId = 2,
                CreatedAt = seedDate.AddHours(1),
                UpdatedAt = seedDate.AddHours(3)
            },
            new Ticket
            {
                Id = 3,
                Title = "Bildirim e-postalari gelmiyor",
                Description = "Yeni talep acildiginda sorumlu kullaniciya e-posta ulasmiyor.",
                Status = "Open",
                Priority = "High",
                AssignedToId = 3,
                CreatedAt = seedDate.AddHours(2)
            },
            new Ticket
            {
                Id = 4,
                Title = "Profil sayfasi yavas aciliyor",
                Description = "Profil detaylari yuklenirken bekleme suresi cok uzun.",
                Status = "Resolved",
                Priority = "Low",
                AssignedToId = 1,
                CreatedAt = seedDate.AddHours(4),
                UpdatedAt = seedDate.AddHours(8)
            },
            new Ticket
            {
                Id = 5,
                Title = "Yeni kategori ekleme",
                Description = "Destek talepleri icin kategori secimi eklenmesi isteniyor.",
                Status = "Open",
                Priority = "Medium",
                CreatedAt = seedDate.AddHours(5)
            }
        );

        modelBuilder.Entity<Comment>().HasData(
            new Comment
            {
                Id = 1,
                TicketId = 1,
                Text = "Hata tekrar uretildi, loglar inceleniyor.",
                CreatedAt = seedDate.AddMinutes(30)
            },
            new Comment
            {
                Id = 2,
                TicketId = 2,
                Text = "Excel export icin gerekli alanlar belirlendi.",
                CreatedAt = seedDate.AddHours(2)
            },
            new Comment
            {
                Id = 3,
                TicketId = 4,
                Text = "Sorgu optimizasyonu sonrasi performans iyilesti.",
                CreatedAt = seedDate.AddHours(7)
            }
        );
    }
}

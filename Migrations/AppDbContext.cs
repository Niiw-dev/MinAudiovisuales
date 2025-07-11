using Microsoft.EntityFrameworkCore;
using MinAudiovisual.Servicios.Domain.Entities;
using MinAudiovisual.Diary.Domain.Dto;


namespace MinAudiovisual.Migrations;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Servicio> Servicios => Set<Servicio>();
    public DbSet<DetalleServicio> DetallesServicio => Set<DetalleServicio>();
    public DbSet<DevocionalDto> Devocionales => Set<DevocionalDto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Servicio>()
            .HasOne(s => s.Detalle)
            .WithOne()
            .HasForeignKey<DetalleServicio>(d => d.ServicioId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
using Microsoft.EntityFrameworkCore;
using MinAudiovisual.Servicios.Application.Interfaces;
using MinAudiovisual.Servicios.Domain.Entities;
using MinAudiovisual.Migrations;

namespace MinAudiovisual.Servicios.Infrastructure.Persistence.Repositories;

public class ServicioRepository : IServicioRepository
{
    private readonly AppDbContext _context;

    public ServicioRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Servicio servicio)
    {
        _context.Servicios.Add(servicio);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Servicio>> GetAllAsync()
    {
        return await _context.Servicios
            .Include(s => s.Detalle)
            .ToListAsync();
    }
}
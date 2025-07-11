using MinAudiovisual.Diary.Domain.Dto;
using MinAudiovisual.Diary.Infrastructure.Interfaces;
using MinAudiovisual.Migrations;

namespace MinAudiovisual.Diary.Infrastructure.Repositories;

public class DevocionalRepository : IDevocionalRepository
{
    private readonly AppDbContext _context;

    public DevocionalRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task SaveAsync(DevocionalDto devocional)
    {
        _context.Devocionales.Add(devocional);
        await _context.SaveChangesAsync();
    }
}
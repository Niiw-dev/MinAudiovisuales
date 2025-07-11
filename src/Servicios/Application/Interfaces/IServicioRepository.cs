using MinAudiovisual.Servicios.Domain.Entities;

namespace MinAudiovisual.Servicios.Application.Interfaces;

public interface IServicioRepository
{
    Task AddAsync(Servicio servicio);
    Task<List<Servicio>> GetAllAsync();
}

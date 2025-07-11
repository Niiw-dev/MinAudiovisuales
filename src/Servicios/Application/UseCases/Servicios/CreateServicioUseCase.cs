using MinAudiovisual.Servicios.Application.DTOs;
using MinAudiovisual.Servicios.Application.Interfaces;
using MinAudiovisual.Servicios.Domain.Entities;

namespace MinAudiovisual.Servicios.Application.UseCases.Servicios;

public class CreateServicioUseCase
{
    private readonly IServicioRepository _repo;

    public CreateServicioUseCase(IServicioRepository repo)
    {
        _repo = repo;
    }

    public async Task ExecuteAsync(ServicioDto dto)
    {
        var servicio = new Servicio
        {
            Tipo = dto.Tipo,
            NombreEvento = dto.NombreEvento,
            FechaHora = dto.FechaHora,
            NombreResponsable = dto.NombreResponsable,
            Observaciones = dto.Observaciones,
            Mejoras = dto.Mejoras,
            Detalle = new DetalleServicio
            {
                OrdenCanciones = dto.Detalle.OrdenCanciones,
                HayAnuncios = dto.Detalle.HayAnuncios,
                Anuncios = dto.Detalle.Anuncios,
                Predica = dto.Detalle.Predica,
                CancionMinistracion = dto.Detalle.CancionMinistracion,
                HaySantaCena = dto.Detalle.HaySantaCena,
                CancionSantaCena = dto.Detalle.CancionSantaCena,
                ObservacionesAdicionales = dto.Detalle.ObservacionesAdicionales
            }
        };

        await _repo.AddAsync(servicio);
    }
}

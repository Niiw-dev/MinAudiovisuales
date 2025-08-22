namespace MinAudiovisual.Servicios.Application.DTOs;

public class ServicioDto
{
    public string Tipo { get; set; } = null!;
    public string? NombreEvento { get; set; }
    public DateTime? FechaHora { get; set; }
    public string NombreResponsable { get; set; } = null!;
    public string Observaciones { get; set; } = null!;
    public string? Mejoras { get; set; }

    public DetalleServicioDto Detalle { get; set; } = new();
}
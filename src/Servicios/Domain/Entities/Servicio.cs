namespace MinAudiovisual.Servicios.Domain.Entities;

public class Servicio
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Tipo { get; set; } = null!;
    public string? NombreEvento { get; set; }
    public DateTime FechaHora { get; set; }
    public string NombreResponsable { get; set; } = null!;
    public string Observaciones { get; set; } = null!;
    public string? Mejoras { get; set; }

    public DetalleServicio Detalle { get; set; } = new();
}

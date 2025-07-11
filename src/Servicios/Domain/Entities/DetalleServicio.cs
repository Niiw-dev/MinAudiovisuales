namespace MinAudiovisual.Servicios.Domain.Entities;

public class DetalleServicio
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ServicioId { get; set; }

    public string OrdenCanciones { get; set; } = null!;
    public bool HayAnuncios { get; set; }
    public string? Anuncios { get; set; }
    public string Predica { get; set; } = null!;
    public string CancionMinistracion { get; set; } = null!;
    public bool HaySantaCena { get; set; }
    public string? CancionSantaCena { get; set; }
    public string? ObservacionesAdicionales { get; set; }
}

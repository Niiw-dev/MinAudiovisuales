namespace MinAudiovisual.Diary.Domain.Dto;

public class DevocionalDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public DateTime Fecha { get; set; } = DateTime.Today;
    public DateTime FechaCreado { get; set; } = DateTime.Today;
    public string Tema { get; set; } = string.Empty;
    public string Versiculos { get; set; } = string.Empty;
    public string Promesa { get; set; } = string.Empty;
    public string Practica { get; set; } = string.Empty;
    public string Documento { get; set; } = string.Empty;
}

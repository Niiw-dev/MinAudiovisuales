
using MinAudiovisual.Diary.Application.Interfaces;
using MinAudiovisual.Diary.Domain.Dto;
using Microsoft.AspNetCore.Mvc;

namespace MinAudiovisual.Diary.Infrastructure.ServiceLayer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DevocionalController : ControllerBase
{
    private readonly IDevocionalPdfService _pdfService;

    public DevocionalController(IDevocionalPdfService pdfService)
    {
        _pdfService = pdfService;
    }

    [HttpPost("generar-pdf")]
    public async Task<IActionResult> Generar([FromBody] DevocionalDto dto)
    {
        try
        {
            Console.WriteLine($"Nombre: {dto.Nombre}, Tema: {dto.Tema}, Versículos: {dto.Versiculos}");
            var pdf = await _pdfService.GeneratePdf(dto);
            return File(pdf, "application/pdf", $"devocional_{dto.Fecha:yyyyMMdd}.pdf");
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERROR AL GENERAR PDF: " + ex.Message);
            Console.WriteLine(ex.StackTrace);
            return StatusCode(500, $"Error interno: {ex.Message}");
        }
    }
    
    [HttpPost("subir-devocional")]
    public async Task<IActionResult> Subir([FromForm] IFormFile imagen, [FromForm] string nombre)
    {
        if (imagen == null || imagen.Length == 0)
            return BadRequest("Imagen no válida.");

        using var ms = new MemoryStream();
        await imagen.CopyToAsync(ms);
        var bytes = ms.ToArray();

        var mime = MimeHelper.DetectMime(bytes);
        if (!mime.StartsWith("image/"))
            return BadRequest("Solo se permiten imágenes.");

        var pdfBytes = await _pdfService.GeneratePdfFromImageAsync(bytes, nombre);
        return File(pdfBytes, "application/pdf", $"Imagen_{nombre}.pdf");
    }
}

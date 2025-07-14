using MinAudiovisual.Diary.Application.Interfaces;
using MinAudiovisual.Diary.Domain.Dto;
using MinAudiovisual.Diary.Infrastructure.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace MinAudiovisual.Diary.Application.Services;

public class DevocionalPdfService : IDevocionalPdfService
{
    private readonly IDevocionalRepository _repository;

    public DevocionalPdfService(IDevocionalRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<byte[]> GeneratePdf(DevocionalDto dto)
    {
        var devocional = new DevocionalDto
        {
            Nombre = dto.Nombre,
            Fecha = dto.Fecha,
            FechaCreado = DateTime.Today,
            Tema = dto.Tema,
            Versiculos = dto.Versiculos,
            Promesa = dto.Promesa,
            Practica = dto.Practica,
            Documento = ""
        };
        
        QuestPDF.Settings.License = LicenseType.Community;

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(30);
                page.DefaultTextStyle(x => x.FontSize(12));
                
                page.Content().Column(col =>
                {
                    col.Item().PaddingBottom(20).AlignCenter().Text("Devocional").FontSize(20).Bold();

                    // Primera fila: Nombre y Fecha
                    col.Item().PaddingBottom(20).Row(row =>
                    {
                        row.RelativeItem(3).Column(c =>
                        {
                            c.Item().Text("Nombre:").Bold();
                            c.Item().Text(dto.Nombre);
                        });

                        row.RelativeItem(2).Column(c =>
                        {
                            c.Item().Text("Fecha:").Bold();
                            c.Item().Text($"{dto.Fecha:dd/MM/yyyy}");
                        });
                    });

                    // Segunda fila: Tema
                    col.Item().PaddingBottom(20).Column(c =>
                    {
                        c.Item().Text("Tema:").Bold();
                        c.Item().Text(dto.Tema);
                    });

                    // Tercera fila: Versículos
                    col.Item().PaddingBottom(20).Column(c =>
                    {
                        c.Item().Text("Versículos:").Bold();
                        c.Item().Text(dto.Versiculos).Italic();
                    });

                    // Cuarta fila: Promesa y Práctica (en columnas)
                    col.Item().Row(row =>
                    {
                        row.RelativeItem(1).Column(c =>
                        {
                            c.Item().Text("Promesa o Mandamiento:").Bold();
                            c.Item().Text(dto.Promesa);
                        });

                        row.RelativeItem(1).Column(c =>
                        {
                            c.Item().Text("Para poner en práctica:").Bold();
                            c.Item().Text(dto.Practica);
                        });
                    });

                    // Footer
                    col.Item().AlignCenter().PaddingTop(30).Text("Fuente de Vida Sogamoso");
                });
            });
        });

        using var stream = new MemoryStream();
        document.GeneratePdf(stream);
        
        var pdfBytes = stream.ToArray();
        var base64 = Convert.ToBase64String(pdfBytes);

        devocional.Documento = base64;
        await _repository.SaveAsync(devocional);

        return pdfBytes;
    }

    public async Task<byte[]> GeneratePdfFromImageAsync(byte[] imageBytes, string nombre)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(30);
                page.DefaultTextStyle(x => x.FontSize(12));
                page.PageColor(Colors.White);

                page.Content().Column(col =>
                {
                    col.Item().PaddingBottom(20).AlignCenter().Text(nombre).FontSize(18).Bold();
                    col.Item().AlignCenter()
                        .Height(700)
                        .Image(imageBytes).FitHeight();
                    col.Item().AlignCenter().PaddingTop(20).Text("Fuente de Vida Sogamoso");
                });
            });
        });

        using var stream = new MemoryStream();
        document.GeneratePdf(stream);
        
        var pdfBytes = stream.ToArray();
        var base64 = Convert.ToBase64String(pdfBytes);

        var devocional = new DevocionalDto()
        {
            Nombre = nombre,
            Fecha = DateTime.Today,
            FechaCreado = DateTime.Today,
            Documento = base64
        };
        
        await _repository.SaveAsync(devocional);
        
        return stream.ToArray();
    }

}

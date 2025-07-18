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
                page.Background().AlignCenter().AlignMiddle().Image(Image.FromFile("wwwroot/images/Logo.png")).FitArea();
                
                page.DefaultTextStyle(x => x.FontSize(12));
                
                page.Content().Column(col =>
                {
                    col.Item().Row(row =>
                    {
                        row.RelativeItem().Column(c =>
                        {
                            c.Item().AlignLeft().Text(dto.Nombre).Bold();
                        });

                        row.RelativeItem().Column(c =>
                        {
                            c.Item().AlignRight().Text($"{dto.Fecha:dd/MM/yyyy}").FontSize(10).Bold();
                        });
                    });
                    
                    col.Item().AlignCenter().Text("Devocional").FontSize(22).Bold();
                    col.Item().PaddingBottom(20).AlignCenter().Text(dto.Tema).FontSize(18).Bold();
                    col.Item().PaddingBottom(20).Row(row =>
                    {
                        row.AutoItem().Column(c =>
                        {
                            c.Item().BorderBottom(1).PaddingBottom(5).Text("Versículos:").Bold();
                            c.Item().PaddingTop(5).PaddingLeft(10).Text(dto.Versiculos);
                        });
                    });

                    col.Item().Row(row =>
                    {
                        col.Item().Extend();
                        row.RelativeItem(1).Column(c =>
                        {
                            c.Item().BorderBottom(1).PaddingBottom(5).Text("Promesa o Mandamiento:").Bold();
                            c.Item().PaddingTop(5).PaddingLeft(10).Text(dto.Promesa);
                        });

                        row.RelativeItem(1).Column(c =>
                        {
                            c.Item().BorderBottom(1).PaddingBottom(5).Text("Para poner en práctica:").Bold();
                            c.Item().PaddingTop(5).PaddingLeft(10).Text(dto.Practica);
                        });
                    });

                });
                
                page.Footer().AlignCenter().Column(col =>
                {
                    col.Item().AlignCenter().Text("Fuente de Vida Sogamoso");
                    col.Item().AlignCenter().Text(text =>
                    {
                        text.DefaultTextStyle(x => x.FontSize(10));
                        text.Span("Página ");
                        text.CurrentPageNumber();
                        text.Span(" de ");
                        text.TotalPages();
                    });

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

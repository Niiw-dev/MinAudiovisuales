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
                page.Size(PageSizes.A4.Landscape());
                page.Margin(30);
                page.Background().AlignCenter().AlignMiddle().Image(Image.FromFile("wwwroot/images/LogoII.png")).FitArea();
                
                page.DefaultTextStyle(x => x.FontSize(12));
                
                page.Content().Column(col =>
                {
                    col.Item().Row(row =>
                    {
                        row.RelativeItem().Column(c =>
                        {
                            c.Item().AlignLeft().Text(dto.Nombre);
                        });

                        row.RelativeItem().Column(c =>
                        {
                            c.Item().AlignRight().Text($"{dto.Fecha:dd/MM/yyyy}").FontSize(10);
                        });
                    });
                    
                    col.Item().AlignCenter().Text("Devocional").FontSize(22).Bold();
                    col.Item().PaddingBottom(30).AlignCenter().Text(dto.Tema).FontSize(18).Bold();
                    col.Item().Row(row =>
                    {
                        row.RelativeItem().Column(c =>
                        {
                            c.Item().BorderBottom(1).PaddingBottom(5).Text("Versículos:").Bold();
                            c.Item().PaddingTop(5).PaddingLeft(10).Text(dto.Versiculos);
                        });
                    });

                    col.Item().PaddingTop(25).Row(row =>
                    {
                        col.Item().Extend();
                        row.RelativeItem().PaddingRight(12).Column(c =>
                        {
                            c.Item().BorderBottom(1).PaddingBottom(5).Text("Promesa o Mandamiento:").Bold();
                            c.Item().PaddingTop(5).PaddingLeft(10).Text(dto.Promesa);
                        });

                        row.RelativeItem().Column(c =>
                        {
                            c.Item().BorderBottom(1).PaddingBottom(5).Text("Para poner en práctica:").Bold();
                            c.Item().PaddingTop(5).PaddingLeft(10).Text(dto.Practica);
                        });
                    });

                });
                
                page.Footer().AlignBottom().Column(col =>
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

                page.Header().Column(col =>
                {
                    col.Item().Row(row =>
                    {
                        row.RelativeItem().Column(c =>
                        {
                            c.Item().AlignLeft().Text(nombre).Bold();
                        });

                        row.RelativeItem().Column(c =>
                        {
                            c.Item().AlignRight().Text($"{DateTime.Today:dd/MM/yyyy}").FontSize(10).Bold();
                        });
                    });
                });
                
                page.Content().Column(col =>
                {
                    col.Item().AlignCenter()
                        .AlignMiddle()
                        .Image(imageBytes).FitArea();
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

        var devocional = new DevocionalDto()
        {
            Nombre = nombre,
            Fecha = DateTime.Now,
            FechaCreado = DateTime.Now,
            Documento = base64
        };
        
        await _repository.SaveAsync(devocional);
        
        return stream.ToArray();
    }

}

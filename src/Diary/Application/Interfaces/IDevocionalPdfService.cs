using MinAudiovisual.Diary.Domain.Dto;

namespace MinAudiovisual.Diary.Application.Interfaces;

public interface IDevocionalPdfService
{
    Task<byte[]> GeneratePdf(DevocionalDto dto);
    Task<byte[]> GeneratePdfFromImageAsync(byte[] imageBytes, string nombre);
}

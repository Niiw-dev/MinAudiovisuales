using MinAudiovisual.Diary.Domain.Dto;

namespace MinAudiovisual.Diary.Infrastructure.Interfaces;

public interface IDevocionalRepository
{
    Task SaveAsync(DevocionalDto devocional);

    public Task<List<DevocionalDto>> GetAllAsync();

}
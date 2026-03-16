using EventsManagement.DTOs;

namespace EventsManagement.Repository
{
    public interface IInstitutionsRepository
    {
        Task<InstitutionOutDto?> CreateInstitutionAsync(InstitutionInDto request);
        Task<List<InstitutionOutDto>> GetAllInstutionsAsync();
        Task<InstitutionOutDto?> GetInstitutionAsync(string requestId);
        Task<InstitutionOutDto?> UpdateInstitutionAsync(string institutionId, InstitutionInDto request);
        Task<bool?> DeleteInstitutionAsync(string requestId);

    }
}

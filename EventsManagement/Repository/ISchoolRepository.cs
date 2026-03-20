using EventsManagement.DTOs;

namespace EventsManagement.Repository
{
    public interface ISchoolRepository
    {
        public Task<SchoolOutDto> CreateSchoolAsync(SchoolCreateDto request);
        public Task<SchoolOutDto?> GetSchoolByIdAsync(string schoolId);
        public Task<IEnumerable<SchoolOutDto>> GetAllSchoolsAsync();
        public Task<IEnumerable<SchoolOutDto>> GetAllSchoolsByInstitutionAsync(string institutionId);

        public Task<bool?> DeleteSchoolAsync(string schoolId);
        public Task<SchoolOutDto?> UpdateSchoolAsync(SchoolUpdateDto request);

        Task<IEnumerable<UserOutDto>> GetSchoolMembersAsync(string institutionId);
        Task<IEnumerable<UserOutDto>> GetSchoolLeadersAsync(string institutionId);


    }
}

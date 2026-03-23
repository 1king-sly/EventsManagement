using EventsManagement.DTOs;

namespace EventsManagement.Repository
{
    public interface IClubRepository
    {
        Task<ClubOutDto?> CreateClubAsync(ClubInDto request);

        Task<IEnumerable<ClubOutDto>?> GetAllClubsAsync();
        Task<IEnumerable<ClubOutDto>?> GetAllClubsBySchoolAsync(string schoolId);
        Task<IEnumerable<ClubOutDto>?> GetAllClubsByInstitutionAsync(string institutionId);

        Task<ClubOutDto?> GetClubAsync(string clubId);

        Task<ClubOutDto?> UpdateClubAsync(string clubId, ClubInDto request);

        Task<bool?> DeleteClubAsync(string clubId);

        Task<IEnumerable<UserOutDto>?> GetClubUsersAsync(string clubId);
        Task<IEnumerable<LeaderOutDto>?> GetClubLeadersAsync(string clubId);

        Task<bool> AddClubLeaderAsync(string clubId, LeaderInDto request);
        Task<bool> AddClubMemberAsync(ClubMemberAddDto request);


    }
}

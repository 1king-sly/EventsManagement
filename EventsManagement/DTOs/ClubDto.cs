using EventsManagement.Models;

namespace EventsManagement.DTOs
{
    public class ClubInDto:ClubBase
    {

    }

    public class ClubOutDto : Club { }

    public class ClubMemberAddDto
    {
        public required string userId { get; set; } 

        public required string ClubId {  get; set; }
    }
}

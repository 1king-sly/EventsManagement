using EventsManagement.DTOs;
using EventsManagement.Repository;
using Microsoft.AspNetCore.Mvc;

namespace EventsManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClubController(IClubRepository clubRepository) : ControllerBase
    {

        [HttpGet]

        public async Task<ActionResult<IEnumerable<ClubOutDto>>> GetAllClubsAsync()
        {
            var clubs = await clubRepository.GetAllClubsAsync();

            return clubs is null ? NoContent() : Ok(clubs);

        }
        [HttpGet("institution/{id}")]

        public async Task<ActionResult<IEnumerable<ClubOutDto>>> GetAllInstitutionClubsAsync(string id)
        {
            var clubs = await clubRepository.GetAllClubsByInstitutionAsync(id);

            return clubs is null ? NoContent() : Ok(clubs);

        }
        [HttpGet("school/{id}")]

        public async Task<ActionResult<IEnumerable<ClubOutDto>>> GetAllSchoolClubsAsync(string id)
        {
            var clubs = await clubRepository.GetAllClubsBySchoolAsync(id);

            return clubs is null ? NoContent() : Ok(clubs);

        }
        [HttpGet("{id}")]

        public async Task<ActionResult<ClubOutDto>> GetClubAsync(string id)
        {
            var club = await clubRepository.GetClubAsync(id);

            return club is null ? NotFound("Club does not exist yet!") : Ok(club);

        }
        [HttpPut("{id}")]

        public async Task<ActionResult<ClubOutDto>> UpdateClubAsync(string id,ClubInDto request)
        {
            var club = await clubRepository.UpdateClubAsync(id,request);

            return club is null ? NotFound("Club does not exist yet!") : Ok(club);

        }
        [HttpDelete("{id}")]

        public async Task<ActionResult> DeleteClubAsync(string id)
        {
            var club = await clubRepository.DeleteClubAsync(id);

            return club is null ? NotFound("Club does not exist yet!") :club is false? StatusCode(500,"Something went wrong") : NoContent();

        }
        [HttpPost]

        public async Task<ActionResult<ClubOutDto>> CreateClubAsync(ClubInDto request)
        {
            var club = await clubRepository.CreateClubAsync(request);

            return club is null ? BadRequest("Club with name already exists") : StatusCode(201,club);

        }
    }
}

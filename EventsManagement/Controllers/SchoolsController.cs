using EventsManagement.DTOs;
using EventsManagement.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventsManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolsController(ISchoolRepository schoolRepository) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SchoolOutDto>>> GetAllSchoolsAsync()
        {
            var schools = await schoolRepository.GetAllSchoolsAsync();

            return Ok(schools);
        }
        [HttpGet("{institutionId}")]
        public async Task<ActionResult<IEnumerable<SchoolOutDto>>> GetAllSchoolsByInstitutionIdAsync(string institutionId)
        {
            var schools = await schoolRepository.GetAllSchoolsByInstitutionAsync(institutionId);


            return schools is null ? NotFound("Institution does not exist") : Ok(schools);
        }
        [HttpGet("/school/{schoolId}")]
        public async Task<ActionResult<SchoolOutDto>> GetSchoolByIdAsync(string schoolId)
        {
            var school = await schoolRepository.GetSchoolByIdAsync(schoolId);


            return school is null ? NotFound("School does not exist") : Ok(school);
        }

        [HttpGet("/school/members/{schoolId}")]

        public async Task<ActionResult<IEnumerable<UserOutDto>>> GetSchoolMembersAsync(string schoolId)
        {
            var users = await schoolRepository.GetSchoolMembersAsync(schoolId);

            return users is null ? BadRequest() : Ok(users);
        }
        [HttpPut("/school/{schoolId}")]
        public async Task<ActionResult<SchoolOutDto>> UpdateSchoolAsync(string schoolId, SchoolUpdateDto request)
        {

            if (schoolId != request.schoolId) return BadRequest();
            var school = await schoolRepository.UpdateSchoolAsync(request);


            return school is null ? NotFound("School does not exist") : Ok(school);
        }
        [HttpDelete("/school/{schoolId}")]
        public async Task<ActionResult> DeleteSchoolAsync(string schoolId)
        {

            var school = await schoolRepository.DeleteSchoolAsync(schoolId);


            return school is null ? NotFound("School does not exist") : NoContent();
        }


    }
}

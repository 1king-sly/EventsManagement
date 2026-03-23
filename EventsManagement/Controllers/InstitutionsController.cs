using EventsManagement.DTOs;
using EventsManagement.Models;
using EventsManagement.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventsManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstitutionsController(IInstitutionsRepository repository) : ControllerBase
    {
        [Authorize]
        [HttpPost]

        public async Task<ActionResult<InstitutionOutDto>> CreateInstitutionAsync(InstitutionInDto request)
        {
            var institution = await repository.CreateInstitutionAsync(request);

            return institution is null ? BadRequest("Instiution with name or email already exists") : Ok(institution);
        }


        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<InstitutionOutDto>>> GetInstitutionsAsync()
        {
            var institutions = await repository.GetAllInstutionsAsync();


            return Ok(institutions);
        }

        [Authorize]
        [HttpGet("{institutionId}")]

        public async Task<ActionResult<InstitutionOutDto>> GetInstitutionByIdAsync(string institutionId) {
            var institution = await repository.GetInstitutionAsync(institutionId);

            return institution is null ? BadRequest() : Ok(institution);
        }
        [Authorize]
        [HttpGet("/members/{institutionId}")]

        public async Task<ActionResult<IEnumerable<UserOutDto>>> GetInstitutionMembersAsync(string institutionId) {
            var users = await repository.GetInstitutionMembersAsync(institutionId);

            return users is null ? BadRequest() : Ok(users);
        }

        [Authorize]
        [HttpPut("{institutionId}")]

        public async Task<ActionResult<InstitutionOutDto>> UpdateInstitutionAsync(string institutionId, InstitutionInDto request)
        {
            var institution = await repository.UpdateInstitutionAsync(institutionId, request);
            return institution is null ? BadRequest() : Ok(institution);
        }

        [Authorize]
        [HttpDelete("{institutionId}")]

        public async Task<ActionResult> DeleteInstiutionAsync(string institutionId)
        {
            var result = await repository.DeleteInstitutionAsync(institutionId);

            return result is null ? NotFound("Institution does not exist") :result is false ? BadRequest() : NoContent();
        }
    }
}

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
        [HttpGet("{id}")]

        public async Task<ActionResult<InstitutionOutDto>> GetInstitutionByIdAsync(string id) {
            var institution = await repository.GetInstitutionAsync(id);

            return institution is null ? BadRequest() : Ok(institution);
        }
        [Authorize]
        [HttpGet("members/{id}")]

        public async Task<ActionResult<IEnumerable<UserOutDto>>> GetInstitutionMembersAsync(string id) {
            var users = await repository.GetInstitutionMembersAsync(id);

            return users is null ? BadRequest() : Ok(users);
        }

        [Authorize]
        [HttpPut("{id}")]

        public async Task<ActionResult<InstitutionOutDto>> UpdateInstitutionAsync(string id, InstitutionInDto request)
        {
            var institution = await repository.UpdateInstitutionAsync(id, request);
            return institution is null ? BadRequest() : Ok(institution);
        }

        [Authorize]
        [HttpDelete("{id}")]

        public async Task<ActionResult> DeleteInstiutionAsync(string id)
        {
            var result = await repository.DeleteInstitutionAsync(id);

            return result is null ? NotFound("Institution does not exist") :result is false ? BadRequest() : NoContent();
        }
    }
}

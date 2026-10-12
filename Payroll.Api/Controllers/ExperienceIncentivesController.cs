using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Payroll.Application.DTOs.ExperienceIncentives;
using Payroll.Application.Interfaces.ServiceIntefaces;

namespace Payroll.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExperienceIncentivesController(IExperienceIncentiveService service) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateExperienceIncentiveDto request, CancellationToken cancellationToken)
        {
            var incentive = await service.CreateAsync(request, cancellationToken);

            return StatusCode(
                StatusCodes.Status201Created,
                incentive);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateExperienceIncentiveDto request, CancellationToken cancellationToken)
        {
            await service.UpdateAsync(request, cancellationToken);

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var incentives = await service.GetAllAsync(cancellationToken);

            return Ok(incentives);
        }
    }
}

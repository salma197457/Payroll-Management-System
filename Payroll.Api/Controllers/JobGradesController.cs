using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Payroll.Application.DTOs.JobGrades;
using Payroll.Application.Interfaces.ServiceIntefaces;

namespace Payroll.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobGradesController(IJobGradesService service) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateJobGradeDto request, CancellationToken cancellationToken)
        {
            var jobGrade = await service.CreateAsync(request, cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = jobGrade.Id },
                jobGrade);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateJobGradeDto request, CancellationToken cancellationToken)
        {
            await service.UpdateAsync(request, cancellationToken);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var jobGrades = await service.GetAllAsync(cancellationToken);

            return Ok(jobGrades);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var jobGrade = await service.GetByIdAsync(id, cancellationToken);

            return Ok(jobGrade);
        }
    }
}

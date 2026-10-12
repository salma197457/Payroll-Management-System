using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Payroll.Application.DTOs.EmployeeAbsences;
using Payroll.Application.Interfaces.ServiceIntefaces;

namespace Payroll.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeAbsencesController(IEmployeeAbsenceService service) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEmployeeAbsenceDto request, CancellationToken cancellationToken)
        {
            var absence = await service.CreateAsync(request, cancellationToken);

            return StatusCode(StatusCodes.Status201Created, absence);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber, [FromQuery] int pageSize, CancellationToken cancellationToken)
        {
            var absences = await service.GetAllAsync(pageNumber, pageSize, cancellationToken);

            return Ok(absences);
        }
    }
}

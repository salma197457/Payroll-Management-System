using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Payroll.Application.DTOs.AbsenceDeductionRules;
using Payroll.Application.Interfaces.ServiceIntefaces;

namespace Payroll.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AbsenceDeductionRuleController(IAbsenceDeductionRuleService service) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAbsenceDeductionRuleDto request, CancellationToken cancellationToken)
        {
            var rule = await service.CreateAsync(request, cancellationToken);

            return StatusCode(
                StatusCodes.Status201Created,
                rule);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateAbsenceDeductionRuleDto request, CancellationToken cancellationToken)
        {
            await service.UpdateAsync(request, cancellationToken);

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var rules = await service.GetAllAsync(cancellationToken);

            return Ok(rules);
        }
    }
}

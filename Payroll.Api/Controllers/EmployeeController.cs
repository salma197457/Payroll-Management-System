using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Payroll.Application.DTOs;
using Payroll.Application.DTOs.Employees;
using Payroll.Application.Interfaces.ServiceIntefaces;

namespace Payroll.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController(IEmployeeService service) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEmployeeDto request, CancellationToken cancellationToken)
        {
            var employee = await service.CreateAsync(request, cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = employee.Id },
                employee);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateEmployeeDto request, CancellationToken cancellationToken)
        {
            await service.UpdateAsync(request, cancellationToken);

            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteEmployeeDto request, CancellationToken cancellationToken)
        {
            await service.DeleteAsync(request, cancellationToken);

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber, [FromQuery] int pageSize, CancellationToken cancellationToken)
        {
            var employees = await service.GetAllAsync(pageNumber, pageSize, cancellationToken);

            return Ok(employees);
        }
        [HttpGet("EmployeeFilter")]
        public async Task<IActionResult> GetAllforFilter(CancellationToken cancellationToken)
        {
            var employees = await service.GetAllforFilterAsync(cancellationToken);

            return Ok(employees);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var employee = await service.GetByIdAsync(id, cancellationToken);

            return Ok(employee);
        }
    }
}

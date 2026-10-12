using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Payroll.Application.DTOs.Departments;
using Payroll.Application.Exceptions;
using Payroll.Application.Interfaces.ServiceIntefaces;

namespace Payroll.Api.Controllers
{
    [ApiController]
    [Route("api/departments")]
    public class DepartmentsController(IDepartmentService _service) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateDepartmentDto request, CancellationToken cancellationToken)
        {
            var department = await _service.CreateAsync(request, cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = department.Id },
                department);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            return Ok(await _service.GetAllAsync(cancellationToken));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var department = await _service.GetByIdAsync(id, cancellationToken);

            return department is null ? NotFound() : Ok(department);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateDepartmentDto request,CancellationToken cancellationToken)
        {
            var updated = await _service.UpdateAsync(request, cancellationToken);

            return updated ? Ok(): NotFound();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(DeleteDepartmentDto request, CancellationToken cancellationToken)
        {
            await _service.DeleteAsync(request, cancellationToken);

            return NoContent();
        }
    }
}

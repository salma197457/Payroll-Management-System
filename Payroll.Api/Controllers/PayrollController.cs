using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Payroll.Application.Interfaces.ServiceIntefaces;

namespace Payroll.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Obsolete]
    public class PayrollsController(IPayrollService payrollService) : ControllerBase
    {
        //[HttpGet]
        //public async Task<IActionResult> GetAll(
        //    CancellationToken cancellationToken)
        //{
        //    var payrolls = await payrollService.GetAllAsync(cancellationToken);

        //    return Ok(payrolls);
        //}

        //[HttpGet("latest-month")]
        //public async Task<IActionResult> GetLatestMonth(
        //    CancellationToken cancellationToken)
        //{
        //    var payrolls = await payrollService.GetLatestMonthAsync(
        //        cancellationToken);

        //    return Ok(payrolls);
        //}

        //[HttpGet("employees/{employeeId:int}/latest")]
        //public async Task<IActionResult> GetLatestByEmployee([FromRoute] int employeeId, CancellationToken cancellationToken)
        //{
        //    var payroll = await payrollService.GetLatestByEmployeeAsync(
        //        employeeId,
        //        cancellationToken);

        //    return Ok(payroll);
        //}
    }
}

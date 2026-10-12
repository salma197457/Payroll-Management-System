using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Payroll.Application.DTOs.Reports.Attendance;
using Payroll.Application.DTOs.Reports.EmployeeReport;
using Payroll.Application.DTOs.Reports.IncentiveDeductionReport;
using Payroll.Application.DTOs.Reports.SalaryReport;
using Payroll.Application.Interfaces.ServiceIntefaces;

namespace Payroll.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController(IReportService reportService) : ControllerBase
    {
        [HttpGet("employees")]
        public async Task<IActionResult> GetEmployeesReport([FromQuery] EmployeeReportFilterDto filter, CancellationToken cancellationToken)
        {
            var report = await reportService.GetEmployeesReportAsync(filter, cancellationToken);

            return Ok(report);
        }

        [HttpGet("attendance")]
        public async Task<IActionResult> GetAttendanceReport([FromQuery] AttendanceReportFilterDto filter, CancellationToken cancellationToken)
        {
            var report = await reportService.GetAttendanceReportAsync(filter, cancellationToken);

            return Ok(report);
        }

        [HttpGet("incentives-deductions")]
        public async Task<IActionResult> GetIncentivesDeductionsReport([FromQuery] IncentivesDeductionsReportFilterDto filter, CancellationToken cancellationToken)
        {
            var report = await reportService.GetIncentivesDeductionsReportAsync(filter, cancellationToken);

            return Ok(report);
        }

        [HttpGet("salaries")]
        public async Task<IActionResult> GetSalariesReport([FromQuery] SalaryReportFilterDto filter, CancellationToken cancellationToken)
        {
            var report = await reportService.GetSalariesReportAsync(filter, cancellationToken);

            return Ok(report);
        }
    }
}

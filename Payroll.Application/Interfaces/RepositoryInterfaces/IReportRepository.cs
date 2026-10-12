using Payroll.Application.DTOs;
using Payroll.Application.DTOs.Reports.Attendance;
using Payroll.Application.DTOs.Reports.EmployeeReport;
using Payroll.Application.DTOs.Reports.IncentiveDeductionReport;
using Payroll.Application.DTOs.Reports.SalaryReport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Application.Interfaces.RepositoryInterfaces
{
    public interface IReportRepository
    {
        Task<PagedResultDto<EmployeeReportDto>> GetEmployeesReportAsync(EmployeeReportFilterDto filter, CancellationToken cancellationToken = default);
        Task<PagedResultDto<AttendanceReportDto>> GetAttendanceReportAsync(AttendanceReportFilterDto filter, CancellationToken cancellationToken = default);
        Task<IncentivesDeductionsReportResultDto> GetIncentivesDeductionsReportAsync(IncentivesDeductionsReportFilterDto filter, CancellationToken cancellationToken = default);
        Task<PagedResultDto<SalaryReportDto>> GetSalariesReportAsync(SalaryReportFilterDto filter, CancellationToken cancellationToken = default); 
            
    }
}

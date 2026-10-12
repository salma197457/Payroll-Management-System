using Payroll.Application.DTOs;
using Payroll.Application.DTOs.Reports.Attendance;
using Payroll.Application.DTOs.Reports.EmployeeReport;
using Payroll.Application.DTOs.Reports.IncentiveDeductionReport;
using Payroll.Application.DTOs.Reports.SalaryReport;
using Payroll.Application.Interfaces.RepositoryInterfaces;
using Payroll.Application.Interfaces.ServiceIntefaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Application.Services
{
    public class ReportService(IReportRepository reportRepository) : IReportService
    {
        public async Task<PagedResultDto<EmployeeReportDto>> GetEmployeesReportAsync(EmployeeReportFilterDto filter, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(filter);

            return await reportRepository.GetEmployeesReportAsync(filter, cancellationToken);
        }

        public async Task<PagedResultDto<AttendanceReportDto>> GetAttendanceReportAsync(AttendanceReportFilterDto filter, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(filter);

            return await reportRepository.GetAttendanceReportAsync(filter, cancellationToken);
        }
        public async Task<IncentivesDeductionsReportResultDto> GetIncentivesDeductionsReportAsync(IncentivesDeductionsReportFilterDto filter, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(filter);

            return await reportRepository.GetIncentivesDeductionsReportAsync(filter, cancellationToken);
        }

        public async Task<PagedResultDto<SalaryReportDto>> GetSalariesReportAsync(SalaryReportFilterDto filter, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(filter);

            return await reportRepository.GetSalariesReportAsync(filter, cancellationToken);
        }
    }
}

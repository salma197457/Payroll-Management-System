using Microsoft.EntityFrameworkCore;
using Payroll.Application.DTOs;
using Payroll.Application.DTOs.Reports.Attendance;
using Payroll.Application.DTOs.Reports.EmployeeReport;
using Payroll.Application.DTOs.Reports.IncentiveDeductionReport;
using Payroll.Application.DTOs.Reports.SalaryReport;
using Payroll.Application.Interfaces.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Infrastructure.Repositories
{
    public class ReportRepository(PayrollDbContext context) : IReportRepository
    {
        public async Task<PagedResultDto<EmployeeReportDto>> GetEmployeesReportAsync(EmployeeReportFilterDto filter, CancellationToken cancellationToken = default)
        {
            var query = context.Employees.AsNoTracking().AsQueryable();

            if (filter.EmployeeId.HasValue)
            {
                query = query.Where(x => x.Id == filter.EmployeeId.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.EmployeeName))
            {
                var name = filter.EmployeeName.Trim();

                query = query.Where(x => x.Name.ToLower().Contains(name.ToLower()));
            }

            if (filter.DepartmentId.HasValue)
            {
                query = query.Where(x => x.DepartmentId == filter.DepartmentId.Value);
            }

            if (filter.JobGradeId.HasValue)
            {
                query = query.Where(x => x.JobGradeId == filter.JobGradeId.Value);
            }
            var totalCount = await query.CountAsync(cancellationToken);

            var employees = await query.OrderBy(x => x.Name).ThenBy(x => x.Id)
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize)
                .Select(x => new EmployeeReportDto
                {
                    EmployeeId = x.Id,
                    EmployeeName = x.Name,

                    DepartmentId = x.DepartmentId,
                    DepartmentName = x.Department.Name,

                    JobGradeId = x.JobGradeId,
                    JobGradeName = x.JobGrade.Name,

                    BaseSalary = x.JobGrade.Salary,
                    JoinDate = x.JoinDate
                })
                .ToListAsync(cancellationToken);

            return new PagedResultDto<EmployeeReportDto>
            {
                Items = employees,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }


        public async Task<PagedResultDto<AttendanceReportDto>> GetAttendanceReportAsync(AttendanceReportFilterDto filter, CancellationToken cancellationToken = default)
        {
            var fromDate = filter.FromDate!.Value;
            var toDate = filter.ToDate!.Value;

            var query = context.Employees.AsNoTracking().Where(x => x.JoinDate <= toDate);

            if (filter.EmployeeId.HasValue)
                query = query.Where(x =>x.Id == filter.EmployeeId.Value);

            var totalCount = await query.CountAsync(cancellationToken);


            var employees = await query.OrderBy(x => x.Name).ThenBy(x => x.Id).Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize)
                    .Select(x => new
                    {
                        x.Id,
                        x.Name,
                        x.JoinDate,

                        AbsenceDates = context.EmployeeAbsences
                            .Where(a =>
                                a.EmployeeId == x.Id &&
                                a.Date >= fromDate &&
                                a.Date <= toDate)
                            .Select(a => a.Date)
                            .ToList()
                    })
                    .ToListAsync(cancellationToken);

            var items = employees.Select(employee =>
            {
                var effectiveFromDate = employee.JoinDate > fromDate ? employee.JoinDate : fromDate;

                var workingDays = toDate.DayNumber - effectiveFromDate.DayNumber + 1;

                var absenceDays = employee.AbsenceDates.Count(date => date >= effectiveFromDate);

                return new AttendanceReportDto
                {
                    EmployeeId = employee.Id,
                    EmployeeName = employee.Name,
                    WorkingDays = workingDays,
                    AbsenceDays = absenceDays,
                    AttendanceDays = workingDays - absenceDays
                };
            }).ToList();

            return new PagedResultDto<AttendanceReportDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }


        public async Task<IncentivesDeductionsReportResultDto> GetIncentivesDeductionsReportAsync(IncentivesDeductionsReportFilterDto filter, CancellationToken cancellationToken = default)
        {
            var query = context.Payrolls.IgnoreQueryFilters().AsNoTracking().AsQueryable();

            if (filter.Year.HasValue)
                query = query.Where(x => x.Year == filter.Year.Value);

            if (filter.Month.HasValue)
                query = query.Where(x => x.Month == filter.Month.Value);

            if (filter.EmployeeId.HasValue)
                query = query.Where(x => x.EmployeeId == filter.EmployeeId.Value);

            var totalCount = await query.CountAsync(cancellationToken);

            var totals = await query.GroupBy(x => 1)
                .Select(g => new
                {
                    TotalIncentives = g.Sum(x =>
                        x.DepartmentIncentive
                        + x.ExperienceIncentive
                        + (x.AttendanceAdjustment > 0
                            ? x.AttendanceAdjustment
                            : 0m)),

                    TotalDeductions = g.Sum(x =>
                        x.AttendanceAdjustment < 0
                            ? -x.AttendanceAdjustment
                            : 0m)
                })
                .FirstOrDefaultAsync(cancellationToken);

            var items = await query
               .OrderByDescending(x => x.Year)
               .ThenByDescending(x => x.Month)
               .ThenBy(x => x.Employee.Name)
               .ThenBy(x => x.Id)
               .Skip((filter.PageNumber - 1) * filter.PageSize)
               .Take(filter.PageSize)
               .Select(x => new IncentivesDeductionsReportDto
               {
                   EmployeeId = x.EmployeeId,
                   EmployeeName = x.Employee.Name,

                   Year = x.Year,
                   Month = x.Month,

                   DepartmentIncentive = x.DepartmentIncentive,
                   ExperienceIncentive = x.ExperienceIncentive,

                   AttendanceIncentive =
                       x.AttendanceAdjustment > 0
                           ? x.AttendanceAdjustment
                           : 0m,

                   AbsenceDeduction =
                       x.AttendanceAdjustment < 0
                           ? -x.AttendanceAdjustment
                           : 0m,

                   TotalIncentives =
                       x.DepartmentIncentive
                       + x.ExperienceIncentive
                       + (x.AttendanceAdjustment > 0
                           ? x.AttendanceAdjustment
                           : 0m),

                   TotalDeductions =
                       x.AttendanceAdjustment < 0
                           ? -x.AttendanceAdjustment
                           : 0m
               })
               .ToListAsync(cancellationToken);

            return new IncentivesDeductionsReportResultDto
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalIncentives = totals?.TotalIncentives ?? 0m,
                TotalDeductions = totals?.TotalDeductions ?? 0m
            };
        }

        public async Task<PagedResultDto<SalaryReportDto>> GetSalariesReportAsync(SalaryReportFilterDto filter, CancellationToken cancellationToken = default)
        {
            var query = context.Payrolls.IgnoreQueryFilters().AsNoTracking().AsQueryable();

            if (filter.Year.HasValue)
                query = query.Where(x => x.Year == filter.Year.Value);
            

            if (filter.Month.HasValue)
                query = query.Where(x => x.Month == filter.Month.Value);
            

            if (filter.EmployeeId.HasValue)
                query = query.Where(x => x.EmployeeId == filter.EmployeeId.Value);

            var totalCount = await query.CountAsync(cancellationToken);


            var items = await query
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Month)
                .ThenBy(x => x.Employee.Name)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(x => new SalaryReportDto
                {
                    PayrollId = x.Id,

                    EmployeeId = x.EmployeeId,
                    EmployeeName = x.Employee.Name,

                    DepartmentName = x.Employee.Department.Name,
                    JobGradeName = x.Employee.JobGrade.Name,

                    Year = x.Year,
                    Month = x.Month,

                    BaseSalary = x.BaseSalary,

                    DepartmentIncentive = x.DepartmentIncentive,
                    ExperienceIncentive = x.ExperienceIncentive,

                    AttendanceIncentive = x.AttendanceAdjustment > 0 ? x.AttendanceAdjustment : 0m,

                    TotalIncentives = x.DepartmentIncentive + x.ExperienceIncentive + 
                    (x.AttendanceAdjustment > 0 ? x.AttendanceAdjustment : 0m),

                    AbsenceDeduction = x.AttendanceAdjustment < 0
                        ? -x.AttendanceAdjustment : 0m,

                    TotalDeductions = x.AttendanceAdjustment < 0 ? -x.AttendanceAdjustment : 0m,

                    NetSalary = x.NetSalary
                })
                .ToListAsync(cancellationToken);

            return new PagedResultDto<SalaryReportDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }
    }
}
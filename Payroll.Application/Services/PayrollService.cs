using Payroll.Application.DTOs.Payrolls;
using Payroll.Application.Interfaces;
using Payroll.Application.Interfaces.RepositoryInterfaces;
using Payroll.Application.Interfaces.ServiceIntefaces;
using Payroll.Domain.Calculator;
using Payroll.Domain.Calculator.CustomEntities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Payroll.Application.Services
{
    public class PayrollService(
    IPayrollRepository payrollRepository,
    IEmployeeRepository employeeRepository,
    IJobGradesRepository jobGradeRepository,
    IDepartmentIncentiveRepository departmentIncentiveRepository,
    IExperienceIncentiveRepository experienceIncentiveRepository,
    IAbsenceDeductionRuleRepository absenceRuleRepository,
    IEmployeeAbsenceRepository employeeAbsenceRepository,
    IUnitOfWork unitOfWork) : IPayrollService
    {
        public async Task GenerateMonthlyPayrollAsync(int year,int month,CancellationToken cancellationToken = default)
        {

            var startDate = new DateOnly(year, month, 1);
            var endDate = new DateOnly(year, month, DateTime.DaysInMonth(year, month));

            var today = DateOnly.FromDateTime(DateTime.Now);

            if (endDate >= today)
                throw new ArgumentException("Payroll can only be generated for completed months");

            var employees = await employeeRepository.GetAllAsync(cancellationToken);

            var jobGrades = await jobGradeRepository.GetAllAsync(cancellationToken);

            var departmentIncentives = await departmentIncentiveRepository.GetAllAsync(cancellationToken);

            var experienceIncentives = await experienceIncentiveRepository.GetAllAsync(cancellationToken);

            var absenceRules = await absenceRuleRepository.GetAllAsync(cancellationToken);

            foreach (var employee in employees)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (employee.JoinDate > endDate)
                    continue;

                var exists = await payrollRepository.AnyAsync(
                    x => x.EmployeeId == employee.Id &&
                    x.Year == year &&
                    x.Month == month,
                    cancellationToken);

                if (exists)
                    throw new ArgumentException($"Payroll already exists for employee {employee.Id}");

                var jobGrade = jobGrades.FirstOrDefault(x => x.Id == employee.JobGradeId);

                if (jobGrade is null)
                    throw new KeyNotFoundException($"Job grade not found for employee {employee.Id}");

                //var baseSalary = jobGrade.Salary;
                /////////
                var daysInMonth = DateTime.DaysInMonth(year, month);

                var effectiveStartDate = employee.JoinDate > startDate ? employee.JoinDate : startDate;

                var eligibleDays = endDate.DayNumber - effectiveStartDate.DayNumber + 1;

                var baseSalary = Math.Round(jobGrade.Salary * eligibleDays / daysInMonth, 2, MidpointRounding.AwayFromZero);
                /////

                var departmentPercentage = departmentIncentives.FirstOrDefault(x => x.DepartmentId == employee.DepartmentId)?.Percentage ?? 0m;

                var experiencePercentage = experienceIncentives.Where(x => employee.JoinDate.AddYears(x.MinimumYears) <= endDate)
                    .OrderByDescending(x => x.MinimumYears)
                    .Select(x => x.Percentage)
                    .FirstOrDefault();

                var absenceDays = await employeeAbsenceRepository.CountAbsencesAsync(employee.Id, startDate, endDate, cancellationToken);

                
                var calculation = PayrollCalculator.Calculate(
                    new PayrollCalculationInput(
                        baseSalary,
                        departmentPercentage,
                        experiencePercentage,
                        absenceDays,
                        absenceRules));

                var payroll = Payroll.Domain.Entities.Payroll.Create(employee.Id, year, month, calculation);

                await payrollRepository.AddAsync(payroll, cancellationToken);
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        //        private static readonly Expression<Func<Payroll.Domain.Entities.Payroll, PayrollDto>>
        //    PayrollProjection = payroll => new PayrollDto
        //    {
        //        Id = payroll.Id,
        //        EmployeeId = payroll.EmployeeId,
        //        EmployeeName = payroll.Employee.Name,
        //        Year = payroll.Year,
        //        Month = payroll.Month,
        //        BaseSalary = payroll.BaseSalary,
        //        DepartmentIncentive = payroll.DepartmentIncentive,
        //        ExperienceIncentive = payroll.ExperienceIncentive,
        //        AttendanceAdjustment = payroll.AttendanceAdjustment,
        //        NetSalary = payroll.NetSalary
        //    };
        //        public async Task<IReadOnlyList<PayrollDto>> GetAllAsync(CancellationToken cancellationToken = default)
        //        {
        //            return await payrollRepository.GetAllProjectedAsync(
        //                PayrollProjection,
        //                cancellationToken);
        //        }

        //        public async Task<PayrollDto> GetLatestByEmployeeAsync(int employeeId, CancellationToken cancellationToken = default)
        //        {
        //            if (employeeId <= 0)
        //                throw new ArgumentException("Invalid employee ID.");

        //            var payroll = await payrollRepository.GetLatestByEmployeeAsync(employeeId, PayrollProjection, cancellationToken);

        //            return payroll ?? throw new KeyNotFoundException(
        //                "No payroll records found for this employee.");
        //        }

        //        public async Task<IReadOnlyList<PayrollDto>> GetLatestMonthAsync(CancellationToken cancellationToken = default)
        //        {
        //            return await payrollRepository.GetLatestMonthAsync(PayrollProjection, cancellationToken);
        //        }
        //    }
    }
}

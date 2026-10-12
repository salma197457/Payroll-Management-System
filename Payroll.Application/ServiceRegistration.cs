using Microsoft.Extensions.DependencyInjection;
using Payroll.Application.Interfaces.ServiceIntefaces;
using Payroll.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Application
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplication(
        this IServiceCollection services)
        {
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IJobGradesService, JobGradesService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IDepartmentIncentiveService, DepartmentIncentiveService>();
            services.AddScoped<IExperienceIncentiveService, ExperienceIncentiveService>();
            services.AddScoped<IAbsenceDeductionRuleService, AbsenceDeductionRuleService>();
            services.AddScoped<IEmployeeAbsenceService, EmployeeAbsenceService>();
            services.AddScoped<IPayrollService, PayrollService>();
            services.AddScoped<IReportService, ReportService>();

            return services;
        }
    }
}

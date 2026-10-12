using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Payroll.Application.Interfaces;
using Payroll.Application.Interfaces.RepositoryInterfaces;
using Payroll.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Infrastructure
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<PayrollDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IJobGradesRepository, JobGradesRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IDepartmentIncentiveRepository,DepartmentIncentiveRepository>();
            services.AddScoped<IExperienceIncentiveRepository,ExperienceIncentiveRepository>();
            services.AddScoped<IAbsenceDeductionRuleRepository, AbsenceDeductionRuleRepository>();
            services.AddScoped<IEmployeeAbsenceRepository, EmployeeAbsenceRepository>();
            services.AddScoped<IPayrollRepository, PayrollRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}

using Payroll.Application.DTOs;
using Payroll.Application.DTOs.Employees;
using Payroll.Application.Interfaces;
using Payroll.Application.Interfaces.RepositoryInterfaces;
using Payroll.Application.Interfaces.ServiceIntefaces;
using Payroll.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Payroll.Application.Services
{
    public class EmployeeService(
    IEmployeeRepository employeeRepository,
    IDepartmentRepository departmentRepository,
    IJobGradesRepository jobGradeRepository,
    IUnitOfWork unitOfWork) : IEmployeeService
    {
        public async Task<EmployeeDto> CreateAsync(CreateEmployeeDto request, CancellationToken cancellationToken = default)
        {
            var departmentExists = await departmentRepository.AnyAsync(d => d.Id == request.DepartmentId, cancellationToken);

            if (!departmentExists)
                throw new KeyNotFoundException("Department not found");

            var jobGradeExists = await jobGradeRepository.AnyAsync(j => j.Id == request.JobGradeId, cancellationToken);

            if (!jobGradeExists)
                throw new KeyNotFoundException("Job grade not found");

            var employee = Employee.Create(
                request.Name,
                request.BirthDate,
                request.Address,
                request.PhoneNumber,
                request.Email,
                request.JoinDate,
                request.DepartmentId,
                request.JobGradeId);

            await employeeRepository.AddAsync(employee, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new EmployeeDto
            {
                Id = employee.Id,
                Name = employee.Name,
                BirthDate = employee.BirthDate,
                Address = employee.Address,
                PhoneNumber = employee.PhoneNumber,
                Email = employee.Email,
                JoinDate = employee.JoinDate,
                DepartmentId = employee.DepartmentId,
                JobGradeId = employee.JobGradeId,
                RowVersion = employee.RowVersion
            };
        }

        public async Task UpdateAsync(UpdateEmployeeDto request, CancellationToken cancellationToken = default)
        {
            var employee = await employeeRepository.GetForUpdateAsync(request.Id, request.RowVersion, cancellationToken);

            var departmentExists = await departmentRepository.AnyAsync(d => d.Id == request.DepartmentId, cancellationToken);

            if (!departmentExists)
                throw new KeyNotFoundException("Department not found.");

            var jobGradeExists = await jobGradeRepository.AnyAsync(j => j.Id == request.JobGradeId, cancellationToken);

            if (!jobGradeExists)
                throw new KeyNotFoundException("Job grade not found.");

            employee.Update(
                request.Name,
                request.BirthDate,
                request.Address,
                request.PhoneNumber,
                request.Email,
                request.DepartmentId,
                request.JobGradeId);

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(DeleteEmployeeDto request, CancellationToken cancellationToken = default)
        {
            var employee = await employeeRepository.GetForUpdateAsync(request.Id, request.RowVersion, cancellationToken);

            employee.Delete();

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }


        private static readonly Expression<Func<Employee, EmployeeDto>>
            EmployeeProjection = e => new EmployeeDto
            {
                Id = e.Id,
                Name = e.Name,
                BirthDate = e.BirthDate,
                Address = e.Address,
                PhoneNumber = e.PhoneNumber,
                Email = e.Email,
                JoinDate = e.JoinDate,

                DepartmentId = e.DepartmentId,
                DepartmentName = e.Department.Name,

                JobGradeId = e.JobGradeId,
                JobGradeName = e.JobGrade.Name,
                BaseSalary = e.JobGrade.Salary,

                RowVersion = e.RowVersion
            };

        public async Task<PagedResultDto<EmployeeDto>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var totalCount = await employeeRepository.CountAsync(cancellationToken);
            
            var employees =  await employeeRepository.GetAllProjectedAsync(EmployeeProjection, cancellationToken,pageNumber,pageSize);
            
            return new PagedResultDto<EmployeeDto>
            {
                Items = employees.ToList(),
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
        public async Task<IReadOnlyList<EmployeeDto>> GetAllforFilterAsync(CancellationToken cancellationToken = default)
        {
            
            return await employeeRepository.GetAllProjectedAsync(EmployeeProjection, cancellationToken);
            
        }
        public async Task<EmployeeDto> GetByIdAsync(int id,CancellationToken cancellationToken = default)
        {
            var employee = await employeeRepository.GetByIdProjectedAsync(id, EmployeeProjection, cancellationToken);

            return employee ?? throw new KeyNotFoundException("Employee not found.");
        }
    }
}


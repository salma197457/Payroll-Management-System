using Payroll.Application.DTOs;
using Payroll.Application.DTOs.EmployeeAbsences;
using Payroll.Application.DTOs.Employees;
using Payroll.Application.Exceptions;
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
    public class EmployeeAbsenceService(IEmployeeAbsenceRepository absenceRepository, IEmployeeRepository employeeRepository, IUnitOfWork unitOfWork) : IEmployeeAbsenceService
    {
        public async Task<EmployeeAbsenceDto> CreateAsync(CreateEmployeeAbsenceDto request, CancellationToken cancellationToken = default)
        {
            var employee = await employeeRepository.AnyAsync(x => x.Id == request.EmployeeId, cancellationToken);

            if (!employee)
                throw new KeyNotFoundException("Employee not found");

            var exists = await absenceRepository.AnyAsync(x => x.EmployeeId == request.EmployeeId && x.Date == request.Date, cancellationToken);

            if (exists)
                throw new ConflictException("An absence record already exists for this employee on this date");

            var absence = EmployeeAbsence.Create(request.EmployeeId, request.Date);

            await absenceRepository.AddAsync(absence, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new EmployeeAbsenceDto
            {
                Id = absence.Id,
                EmployeeId = absence.EmployeeId,
                Date = absence.Date
            };
        }

        private static readonly Expression<Func<EmployeeAbsence, EmployeeAbsenceDto>>
            AbsenceProjection = absence => new EmployeeAbsenceDto
            {
                Id = absence.Id,
                EmployeeId = absence.EmployeeId,
                EmployeeName = absence.Employee.Name,
                Date = absence.Date
            };
        public async Task<PagedResultDto<EmployeeAbsenceDto>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var totalCount = await absenceRepository.CountAsync(cancellationToken);

            var employees = await absenceRepository.GetAllProjectedAsync(AbsenceProjection, cancellationToken,pageNumber,pageSize);

            return new PagedResultDto<EmployeeAbsenceDto>
            {
                Items = employees.ToList(),
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

       
    }
}
    
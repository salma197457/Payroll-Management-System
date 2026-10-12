using Payroll.Application.DTOs;
using Payroll.Application.DTOs.EmployeeAbsences;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Application.Interfaces.ServiceIntefaces
{
    public interface IEmployeeAbsenceService
    {
        Task<EmployeeAbsenceDto> CreateAsync(CreateEmployeeAbsenceDto request, CancellationToken cancellationToken = default);
        Task<PagedResultDto<EmployeeAbsenceDto>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);    }
}

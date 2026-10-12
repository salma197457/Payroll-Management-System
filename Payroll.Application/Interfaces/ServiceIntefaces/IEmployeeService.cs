using Payroll.Application.DTOs;
using Payroll.Application.DTOs.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Application.Interfaces.ServiceIntefaces
{
    public interface IEmployeeService
    {
        Task<EmployeeDto> CreateAsync(CreateEmployeeDto request, CancellationToken cancellationToken = default);
        Task UpdateAsync(UpdateEmployeeDto request, CancellationToken cancellationToken = default);
        Task DeleteAsync(DeleteEmployeeDto request, CancellationToken cancellationToken = default);
        //Task<IReadOnlyList<EmployeeDto>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<EmployeeDto> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<PagedResultDto<EmployeeDto>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<EmployeeDto>> GetAllforFilterAsync(CancellationToken cancellationToken = default);
    }
}

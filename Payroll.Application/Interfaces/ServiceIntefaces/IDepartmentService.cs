using Payroll.Application.DTOs.Departments;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Application.Interfaces.ServiceIntefaces
{
    public interface IDepartmentService
    {
        Task<IReadOnlyList<DepartmentDto>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<DepartmentDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<DepartmentDto> CreateAsync(CreateDepartmentDto request, CancellationToken cancellationToken = default);

        Task<bool> UpdateAsync(UpdateDepartmentDto request, CancellationToken cancellationToken = default);

        Task DeleteAsync(DeleteDepartmentDto request, CancellationToken cancellationToken = default);
    }


}

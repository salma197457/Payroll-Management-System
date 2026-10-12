using Payroll.Application.DTOs.DepartmentIncentives;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Application.Interfaces.ServiceIntefaces
{
    public interface IDepartmentIncentiveService
    {
        Task<DepartmentIncentiveDto> CreateAsync(CreateDepartmentIncentiveDto request, CancellationToken cancellationToken = default);
        Task UpdateAsync(UpdateDepartmentIncentiveDto request, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<DepartmentIncentiveDto>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}

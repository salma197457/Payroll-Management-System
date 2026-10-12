using Payroll.Application.DTOs.ExperienceIncentives;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Application.Interfaces.ServiceIntefaces
{
    public interface IExperienceIncentiveService
    {
        Task<ExperienceIncentiveDto> CreateAsync(CreateExperienceIncentiveDto request, CancellationToken cancellationToken = default);

        Task UpdateAsync(UpdateExperienceIncentiveDto request, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<ExperienceIncentiveDto>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}

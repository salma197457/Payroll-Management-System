using Payroll.Application.DTOs.JobGrades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Application.Interfaces.ServiceIntefaces
{
    public interface IJobGradesService
    {
        Task<JobGradeDto> CreateAsync(CreateJobGradeDto request, CancellationToken cancellationToken = default);
        Task UpdateAsync(UpdateJobGradeDto request, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<JobGradeDto>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<JobGradeDto> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    }
}

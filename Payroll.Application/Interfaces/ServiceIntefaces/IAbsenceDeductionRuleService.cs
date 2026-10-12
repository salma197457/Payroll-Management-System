using Payroll.Application.DTOs.AbsenceDeductionRules;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Application.Interfaces.ServiceIntefaces
{
    public interface IAbsenceDeductionRuleService
    {
        Task<AbsenceDeductionRuleDto> CreateAsync(CreateAbsenceDeductionRuleDto request, CancellationToken cancellationToken = default);
        Task UpdateAsync(UpdateAbsenceDeductionRuleDto request, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<AbsenceDeductionRuleDto>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}

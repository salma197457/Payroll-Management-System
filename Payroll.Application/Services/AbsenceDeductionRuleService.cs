using Payroll.Application.DTOs.AbsenceDeductionRules;
using Payroll.Application.Exceptions;
using Payroll.Application.Interfaces;
using Payroll.Application.Interfaces.RepositoryInterfaces;
using Payroll.Application.Interfaces.ServiceIntefaces;
using Payroll.Domain.Rules;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Payroll.Application.Services
{
    public class AbsenceDeductionRuleService(IAbsenceDeductionRuleRepository ruleRepository,IUnitOfWork unitOfWork) :IAbsenceDeductionRuleService
    {
        public async Task<AbsenceDeductionRuleDto> CreateAsync(CreateAbsenceDeductionRuleDto request, CancellationToken cancellationToken = default)
        {

            var exists = await ruleRepository.AnyAsync(x => x.MinimumDays == request.MinimumDays,cancellationToken);

            if (exists)
                throw new ConflictException("An absence rule already exists for this type");

            var rule = AbsenceDeductionRule.Create(request.MinimumDays, request.Percentage);

            await ruleRepository.AddAsync(rule, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new AbsenceDeductionRuleDto
            {
                Id = rule.Id,
                MinimumDays = rule.MinimumDays,
                Percentage = rule.Percentage,
                Type = rule.Type.ToString(),
                RowVersion = rule.RowVersion
            };
        }
        public async Task UpdateAsync(UpdateAbsenceDeductionRuleDto request, CancellationToken cancellationToken = default)
        {
            var rule = await ruleRepository.GetForUpdateAsync(request.Id, request.RowVersion, cancellationToken);

            rule.UpdatePercentage(request.Percentage);

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        private static readonly Expression<Func<AbsenceDeductionRule, AbsenceDeductionRuleDto>>
            RuleProjection = rule => new AbsenceDeductionRuleDto
            {
                Id = rule.Id,
                MinimumDays = rule.MinimumDays,
                Percentage = rule.Percentage,
                Type = rule.Type.ToString(),
                RowVersion = rule.RowVersion
            };

        public async Task<IReadOnlyList<AbsenceDeductionRuleDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await ruleRepository.GetAllProjectedAsync(RuleProjection, cancellationToken);
        }
    }
}


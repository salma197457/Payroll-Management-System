using Payroll.Application.DTOs.ExperienceIncentives;
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
    public class ExperienceIncentiveService(IExperienceIncentiveRepository incentiveRepository, IUnitOfWork unitOfWork) : IExperienceIncentiveService
    {
        public async Task<ExperienceIncentiveDto> CreateAsync(CreateExperienceIncentiveDto request, CancellationToken cancellationToken = default)
        {
            var exists = await incentiveRepository.AnyAsync(x => x.MinimumYears == request.MinimumYears, cancellationToken);

            if (exists)
                throw new ConflictException("An experience incentive already exists for this threshold");

            var incentive = ExperienceIncentive.Create(request.MinimumYears, request.Percentage);

            await incentiveRepository.AddAsync(incentive, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new ExperienceIncentiveDto
            {
                Id = incentive.Id,
                MinimumYears = incentive.MinimumYears,
                Percentage = incentive.Percentage,
                RowVersion = incentive.RowVersion
            };
        }

        public async Task UpdateAsync(UpdateExperienceIncentiveDto request, CancellationToken cancellationToken = default)
        {
            var incentive = await incentiveRepository.GetForUpdateAsync(request.Id, request.RowVersion, cancellationToken);

            incentive.UpdatePercentage(request.Percentage);

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        private static readonly Expression<Func<ExperienceIncentive, ExperienceIncentiveDto>>
            IncentiveProjection = incentive => new ExperienceIncentiveDto
            {
                Id = incentive.Id,
                MinimumYears = incentive.MinimumYears,
                Percentage = incentive.Percentage,
                RowVersion = incentive.RowVersion
            };

        public async Task<IReadOnlyList<ExperienceIncentiveDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await incentiveRepository.GetAllProjectedAsync(IncentiveProjection, cancellationToken);
        }
    }
}

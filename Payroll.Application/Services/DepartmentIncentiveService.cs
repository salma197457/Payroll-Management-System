using Payroll.Application.DTOs.DepartmentIncentives;
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
    public class DepartmentIncentiveService(
        IDepartmentIncentiveRepository incentiveRepository,
        IDepartmentRepository departmentRepository,
        IUnitOfWork unitOfWork) : IDepartmentIncentiveService
    {
        public async Task<DepartmentIncentiveDto> CreateAsync(CreateDepartmentIncentiveDto request, CancellationToken cancellationToken = default)
        {
            var department = await departmentRepository.AnyAsync(j => j.Id == request.DepartmentId, cancellationToken);

            if (!department)
                throw new KeyNotFoundException("Department not found.");

            var incentiveExists = await incentiveRepository.AnyAsync(i => i.DepartmentId == request.DepartmentId, cancellationToken);

            if (incentiveExists)
                throw new ConflictException("An incentive already exists for this department.");

            var incentive = DepartmentIncentive.Create(request.DepartmentId, request.Percentage);

            await incentiveRepository.AddAsync(incentive, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new DepartmentIncentiveDto
            {
                Id = incentive.Id,
                DepartmentId = incentive.DepartmentId,
                Percentage = incentive.Percentage,
                RowVersion = incentive.RowVersion
            };
        }

        public async Task UpdateAsync(UpdateDepartmentIncentiveDto request, CancellationToken cancellationToken = default)
        {
            var incentive = await incentiveRepository.GetForUpdateAsync(request.Id, request.RowVersion, cancellationToken);

            incentive.UpdatePercentage(request.Percentage);

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        private static readonly Expression<Func<DepartmentIncentive, DepartmentIncentiveDto>>
        IncentiveProjection = incentive => new DepartmentIncentiveDto
        {
            Id = incentive.Id,
            DepartmentId = incentive.DepartmentId,
            DepartmentName = incentive.Department.Name,
            Percentage = incentive.Percentage,
            RowVersion = incentive.RowVersion
        };
        public async Task<IReadOnlyList<DepartmentIncentiveDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await incentiveRepository.GetAllProjectedAsync(IncentiveProjection, cancellationToken);
        }
    }   
}

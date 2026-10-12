using Payroll.Application.DTOs.Departments;
using Payroll.Application.DTOs.JobGrades;
using Payroll.Application.Exceptions;
using Payroll.Application.Interfaces;
using Payroll.Application.Interfaces.RepositoryInterfaces;
using Payroll.Application.Interfaces.ServiceIntefaces;
using Payroll.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Payroll.Application.Services
{
    public class JobGradesService : IJobGradesService
    {
        private readonly IJobGradesRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public JobGradesService(IJobGradesRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<JobGradeDto> CreateAsync(CreateJobGradeDto request, CancellationToken cancellationToken = default)
        {
            var name = request.Name.Trim();

            var exists = await _repository.AnyAsync(j => j.Name.ToLower() == name.ToLower() || j.Salary == request.Salary, cancellationToken);
            if (exists)
                throw new ConflictException("JobGrade already exists.");

            var jobGrade = JobGrade.Create(name, request.Salary);


            await _repository.AddAsync(jobGrade, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new JobGradeDto
            {
                Id = jobGrade.Id,
                Name = jobGrade.Name,
                Salary = jobGrade.Salary,
                RowVersion = jobGrade.RowVersion
            };
        }
        public async Task UpdateAsync(UpdateJobGradeDto request, CancellationToken cancellationToken = default)
        {
            var exists = await _repository.AnyAsync(j => j.Salary == request.Salary, cancellationToken);
            if (exists)
                throw new ConflictException("Salary already exists.");

            var jobGrade = await _repository.GetForUpdateAsync(request.Id, request.RowVersion, cancellationToken);

            jobGrade.SetSalary(request.Salary);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<JobGradeDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _repository.GetAllProjectedAsync(
                 jg => new JobGradeDto
                 {
                     Id = jg.Id,
                     Name = jg.Name,
                     Salary = jg.Salary,
                     RowVersion = jg.RowVersion
                 },
                cancellationToken);
        }
        public async Task<JobGradeDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var jobGrade = await _repository.GetByIdProjectedAsync(id,
                jg => new JobGradeDto
                {
                    Id = jg.Id,
                    Name = jg.Name,
                    Salary = jg.Salary,
                    RowVersion = jg.RowVersion
                }, cancellationToken);

            return jobGrade ?? throw new KeyNotFoundException("Job grade not found.");
        }
    }
}

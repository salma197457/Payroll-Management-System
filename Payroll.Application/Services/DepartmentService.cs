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
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentService(IDepartmentRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task<DepartmentDto> CreateAsync(CreateDepartmentDto request, CancellationToken cancellationToken = default)
        {
            var name = request.Name.Trim();
            var exists = await _repository.AnyAsync(j => j.Name.ToLower() == name.ToLower(), cancellationToken);
            if (exists)
                throw new ConflictException("Department name already exists");


            var department = Department.Create(name);


            await _repository.AddAsync(department, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new DepartmentDto
            {
                Id = department.Id,
                Name = department.Name,
                RowVersion = department.RowVersion
            };
        }
        public async Task<IReadOnlyList<DepartmentDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var departments = await _repository.GetAllProjectedAsync(
                d => new DepartmentDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    RowVersion = d.RowVersion
                }, cancellationToken);

            return departments;
        }
        public async Task<DepartmentDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var department = await _repository.GetByIdProjectedAsync(id,
                d => new DepartmentDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    RowVersion = d.RowVersion
                },
                cancellationToken);

            return department;
        }
        public async Task<bool> UpdateAsync(UpdateDepartmentDto request, CancellationToken cancellationToken = default)
        {
            var department = await _repository.GetForUpdateAsync(request.Id, request.RowVersion, cancellationToken);

            var name = request.Name.Trim();

            var exists = await _repository.AnyAsync(d => d.Name.ToLower() == name.ToLower() && d.Id != request.Id, cancellationToken);

            if (exists)
                throw new ConflictException("Department name already exists.");

            department.Rename(name);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task DeleteAsync(DeleteDepartmentDto request, CancellationToken cancellationToken = default)
        {
            var department = await _repository.GetForUpdateAsync(request.Id, request.RowVersion, cancellationToken);

            if (await _repository.HasActiveEmployeesAsync(request.Id, cancellationToken))
                throw new ConflictException("Cannot delete a department with active employees.");
            
            department.Delete();

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
       
    }
}
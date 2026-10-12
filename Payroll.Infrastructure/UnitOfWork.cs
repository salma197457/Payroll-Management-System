using Microsoft.EntityFrameworkCore;
using Payroll.Application.Exceptions;
using Payroll.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PayrollDbContext _context;

        public UnitOfWork(PayrollDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}

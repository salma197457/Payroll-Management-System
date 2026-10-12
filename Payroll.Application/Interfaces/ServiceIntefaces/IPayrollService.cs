using Payroll.Application.DTOs.Payrolls;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Application.Interfaces.ServiceIntefaces
{
    public interface IPayrollService
    {
        Task GenerateMonthlyPayrollAsync(int year, int month, CancellationToken cancellationToken = default);

        //Task<IReadOnlyList<PayrollDto>> GetAllAsync(CancellationToken cancellationToken = default);

        //Task<PayrollDto> GetLatestByEmployeeAsync(int employeeId, CancellationToken cancellationToken = default);
        //Task<IReadOnlyList<PayrollDto>> GetLatestMonthAsync(CancellationToken cancellationToken = default);
    }
}

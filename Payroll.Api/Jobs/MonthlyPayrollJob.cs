using Payroll.Application.Interfaces.ServiceIntefaces;

namespace Payroll.Api.Jobs
{
    public class MonthlyPayrollJob(IPayrollService payrollService)
    {
        public async Task ExecuteAsync()
        {
            var previousMonth = DateTime.Now.AddMonths(-1);

            await payrollService.GenerateMonthlyPayrollAsync(
                previousMonth.Year,
                previousMonth.Month);
        }
    }
}

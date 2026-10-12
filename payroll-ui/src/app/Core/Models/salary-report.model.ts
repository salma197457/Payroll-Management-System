
export interface SalaryReport {
  payrollId: number;

  employeeId: number;
  employeeName: string;

  year: number;
  month: number;

  baseSalary: number;

  departmentIncentive: number;
  experienceIncentive: number;
  attendanceIncentive: number;
  totalIncentives: number;

  absenceDeduction: number;
  totalDeductions: number;

  netSalary: number;
}

export interface SalaryReportFilter {
  year?: number;
  month?: number;
  employeeId?: number;
  pageNumber?: number;
  pageSize?: number;
}

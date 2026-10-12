
export interface IncentivesDeductionsReport {
  employeeId: number;
  employeeName: string;

  year: number;
  month: number;

  departmentIncentive: number;
  experienceIncentive: number;
  attendanceIncentive: number;
  absenceDeduction: number;

  totalIncentives: number;
  totalDeductions: number;
}

export interface IncentivesDeductionsReportFilter {
  year?: number;
  month?: number;
  employeeId?: number;
  pageNumber?: number;
  pageSize?: number;
}

export interface IncentivesDeductionsReportResult {
  items: IncentivesDeductionsReport[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
  totalIncentives: number;
  totalDeductions: number;
}

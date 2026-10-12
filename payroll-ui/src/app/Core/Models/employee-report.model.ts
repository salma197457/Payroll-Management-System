
export interface EmployeeReport {
  employeeId: number;
  employeeName: string;

  departmentId: number;
  departmentName: string;

  jobGradeId: number;
  jobGradeName: string;

  baseSalary: number;
  joinDate: string;
}

export interface EmployeeReportFilter {
  employeeId?: number;
  employeeName?: string;
  departmentId?: number;
  jobGradeId?: number;
  pageNumber?: number;
  pageSize?: number;
}

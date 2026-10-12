
export interface AttendanceReport {
  employeeId: number;
  employeeName: string;
  workingDays: number;
  absenceDays: number;
  attendanceDays: number;
}

export interface AttendanceReportFilter {
  fromDate: string;
  toDate: string;
  employeeId?: number;
  pageNumber?: number;
  pageSize?: number;
}

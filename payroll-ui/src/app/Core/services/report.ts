    import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import {AttendanceReport,AttendanceReportFilter} from '../Models/attendance-report.model';
import { PagedResult } from '../Models/paged-result.model';

import {
  EmployeeReport,
  EmployeeReportFilter
} from '../Models/employee-report.model';

import {
  IncentivesDeductionsReport,
  IncentivesDeductionsReportFilter,
  IncentivesDeductionsReportResult
} from '../Models/incentives-deductions-report.model';

import {
  SalaryReport,
  SalaryReportFilter
} from '../Models/salary-report.model';

@Injectable({
  providedIn: 'root'
})
export class ReportService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = '/api/Reports';

 
getEmployeesReport(
  filter: EmployeeReportFilter = {}
): Observable<PagedResult<EmployeeReport>> {

  let params = new HttpParams()
    .set('PageNumber', filter.pageNumber ?? 1)
    .set('PageSize', filter.pageSize ?? 10);

  if (filter.employeeId != null) {
    params = params.set('EmployeeId', filter.employeeId);
  }

  if (filter.employeeName?.trim()) {
    params = params.set('EmployeeName', filter.employeeName.trim());
  }

  if (filter.departmentId != null) {
    params = params.set('DepartmentId', filter.departmentId);
  }

  if (filter.jobGradeId != null) {
    params = params.set('JobGradeId', filter.jobGradeId);
  }

  return this.http.get<PagedResult<EmployeeReport>>(
    '/api/Reports/employees',
    { params }
  );
}



getAttendanceReport(
  filter: AttendanceReportFilter
): Observable<PagedResult<AttendanceReport>> {

  let params = new HttpParams()
    .set('FromDate', filter.fromDate)
    .set('ToDate', filter.toDate)
    .set('PageNumber', filter.pageNumber ?? 1)
    .set('PageSize', filter.pageSize ?? 10);

  if (filter.employeeId != null) {
    params = params.set('EmployeeId', filter.employeeId);
  }

  return this.http.get<PagedResult<AttendanceReport>>(
    '/api/Reports/attendance',
    { params }
  );
}



getIncentivesDeductionsReport(
  filter: IncentivesDeductionsReportFilter = {}
): Observable<IncentivesDeductionsReportResult> {

  let params = new HttpParams()
    .set('PageNumber', filter.pageNumber ?? 1)
    .set('PageSize', filter.pageSize ?? 10);

  if (filter.year != null) {
    params = params.set('Year', filter.year);
  }

  if (filter.month != null) {
    params = params.set('Month', filter.month);
  }

  if (filter.employeeId != null) {
    params = params.set('EmployeeId', filter.employeeId);
  }

  return this.http.get<IncentivesDeductionsReportResult>(
    '/api/Reports/incentives-deductions',
    { params }
  );
}

getSalariesReport(
    filter: SalaryReportFilter = {}
  ): Observable<PagedResult<SalaryReport>> {
    let params = new HttpParams()
      .set('PageNumber', filter.pageNumber ?? 1)
      .set('PageSize', filter.pageSize ?? 10);

    if (filter.year != null) {
      params = params.set('Year', filter.year);
    }

    if (filter.month != null) {
      params = params.set('Month', filter.month);
    }

    if (filter.employeeId != null) {
      params = params.set('EmployeeId', filter.employeeId);
    }

    return this.http.get<PagedResult<SalaryReport>>(
      `${this.apiUrl}/salaries`,
      { params }
    );
  }
}
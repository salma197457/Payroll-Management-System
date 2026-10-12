
import { Component, OnInit, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

import { ReportService } from '../../../Core/services/report';
import { DepartmentService } from '../../../Core/services/department';
import { JobGradeService } from '../../../Core/services/job-grade';

import {
  EmployeeReport as EmployeeReportModel,
  EmployeeReportFilter
} from '../../../Core/Models/employee-report.model';

import { Department } from '../../../Core/Models/department.model';
import { JobGrade } from '../../../Core/Models/job-grade.model';

@Component({
  selector: 'app-employee-report',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './employee-report.html',
  styleUrl: './employee-report.css'
})
export class EmployeeReport implements OnInit {

  private readonly reportService = inject(ReportService);
  private readonly departmentService = inject(DepartmentService);
  private readonly jobGradeService = inject(JobGradeService);

  employees = signal<EmployeeReportModel[]>([]);
  departments = signal<Department[]>([]);
  jobGrades = signal<JobGrade[]>([]);

  selectedEmployeeId = signal<number | null>(null);
  selectedEmployeeName = signal('');

  selectedDepartmentId = signal(0);
  selectedJobGradeId = signal(0);

  pageNumber = signal(1);
  pageSize = signal(10);
  totalCount = signal(0);
  totalPages = signal(0);

  loading = signal(false);
  errorMessage = signal('');
  hasLoaded = signal(false);

  appliedFilters = signal('All employees');

  ngOnInit(): void {
    this.loadReport();

    this.departmentService.getAll().subscribe({
      next: data => this.departments.set(data),
      error: () => this.errorMessage.set(
        'Failed to load departments.'
      )
    });

    this.jobGradeService.getAll().subscribe({
      next: data => this.jobGrades.set(data),
      error: () => this.errorMessage.set(
        'Failed to load job grades.'
      )
    });
  }

  loadReport(): void {
    const employeeId = this.selectedEmployeeId();
    const employeeName = this.selectedEmployeeName().trim();

    if (
      employeeId != null &&
      (!Number.isInteger(employeeId) || employeeId < 1)
    ) {
      this.errorMessage.set('Please enter a valid Employee ID.');
      return;
    }

    const filter: EmployeeReportFilter = {
      employeeId: employeeId ?? undefined,
      employeeName: employeeName || undefined,

      departmentId: this.selectedDepartmentId() || undefined,
      jobGradeId: this.selectedJobGradeId() || undefined,

      pageNumber: this.pageNumber(),
      pageSize: this.pageSize()
    };

    this.loading.set(true);
    this.errorMessage.set('');

    this.reportService.getEmployeesReport(filter).subscribe({
      next: response => {
        this.employees.set(response.items);

        this.totalCount.set(response.totalCount);
        this.totalPages.set(response.totalPages);
        this.pageNumber.set(response.pageNumber);

        this.appliedFilters.set(
          this.getFilterDescription(filter)
        );

        this.hasLoaded.set(true);
        this.loading.set(false);
      },

      error: (error: HttpErrorResponse) => {
        this.errorMessage.set(
          error.error?.detail ||
          'Failed to load employee report.'
        );

        this.employees.set([]);
        this.totalCount.set(0);
        this.totalPages.set(0);

        this.hasLoaded.set(false);
        this.loading.set(false);
      }
    });
  }

  applyFilters(): void {
    this.pageNumber.set(1);
    this.loadReport();
  }

  resetFilters(): void {
    this.selectedEmployeeId.set(null);
    this.selectedEmployeeName.set('');

    this.selectedDepartmentId.set(0);
    this.selectedJobGradeId.set(0);

    this.pageNumber.set(1);

    this.loadReport();
  }

  goToPage(page: number): void {
    if (
      page < 1 ||
      page > this.totalPages() ||
      this.loading()
    ) {
      return;
    }

    this.pageNumber.set(page);
    this.loadReport();
  }

  changePageSize(size: number): void {
    if (
      ![10, 20, 50].includes(size) ||
      this.loading()
    ) {
      return;
    }

    this.pageSize.set(size);
    this.pageNumber.set(1);

    this.loadReport();
  }

  printReport(): void {
    window.print();
  }

  private getFilterDescription(
    filter: EmployeeReportFilter
  ): string {

    const parts: string[] = [];

    if (filter.employeeId != null) {
      parts.push(`Employee ID: ${filter.employeeId}`);
    }

    if (filter.employeeName) {
      parts.push(`Employee Name: ${filter.employeeName}`);
    }

    if (filter.departmentId) {
      const department = this.departments().find(
        d => d.id === filter.departmentId
      );

      parts.push(
        `Department: ${department?.name ?? filter.departmentId}`
      );
    }

    if (filter.jobGradeId) {
      const grade = this.jobGrades().find(
        g => g.id === filter.jobGradeId
      );

      parts.push(
        `Job Grade: ${grade?.name ?? filter.jobGradeId}`
      );
    }

    return parts.length
      ? parts.join(' | ')
      : 'All employees';
  }
}

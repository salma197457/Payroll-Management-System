
import { Component, OnInit, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

import { ReportService } from '../../../Core/services/report';

import {
  SalaryReport as SalaryReportModel,
  SalaryReportFilter
} from '../../../Core/Models/salary-report.model';

@Component({
  selector: 'app-salary-report',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './salary-report.html',
  styleUrl: './salary-report.css'
})
export class SalaryReport implements OnInit {
  private readonly reportService = inject(ReportService);

  records = signal<SalaryReportModel[]>([]);

  selectedYear = signal<number | null>(null);
  selectedMonth = signal(0);
  selectedEmployeeId = signal<number | null>(null);

  pageNumber = signal(1);
  pageSize = signal(10);
  totalCount = signal(0);
  totalPages = signal(0);

  loading = signal(false);
  errorMessage = signal('');
  hasLoaded = signal(false);

  appliedFilters = signal('All periods and employees');

  readonly months = [
    { value: 1, name: 'January' },
    { value: 2, name: 'February' },
    { value: 3, name: 'March' },
    { value: 4, name: 'April' },
    { value: 5, name: 'May' },
    { value: 6, name: 'June' },
    { value: 7, name: 'July' },
    { value: 8, name: 'August' },
    { value: 9, name: 'September' },
    { value: 10, name: 'October' },
    { value: 11, name: 'November' },
    { value: 12, name: 'December' }
  ];

  ngOnInit(): void {
    this.loadReport();
  }

  loadReport(): void {
    const year = this.selectedYear();
    const month = this.selectedMonth();
    const employeeId = this.selectedEmployeeId();

    if (
      year != null &&
      (!Number.isInteger(year) || year < 1 || year > 9999)
    ) {
      this.errorMessage.set('Year must be between 1 and 9999.');
      return;
    }

    if (!Number.isInteger(month) || month < 0 || month > 12) {
      this.errorMessage.set('Please select a valid month.');
      return;
    }

    if (
      employeeId != null &&
      (!Number.isInteger(employeeId) || employeeId < 1)
    ) {
      this.errorMessage.set('Please enter a valid Employee ID.');
      return;
    }

    const filter: SalaryReportFilter = {
      year: year ?? undefined,
      month: month || undefined,
      employeeId: employeeId ?? undefined,
      pageNumber: this.pageNumber(),
      pageSize: this.pageSize()
    };

    this.loading.set(true);
    this.errorMessage.set('');

    this.reportService.getSalariesReport(filter).subscribe({
      next: response => {
        this.records.set(response.items);

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
          error.error?.detail || 'Failed to load salary report.'
        );

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
    this.selectedYear.set(null);
    this.selectedMonth.set(0);
    this.selectedEmployeeId.set(null);

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
    if (!Number.isInteger(size) || size < 1) {
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
    filter: SalaryReportFilter
  ): string {
    const parts: string[] = [];

    if (filter.year) {
      parts.push(`Year: ${filter.year}`);
    }

    if (filter.month) {
      const month = this.months.find(
        m => m.value === filter.month
      );

      parts.push(`Month: ${month?.name}`);
    }

    if (filter.employeeId) {
      parts.push(`Employee ID: ${filter.employeeId}`);
    }

    return parts.length
      ? parts.join(' | ')
      : 'All periods and employees';
  }
}

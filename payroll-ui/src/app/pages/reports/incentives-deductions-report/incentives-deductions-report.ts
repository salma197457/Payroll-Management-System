
import { Component, OnInit, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

import { ReportService } from '../../../Core/services/report';

import {
  IncentivesDeductionsReport as IncentivesDeductionsReportModel,
  IncentivesDeductionsReportFilter
} from '../../../Core/Models/incentives-deductions-report.model';

@Component({
  selector: 'app-incentives-deductions-report',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './incentives-deductions-report.html',
  styleUrl: './incentives-deductions-report.css'
})
export class IncentivesDeductionsReport implements OnInit {

  private readonly reportService = inject(ReportService);

  records = signal<IncentivesDeductionsReportModel[]>([]);

  selectedYear = signal<number | null>(null);
  selectedMonth = signal<number>(0);
  selectedEmployeeId = signal<number | null>(null);

  pageNumber = signal(1);
  pageSize = signal(10);
  totalCount = signal(0);
  totalPages = signal(0);

  totalIncentives = signal(0);
  totalDeductions = signal(0);

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

    // Validate year.
    if (
      year !== null &&
      (!Number.isInteger(year) || year < 1 || year > 9999)
    ) {
      this.errorMessage.set('Year must be between 1 and 9999.');
      return;
    }

    // Validate month.
    if (
      !Number.isInteger(month) ||
      month < 0 ||
      month > 12
    ) {
      this.errorMessage.set('Please select a valid month.');
      return;
    }

    // Validate Employee ID.
    if (
      employeeId !== null &&
      (!Number.isInteger(employeeId) || employeeId < 1)
    ) {
      this.errorMessage.set('Please enter a valid Employee ID.');
      return;
    }

    const filter: IncentivesDeductionsReportFilter = {
      year: year ?? undefined,
      month: month || undefined,
      employeeId: employeeId ?? undefined,
      pageNumber: this.pageNumber(),
      pageSize: this.pageSize()
    };

    this.loading.set(true);
    this.errorMessage.set('');

    this.reportService
      .getIncentivesDeductionsReport(filter)
      .subscribe({
        next: response => {
          this.records.set(response.items);

          this.totalCount.set(response.totalCount);
          this.totalPages.set(response.totalPages);
          this.pageNumber.set(response.pageNumber);

          // Totals are calculated by the backend
          // across ALL matching records, not just this page.
          this.totalIncentives.set(response.totalIncentives);
          this.totalDeductions.set(response.totalDeductions);

          this.appliedFilters.set(
            this.getFilterDescription(filter)
          );

          this.hasLoaded.set(true);
          this.loading.set(false);
        },

        error: (error: HttpErrorResponse) => {
          this.errorMessage.set(
            error.error?.detail ||
            'Failed to load incentives and deductions report.'
          );

          this.records.set([]);

          this.totalCount.set(0);
          this.totalPages.set(0);

          this.totalIncentives.set(0);
          this.totalDeductions.set(0);

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
      this.loading() ||
      page < 1 ||
      page > this.totalPages()
    ) {
      return;
    }

    this.pageNumber.set(page);
    this.loadReport();
  }

  changePageSize(size: number): void {
    if (
      this.loading() ||
      !Number.isInteger(size) ||
      ![10, 20, 50].includes(size)
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
    filter: IncentivesDeductionsReportFilter
  ): string {

    const parts: string[] = [];

    if (filter.year !== undefined) {
      parts.push(`Year: ${filter.year}`);
    }

    if (filter.month !== undefined) {
      const month = this.months.find(
        m => m.value === filter.month
      );

      if (month) {
        parts.push(`Month: ${month.name}`);
      }
    }

    if (filter.employeeId !== undefined) {
      parts.push(`Employee ID: ${filter.employeeId}`);
    }

    return parts.length > 0
      ? parts.join(' | ')
      : 'All periods and employees';
  }
}

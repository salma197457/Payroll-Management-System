
import { Component, OnInit, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

import { ReportService } from '../../../Core/services/report';
import {
  AttendanceReport as AttendanceReportModel,
  AttendanceReportFilter
} from '../../../Core/Models/attendance-report.model';

@Component({
  selector: 'app-attendance-report',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './attendance-report.html',
  styleUrl: './attendance-report.css'
})
export class AttendanceReport implements OnInit {

  private readonly reportService = inject(ReportService);

  records = signal<AttendanceReportModel[]>([]);

  fromDate = signal('');
  toDate = signal('');
  selectedEmployeeId = signal<number | null>(null);
  today = signal(this.formatDate(new Date()));

  pageNumber = signal(1);
  pageSize = signal(10);
  totalPages = signal(0);
  totalCount = signal(0);

  loading = signal(false);
  errorMessage = signal('');
  hasLoaded = signal(false);
  appliedFilters = signal('');

  ngOnInit(): void {
    this.setCurrentMonth();
    this.loadReport();
  }

private setCurrentMonth(): void {
  const today = new Date();

  const firstDay = new Date(
    today.getFullYear(),
    today.getMonth(),
    1
  );

  this.today.set(this.formatDate(today));

  this.fromDate.set(this.formatDate(firstDay));

  this.toDate.set(this.formatDate(today));
}

  private formatDate(date: Date): string {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');

    return `${year}-${month}-${day}`;
  }

  loadReport(): void {
    const fromDate = this.fromDate();
    const toDate = this.toDate();
    const employeeId = this.selectedEmployeeId();

    if (!fromDate || !toDate) {
      this.errorMessage.set('From Date and To Date are required.');
      return;
    }

    if (fromDate > toDate) {
      this.errorMessage.set('From Date cannot be after To Date.');
      return;
    }

    if (fromDate > this.today() || toDate > this.today()) {
      this.errorMessage.set('Future dates are not allowed.');
      return;
    }
    if (
      employeeId != null &&
      (!Number.isInteger(employeeId) || employeeId < 1)
    ) {
      this.errorMessage.set('Please enter a valid Employee ID.');
      return;
    }

    const filter: AttendanceReportFilter = {
      fromDate,
      toDate,
      employeeId: employeeId ?? undefined,
      pageNumber: this.pageNumber(),
      pageSize: this.pageSize()
    };

    this.loading.set(true);
    this.errorMessage.set('');

    this.reportService.getAttendanceReport(filter).subscribe({
      next: response => {
        this.records.set(response.items);
        this.totalCount.set(response.totalCount);
        this.totalPages.set(response.totalPages);
        this.pageNumber.set(response.pageNumber);

        this.appliedFilters.set(
          `Period: ${fromDate} to ${toDate}` +
          (employeeId != null ? ` | Employee ID: ${employeeId}` : '')
        );

        this.hasLoaded.set(true);
        this.loading.set(false);
      },
      error: (error: HttpErrorResponse) => {
        this.errorMessage.set(
          error.error?.detail || 'Failed to load attendance report.'
        );

        this.records.set([]);
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
    this.pageNumber.set(1);
    this.setCurrentMonth();
    this.loadReport();
  }

  goToPage(page: number): void {
    if (page < 1 || page > this.totalPages() || this.loading()) {
      return;
    }

    this.pageNumber.set(page);
    this.loadReport();
  }

  changePageSize(size: number): void {
    if (![10, 20, 50].includes(size) || this.loading()) {
      return;
    }

    this.pageSize.set(size);
    this.pageNumber.set(1);
    this.loadReport();
  }

  printReport(): void {
    window.print();
  }
}

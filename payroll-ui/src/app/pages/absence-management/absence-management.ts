
import {
  Component,
  OnInit,
  computed,
  inject,
  signal
} from '@angular/core';

import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

import { EmployeeAbsenceService } from '../../Core/services/employee-absence';
import { EmployeeAbsence } from '../../Core/Models/employee-absence.model';

@Component({
  selector: 'app-absence-management',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './absence-management.html',
  styleUrl: './absence-management.css'
})
export class AbsenceManagement implements OnInit {

  private readonly absenceService = inject(EmployeeAbsenceService);

  absences = signal<EmployeeAbsence[]>([]);

  // Client-side filters
  searchTerm = signal('');
  selectedDate = signal('');

  // Server-side pagination
  pageNumber = signal(1);
  pageSize = signal(10);
  totalCount = signal(0);
  totalPages = signal(0);

  loading = signal(false);
  errorMessage = signal('');

  filteredAbsences = computed(() => {
    const search = this.searchTerm().trim().toLowerCase();
    const date = this.selectedDate();

    return this.absences().filter(absence => {

      const matchesSearch =
        !search ||
        String(absence.employeeId).includes(search) ||
        (absence.employeeName ?? '')
          .toLowerCase()
          .includes(search);

      const matchesDate =
        !date || absence.date === date;

      return matchesSearch && matchesDate;
    });
  });

  ngOnInit(): void {
    this.loadAbsences();
  }

  loadAbsences(): void {
    this.loading.set(true);
    this.errorMessage.set('');

    this.absenceService.getPaged(
      this.pageNumber(),
      this.pageSize()
    ).subscribe({
      next: response => {

        if (!response || !Array.isArray(response.items)) {
          this.absences.set([]);
          this.totalCount.set(0);
          this.totalPages.set(0);

          this.errorMessage.set(
            'Invalid API response. Expected a paginated absence result.'
          );

          this.loading.set(false);
          return;
        }

        this.absences.set(response.items);
        this.totalCount.set(response.totalCount);
        this.totalPages.set(response.totalPages);
        this.pageNumber.set(response.pageNumber);

        this.loading.set(false);
      },

      error: (error: HttpErrorResponse) => {
        this.errorMessage.set(
          error.error?.detail ||
          'Failed to load employee absences.'
        );

        this.absences.set([]);
        this.totalCount.set(0);
        this.totalPages.set(0);

        this.loading.set(false);
      }
    });
  }

  goToPage(page: number): void {
    if (
      this.loading() ||
      page < 1 ||
      page > this.totalPages() ||
      page === this.pageNumber()
    ) {
      return;
    }

    this.pageNumber.set(page);
    this.loadAbsences();
  }

  changePageSize(size: number): void {
    if (
      this.loading() ||
      ![10, 20, 50].includes(size)
    ) {
      return;
    }

    this.pageSize.set(size);
    this.pageNumber.set(1);

    this.loadAbsences();
  }

  clearFilters(): void {
    this.searchTerm.set('');
    this.selectedDate.set('');
  }
}

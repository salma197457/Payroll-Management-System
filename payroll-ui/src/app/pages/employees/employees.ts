
import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

import { EmployeeService } from '../../Core/services/employee';
import { DepartmentService } from '../../Core/services/department';
import { JobGradeService } from '../../Core/services/job-grade';

import { Employee } from '../../Core/Models/employee.model';
import { Department } from '../../Core/Models/department.model';
import { JobGrade } from '../../Core/Models/job-grade.model';

@Component({
  selector: 'app-employees',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './employees.html',
  styleUrl: './employees.css'
})
export class Employees implements OnInit {

  private readonly employeeService = inject(EmployeeService);
  private readonly departmentService = inject(DepartmentService);
  private readonly jobGradeService = inject(JobGradeService);

  employees = signal<Employee[]>([]);
  departments = signal<Department[]>([]);
  jobGrades = signal<JobGrade[]>([]);

  // Client-side filters
  searchTerm = signal('');
  selectedDepartmentId = signal(0);
  selectedJobGradeId = signal(0);

  // Server-side pagination
  pageNumber = signal(1);
  pageSize = signal(10);
  totalCount = signal(0);
  totalPages = signal(0);

  loading = signal(false);
  errorMessage = signal('');

  // Filter only the employees returned for the current page
  filteredEmployees = computed(() => {
    const search = this.searchTerm().trim().toLowerCase();
    const departmentId = this.selectedDepartmentId();
    const jobGradeId = this.selectedJobGradeId();

    return this.employees().filter(employee => {
      const matchesSearch =
        employee.name.toLowerCase().includes(search) ||
        (employee.email ?? '').toLowerCase().includes(search);

      const matchesDepartment =
        !departmentId || employee.departmentId === departmentId;

      const matchesJobGrade =
        !jobGradeId || employee.jobGradeId === jobGradeId;

      return matchesSearch && matchesDepartment && matchesJobGrade;
    });
  });

  ngOnInit(): void {
    this.loadEmployees();

    this.departmentService.getAll().subscribe({
      next: data => {
        this.departments.set(data);
      },
      error: () => {
        this.errorMessage.set('Failed to load departments.');
      }
    });

    this.jobGradeService.getAll().subscribe({
      next: data => {
        this.jobGrades.set(data);
      },
      error: () => {
        this.errorMessage.set('Failed to load job grades.');
      }
    });
  }

  loadEmployees(): void {
    this.loading.set(true);
    this.errorMessage.set('');

    this.employeeService.getPaged(
      this.pageNumber(),
      this.pageSize()
    ).subscribe({
      next: response => {

        if (!response || !Array.isArray(response.items)) {
          this.employees.set([]);
          this.totalCount.set(0);
          this.totalPages.set(0);

          this.errorMessage.set(
            'Invalid API response. Expected a paginated employee result.'
          );

          this.loading.set(false);
          return;
        }

        this.employees.set(response.items);
        this.totalCount.set(response.totalCount);
        this.totalPages.set(response.totalPages);
        this.pageNumber.set(response.pageNumber);

        this.loading.set(false);
      },

      error: (error: HttpErrorResponse) => {
        this.errorMessage.set(
          error.error?.detail || 'Failed to load employees.'
        );

        this.employees.set([]);
        this.totalCount.set(0);
        this.totalPages.set(0);

        this.loading.set(false);
      }
    });
  }

  updateSearch(value: string): void {
    this.searchTerm.set(value);
  }

  updateDepartment(value: number): void {
    this.selectedDepartmentId.set(value);
  }

  updateJobGrade(value: number): void {
    this.selectedJobGradeId.set(value);
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
    this.loadEmployees();
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

    this.loadEmployees();
  }

  deleteEmployee(employee: Employee): void {
    const confirmed = confirm(
      `Are you sure you want to delete "${employee.name}"?`
    );

    if (!confirmed) {
      return;
    }

    this.errorMessage.set('');

    this.employeeService.delete({
      id: employee.id,
      rowVersion: employee.rowVersion
    }).subscribe({
      next: () => {

        // If the deleted employee was the only one
        // on the last page, move back one page.
        if (
          this.employees().length === 1 &&
          this.pageNumber() > 1
        ) {
          this.pageNumber.update(page => page - 1);
        }

        this.loadEmployees();
      },

      error: (error: HttpErrorResponse) => {
        this.errorMessage.set(
          error.error?.detail || 'Failed to delete employee.'
        );
      }
    });
  }
}

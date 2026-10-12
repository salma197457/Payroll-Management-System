import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';

import { DepartmentService } from '../../Core/services/department';
import { Department } from '../../Core/Models/department.model';

@Component({
  selector: 'app-departments',
  standalone: true,
  imports: [RouterLink, FormsModule],
  templateUrl: './departments.html',
  styleUrl: './departments.css'
})
export class Departments implements OnInit {
  private readonly departmentService = inject(DepartmentService);

  departments = signal<Department[]>([]);
  searchTerm = signal('');

  loading = signal(false);
  errorMessage = signal('');

  filteredDepartments = computed(() => {
    const search = this.searchTerm().toLowerCase().trim();

    return this.departments().filter(department =>
      department.name.toLowerCase().includes(search)
    );
  });

  ngOnInit(): void {
    this.loadDepartments();
  }

  loadDepartments(): void {
    this.loading.set(true);
    this.errorMessage.set('');

    this.departmentService.getAll().subscribe({
      next: data => {
        this.departments.set(data);
        this.loading.set(false);
      },
      error: () => {
        this.errorMessage.set('Failed to load departments.');
        this.loading.set(false);
      }
    });
  }

deleteDepartment(department: Department): void {
  const confirmed = confirm(
    `Are you sure you want to delete "${department.name}"?`
  );

  if (!confirmed) return;

  this.errorMessage.set('');

  this.departmentService.delete({
    id: department.id,
    rowVersion: department.rowVersion
  }).subscribe({
    next: () => {
      this.loadDepartments();
    },
    error: error => {
      this.errorMessage.set(
        error.error?.detail || 'Failed to delete department.'
      );
    }
  });
}
}
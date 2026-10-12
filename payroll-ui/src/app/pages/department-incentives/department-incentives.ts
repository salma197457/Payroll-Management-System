import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

import { DepartmentIncentiveService } from '../../Core/services/department-incentive';
import { DepartmentIncentive } from '../../Core/Models/department-incentive.model';

@Component({
  selector: 'app-department-incentives',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './department-incentives.html',
  styleUrl: './department-incentives.css'
})
export class DepartmentIncentives implements OnInit {
  private readonly incentiveService = inject(DepartmentIncentiveService);

  incentives = signal<DepartmentIncentive[]>([]);
  searchTerm = signal('');

  loading = signal(false);
  errorMessage = signal('');

  filteredIncentives = computed(() => {
    const search = this.searchTerm().trim().toLowerCase();

    return this.incentives().filter(incentive =>
      incentive.departmentName.toLowerCase().includes(search)
    );
  });

  ngOnInit(): void {
    this.loadIncentives();
  }

  loadIncentives(): void {
    this.loading.set(true);
    this.errorMessage.set('');

    this.incentiveService.getAll().subscribe({
      next: data => {
        this.incentives.set(data);
        this.loading.set(false);
      },
      error: (error: HttpErrorResponse) => {
        this.errorMessage.set(
          error.error?.detail || 'Failed to load department incentives.'
        );
        this.loading.set(false);
      }
    });
  }
}
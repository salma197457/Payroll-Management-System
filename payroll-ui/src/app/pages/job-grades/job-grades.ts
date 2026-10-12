import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';

import { JobGradeService } from '../../Core/services/job-grade';
import { JobGrade } from '../../Core/Models/job-grade.model';

@Component({
  selector: 'app-job-grades',
  standalone: true,
  imports: [RouterLink, FormsModule],
  templateUrl: './job-grades.html',
  styleUrl: './job-grades.css'
})
export class JobGrades implements OnInit {
  private readonly jobGradeService = inject(JobGradeService);

  jobGrades = signal<JobGrade[]>([]);
  searchTerm = signal('');

  loading = signal(false);
  errorMessage = signal('');

  filteredJobGrades = computed(() => {
    const search = this.searchTerm().trim().toLowerCase();

    return this.jobGrades().filter(grade =>
      grade.name.toLowerCase().includes(search)
    );
  });

  ngOnInit(): void {
    this.loadJobGrades();
  }

  loadJobGrades(): void {
    this.loading.set(true);
    this.errorMessage.set('');

    this.jobGradeService.getAll().subscribe({
      next: data => {
        this.jobGrades.set(data);
        this.loading.set(false);
      },
      error: (error: HttpErrorResponse) => {
        this.errorMessage.set(
          error.error?.detail || 'Failed to load job grades.'
        );
        this.loading.set(false);
      }
    });
  }
}
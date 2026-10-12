import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

import { ExperienceIncentiveService } from '../../Core/services/experience-incentive';
import { ExperienceIncentive } from '../../Core/Models/experience-incentive.model';

@Component({
  selector: 'app-experience-incentives',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './experience-incentives.html',
  styleUrl: './experience-incentives.css'
})
export class ExperienceIncentives implements OnInit {
  private readonly incentiveService = inject(ExperienceIncentiveService);

  incentives = signal<ExperienceIncentive[]>([]);
  searchTerm = signal('');

  loading = signal(false);
  errorMessage = signal('');

  filteredIncentives = computed(() => {
    const search = this.searchTerm().trim();

    return this.incentives().filter(incentive =>
      incentive.minimumYears.toString().includes(search)
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
          error.error?.detail || 'Failed to load experience incentives.'
        );
        this.loading.set(false);
      }
    });
  }
}
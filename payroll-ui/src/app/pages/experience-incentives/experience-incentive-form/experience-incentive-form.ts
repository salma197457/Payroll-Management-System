import { Component, OnInit, inject, signal } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { Observable } from 'rxjs';

import { ExperienceIncentiveService } from '../../../Core/services/experience-incentive';

@Component({
  selector: 'app-experience-incentive-form',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './experience-incentive-form.html',
  styleUrl: './experience-incentive-form.css'
})
export class ExperienceIncentiveForm implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly incentiveService = inject(ExperienceIncentiveService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);

  isEditMode = this.route.snapshot.paramMap.has('id');
  incentiveId = Number(this.route.snapshot.paramMap.get('id'));

  rowVersion = '';

  loading = signal(false);
  saving = signal(false);
  errorMessage = signal('');

  form = this.fb.nonNullable.group({
    minimumYears: [
      0,
      [Validators.required, Validators.min(0), Validators.pattern(/^\d+$/)]
    ],
    percentage: [
      0,
      [Validators.required, Validators.min(0), Validators.max(100)]
    ]
  });

  ngOnInit(): void {
    if (this.isEditMode) {
      this.loadIncentive();
    }
  }

  loadIncentive(): void {
    this.loading.set(true);
    this.errorMessage.set('');

    this.incentiveService.getAll().subscribe({
      next: incentives => {
        const incentive = incentives.find(
          item => item.id === this.incentiveId
        );

        if (!incentive) {
          this.errorMessage.set('Experience incentive not found.');
          this.loading.set(false);
          return;
        }

        this.form.patchValue({
          minimumYears: incentive.minimumYears,
          percentage: incentive.percentage
        });

        this.rowVersion = incentive.rowVersion;

        // MinimumYears cannot be changed after creation.
        this.form.controls.minimumYears.disable();

        this.loading.set(false);
      },
      error: (error: HttpErrorResponse) => {
        this.errorMessage.set(
          error.error?.detail || 'Failed to load experience incentive.'
        );
        this.loading.set(false);
      }
    });
  }

  save(): void {
    if (this.form.invalid || this.loading() || this.saving()) {
      this.form.markAllAsTouched();
      return;
    }

    const values = this.form.getRawValue();

    this.saving.set(true);
    this.errorMessage.set('');

    const request: Observable<unknown> = this.isEditMode
      ? this.incentiveService.update({
          id: this.incentiveId,
          percentage: Number(values.percentage),
          rowVersion: this.rowVersion
        })
      : this.incentiveService.create({
          minimumYears: Number(values.minimumYears),
          percentage: Number(values.percentage)
        });

    request.subscribe({
      next: () => {
        this.router.navigate(['/experience-incentives']);
      },
      error: (error: HttpErrorResponse) => {
        this.errorMessage.set(
          error.error?.detail || 'Failed to save experience incentive.'
        );
        this.saving.set(false);
      }
    });
  }
}
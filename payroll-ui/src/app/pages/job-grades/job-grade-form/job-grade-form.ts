import { Component, OnInit, inject, signal } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import {
  ActivatedRoute,
  Router,
  RouterLink
} from '@angular/router';
import { Observable } from 'rxjs';

import { JobGradeService } from '../../../Core/services/job-grade';

@Component({
  selector: 'app-job-grade-form',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './job-grade-form.html',
  styleUrl: './job-grade-form.css'
})
export class JobGradeForm implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly jobGradeService = inject(JobGradeService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);

  isEditMode = this.route.snapshot.paramMap.has('id');
  jobGradeId = Number(this.route.snapshot.paramMap.get('id'));

  rowVersion = '';

  loading = signal(false);
  saving = signal(false);
  errorMessage = signal('');

  form = this.fb.nonNullable.group({
    name: ['', [Validators.required, Validators.maxLength(100)]],
    salary: [0, [Validators.required, Validators.min(0.01)]]
  });

  ngOnInit(): void {
    if (this.isEditMode) {
      this.loadJobGrade();
    }
  }

  loadJobGrade(): void {
    this.loading.set(true);
    this.errorMessage.set('');

    this.jobGradeService.getById(this.jobGradeId).subscribe({
      next: grade => {
        this.form.patchValue({
          name: grade.name,
          salary: grade.salary
        });

        this.rowVersion = grade.rowVersion;

        // Only salary can be updated.
        this.form.controls.name.disable();

        this.loading.set(false);
      },
      error: (error: HttpErrorResponse) => {
        this.errorMessage.set(
          error.error?.detail || 'Failed to load job grade.'
        );
        this.loading.set(false);
      }
    });
  }

  save(): void {
    if (this.form.invalid || this.saving() || this.loading()) {
      this.form.markAllAsTouched();
      return;
    }

    const values = this.form.getRawValue();
    const name = values.name.trim();
    const salary = Number(values.salary);

    if (!this.isEditMode && !name) {
      this.form.controls.name.setErrors({ required: true });
      return;
    }

    this.saving.set(true);
    this.errorMessage.set('');

    const request: Observable<unknown> = this.isEditMode
      ? this.jobGradeService.update({
          id: this.jobGradeId,
          salary,
          rowVersion: this.rowVersion
        })
      : this.jobGradeService.create({
          name,
          salary
        });

    request.subscribe({
      next: () => {
        this.router.navigate(['/job-grades']);
      },
      error: (error: HttpErrorResponse) => {
        this.errorMessage.set(
          error.error?.detail || 'Failed to save job grade.'
        );
        this.saving.set(false);
      }
    });
  }
}
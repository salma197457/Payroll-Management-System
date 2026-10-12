import { Component, OnInit, inject, signal } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { Observable, forkJoin } from 'rxjs';

import { DepartmentIncentiveService } from '../../../Core/services/department-incentive';
import { DepartmentService } from '../../../Core/services/department';

import { DepartmentIncentive } from '../../../Core/Models/department-incentive.model';
import { Department } from '../../../Core/Models/department.model';

@Component({
  selector: 'app-department-incentive-form',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './department-incentive-form.html',
  styleUrl: './department-incentive-form.css'
})
export class DepartmentIncentiveForm implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly incentiveService = inject(DepartmentIncentiveService);
  private readonly departmentService = inject(DepartmentService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);

  isEditMode = this.route.snapshot.paramMap.has('id');
  incentiveId = Number(this.route.snapshot.paramMap.get('id'));

  rowVersion = '';

  departments = signal<Department[]>([]);
  loading = signal(false);
  saving = signal(false);
  errorMessage = signal('');

  form = this.fb.nonNullable.group({
    departmentId: [0, [Validators.required, Validators.min(1)]],
    percentage: [
      0,
      [Validators.required, Validators.min(0), Validators.max(100)]
    ]
  });

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.loading.set(true);
    this.errorMessage.set('');

    forkJoin({
      departments: this.departmentService.getAll(),
      incentives: this.incentiveService.getAll()
    }).subscribe({
      next: ({ departments, incentives }) => {
        this.departments.set(departments);

        if (this.isEditMode) {
          const incentive = incentives.find(
            item => item.id === this.incentiveId
          );

          if (!incentive) {
            this.errorMessage.set('Department incentive not found.');
            this.loading.set(false);
            return;
          }

          this.form.patchValue({
            departmentId: incentive.departmentId,
            percentage: incentive.percentage
          });

          this.rowVersion = incentive.rowVersion;

          // Department cannot be changed after creation.
          this.form.controls.departmentId.disable();
        }

        this.loading.set(false);
      },
      error: (error: HttpErrorResponse) => {
        this.errorMessage.set(
          error.error?.detail || 'Failed to load data.'
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
          departmentId: Number(values.departmentId),
          percentage: Number(values.percentage)
        });

    request.subscribe({
      next: () => {
        this.router.navigate(['/department-incentives']);
      },
      error: (error: HttpErrorResponse) => {
        this.errorMessage.set(
          error.error?.detail || 'Failed to save department incentive.'
        );
        this.saving.set(false);
      }
    });
  }
}
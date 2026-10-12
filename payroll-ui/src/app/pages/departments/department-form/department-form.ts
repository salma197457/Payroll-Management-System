import { Component, OnInit, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { Observable } from 'rxjs';
import { DepartmentService } from '../../../Core/services/department';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-department-form',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './department-form.html',
  styleUrl: './department-form.css'
})
export class DepartmentForm implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly departmentService = inject(DepartmentService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);

  departmentId = Number(this.route.snapshot.paramMap.get('id'));
  isEditMode = this.route.snapshot.paramMap.has('id');

  rowVersion = '';

  loading = signal(false);
  saving = signal(false);
  errorMessage = signal('');

  form = this.fb.nonNullable.group({
    name: ['', [Validators.required, Validators.maxLength(100)]]
  });

  ngOnInit(): void {
    if (this.isEditMode) {
      this.loadDepartment();
    }
  }

  loadDepartment(): void {
    this.loading.set(true);

    this.departmentService.getById(this.departmentId).subscribe({
      next: department => {
        this.form.patchValue({ name: department.name });
        this.rowVersion = department.rowVersion;
        this.loading.set(false);
      },
      error: () => {
        this.errorMessage.set('Failed to load department.');
        this.loading.set(false);
      }
    });
  }

save(): void {
  if (this.form.invalid || this.saving() || this.loading()) {
    this.form.markAllAsTouched();
    return;
  }

  this.saving.set(true);
  this.errorMessage.set('');

  const name = this.form.getRawValue().name.trim();

  if (!name) {
    this.form.controls.name.setErrors({ required: true });
    this.saving.set(false);
    return;
  }

  const request: Observable<unknown> = this.isEditMode
    ? this.departmentService.update({
        id: this.departmentId,
        name,
        rowVersion: this.rowVersion
      })
    : this.departmentService.create({ name });

  request.subscribe({
    next: () => {
      this.router.navigate(['/departments']);
    },
    error: (error: HttpErrorResponse) => {
      this.errorMessage.set(
        error.error?.detail || 'Failed to save department.'
      );
      this.saving.set(false);
    }
  });
}
}
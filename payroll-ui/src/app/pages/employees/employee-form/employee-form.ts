import { Component, OnInit, inject, signal } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { Observable } from 'rxjs';

import { EmployeeService } from '../../../Core/services/employee';
import { DepartmentService } from '../../../Core/services/department';
import { JobGradeService } from '../../../Core/services/job-grade';

import { Department } from '../../../Core/Models/department.model';
import { JobGrade } from '../../../Core/Models/job-grade.model';

@Component({
  selector: 'app-employee-form',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './employee-form.html',
  styleUrl: './employee-form.css'
})
export class EmployeeForm implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly employeeService = inject(EmployeeService);
  private readonly departmentService = inject(DepartmentService);
  private readonly jobGradeService = inject(JobGradeService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);

  isEditMode = this.route.snapshot.paramMap.has('id');
  employeeId = Number(this.route.snapshot.paramMap.get('id'));

  rowVersion = '';

  departments = signal<Department[]>([]);
  jobGrades = signal<JobGrade[]>([]);

  loading = signal(false);
  saving = signal(false);
  errorMessage = signal('');

  form = this.fb.nonNullable.group({
    name: ['', [Validators.required, Validators.maxLength(100)]],
    birthDate: ['', Validators.required],
    address: ['', [Validators.required, Validators.maxLength(200)]],
    phoneNumber: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    joinDate: ['', Validators.required],
    departmentId: [0, Validators.min(1)],
    jobGradeId: [0, Validators.min(1)]
  });

  ngOnInit(): void {
    this.departmentService.getAll().subscribe({
      next: data => this.departments.set(data),
      error: () => this.errorMessage.set('Failed to load departments.')
    });

    this.jobGradeService.getAll().subscribe({
      next: data => this.jobGrades.set(data),
      error: () => this.errorMessage.set('Failed to load job grades.')
    });

    if (this.isEditMode) {
      this.loadEmployee();
    }
  }

  loadEmployee(): void {
    this.loading.set(true);

    this.employeeService.getById(this.employeeId).subscribe({
      next: employee => {
        this.form.patchValue({
          name: employee.name,
          birthDate: employee.birthDate,
          address: employee.address,
          phoneNumber: employee.phoneNumber,
          email: employee.email,
          joinDate: employee.joinDate,
          departmentId: employee.departmentId,
          jobGradeId: employee.jobGradeId
        });

        this.rowVersion = employee.rowVersion;

        // Join date cannot be changed after creation.
        this.form.controls.joinDate.disable();

        this.loading.set(false);
      },
      error: (error: HttpErrorResponse) => {
        this.errorMessage.set(
          error.error?.detail || 'Failed to load employee.'
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

    this.saving.set(true);
    this.errorMessage.set('');

    const values = this.form.getRawValue();

    const request: Observable<unknown> = this.isEditMode
      ? this.employeeService.update({
          id: this.employeeId,
          name: values.name.trim(),
          birthDate: values.birthDate,
          address: values.address.trim(),
          phoneNumber: values.phoneNumber.trim(),
          email: values.email.trim(),
          departmentId: Number(values.departmentId),
          jobGradeId: Number(values.jobGradeId),
          rowVersion: this.rowVersion
        })
      : this.employeeService.create({
          name: values.name.trim(),
          birthDate: values.birthDate,
          address: values.address.trim(),
          phoneNumber: values.phoneNumber.trim(),
          email: values.email.trim(),
          joinDate: values.joinDate,
          departmentId: Number(values.departmentId),
          jobGradeId: Number(values.jobGradeId)
        });

    request.subscribe({
      next: () => {
        this.router.navigate(['/employees']);
      },
      error: (error: HttpErrorResponse) => {
        this.errorMessage.set(
          error.error?.detail || 'Failed to save employee.'
        );
        this.saving.set(false);
      }
    });
  }
}
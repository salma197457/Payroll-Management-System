import { Component, OnInit, inject, signal } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

import { EmployeeAbsenceService } from '../../../Core/services/employee-absence';
import { EmployeeService } from '../../../Core/services/employee';
import { Employee } from '../../../Core/Models/employee.model';

@Component({
  selector: 'app-absence-form',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './absence-form.html',
  styleUrl: './absence-form.css'
})
export class AbsenceForm implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly absenceService = inject(EmployeeAbsenceService);
  private readonly employeeService = inject(EmployeeService);
  private readonly router = inject(Router);

  employees = signal<Employee[]>([]);

  loading = signal(false);
  saving = signal(false);
  errorMessage = signal('');

  form = this.fb.nonNullable.group({
    employeeId: [0, [Validators.required, Validators.min(1)]],
    date: ['', Validators.required]
  });

  ngOnInit(): void {
    this.loading.set(true);

    this.employeeService.getAllForFilter().subscribe({
      next: data => {
        this.employees.set(data);
        this.loading.set(false);
      },
      error: (error: HttpErrorResponse) => {
        this.errorMessage.set(
          error.error?.detail || 'Failed to load employees.'
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

    this.saving.set(true);
    this.errorMessage.set('');

    this.absenceService.create({
      employeeId: Number(values.employeeId),
      date: values.date
    }).subscribe({
      next: () => {
        this.router.navigate(['/absence-management']);
      },
      error: (error: HttpErrorResponse) => {
        this.errorMessage.set(
          error.error?.detail || 'Failed to record absence.'
        );
        this.saving.set(false);
      }
    });
  }
}
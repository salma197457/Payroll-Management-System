import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

import { AbsenceDeductionRuleService } from '../../Core/services/absence-deduction-rule';
import { AbsenceDeductionRule } from '../../Core/Models/absence-deduction-rule.model';

@Component({
  selector: 'app-absence-rules',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './absence-rules.html',
  styleUrl: './absence-rules.css'
})
export class AbsenceRules implements OnInit {
  private readonly ruleService = inject(AbsenceDeductionRuleService);

  rules = signal<AbsenceDeductionRule[]>([]);
  searchTerm = signal('');

  loading = signal(false);
  errorMessage = signal('');

  filteredRules = computed(() => {
    const search = this.searchTerm().trim().toLowerCase();

    return this.rules().filter(rule =>
      rule.minimumDays.toString().includes(search) ||
      rule.type.toLowerCase().includes(search)
    );
  });

  ngOnInit(): void {
    this.loadRules();
  }

  loadRules(): void {
    this.loading.set(true);
    this.errorMessage.set('');

    this.ruleService.getAll().subscribe({
      next: data => {
        this.rules.set(data);
        this.loading.set(false);
      },
      error: (error: HttpErrorResponse) => {
        this.errorMessage.set(
          error.error?.detail || 'Failed to load absence deduction rules.'
        );
        this.loading.set(false);
      }
    });
  }
}
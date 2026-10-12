import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  AbsenceDeductionRule,
  CreateAbsenceDeductionRule,
  UpdateAbsenceDeductionRule
} from '../Models/absence-deduction-rule.model';

@Injectable({
  providedIn: 'root'
})
export class AbsenceDeductionRuleService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = '/api/AbsenceDeductionRule';

  getAll(): Observable<AbsenceDeductionRule[]> {
    return this.http.get<AbsenceDeductionRule[]>(this.apiUrl);
  }

  create(
    request: CreateAbsenceDeductionRule
  ): Observable<AbsenceDeductionRule> {
    return this.http.post<AbsenceDeductionRule>(this.apiUrl, request);
  }

  update(request: UpdateAbsenceDeductionRule): Observable<void> {
    return this.http.put<void>(this.apiUrl, request);
  }
}
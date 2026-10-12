
import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  EmployeeAbsence,
  CreateEmployeeAbsence
} from '../Models/employee-absence.model';

import { PagedResult } from '../Models/paged-result.model';

@Injectable({
  providedIn: 'root'
})
export class EmployeeAbsenceService {

  private readonly http = inject(HttpClient);
  private readonly apiUrl = '/api/EmployeeAbsences';

  getAll(): Observable<EmployeeAbsence[]> {
    return this.http.get<EmployeeAbsence[]>(this.apiUrl);
  }

  getPaged(
    pageNumber: number = 1,
    pageSize: number = 10
  ): Observable<PagedResult<EmployeeAbsence>> {

    const params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);

    return this.http.get<PagedResult<EmployeeAbsence>>(
      this.apiUrl,
      { params }
    );
  }

  create(
    request: CreateEmployeeAbsence
  ): Observable<EmployeeAbsence> {
    return this.http.post<EmployeeAbsence>(
      this.apiUrl,
      request
    );
  }
}

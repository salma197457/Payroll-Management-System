import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  DepartmentIncentive,
  CreateDepartmentIncentive,
  UpdateDepartmentIncentive
} from '../Models/department-incentive.model';

@Injectable({
  providedIn: 'root'
})
export class DepartmentIncentiveService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = '/api/DepartmentIncentives';

  getAll(): Observable<DepartmentIncentive[]> {
    return this.http.get<DepartmentIncentive[]>(this.apiUrl);
  }

  create(
    request: CreateDepartmentIncentive
  ): Observable<DepartmentIncentive> {
    return this.http.post<DepartmentIncentive>(this.apiUrl, request);
  }

  update(request: UpdateDepartmentIncentive): Observable<void> {
    return this.http.put<void>(this.apiUrl, request);
  }
}
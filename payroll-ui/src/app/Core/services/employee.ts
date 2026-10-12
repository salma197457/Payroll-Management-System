import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PagedResult } from '../Models/paged-result.model';

import {
  Employee,
  CreateEmployee,
  UpdateEmployee,
  DeleteEmployee
} from '../Models/employee.model';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = '/api/Employee';

  getAll(): Observable<Employee[]> {
    return this.http.get<Employee[]>(this.apiUrl);
  }

  getById(id: number): Observable<Employee> {
    return this.http.get<Employee>(`${this.apiUrl}/${id}`);
  }

  create(request: CreateEmployee): Observable<Employee> {
    return this.http.post<Employee>(this.apiUrl, request);
  }

  update(request: UpdateEmployee): Observable<void> {
    return this.http.put<void>(this.apiUrl, request);
  }

  delete(request: DeleteEmployee): Observable<void> {
    return this.http.delete<void>(this.apiUrl, {
      body: request
    });
  }
getAllForFilter(): Observable<Employee[]> {
  return this.http.get<Employee[]>(
    `${this.apiUrl}/EmployeeFilter`
  );
}
getPaged(
  pageNumber: number,
  pageSize: number
): Observable<PagedResult<Employee>> {

  const params = new HttpParams()
    .set('pageNumber', pageNumber)
    .set('pageSize', pageSize);

  return this.http.get<PagedResult<Employee>>(
    this.apiUrl,
    { params }
  );
}
}
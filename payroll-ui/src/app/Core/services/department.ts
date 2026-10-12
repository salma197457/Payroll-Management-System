import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  Department,
  CreateDepartment,
  UpdateDepartment,
  DeleteDepartment
} from '../Models/department.model';

@Injectable({
  providedIn: 'root'
})
export class DepartmentService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl = '/api/Departments';

  getAll(): Observable<Department[]> {
    return this.http.get<Department[]>(this.apiUrl);
  }

  getById(id: number): Observable<Department> {
    return this.http.get<Department>(`${this.apiUrl}/${id}`);
  }

  create(request: CreateDepartment): Observable<Department> {
    return this.http.post<Department>(this.apiUrl, request);
  }

update(request: UpdateDepartment): Observable<void> {
  return this.http.put<void>(
    this.apiUrl,
    request
  );
}

delete(request: DeleteDepartment): Observable<void> {
  return this.http.delete<void>(
    this.apiUrl,
    { body: request }
  );
}
}
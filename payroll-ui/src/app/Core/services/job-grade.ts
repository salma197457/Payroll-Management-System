import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  JobGrade,
  CreateJobGrade,
  UpdateJobGrade
} from '../Models/job-grade.model';

@Injectable({
  providedIn: 'root'
})
export class JobGradeService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = '/api/JobGrades';

  getAll(): Observable<JobGrade[]> {
    return this.http.get<JobGrade[]>(this.apiUrl);
  }

  getById(id: number): Observable<JobGrade> {
    return this.http.get<JobGrade>(`${this.apiUrl}/${id}`);
  }

  create(request: CreateJobGrade): Observable<JobGrade> {
    return this.http.post<JobGrade>(this.apiUrl, request);
  }

  update(request: UpdateJobGrade): Observable<void> {
    return this.http.put<void>(this.apiUrl, request);
  }
}
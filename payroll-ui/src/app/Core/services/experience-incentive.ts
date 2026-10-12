import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  ExperienceIncentive,
  CreateExperienceIncentive,
  UpdateExperienceIncentive
} from '../Models/experience-incentive.model';

@Injectable({
  providedIn: 'root'
})
export class ExperienceIncentiveService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = '/api/ExperienceIncentives';

  getAll(): Observable<ExperienceIncentive[]> {
    return this.http.get<ExperienceIncentive[]>(this.apiUrl);
  }

  create(
    request: CreateExperienceIncentive
  ): Observable<ExperienceIncentive> {
    return this.http.post<ExperienceIncentive>(this.apiUrl, request);
  }

  update(request: UpdateExperienceIncentive): Observable<void> {
    return this.http.put<void>(this.apiUrl, request);
  }
}
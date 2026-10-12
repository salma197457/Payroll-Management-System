export interface ExperienceIncentive {
  id: number;
  minimumYears: number;
  percentage: number;
  rowVersion: string;
}

export interface CreateExperienceIncentive {
  minimumYears: number;
  percentage: number;
}

export interface UpdateExperienceIncentive {
  id: number;
  percentage: number;
  rowVersion: string;
}
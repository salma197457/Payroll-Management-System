export interface AbsenceDeductionRule {
  id: number;
  minimumDays: number;
  percentage: number;
  type: string;
  rowVersion: string;
}

export interface CreateAbsenceDeductionRule {
  minimumDays: number;
  percentage: number;
}

export interface UpdateAbsenceDeductionRule {
  id: number;
  percentage: number;
  rowVersion: string;
}
export interface DepartmentIncentive {
  id: number;
  departmentId: number;
  departmentName: string;
  percentage: number;
  rowVersion: string;
}

export interface CreateDepartmentIncentive {
  departmentId: number;
  percentage: number;
}

export interface UpdateDepartmentIncentive {
  id: number;
  percentage: number;
  rowVersion: string;
}
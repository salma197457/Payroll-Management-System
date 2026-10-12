export interface EmployeeAbsence {
  id: number;
  employeeId: number;
  employeeName: string;
  date: string;
}

export interface CreateEmployeeAbsence {
  employeeId: number;
  date: string;
}
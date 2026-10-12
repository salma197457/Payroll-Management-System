export interface Employee {
  id: number;
  name: string;
  birthDate: string;
  address: string;
  phoneNumber: string;
  email: string;
  joinDate: string;
  departmentId: number;
  departmentName: string;
  jobGradeId: number;
  jobGradeName: string;
  baseSalary: number;
  rowVersion: string;
}

export interface CreateEmployee {
  name: string;
  birthDate: string;
  address: string;
  phoneNumber: string;
  email: string;
  joinDate: string;
  departmentId: number;
  jobGradeId: number;
}

export interface UpdateEmployee {
  id: number;
  name: string;
  birthDate: string;
  address: string;
  phoneNumber: string;
  email: string;
  departmentId: number;
  jobGradeId: number;
  rowVersion: string;
}

export interface DeleteEmployee {
  id: number;
  rowVersion: string;
}
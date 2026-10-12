export interface Department {
  id: number;
  name: string;
  rowVersion: string;
}

export interface CreateDepartment {
  name: string;
}

export interface UpdateDepartment {
  id: number;
  name: string;
  rowVersion: string;
}

export interface DeleteDepartment {
  id: number;
  rowVersion: string;
}
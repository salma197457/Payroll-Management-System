export interface JobGrade {
  id: number;
  name: string;
  salary: number;
  rowVersion: string;
}

export interface CreateJobGrade {
  name: string;
  salary: number;
}

export interface UpdateJobGrade {
  id: number;
  salary: number;
  rowVersion: string;
}
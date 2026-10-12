import { Routes } from '@angular/router';

import { Home } from './pages/home/home';
import { Departments } from './pages/departments/departments';
import { JobGrades } from './pages/job-grades/job-grades';
import { Employees } from './pages/employees/employees';
import { DepartmentIncentives } from './pages/department-incentives/department-incentives';
import { ExperienceIncentives } from './pages/experience-incentives/experience-incentives';
import { AbsenceRules } from './pages/absence-rules/absence-rules';
import { AbsenceManagement } from './pages/absence-management/absence-management';
import { Reports } from './pages/reports/reports';
import { DepartmentForm } from './pages/departments/department-form/department-form';
import { JobGradeForm } from './pages/job-grades/job-grade-form/job-grade-form';
import { EmployeeForm } from './pages/employees/employee-form/employee-form';
import { DepartmentIncentiveForm } from './pages/department-incentives/department-incentive-form/department-incentive-form';
import { ExperienceIncentiveForm } from './pages/experience-incentives/experience-incentive-form/experience-incentive-form';
import { AbsenceRuleForm } from './pages/absence-rules/absence-rule-form/absence-rule-form';
import { AbsenceForm } from './pages/absence-management/absence-form/absence-form';
import { EmployeeReport } from './pages/reports/employee-report/employee-report';
import { AttendanceReport } from './pages/reports/attendance-report/attendance-report';
import { IncentivesDeductionsReport } from './pages/reports/incentives-deductions-report/incentives-deductions-report';
import { SalaryReport } from './pages/reports/salary-report/salary-report';


export const routes: Routes = [
  { path: '', component: Home },
{ path: 'job-grades', component: JobGrades },
{ path: 'job-grades/create', component: JobGradeForm },
{ path: 'job-grades/edit/:id', component: JobGradeForm },
{ path: 'employees', component: Employees },
{ path: 'employees/create', component: EmployeeForm },
{ path: 'employees/edit/:id', component: EmployeeForm },  
{ path: 'department-incentives', component: DepartmentIncentives },
{ path: 'experience-incentives', component: ExperienceIncentives },
{ path: 'experience-incentives/create', component: ExperienceIncentiveForm },
{ path: 'experience-incentives/edit/:id', component: ExperienceIncentiveForm },
{ path: 'absence-rules', component: AbsenceRules },
{ path: 'absence-rules/create', component: AbsenceRuleForm },
{ path: 'absence-rules/edit/:id', component: AbsenceRuleForm },
{ path: 'absence-management', component: AbsenceManagement },
{ path: 'absence-management/create', component: AbsenceForm },
{ path: 'departments', component: Departments },
{ path: 'departments/create', component: DepartmentForm },
{ path: 'departments/edit/:id', component: DepartmentForm },  
{ path: 'department-incentives', component: DepartmentIncentives },
{ path: 'department-incentives/create', component: DepartmentIncentiveForm },
{ path: 'department-incentives/edit/:id', component: DepartmentIncentiveForm },
{ path: 'reports', component: Reports },
{ path: 'reports/employees', component: EmployeeReport },
{ path: 'reports/attendance', component: AttendanceReport },
{path: 'reports/incentives-deductions', component: IncentivesDeductionsReport},
{path: 'reports/salaries', component: SalaryReport},
  { path: '**', redirectTo: '' }
];
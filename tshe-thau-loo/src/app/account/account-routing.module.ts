import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdministratorRegisterComponent } from './register/administrator-register/administrator-register.component';
import { SelectRegisterComponent } from './register/select-register/select-register.component';
import { AlumnusRegisterComponent } from './register/alumnus-register/alumnus-register.component';
import { EmployeeRegisterComponent } from './register/employee-register/employee-register.component';
import { ExaminerRegisterComponent } from './register/examiner-register/examiner-register.component';
import { ManagerRegisterComponent } from './register/manager-register/manager-register.component';
import { StudentRegisterComponent } from './register/student-register/student-register.component';
import { LoginComponent } from './login/login/login.component';
import { AccountRedirectGuard } from '../guards/account-redirect/account-redirect.guard';

const routes: Routes = [
  { path: 'login', component: LoginComponent, pathMatch: 'full' },
  {
    path: 'register',
    children: [
      { path: '', component: SelectRegisterComponent, pathMatch: 'full' },
      { path: 'administrator', component: AdministratorRegisterComponent, pathMatch: 'full' },
      { path: 'alumnus', component: AlumnusRegisterComponent, pathMatch: 'full' },
      { path: 'employee', component: EmployeeRegisterComponent, pathMatch: 'full' },
      { path: 'examiner', component: ExaminerRegisterComponent, pathMatch: 'full' },
      { path: 'manager', component: ManagerRegisterComponent, pathMatch: 'full' },
      { path: 'student', component: StudentRegisterComponent, pathMatch: 'full' }
    ]
  },
  { path: '', pathMatch: 'full', canActivate: [AccountRedirectGuard], runGuardsAndResolvers: 'always' }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AccountRoutingModule { }

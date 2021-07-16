import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RequiredLoginGuard } from '../guards/required-login/required-login.guard';
import { IsAdministratorGuard } from '../guards/is-administrator/is-administrator.guard';
import { AdministratorManageListComponent } from './administrator-manage-list/administrator-manage-list.component';
import { AdministratorManageComponent } from './administrator-manage/administrator-manage.component';
import { AdministratorManageEditComponent } from './administrator-manage-edit/administrator-manage-edit.component';
import { AlumnusManageListComponent } from './alumnus-manage-list/alumnus-manage-list.component';
import { AlumnusManageComponent } from './alumnus-manage/alumnus-manage.component';
import { AlumnusManageEditComponent } from './alumnus-manage-edit/alumnus-manage-edit.component';
import { EmployeeManageListComponent } from './employee-manage-list/employee-manage-list.component';
import { EmployeeManageComponent } from './employee-manage/employee-manage.component';
import { EmployeeManageEditComponent } from './employee-manage-edit/employee-manage-edit.component';
import { ExaminerManageListComponent } from './examiner-manage-list/examiner-manage-list.component';
import { ExaminerManageComponent } from './examiner-manage/examiner-manage.component';
import { ExaminerManageEditComponent } from './examiner-manage-edit/examiner-manage-edit.component';
import { ManagerManageListComponent } from './manager-manage-list/manager-manage-list.component';
import { ManagerManageComponent } from './manager-manage/manager-manage.component';
import { ManagerManageEditComponent } from './manager-manage-edit/manager-manage-edit.component';
import { StudentManageListComponent } from './student-manage-list/student-manage-list.component';
import { StudentManageComponent } from './student-manage/student-manage.component';
import { StudentManageEditComponent } from './student-manage-edit/student-manage-edit.component';
import { ManageHomeComponent } from './manage-home/manage-home.component';

const routes: Routes = [
  {
    path: '',
    component: ManageHomeComponent,
    pathMatch: 'full',
    canActivate: [RequiredLoginGuard, IsAdministratorGuard]
  },
  {
    path: 'administrator',
    children: [
      { path: 'list', component: AdministratorManageListComponent, pathMatch: 'full' },
      { path: ':userId', component: AdministratorManageComponent, pathMatch: 'full' },
      { path: ':userId/edit', component: AdministratorManageEditComponent, pathMatch: 'full' }
    ],
    canActivate: [RequiredLoginGuard, IsAdministratorGuard]
  },
  {
    path: 'alumnus',
    children: [
      { path: 'list', component: AlumnusManageListComponent, pathMatch: 'full' },
      { path: ':userId', component: AlumnusManageComponent, pathMatch: 'full' },
      { path: ':userId/edit', component: AlumnusManageEditComponent, pathMatch: 'full' },
    ],
    canActivate: [RequiredLoginGuard, IsAdministratorGuard]
  },
  {
    path: 'employee',
    children: [
      { path: 'list', component: EmployeeManageListComponent, pathMatch: 'full' },
      { path: ':userId', component: EmployeeManageComponent, pathMatch: 'full' },
      { path: ':userId/edit', component: EmployeeManageEditComponent, pathMatch: 'full' },
    ],
    canActivate: [RequiredLoginGuard, IsAdministratorGuard]
  },
  {
    path: 'examiner',
    children: [
      { path: 'list', component: ExaminerManageListComponent, pathMatch: 'full' },
      { path: ':userId', component: ExaminerManageComponent, pathMatch: 'full' },
      { path: ':userId/edit', component: ExaminerManageEditComponent, pathMatch: 'full' },
    ],
    canActivate: [RequiredLoginGuard, IsAdministratorGuard]
  },
  {
    path: 'manager',
    children: [
      { path: 'list', component: ManagerManageListComponent, pathMatch: 'full' },
      { path: ':userId', component: ManagerManageComponent, pathMatch: 'full' },
      { path: ':userId/edit', component: ManagerManageEditComponent, pathMatch: 'full' },
    ],
    canActivate: [RequiredLoginGuard, IsAdministratorGuard]
  },
  {
    path: 'student',
    children: [
      { path: 'list', component: StudentManageListComponent, pathMatch: 'full' },
      { path: ':userId', component: StudentManageComponent, pathMatch: 'full' },
      { path: ':userId/edit', component: StudentManageEditComponent, pathMatch: 'full' },
    ],
    canActivate: [RequiredLoginGuard, IsAdministratorGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ManageRoutingModule { }

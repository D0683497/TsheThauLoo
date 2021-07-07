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
import { RequiredLoginGuard } from '../guards/required-login/required-login.guard';
import { AdministratorProfileComponent } from './profile/administrator/administrator-profile/administrator-profile.component';
import { AdministratorEditProfileComponent } from './profile/administrator/administrator-edit-profile/administrator-edit-profile.component';
import { AlumnusProfileComponent } from './profile/alumnus/alumnus-profile/alumnus-profile.component';
import { AlumnusEditProfileComponent } from './profile/alumnus/alumnus-edit-profile/alumnus-edit-profile.component';
import { EmployeeProfileComponent } from './profile/employee/employee-profile/employee-profile.component';
import { EmployeeEditProfileComponent } from './profile/employee/employee-edit-profile/employee-edit-profile.component';
import { ExaminerProfileComponent } from './profile/examiner/examiner-profile/examiner-profile.component';
import { ExaminerEditProfileComponent } from './profile/examiner/examiner-edit-profile/examiner-edit-profile.component';
import { ManagerProfileComponent } from './profile/manager/manager-profile/manager-profile.component';
import { ManagerEditProfileComponent } from './profile/manager/manager-edit-profile/manager-edit-profile.component';
import { StudentProfileComponent } from './profile/student/student-profile/student-profile.component';
import { StudentEditProfileComponent } from './profile/student/student-edit-profile/student-edit-profile.component';
// eslint-disable-next-line max-len
import { AdministratorResponsibilityComponent } from './profile/administrator/administrator-responsibility/administrator-responsibility.component';
import { ManagerSubstituteComponent } from './profile/manager/manager-substitute/manager-substitute.component';
import { StudentVerifyComponent } from './profile/student/student-verify/student-verify.component';
import { AlumnusVerifyComponent } from './profile/alumnus/alumnus-verify/alumnus-verify.component';
import { ChangeUserNameComponent } from './change-user-name/change-user-name.component';
import { ChangeEmailComponent } from './email/change-email/change-email.component';
import { ConfirmEmailComponent } from './email/confirm-email/confirm-email.component';
import { ChangePhoneComponent } from './change-phone/change-phone.component';
import { ChangePasswordComponent } from './password/change-password/change-password.component';
import { ForgetPasswordComponent } from './password/forget-password/forget-password.component';
import { ResetPasswordComponent } from './password/reset-password/reset-password.component';
import { NationalEditComponent } from './national/national-edit/national-edit.component';
import { NationalVerifyComponent } from './national/national-verify/national-verify.component';
import { NidLoginComponent } from './login/nid-login/nid-login.component';

const routes: Routes = [
  {
    path: 'login',
    children: [
      { path: '', component: LoginComponent, pathMatch: 'full' },
      { path: 'nid', component: NidLoginComponent, pathMatch: 'full' }
    ]
  },
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
  {
    path: 'profile',
    children: [
      {
        path: 'administrator',
        children: [
          { path: '', component: AdministratorProfileComponent, pathMatch: 'full' },
          { path: 'info', component: AdministratorEditProfileComponent, pathMatch: 'full' },
          { path: 'responsibility', component: AdministratorResponsibilityComponent, pathMatch: 'full' }
        ]
      },
      {
        path: 'alumnus',
        children: [
          { path: '', component: AlumnusProfileComponent, pathMatch: 'full' },
          { path: 'info', component: AlumnusEditProfileComponent, pathMatch: 'full' },
          { path: 'verify', component: AlumnusVerifyComponent, pathMatch: 'full'}
        ]
      },
      {
        path: 'employee',
        children: [
          { path: '', component: EmployeeProfileComponent, pathMatch: 'full' },
          { path: 'info', component: EmployeeEditProfileComponent, pathMatch: 'full' }
        ]
      },
      {
        path: 'examiner',
        children: [
          { path: '', component: ExaminerProfileComponent, pathMatch: 'full' },
          { path: 'info', component: ExaminerEditProfileComponent, pathMatch: 'full' }
        ]
      },
      {
        path: 'manager',
        children: [
          { path: '', component: ManagerProfileComponent, pathMatch: 'full' },
          { path: 'info', component: ManagerEditProfileComponent, pathMatch: 'full' },
          { path: 'substitute', component: ManagerSubstituteComponent, pathMatch: 'full' }
        ]
      },
      {
        path: 'student',
        children: [
          { path: '', component: StudentProfileComponent, pathMatch: 'full' },
          { path: 'info', component: StudentEditProfileComponent, pathMatch: 'full' },
          { path: 'verify', component: StudentVerifyComponent, pathMatch: 'full'}
        ]
      }
    ],
    canActivate: [RequiredLoginGuard]
  },
  { path: 'username', component: ChangeUserNameComponent, pathMatch: 'full', canActivate: [RequiredLoginGuard] },
  {
    path: 'email',
    children: [
      { path: '', component: ChangeEmailComponent, pathMatch: 'full', canActivate: [RequiredLoginGuard] },
      { path: 'confirm', component: ConfirmEmailComponent, pathMatch: 'full' }
    ]
  },
  { path: 'phone', component: ChangePhoneComponent, pathMatch: 'full', canActivate: [RequiredLoginGuard] },
  {
    path: 'password',
    children: [
      { path: '', component: ChangePasswordComponent, pathMatch: 'full', canActivate: [RequiredLoginGuard] },
      { path: 'forget', component: ForgetPasswordComponent, pathMatch: 'full' },
      { path: 'reset', component: ResetPasswordComponent, pathMatch: 'full' }
    ]
  },
  {
    path: 'national',
    children: [
      { path: '', component: NationalEditComponent, pathMatch: 'full' },
      { path: 'verify', component: NationalVerifyComponent, pathMatch: 'full' }
    ],
    canActivate: [RequiredLoginGuard]
  },
  { path: '', pathMatch: 'full', canActivate: [AccountRedirectGuard], runGuardsAndResolvers: 'always' }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AccountRoutingModule { }

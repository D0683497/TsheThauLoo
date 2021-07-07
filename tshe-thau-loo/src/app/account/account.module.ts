import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AccountRoutingModule } from './account-routing.module';
import { IonicModule } from '@ionic/angular';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SelectRegisterComponent } from './register/select-register/select-register.component';
import { AdministratorRegisterComponent } from './register/administrator-register/administrator-register.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { TermsRegisterComponent } from './register/terms-register/terms-register.component';
import { AlumnusRegisterComponent } from './register/alumnus-register/alumnus-register.component';
import { EmployeeRegisterComponent } from './register/employee-register/employee-register.component';
import { ExaminerRegisterComponent } from './register/examiner-register/examiner-register.component';
import { ManagerRegisterComponent } from './register/manager-register/manager-register.component';
import { StudentRegisterComponent } from './register/student-register/student-register.component';
import { LoginComponent } from './login/login/login.component';
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
// eslint-disable-next-line max-len
import { AdministratorEditResponsibilityComponent } from './profile/administrator/administrator-edit-responsibility/administrator-edit-responsibility.component';
// eslint-disable-next-line max-len
import { AdministratorCreateResponsibilityComponent } from './profile/administrator/administrator-create-responsibility/administrator-create-responsibility.component';
import { ManagerSubstituteComponent } from './profile/manager/manager-substitute/manager-substitute.component';
import { StudentVerifyComponent } from './profile/student/student-verify/student-verify.component';
import { AlumnusVerifyComponent } from './profile/alumnus/alumnus-verify/alumnus-verify.component';
import { EditVerifyFileComponent } from './profile/edit-verify-file/edit-verify-file.component';
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


@NgModule({
  declarations: [
    SelectRegisterComponent,
    AdministratorRegisterComponent,
    TermsRegisterComponent,
    AlumnusRegisterComponent,
    EmployeeRegisterComponent,
    ExaminerRegisterComponent,
    ManagerRegisterComponent,
    StudentRegisterComponent,
    LoginComponent,
    AdministratorProfileComponent,
    AdministratorEditProfileComponent,
    AlumnusProfileComponent,
    AlumnusEditProfileComponent,
    EmployeeProfileComponent,
    EmployeeEditProfileComponent,
    ExaminerProfileComponent,
    ExaminerEditProfileComponent,
    ManagerProfileComponent,
    ManagerEditProfileComponent,
    StudentProfileComponent,
    StudentEditProfileComponent,
    AdministratorResponsibilityComponent,
    AdministratorEditResponsibilityComponent,
    AdministratorCreateResponsibilityComponent,
    ManagerSubstituteComponent,
    StudentVerifyComponent,
    AlumnusVerifyComponent,
    EditVerifyFileComponent,
    ChangeUserNameComponent,
    ChangeEmailComponent,
    ConfirmEmailComponent,
    ChangePhoneComponent,
    ChangePasswordComponent,
    ForgetPasswordComponent,
    ResetPasswordComponent,
    NationalEditComponent,
    NationalVerifyComponent,
    NidLoginComponent
  ],
  imports: [
    CommonModule,
    AccountRoutingModule,
    IonicModule,
    FormsModule,
    ReactiveFormsModule,
    FontAwesomeModule
  ]
})
export class AccountModule { }

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
import { AlumnusProfileComponent } from './profile/alumnus-profile/alumnus-profile.component';
import { EmployeeProfileComponent } from './profile/employee-profile/employee-profile.component';
import { ExaminerProfileComponent } from './profile/examiner-profile/examiner-profile.component';
import { ManagerProfileComponent } from './profile/manager-profile/manager-profile.component';
import { StudentProfileComponent } from './profile/student-profile/student-profile.component';
import { AdministratorProfileComponent } from './profile/administrator/administrator-profile/administrator-profile.component';
import { AdministratorEditProfileComponent } from './profile/administrator/administrator-edit-profile/administrator-edit-profile.component';

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
    EmployeeProfileComponent,
    ExaminerProfileComponent,
    ManagerProfileComponent,
    StudentProfileComponent
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

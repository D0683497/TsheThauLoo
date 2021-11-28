import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ManageRoutingModule } from './manage-routing.module';
import { IonicModule } from '@ionic/angular';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { ManageHomeComponent } from './manage-home/manage-home.component';
import { QuillModule } from 'ngx-quill';
import * as QuillBlotFormatter from 'quill-blot-formatter';
import { AdministratorManageListComponent } from './administrator-manage-list/administrator-manage-list.component';
import { AlumnusManageListComponent } from './alumnus-manage-list/alumnus-manage-list.component';
import { EmployeeManageListComponent } from './employee-manage-list/employee-manage-list.component';
import { ExaminerManageListComponent } from './examiner-manage-list/examiner-manage-list.component';
import { ManagerManageListComponent } from './manager-manage-list/manager-manage-list.component';
import { StudentManageListComponent } from './student-manage-list/student-manage-list.component';
import { AdministratorManageComponent } from './administrator-manage/administrator-manage.component';
import { AlumnusManageComponent } from './alumnus-manage/alumnus-manage.component';
import { EmployeeManageComponent } from './employee-manage/employee-manage.component';
import { ExaminerManageComponent } from './examiner-manage/examiner-manage.component';
import { ManagerManageComponent } from './manager-manage/manager-manage.component';
import { StudentManageComponent } from './student-manage/student-manage.component';
import { AdministratorManageEditComponent } from './administrator-manage-edit/administrator-manage-edit.component';
import { AlumnusManageEditComponent } from './alumnus-manage-edit/alumnus-manage-edit.component';
import { EmployeeManageEditComponent } from './employee-manage-edit/employee-manage-edit.component';
import { ExaminerManageEditComponent } from './examiner-manage-edit/examiner-manage-edit.component';
import { ManagerManageEditComponent } from './manager-manage-edit/manager-manage-edit.component';
import { StudentManageEditComponent } from './student-manage-edit/student-manage-edit.component';

@NgModule({
  declarations: [
    ManageHomeComponent,
    AdministratorManageListComponent,
    AlumnusManageListComponent,
    EmployeeManageListComponent,
    ExaminerManageListComponent,
    ManagerManageListComponent,
    StudentManageListComponent,
    AdministratorManageComponent,
    AlumnusManageComponent,
    EmployeeManageComponent,
    ExaminerManageComponent,
    ManagerManageComponent,
    StudentManageComponent,
    AdministratorManageEditComponent,
    AlumnusManageEditComponent,
    EmployeeManageEditComponent,
    ExaminerManageEditComponent,
    ManagerManageEditComponent,
    StudentManageEditComponent
  ],
  imports: [
    CommonModule,
    ManageRoutingModule,
    IonicModule,
    FormsModule,
    ReactiveFormsModule,
    FontAwesomeModule,
    QuillModule.forRoot({
      modules: {
        syntax: false, // 程式碼語法檢測
        toolbar: [
          [{ header: [1, 2, 3, 4, 5, 6, false] }], // 標題大小
          ['bold', 'italic', 'underline', 'strike'],
          [{ list: 'ordered'}, { list: 'bullet' }],
          [{ align: [] }],
          [{ indent: '-1'}, { indent: '+1' }],
          [{ color: [] }, { background: [] }],
          ['blockquote', 'code-block'],
          ['link', 'image'],
          ['clean'],
        ],
        blotFormatter: {}
      },
      customModules: [{
        implementation: QuillBlotFormatter.default,
        path: 'modules/blotFormatter'
      }],
    })
  ]
})
export class ManageModule { }

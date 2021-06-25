import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ResumeRoutingModule } from './resume-routing.module';
import { IonicModule } from '@ionic/angular';
import { ResumeHomeComponent } from './resume-home/resume-home.component';
import { ResumeFileListComponent } from './resume-file-list/resume-file-list.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ResumeFileEditComponent } from './resume-file-edit/resume-file-edit.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { ResumeOnlineListComponent } from './resume-online-list/resume-online-list.component';

@NgModule({
  declarations: [
    ResumeHomeComponent,
    ResumeFileListComponent,
    ResumeFileEditComponent,
    ResumeOnlineListComponent
  ],
  imports: [
    CommonModule,
    ResumeRoutingModule,
    IonicModule,
    FormsModule,
    ReactiveFormsModule,
    FontAwesomeModule
  ]
})
export class ResumeModule { }

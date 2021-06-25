import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ResumeHomeComponent } from './resume-home/resume-home.component';
import { ResumeFileListComponent } from './resume-file-list/resume-file-list.component';
import { ResumeOnlineListComponent } from './resume-online-list/resume-online-list.component';
import { RequiredLoginGuard } from '../guards/required-login/required-login.guard';

const routes: Routes = [
  { path: '', component: ResumeHomeComponent, pathMatch: 'full', canActivate: [RequiredLoginGuard] },
  {
    path: 'file',
    children: [
      { path: 'list', component: ResumeFileListComponent, pathMatch: 'full' }
    ],
    canActivate: [RequiredLoginGuard]
  },
  {
    path: 'online',
    children: [
      { path: 'list', component: ResumeOnlineListComponent, pathMatch: 'full' }
    ],
    canActivate: [RequiredLoginGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ResumeRoutingModule { }

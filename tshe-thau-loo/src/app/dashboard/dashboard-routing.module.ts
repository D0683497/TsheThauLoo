import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { PrivacyStatementComponent } from './privacy-statement/privacy-statement.component';
import { PrivacyAnnouncementComponent } from './privacy-announcement/privacy-announcement.component';
import { PrivacyRightsComponent } from './privacy-rights/privacy-rights.component';
import { PrivacyProtectionComponent } from './privacy-protection/privacy-protection.component';

const routes: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full' },
  {
    path: 'privacy',
    children: [
      { path: 'statement', component: PrivacyStatementComponent, pathMatch: 'full' },
      { path: 'protection', component: PrivacyProtectionComponent, pathMatch: 'full' },
      { path: 'rights', component: PrivacyRightsComponent, pathMatch: 'full' },
      { path: 'announcement', component: PrivacyAnnouncementComponent, pathMatch: 'full' }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DashboardRoutingModule { }
